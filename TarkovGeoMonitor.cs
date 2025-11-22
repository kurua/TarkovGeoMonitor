using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MaxMind.GeoIP2;

namespace TarkovGeoMonitor
{
    public partial class TarkovGeoMonitor : Form
    {
        // パス関連
        string mmdbFullPath;
        string eftLogDirPath;
        string targetLogDir;
        string targetAppLogFilePath;
        string latestDirectory;

        string lastConSrvIP = "waiting for Raid";
        string lastConSrvGeo = "-----";
        DateTime lastRaidDate = DateTime.MinValue;

        DataTable dt_Log = new DataTable();

        private DatabaseReader _dbReader;

        private static readonly Regex _regIp = new Regex(@"Ip:\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})", RegexOptions.Compiled);
        private static readonly Regex _regDate = new Regex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}", RegexOptions.Compiled);
        private static readonly Regex _regLoc = new Regex(@"Location:\s(?<location>\w+)", RegexOptions.Compiled);
        private static readonly Regex _regShotId = new Regex(@"(?<=shortId:\s*)\w+", RegexOptions.Compiled);

        public TarkovGeoMonitor()
        {
            InitializeComponent();

            // フォーム初期化
            SetupFormLayout();

            // DataTable初期化
            InitializeDataTable();

            // ログパスとMMDBの検索
            SearchEftLogPath();
            SearchMMDB();

            // 過去ログ総洗い出し
            SearchTargetAllDir();

            // 終了時にリソースを開放するイベント追加
            this.FormClosed += (s, e) => { _dbReader?.Dispose(); };

            // 通常処理の開始
            runTimer.Start();
        }

        private void SetupFormLayout()
        {
            tabControl1.Size = new Size(407, 189);
            labelA1.Location = new Point(273, 194);
            linkLabelA2.Location = new Point(368, 194);
            this.Size = new Size(425, 251); // タブ0選択時のサイズ

            // ラベル初期化
            label_MMDB.Text = "-----";
            label_EFTLogDir.Text = "-----";
            label_targetLogDir.Text = "-----";
            label_targetLogFile.Text = "-----";
            label_Date.Text = "-----";
            label_IP.Text = "-----";
            label_Geo.Text = "-----";
        }

        private void InitializeDataTable()
        {
            dt_Log.Columns.Add("Date", typeof(string));
            dt_Log.Columns.Add("Map", typeof(string));
            dt_Log.Columns.Add("IP", typeof(string));
            dt_Log.Columns.Add("Geo", typeof(string));
            dt_Log.Columns.Add("shortId", typeof(string));
            dt_Log.Columns.Add("ConnectionLost", typeof(string));

            dataGridView_log.DataBindingComplete += DataGridView_log_DataBindingComplete;
            dataGridView_log.DataSource = dt_Log;

        }

