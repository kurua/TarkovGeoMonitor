using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using MaxMind.GeoIP2.Responses;
using System.Net;
using MaxMind.GeoIP2.Model;
using System.CodeDom.Compiler;

namespace TarkovGeoMonitor
{
    public partial class TarkovGeoMonitor : Form
    {
        bool dMode = false;

        string mmdbFullPath;
        string eftLogDirPath;
        string targetLogDir;
        string targetAppLogFilePath;
        string targetNwConLogFilePath;
        string latestDirectory;
        string lastConSrvIP;
        string lastConSrvGeo;

        DataTable dt_Log = new DataTable();

        string[] tempData = new string[] { "", "", "", "", "" };
        string[] lastData = new string[] { "2000-01-01 00:00:00", "", "", "", "" };



        public TarkovGeoMonitor()
        {
            InitializeComponent();

            tabControl1.Size = new Size(407, 189);
            labelA1.Location = new Point(273, 194);
            linkLabelA2.Location = new Point(368, 194);
            this.Size = new Size(425, 251); // タブ0が選択された場合のサイズ

            label_MMDB.Text = "-----";
            label_EFTLogDir.Text = "-----";
            label_targetLogDir.Text = "-----";
            label_targetLogFile.Text = "-----";
            label_Date.Text = "-----";
            label_IP.Text = "-----";
            label_Geo.Text = "-----";

            dt_Log.Columns.Add("Date", typeof(string));
            dt_Log.Columns.Add("Map", typeof(string));
            dt_Log.Columns.Add("IP", typeof(string));
            dt_Log.Columns.Add("Geo", typeof(string));
            dt_Log.Columns.Add("shortId", typeof(string));
            dt_Log.Columns.Add("ConnectionLost", typeof(string));

            dataGridView_log.DataSource = dt_Log;

            SearchEftLogPath();
            SearchMMDB();

            // 過去ログ総洗い
            searchTargetAllDir();

            // 通常処理の開始
            if (!dMode)
                runTimer.Start();

        }

