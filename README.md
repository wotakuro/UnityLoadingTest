# このプロジェクトについて
AssetBundleからTextureを読み込むより、Texture2D.LoadRawTextureDataを利用して読み込む場合の方がどの程度早いか計測するためのプロジェクトです。


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

# 実験した結果
## 実験環境
Unity 5.4.2p4 にてRealseビルドでテスト。<br />
iOS/Android共にIL2CPPを利用。いくつかのテクスチャサイズ・フォーマットを試行。

## 結果のサマリー

## 結果データ＠Android
### 2048×2048 ETC_RGB4(mipmapなし) ファイルサイズ：2048KB
<table>
<tr>  <td></td>
<td  colspan="3">Nexus 7(2013)</td>
<td  colspan="3">Galaxy S6</td>
</tr>
<tr>
  <td></td>
  <td>AssetBundle読み込み(秒)</td>
  <td>RawData読み込み(秒)</td>
  <td>RawDataによる高速化(倍率)</td>
  
  <td>AssetBundle読み込み(秒)</td>
  <td>RawData読み込み(秒)</td>
  <td>RawDataによる高速化(倍率)</td>
</tr>

 <tr>
 <td>一回目</td>
  <!-- -->
  <td>0.04769897</td><td>0.01901245</td><td>2.5088</td>
  <!-- -->
  <td>0.02588987</td><td>0.01347923</td><td>1.9207</td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td>0.04412842</td><td>0.01766968</td><td>2.4974</td>
  <!-- -->
  <td>0.02895355</td><td>0.009040843</td><td>3.2025</td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td>0.04074097</td><td>0.01803589</td><td>2.2588</td>
  <!-- -->
  <td>0.02000427</td><td>0.01112366</td><td>1.7983</td>
 </tr>
</table>

### 2048×2048 RGBA32(mipmapなし) ファイルサイズ：16384KB
<table>
<tr>  <td></td>
<td  colspan="3">Nexus 7(2013)</td>
<td  colspan="3">Galaxy S6</td>
</tr>
<tr>
  <td></td>
  <td>AssetBundle読み込み(秒)</td>
  <td>RawData読み込み(秒)</td>
  <td>RawDataによる高速化(倍率)</td>
  
  <td>AssetBundle読み込み(秒)</td>
  <td>RawData読み込み(秒)</td>
  <td>RawDataによる高速化(倍率)</td>
</tr>

 <tr>
 <td>一回目</td>
  <!-- -->
  <td>0.4385681</td><td>0.2448425</td><td>1.7912</td>
  <!-- -->
  <td>0.1509905</td><td>0.08346653</td><td>1.8089</td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td>0.4238892</td><td>0.2460327</td><td>1.7228</td>
  <!-- -->
  <td>0.1455116</td><td>0.08004379</td><td>1.8178</td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td>0.4017029</td><td>0.2455139</td><td>1.6361</td>
  <!-- -->
  <td>0.1482849</td><td>0.06974792</td><td>2.1260</td>
 </tr>
</table>

### 2048×2048 RGBA32(mipmapあり) ファイルサイズ：21845KB
<table>
<tr>  <td></td>
<td  colspan="3">Nexus 7(2013)</td>
<td  colspan="3">Galaxy S6</td>
</tr>
<tr>
  <td></td>
  <td>AssetBundle読み込み(秒)</td>
  <td>RawData読み込み(秒)</td>
  <td>RawDataによる高速化(倍率)</td>
  
  <td>AssetBundle読み込み(秒)</td>
  <td>RawData読み込み(秒)</td>
  <td>RawDataによる高速化(倍率)</td>
</tr>

 <tr>
 <td>一回目</td>
  <!-- -->
  <td>0.6249084</td><td>0.4425659</td><td>1.4120</td>
  <!-- -->
  <td>0.218586</td><td>0.1531868</td><td>1.4269</td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td>0.5959473</td><td>0.4623108</td><td>1.2890</td>
  <!-- -->
  <td>0.1708069</td><td>0.1181488</td><td>1.4456</td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td>0.5961914</td><td>0.4640503</td><td>1.2847</td>
  <!-- -->
  <td>0.1790771</td><td>0.1390076</td><td>1.2882</td>
 </tr>
</table>

