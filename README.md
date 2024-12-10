# misoBOT

# 使用方法

1. 以下のサイトから VB-CABLE のインストーラをダウンロードする<br>
   https://vb-audio.com/Cable/

1. VBCABLE_Setup_x64.exe を実行し、ドライバをインストールする
1. 以下のサイトから日本語の言語モデル"vosk-model-ja-0.22"をダウンロードする<br>
   https://alphacephei.com/vosk/models

1. "vosk-model-ja-0.22"を解凍し、misoBOT フォルダの直下(bin などを同じ階層)に配置する。
1. サウンドの設定より、出力と入力が普段使用しているデバイスであること確認する。
1. 通話アプリの入力を"CABLE Output (VB-Audio Virtual Cable)"に変更する
1. bin\Release\net8.0\misoBOT.exe を起動する。
1. 終了する際は Ctrl+C を押す。

## 実装

### 開発ルール

作業手順は以下の通り

1. イシュー発行
1. ローカルブランチ作成
1. コミット＆プッシュ
1. マージリクエスト
1. マージ＆イシュークローズ
1. 1 に戻る

```
ブランチ名：#${イシュー番号}_${作業内容}
コミットコメント：#${イシュー番号}_${作業内容}
```
