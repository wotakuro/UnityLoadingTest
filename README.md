# このプロジェクトについて
AssetBundleからTextureを読み込むより、Texture2D.LoadRawTextureDataを利用して読み込む場合の方がどの程度早いか計測するためのプロジェクトです。

# フォルダ構成について
* Assets/test.png<br/>
テストに使うTextureになります。こちらのImport設定等を自由に書き換えて実験してください。

* Assets/StreamingAssets/android or Assets/StreamingAssets/ios<br/>
  Android/iOS用の実験データの書き出し先です。<br/>
  メニューの"Tools/LoadTest/CreateData"を呼び出すことでデータが作成されます<br />
  assetbundletest.ab がtest.pngのみを含むアセットバンドル。
  test.data がTexture2DのRawData本体。
  test.headerがTexture2Dの初期化に必要な情報になっています。
