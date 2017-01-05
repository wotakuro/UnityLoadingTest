# このプロジェクトについて
AssetBundleからTextureを読み込むより、Texture2D.LoadRawTextureDataを利用して読み込む場合の方がどの程度早いか計測するためのプロジェクトです。<br />
どちらも byte[]からの読み込みで実験しています。

    /** Textureを生で読み込む場合 */
    byte[] bin = ReadTextureRawData();
    // ここから計測
    Texture2D newTex = new Texture2D(2048, 2048, TextureFormat.ETC2_RGB, false);
    newTex.LoadRawTextureData(bin);
    newTex.Apply();

    /** Textureを無圧縮のAssetBundleから読み込む場合 */
    byte[] bin = ReadUncompressedAssetBundleData();
    // ここから計測
    AssetBundle assetBundle = AssetBundle.LoadFromMemory(bin);
    var texture = assetBundle.Load<Texture2D>("testTexture");  
    
<br/>
AssetBundleのサイズや端末スペックによって効果が大きく変動してしまいますが、目安としては倍近く早くなりそうな結果になりました。<br/>
AssetBundleサイズが大きいほど、テクスチャのRawData読み込みによる高速化は薄れていきます。

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


## 結果データ＠Android
### 32×32 ETC_RGB4(mipmapなし) ファイルサイズ：0.5KB
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
  <td>0.002746582</td><td>0.000915273</td><td>3.0008</td>
  <!-- -->
  <td>0.002131939</td><td>0.0006346703</td><td>3.3591</td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td>0.002288818</td><td>0.000579834</td><td>3.9473</td>
  <!-- -->
  <td>0.002288818</td><td>0.0002727509</td><td>8.3916</td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td>0.003173828</td><td>0.000579834</td><td>5.4736</td>
  <!-- -->
  <td>0.002044678</td><td>0.0003051758</td><td>6.7000</td>
 </tr>

 <tr>
 <td>平均</td>
  <!-- -->
  <td>0.0027364093333333</td><td>0.000691647</td><td>3.9563</td>
  <!-- -->
  <td>0.002155145</td><td>0.000404199</td><td>5.3318</td>
 </tr>

</table>



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
 <tr>
 <td>平均</td>
  <!-- -->
  <td>0.0441894533333333</td><td>0.01823934</td><td>2.4227</td>
  <!-- -->
  <td>0.02494923</td><td>0.0112145776666667</td><td>2.2247</td>
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
 <td>平均</td>
  <!-- -->
  <td>0.4213867333333333</td><td>0.2454630333333333</td><td>1.7167</td>
  <!-- -->
  <td>0.1482623333333333</td><td>0.0777527466666667</td><td>1.9068</td>
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
 <tr>
 <td>平均</td>
  <!-- -->
  <td>0.6056823666666667</td><td>0.456309</td><td>1.3273</td>
  <!-- -->
  <td>0.18949</td><td>0.1367810666666667</td><td>1.3853</td>
 </tr>
 
</table>


## 結果データ＠iOS
### 32×32 PVRTC_RGB4(mipmapなし) ファイルサイズ：0.5KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
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
  <td>0.003101826</td><td>0.0008974075</td><td>3.4564</td>
  <!-- -->
  <td>0.002513885</td><td>0.00008583069</td><td>29.2888</td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td>0.00340271</td><td>0.0009498596</td><td>3.5823</td>
  <!-- -->
  <td>0.00252533</td><td>0.00008010864</td><td>31.5238</td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td>0.003250122</td><td>0.0007858276</td><td>4.1359</td>
  <!-- -->
  <td>0.002304077</td><td>0.000114409</td><td>20.139</td>
 </tr>
 
  <tr>
 <td>平均</td>
  <!-- -->
  <td>0.0032515526666667</td><td>0.00087769823333333</td><td>3.70463</td>
  <!-- -->
  <td>0.002447764</td><td>0.00009344</td><td>26.1961</td>
 </tr>

</table>

### 2048×2048 PVRTC_RGB4(mipmapなし) ファイルサイズ：2048KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
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
  <td>0.03134537</td><td>0.01697922</td><td>1.8461</td>
  <!-- -->
  <td>0.01776981</td><td>0.009437561</td><td>1.8828</td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td>0.03353882</td><td>0.01564789</td><td>2.1433</td>
  <!-- -->
  <td>0.01673889</td><td>0.01058197</td><td>1.5818</td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td>0.03235626</td><td>0.01696014</td><td>1.9077</td>
  <!-- -->
  <td>0.01607513</td><td>0.009803772</td><td>1.6396</td>
 </tr>
 <tr>
 <td>平均</td>
  <!-- -->
  <td>0.0324134833333333</td><td>0.0165290833333333</td><td>1.9060</td>
  <!-- -->
  <td>0.0168612766666667</td><td>0.009941101</td><td>1.696</td>
 </tr>
</table>


### 2048×2048 RGBA32(mipmapなし) ファイルサイズ：16384KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
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
  <td></td><td></td><td></td>
  <!-- -->
  <td></td><td></td><td></td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td></td><td></td><td></td>
  <!-- -->
  <td></td><td></td><td></td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td></td><td></td><td></td>
  <!-- -->
  <td></td><td></td><td></td>
 </tr>
 <tr>
 <td>平均</td>
  <!-- -->
  <td></td><td></td><td></td>
  <!-- -->
  <td></td><td></td><td></td>
 </tr>
</table>

### 2048×2048 RGBA32(mipmapあり) ファイルサイズ：21845KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
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
  <td>0.2029037</td><td>0.138443</td><td>1.4656</td>
  <!-- -->
  <td>0.06794071</td><td>0.04671097</td><td>1.4544</td>
 </tr>
 <tr>
 <td>二回目</td>
  <!-- -->
  <td>0.2051926</td><td>0.1414261</td><td>1.4508</td>
  <!-- -->
  <td>0.06585503</td><td>0.05086327</td><td>1.2947</td>
 </tr>
 <tr>
 <td>三回目</td>
  <!-- -->
  <td>0.1964493</td><td>0.1325378</td><td>1.4822</td>
  <!-- -->
  <td>0.06612396</td><td>0.05685425</td><td>1.1630</td>
 </tr>
 <tr>
 <td>平均</td>
  <!-- -->
  <td>0.2015152</td><td>0.1374689666666667</td><td>1.4658</td>
  <!-- -->
  <td>0.0666399</td><td>0.0514761633333333</td><td>1.2945</td>
 </tr>
</table>