        // 移植
        void GeoIP()
        {
            // MMDBファイルのパス
            string databasePath = mmdbFullPath;

            try
            {
                using (var reader = new DatabaseReader(databasePath))
                {
                    // 国情報を取得
                    CountryResponse countryResponse = reader.Country(lastConSrvIP);

                    // 国名を出力
                    lastConSrvGeo = countryResponse.Country.Name;
                }
            }
            catch (AddressNotFoundException)
            {
                lastConSrvGeo = "No results found.";
            }
            catch (GeoIP2Exception)
            {
                lastConSrvGeo = "No results found.";
            }
        }
        void SearchMMDB()
        {
            // 実行ファイルのパスを取得
            string exePath = AppDomain.CurrentDomain.BaseDirectory;

            // 検索する拡張子を指定
            string fileExtension = "*.mmdb";

            try
            {
                // 指定された拡張子のファイルを検索
                string[] files = Directory.GetFiles(exePath, fileExtension);

                // 日付パターン (例: 2024-08)
                string datePattern = @"\d{4}-\d{2}";

                // 日付が含まれるファイルをフィルタリング
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
                    // 最新の日付のファイルを取得
                    var latestFile = dateFiles.First();
                    label_MMDB.Text = Path.GetFileName(latestFile.FileName);
                    mmdbFullPath = Path.GetFullPath(latestFile.FileName);
                }
                else
                {
                    MessageBox.Show("MMDB was not found.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("",
                "Unexpected error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                Close();
            }
        }

        void SearchEftLogPath()
        {

            label_EFTLogDir.Text = "err.";

            try
            {
                // レジストリキーのパス
                string registryKeyPath = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\EscapeFromTarkov";

                // サブキーからUninstallStringを取得
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKeyPath))
                {
                    if (key != null)
                    {
                        // UninstallStringの取得
                        string uninstallString = key.GetValue("UninstallString") as string;

                        if (!string.IsNullOrEmpty(uninstallString))
                        {
                            // インストールパスを抽出
                            string eftLogPath = Path.GetDirectoryName(uninstallString.Trim('"')) + @"\Logs";
                            if (System.IO.Directory.Exists(eftLogPath))
                            {
                                eftLogDirPath = eftLogPath;
                                label_EFTLogDir.Text = eftLogPath;
                            }
                            else
                            {
                                label_EFTLogDir.Text = "err.";
                            }
                            //Debug.WriteLine($"Escape From Tarkov is installed at: {eftLogPath}");
                        }
                        else
                        {
                            //Debug.WriteLine("UninstallString value is not found.");
                        }
                    }
                    else
                    {
                        //Debug.WriteLine("Escape From Tarkov registry key not found.");
                    }
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        void searchTargetAllDir()
        {
            // サブフォルダー名を格納するリストを作成
            List<string> subFolderNames = new List<string>();

            // 指定されたフォルダー配下のサブフォルダー名を取得
            try
            {
                // GetDirectoriesでフォルダー配下のサブフォルダーを取得
                string[] subFolders = Directory.GetDirectories(eftLogDirPath);

                foreach (string subFolder in subFolders)
                {
                    // フォルダー名を取得してリストに追加
                    subFolderNames.Add(Path.GetFileName(subFolder));
                }

                // サブフォルダー名を出力（オプション）
                foreach (string name in subFolderNames)
                {
                    searchTargetLog(eftLogDirPath + @"\" + name);
                    checkAllLog(targetAppLogFilePath);
                }
            }
            catch (Exception)
            {

            }
        }


        void searchTargetLatestDir()
        {
            try
            {
                // フォルダを列挙
                var directories = Directory.GetDirectories(eftLogDirPath, "log_*");

                // 日付パターンの指定
                string datePattern = "yyyy.MM.dd_H-mm-ss";

                // 最新のフォルダを選択
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
            catch (Exception)
            {
            }
        }

        bool searchTargetLog(string latestDirectory)
        {
            try
            {
                if (latestDirectory != null)
                {
                    //Debug.WriteLine($"最新のフォルダ: {latestDirectory}");
                    targetLogDir = Path.GetFileName(latestDirectory);


                    try
                    {
                        // フォルダ内の「application.log」で終わるファイルを取得
                        var logAppFile = Directory.GetFiles(latestDirectory, "*application.log").FirstOrDefault();

                        if (logAppFile != null)
                        {
                            //Debug.WriteLine($"見つかったファイル: {logFile}");
                            targetAppLogFilePath = logAppFile;
                            return true;
                        }
                        else
                        {
                            //Debug.WriteLine("「application.log」で終わるファイルが見つかりませんでした。");
                        }
                    }
                    catch (Exception)
                    {
                        //Debug.WriteLine($"エラーが発生しました: {ex.Message}");
                    }

                    try
                    {
                        // フォルダ内の「application.log」で終わるファイルを取得
                        var logNwConFile = Directory.GetFiles(latestDirectory, "*network-connection.log").FirstOrDefault();

                        if (logNwConFile != null)
                        {
                            //Debug.WriteLine($"見つかったファイル: {logFile}");
                            targetNwConLogFilePath = logNwConFile;
                            return true;
                        }
                        else
                        {
                            //Debug.WriteLine("「application.log」で終わるファイルが見つかりませんでした。");
                        }
                    }
                    catch (Exception)
                    {
                        //Debug.WriteLine($"エラーが発生しました: {ex.Message}");
                    }

                }
                else
                {
                    //Debug.WriteLine("フォルダが見つかりませんでした。");
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine($"エラーが発生しました: {ex.Message}");
            }
            targetAppLogFilePath = "";
            return false;
        }

        void checkAllLog(String args)
        {
            string filePath = args;
            string ipAddress = string.Empty;

            try
            {
                // ファイルを共有モードで開く
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        // データ抽出パターン
                        string dateTimePattern = @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}";
                        string locationPattern = @"Location:\s(?<location>\w+)";
                        string shotIdPattern = @"(?<=shortId:\s*)\w+";

                        Regex regex = new Regex(@"Ip:\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");

                        // 末尾に近いIPアドレスを取得するために、ファイル全体をループ
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // TRACE-NetworkGameCreateを含む行を探す
                            if (line.Contains("TRACE-NetworkGameCreate profileStatus:"))
                            {
                                Match match = regex.Match(line);
                                if (match.Success)
                                {
                                    ipAddress = match.Groups[1].Value;
                                    tempData[0] = Regex.Match(line, dateTimePattern).Value;
                                    tempData[1] = Regex.Match(line, locationPattern).Groups["location"].Value;
                                    tempData[2] = ipAddress;
                                    tempData[3] = "";
                                    tempData[4] = Regex.Match(line, shotIdPattern).Value;
                                }
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(ipAddress))
                {
                    lastConSrvIP = ipAddress;
                    //Debug.WriteLine($"抽出されたIPアドレス: {ipAddress}");
                    label_IP.Text = lastConSrvIP;

                    if (DateTime.Parse(lastData[0]) < DateTime.Parse(tempData[0]))
                    {
                        label_Date.Text = tempData[0];
                        GeoIP();
                        label_Geo.Text = lastConSrvGeo;

                        lastData[0] = tempData[0];

                        switch (tempData[1])
                        {
                            case "bigmap":
                                {
                                    lastData[1] = "Customs";
                                    break;
                                }
                            case "RezervBase":
                                {
                                    lastData[1] = "Reserve";
                                    break;
                                }
                            case "factory4_day":
                            case "factory4_night":
                                {
                                    lastData[1] = "Factory";
                                    break;
                                }
                            case "Sandbox":
                            case "Sandbox_high":
                                {
                                    lastData[1] = "Ground zero";
                                    break;
                                }
                            case "TarkovStreets":
                                {
                                    lastData[1] = "Streets Of Tarkov";
                                    break;
                                }
                            case "laboratory":
                                {
                                    lastData[1] = "The Lab";
                                    break;
                                }
                            default:
                                {
                                    lastData[1] = tempData[1];
                                    break;
                                }
                        }

                        lastData[2] = tempData[2];
                        lastData[3] = lastConSrvGeo;
                        lastData[4] = tempData[4];
                        dt_Log.Rows.Add(lastData[0], lastData[1], lastData[2], lastData[3], lastData[4], "-----");

                        tempData[0] = "";
                        tempData[1] = "";
                        tempData[2] = "";
                        tempData[3] = "";
                        tempData[4] = "";
                    }
                }
                else
                {
                    lastConSrvIP = "waiting for Raid";
                    //Debug.WriteLine("IPアドレスが見つかりませんでした。");
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        void checkNewLog(String args)
        {
            string filePath = args;
            string ipAddress = string.Empty;

            try
            {
                // ファイルを共有モードで開く
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        // データ抽出パターン
                        string dateTimePattern = @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}";
                        string locationPattern = @"Location:\s(?<location>\w+)";
                        string shotIdPattern = @"(?<=shortId:\s*)\w+";

                        Regex regex = new Regex(@"Ip:\s*(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})");

                        // 末尾に近いIPアドレスを取得するために、ファイル全体をループ
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            // TRACE-NetworkGameCreateを含む行を探す
                            if (line.Contains("TRACE-NetworkGameCreate profileStatus:"))
                            {
                                Match match = regex.Match(line);
                                if (match.Success)
                                {
                                    ipAddress = match.Groups[1].Value;
                                    tempData[0] = Regex.Match(line, dateTimePattern).Value;
                                    tempData[1] = Regex.Match(line, locationPattern).Groups["location"].Value;
                                    tempData[2] = ipAddress;
                                    tempData[3] = "";
                                    tempData[4] = Regex.Match(line, shotIdPattern).Value;
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ipAddress))
                {
                    lastConSrvIP = ipAddress;
                    //Debug.WriteLine($"抽出されたIPアドレス: {ipAddress}");
                }
                else
                {
                    lastConSrvIP = "waiting for Raid";
                    //Debug.WriteLine("IPアドレスが見つかりませんでした。");
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        private void runTimer_Tick(object sender, EventArgs e)
        {

            searchTargetLatestDir();

            if (searchTargetLog(latestDirectory))
            {
                label_targetLogDir.Text = targetLogDir;
                label_targetLogFile.Text = targetAppLogFilePath.Substring((eftLogDirPath + targetLogDir).Length + 2);

                checkNewLog(targetAppLogFilePath);
                label_IP.Text = lastConSrvIP;

                if (lastConSrvIP != "waiting for Raid")
                {
                    Console.WriteLine(lastData[0] + " , " + tempData[0]);
                    if (DateTime.Parse(lastData[0]) < DateTime.Parse(tempData[0]))
                    {
                        label_Date.Text = tempData[0];

                        GeoIP();
                        label_Geo.Text = lastConSrvGeo;

                        lastData[0] = tempData[0];
                        lastData[1] = tempData[1];
                        lastData[2] = tempData[2];
                        lastData[3] = lastConSrvGeo;
                        lastData[4] = tempData[4];
                        dt_Log.Rows.Add(lastData[0], lastData[1], lastData[2], lastData[3], lastData[4], 0);

                        tempData[0] = "";
                        tempData[1] = "";
                        tempData[2] = "";
                        tempData[3] = "";
                        tempData[4] = "";
                    }
                }
                else
                {
                    label_Geo.Text = "-----";
                }
            }
        }

        void SortDataGridView()
        {
            // 「スコア」列で降順にソート
            dataGridView_log.Sort(dataGridView_log.Columns["Date"], System.ComponentModel.ListSortDirection.Descending);

            // ソートインジケーターを更新
            foreach (DataGridViewColumn column in dataGridView_log.Columns)
            {
                column.HeaderCell.SortGlyphDirection = SortOrder.None;
            }
            dataGridView_log.Columns["Date"].HeaderCell.SortGlyphDirection = SortOrder.Descending;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://db-ip.com");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (dMode)
            {
                lastConSrvIP = "87.249.128.2093";
                GeoIP();
                label_IP.Text = lastConSrvIP;
                label_Geo.Text = lastConSrvGeo;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 現在選択されているタブのインデックスを取得
            int selectedIndex = tabControl1.SelectedIndex;

            // インデックスに応じてフォームのサイズを変更
            switch (selectedIndex)
            {
                case 0:
                    tabControl1.Size = new Size(407, 189);
                    labelA1.Location = new Point(273, 194);
                    linkLabelA2.Location = new Point(368, 194);
                    this.Size = new Size(425, 251); // タブ0が選択された場合のサイズ
                    break;
                case 1:
                    tabControl1.Size = new Size(676, 189);
                    labelA1.Location = new Point(542, 194);
                    linkLabelA2.Location = new Point(637, 194);
                    this.Size = new Size(694, 253); // タブ1が選択された場合のサイズ
                    break;
                default:
                    break;
            }
        }

        private void dataGridView_log_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            SortDataGridView();
        }
    }
}
