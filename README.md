# TarkovGeoMonitor
**TarkovGeoMonitor** は、Escape from Tarkov (EFT) のログファイルを監視し、接続しているレイドサーバーのIPアドレスとその物理的な所在地（国名）を表示するツールです。

時間帯によって相性の悪いサーバがあったり、パーティプレイ中にリーダーの回線と相性が悪くコネロスした際に、毎回ログファイルを開いて調べるのが面倒だったので自分用に作成していたものです。

![ScreenShot](https://github.com/user-attachments/assets/96b4aea2-88aa-4175-9e49-c044bde87a76)
![ScreenShot](https://github.com/user-attachments/assets/dfaba4fb-ed41-4879-8af7-8c3cf00dccee)

## 使い方
1. **ダウンロード**
   Releases から zipファイルをダウンロードして解凍してください。

2. **起動**
   `TarkovGeoMonitor.exe` を実行するだけで、自動的にログを監視します。
   
   ※ IP判定用のデータファイル（.mmdb）は同梱されています。

### データの更新について（任意）
あまり効果は無いと思いますが、最新のデータを使いたい場合は [DB-IP (Lite)](https://db-ip.com/db/lite.php) から `.mmdb` ファイル（`dbip-country-lite-xxxx-xx.mmdb`）をダウンロードし、exeと同じフォルダに上書き保存してください。

## 注意事項
* あくまで自己満足で作ったプログラムなので、お使いの環境によっては動かないかもしれません。また、Steam版での動作確認は行っていません。
* ゲームクライアントがローカルに出力するテキストログファイル（`Logs`）を参照するだけの仕組みであり、ゲームプロセスのメモリ参照やパケットの解析は行っていません。クライアントが出力したテキストログ (Logsフォルダ) を読んでいるだけなので安全だとは思いますが、**使用は自己責任でお願いします。**

## クレジット
このツールは以下のデータとライブラリを使用しています。
* IP Data: [IP to Country Lite](https://db-ip.com/db/lite.php) by [DB-IP](https://db-ip.com) (CC BY 4.0)
* Library: [MaxMind.GeoIP2](https://github.com/maxmind/GeoIP2-dotnet)
