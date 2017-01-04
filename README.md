# このプロジェクトについて
AssetBundleからTextureを読み込むより、Texture2D.LoadRawTextureDataを利用して読み込む場合の方がどの程度早いか計測するためのプロジェクトです。

# 実験結果のサマリー

# このプロジェクトの利用の仕方について

## まず初めに…
SwitchPlatformで、Android もしくはiOSへと切り替えてください。<br/>
その後、メニューの"Tools/LoadTest/CreateData"を呼び出して実験用のデータを作成してください。<br/>

## 実行時の見方について
![Alt text](/doc/img/About.png)<br/>
1)実験対象となるテクスチャのフォーマットやサイズ情報です。<br/>
2)読み込み時間の結果を表示する箇所です。"3)Speed Test"ボタンを押すことで結果が表示されます。<br/>
  Ab:XXXXがAssetBundleからTextureを読み込むのにかかった時間。Raw:XXXXがテクスチャをRawDataから読み込むのにかかった時間<br/>
3)読み込みテストを開始するボタンです。<br/>
4)AssetBundleやRawDataからの読み込みがきちんとできているか確認するボタンです。結果は"5)のエリア"に表示されます<br/>
5)画像が正しく読み込めたか確認するためのフィールドです。<br/>


# フォルダ構成について
* Assets/test.png<br/>
テストに使うTextureになります。こちらのImport設定等を自由に書き換えて実験してください。

* Assets/StreamingAssets/android or Assets/StreamingAssets/ios<br/>
  Android/iOS用の実験データの書き出し先です。<br/>
  メニューの"Tools/LoadTest/CreateData"を呼び出すことでデータが作成されます<br />
  assetbundletest.ab がtest.pngのみを含むアセットバンドル。
  test.data がTexture2DのRawData本体。
  test.headerがTexture2Dの初期化に必要な情報になっています。