        private void DataGridView_log_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView_log.Columns.Contains("Date"))
            {
                dataGridView_log.Columns["Date"].Width = 125;
                // dataGridView_log.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        void SearchMMDB()
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string fileExtension = "*.mmdb";

            try
            {
                string[] files = Directory.GetFiles(exePath, fileExtension);
                string datePattern = @"\d{4}-\d{2}";

                var dateFiles = files
                    .Select(file => new
                    {
                        FileName = file,
                        DateMatch = Regex.Match(Path.GetFileName(file), datePattern)
                    })
                    .Where(x => x.DateMatch.Success)
                    .Select(x => new
                    {
                        x.FileName,
                        Date = DateTime.ParseExact(x.DateMatch.Value, "yyyy-MM", null)
                    })
                    .OrderByDescending(x => x.Date)
                    .ToList();

                if (dateFiles.Any())
                {
                    var latestFile = dateFiles.First();
                    label_MMDB.Text = Path.GetFileName(latestFile.FileName);
                    mmdbFullPath = Path.GetFullPath(latestFile.FileName);

                    try
                    {
                        _dbReader = new DatabaseReader(mmdbFullPath);
                    }
                    catch
                    {
                        MessageBox.Show("Failed to open MMDB file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("MMDB was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Unexpected error in SearchMMDB.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        // GeoIP判定
        string GetGeoInfo(string ip)
        {
            if (_dbReader == null || string.IsNullOrEmpty(ip)) return "-----";
            if (ip == "waiting for Raid") return "-----";

            try
            {
                return _dbReader.Country(ip).Country.Name;
            }
            catch
            {
                return "No results found.";
            }
        }

        // マップ名変換
        string GetMapName(string locationCode)
        {
            switch (locationCode)
            {
                case "bigmap": return "Customs";
                case "RezervBase": return "Reserve";
                case "factory4_day":
                case "factory4_night": return "Factory";
                case "Sandbox":
                case "Sandbox_high": return "Ground zero";
                case "TarkovStreets": return "Streets Of Tarkov";
                case "laboratory": return "The Lab";
                default: return locationCode;
            }
        }

        // EFTログフォルダの検索
        void SearchEftLogPath()
        {
            label_EFTLogDir.Text = "err.";
            try
            {
                string registryKeyPath = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\EscapeFromTarkov";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyPath))
                {
                    if (key != null)
                    {
                        string uninstallString = key.GetValue("UninstallString") as string;
                        if (!string.IsNullOrEmpty(uninstallString))
                        {
                            string eftLogPath = Path.GetDirectoryName(uninstallString.Trim('"')) + @"\Logs";
                            if (Directory.Exists(eftLogPath))
                            {
                                eftLogDirPath = eftLogPath;
                                label_EFTLogDir.Text = eftLogPath;
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        // 全過去ログの洗い出し
        void SearchTargetAllDir()
        {
            try
            {
                if (string.IsNullOrEmpty(eftLogDirPath) || !Directory.Exists(eftLogDirPath)) return;

                var subFolders = Directory.GetDirectories(eftLogDirPath, "log_*")
                    .OrderByDescending(dir => dir) // 降順ソート
                    .Take(10);                     // 最新10件のみ

                foreach (string subFolder in subFolders)
                {
                    // 各フォルダのログを特定して解析
                    if (SearchTargetLog(subFolder))
                    {
                        AnalyzeLogFile(targetAppLogFilePath);
                    }
                }
            }
            catch (Exception) { }
        }

        // 最新ログフォルダの特定
        void SearchTargetLatestDir()
        {
            try
            {
                var directories = Directory.GetDirectories(eftLogDirPath, "log_*");
                string datePattern = "yyyy.MM.dd_H-mm-ss";

                latestDirectory = directories
                    .Select(dir => new
                    {
                        Directory = dir,
                        Date = DateTime.ParseExact(
                            Path.GetFileName(dir).Split('_')[1] + "_" + Path.GetFileName(dir).Split('_')[2],
                            datePattern,
                            CultureInfo.InvariantCulture)
                    })
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefault()?.Directory;
            }
            catch (Exception) { }
        }

        // 指定フォルダ内の対象ログファイルを特定
        bool SearchTargetLog(string dirPath)
        {
            try
            {
                if (dirPath != null)
                {
                    targetLogDir = Path.GetFileName(dirPath);
                    var logAppFile = Directory.GetFiles(dirPath, "*application_000.log").FirstOrDefault();

                    if (logAppFile != null)
                    {
                        targetAppLogFilePath = logAppFile;
                        return true;
                    }
                }
            }
            catch (Exception) { }
            targetAppLogFilePath = "";
            return false;
        }

        // ログ解析のメイン処理
        void AnalyzeLogFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader reader = new StreamReader(fs))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // 目的の行をフィルタ
                        if (line.Contains("TRACE-NetworkGameCreate profileStatus:"))
                        {
                            Match matchIp = _regIp.Match(line);
                            if (matchIp.Success)
                            {
                                string rawDate = _regDate.Match(line).Value;

                                if (DateTime.TryParse(rawDate, out DateTime currentLogDate))
                                {
                                    // 保持している最新日時より新しい場合のみ処理
                                    if (currentLogDate > lastRaidDate)
                                    {
                                        string ip = matchIp.Groups[1].Value;
                                        string locCode = _regLoc.Match(line).Groups["location"].Value;
                                        string shortId = _regShotId.Match(line).Value;
                                        string geo = GetGeoInfo(ip);
                                        string mapName = GetMapName(locCode);

                                        // 内部状態の更新
                                        lastRaidDate = currentLogDate;
                                        lastConSrvIP = ip;
                                        lastConSrvGeo = geo;

                                        // DataTableへ追加
                                        dt_Log.Rows.Add(rawDate, mapName, ip, geo, shortId, "-----");

                                        // ラベルの更新
                                        label_IP.Text = ip;
                                        label_Date.Text = rawDate;
                                        label_Geo.Text = geo;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void runTimer_Tick(object sender, EventArgs e)
        {
            SearchTargetLatestDir();

            if (SearchTargetLog(latestDirectory))
            {
                label_targetLogDir.Text = targetLogDir;
                // ラベルのパス表示更新（Logsフォルダより後ろを表示）
                if (targetAppLogFilePath.Length > eftLogDirPath.Length)
                {
                    label_targetLogFile.Text = Path.GetFileName(targetAppLogFilePath);
                }

                // ログファイルを解析して更新があれば反映
                AnalyzeLogFile(targetAppLogFilePath);
            }
        }

        void SortDataGridView()
        {
            if (dataGridView_log.Columns.Contains("Date"))
            {
                dataGridView_log.Sort(dataGridView_log.Columns["Date"], ListSortDirection.Descending);
                foreach (DataGridViewColumn column in dataGridView_log.Columns)
                {
                    column.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
                dataGridView_log.Columns["Date"].HeaderCell.SortGlyphDirection = SortOrder.Descending;
            }
        }

        // ライセンス
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { System.Diagnostics.Process.Start("https://db-ip.com"); } catch { }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = tabControl1.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    tabControl1.Size = new Size(407, 189);
                    labelA1.Location = new Point(273, 194);
                    linkLabelA2.Location = new Point(368, 194);
                    this.Size = new Size(425, 251);
                    break;
                case 1:
                    tabControl1.Size = new Size(676, 189);
                    labelA1.Location = new Point(542, 194);
                    linkLabelA2.Location = new Point(637, 194);
                    this.Size = new Size(694, 253);
                    break;
            }
        }

        private void dataGridView_log_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SortDataGridView();
        }
    }
}