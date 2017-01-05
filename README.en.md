# About this Project
Read this in other languages: [English](README.en.md), [日本語](README.ja.md)<br />

Test Project for "Loading Texture2D from AssetBundle" VS "Loading Texture2D by Texture2D.LoadRawTextureData".<br />
Loading code is like below.

    /** Load Texture from RawData */
    byte[] bin = ReadTextureRawData();
    // Profile from here
    Texture2D newTex = new Texture2D(2048, 2048, TextureFormat.ETC2_RGB, false);
    newTex.LoadRawTextureData(bin);
    newTex.Apply();

    /** Load Texture from uncompressed AssetBundle */
    byte[] bin = ReadUncompressedAssetBundleData();
    // Profile from here
    AssetBundle assetBundle = AssetBundle.LoadFromMemory(bin);
    var texture = assetBundle.Load<Texture2D>("testTexture");  
    
<br/>
The loading time is depends on the size of file or device.<br />
In general situation,  Load Texture from RawData is around x2 faster than Loading from AssetBundles.<br/>
If the size of AssetBundle is bigger , the effect will be lower.

# How to build this project

## 1st of all
Switch the platform to Android or iOS.<br/>
Call "Tools/LoadTest/CreateData" from menu , then test data will be created.<br/>

## In Application
![Alt text](/doc/img/About.png)<br/>
1)Information about test data<br/>
2)The loading time<br/>
  "Ab:XXXX" is the time to load Texture2D from  AssetBundle<br />
  "Raw:XXXX" is the time to load Texture2D from RawData<br/>
3)The button to start profiling the loading time.<br/>
4)The button to make sure that test data is not broken.The result will be in the area "5)" <br/>
5)you can make sure the texture isn't broken.<br/>


# Files in this project.
* Assets/test.png<br/>
This is the texture for test.You can edit the ImportSettings.

* Assets/StreamingAssets/android or Assets/StreamingAssets/ios<br/>
  AssetBundle or Texture RawData will be here.<br/>
  If you call "Tools/LoadTest/CreateData" from menu, the data will be created.<br />
  "assetbundletest.ab" : assetbundle which has only "test.png"
  "test.data"  RawData of Texture2D.
  "test.header" Header data for creating texture2D(width,height,format,mipmap...)

# The result of test
## Test enviroment.
use Unity 5.4.2p4<br />
Realse Build<br />
Script backEngine is IL2CPP( iOS/Android )<br />
Tried some texture format and size.


## Result(Android)
### 32×32 ETC_RGB4(mipmap:disable) Filesize：0.5KB
<table>
<tr>  <td></td>
<td  colspan="3">Nexus 7(2013)</td>
<td  colspan="3">Galaxy S6</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.002746582</td><td>0.000915273</td><td>3.0008</td>
  <!-- -->
  <td>0.002131939</td><td>0.0006346703</td><td>3.3591</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.002288818</td><td>0.000579834</td><td>3.9473</td>
  <!-- -->
  <td>0.002288818</td><td>0.0002727509</td><td>8.3916</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.003173828</td><td>0.000579834</td><td>5.4736</td>
  <!-- -->
  <td>0.002044678</td><td>0.0003051758</td><td>6.7000</td>
 </tr>

 <tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.0027364093333333</td><td>0.000691647</td><td>3.9563</td>
  <!-- -->
  <td>0.002155145</td><td>0.000404199</td><td>5.3318</td>
 </tr>

</table>



### 2048×2048 ETC_RGB4(mipmap:disable) Filesize：2048KB
<table>
<tr>  <td></td>
<td  colspan="3">Nexus 7(2013)</td>
<td  colspan="3">Galaxy S6</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.04769897</td><td>0.01901245</td><td>2.5088</td>
  <!-- -->
  <td>0.02588987</td><td>0.01347923</td><td>1.9207</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.04412842</td><td>0.01766968</td><td>2.4974</td>
  <!-- -->
  <td>0.02895355</td><td>0.009040843</td><td>3.2025</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.04074097</td><td>0.01803589</td><td>2.2588</td>
  <!-- -->
  <td>0.02000427</td><td>0.01112366</td><td>1.7983</td>
 </tr>
 <tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.0441894533333333</td><td>0.01823934</td><td>2.4227</td>
  <!-- -->
  <td>0.02494923</td><td>0.0112145776666667</td><td>2.2247</td>
 </tr>
</table>

### 2048×2048 RGBA32(mipmap:disable) Filesize：16384KB
<table>
<tr>  <td></td>
<td  colspan="3">Nexus 7(2013)</td>
<td  colspan="3">Galaxy S6</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.4385681</td><td>0.2448425</td><td>1.7912</td>
  <!-- -->
  <td>0.1509905</td><td>0.08346653</td><td>1.8089</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.4238892</td><td>0.2460327</td><td>1.7228</td>
  <!-- -->
  <td>0.1455116</td><td>0.08004379</td><td>1.8178</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.4017029</td><td>0.2455139</td><td>1.6361</td>
  <!-- -->
  <td>0.1482849</td><td>0.06974792</td><td>2.1260</td>
 </tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.4213867333333333</td><td>0.2454630333333333</td><td>1.7167</td>
  <!-- -->
  <td>0.1482623333333333</td><td>0.0777527466666667</td><td>1.9068</td>
 </tr>
</table>

### 2048×2048 RGBA32(mipmap:enable) Filesize：21845KB
<table>
<tr>  <td></td>
<td  colspan="3">Nexus 7(2013)</td>
<td  colspan="3">Galaxy S6</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.6249084</td><td>0.4425659</td><td>1.4120</td>
  <!-- -->
  <td>0.218586</td><td>0.1531868</td><td>1.4269</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.5959473</td><td>0.4623108</td><td>1.2890</td>
  <!-- -->
  <td>0.1708069</td><td>0.1181488</td><td>1.4456</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.5961914</td><td>0.4640503</td><td>1.2847</td>
  <!-- -->
  <td>0.1790771</td><td>0.1390076</td><td>1.2882</td>
 </tr>
 <tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.6056823666666667</td><td>0.456309</td><td>1.3273</td>
  <!-- -->
  <td>0.18949</td><td>0.1367810666666667</td><td>1.3853</td>
 </tr>
 
</table>


## Result(iOS)
### 32×32 PVRTC_RGB4(mipmap:disable) Filesize：0.5KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.003101826</td><td>0.0008974075</td><td>3.4564</td>
  <!-- -->
  <td>0.002513885</td><td>0.00008583069</td><td>29.2888</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.00340271</td><td>0.0009498596</td><td>3.5823</td>
  <!-- -->
  <td>0.00252533</td><td>0.00008010864</td><td>31.5238</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.003250122</td><td>0.0007858276</td><td>4.1359</td>
  <!-- -->
  <td>0.002304077</td><td>0.000114409</td><td>20.139</td>
 </tr>
 
  <tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.0032515526666667</td><td>0.00087769823333333</td><td>3.70463</td>
  <!-- -->
  <td>0.002447764</td><td>0.00009344</td><td>26.1961</td>
 </tr>

</table>

### 2048×2048 PVRTC_RGB4(mipmap:disable) Filesize：2048KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.03134537</td><td>0.01697922</td><td>1.8461</td>
  <!-- -->
  <td>0.01776981</td><td>0.009437561</td><td>1.8828</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.03353882</td><td>0.01564789</td><td>2.1433</td>
  <!-- -->
  <td>0.01673889</td><td>0.01058197</td><td>1.5818</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.03235626</td><td>0.01696014</td><td>1.9077</td>
  <!-- -->
  <td>0.01607513</td><td>0.009803772</td><td>1.6396</td>
 </tr>
 <tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.0324134833333333</td><td>0.0165290833333333</td><td>1.9060</td>
  <!-- -->
  <td>0.0168612766666667</td><td>0.009941101</td><td>1.696</td>
 </tr>
</table>


### 2048×2048 RGBA32(mipmap:disable) Filesize：16384KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.2029037</td><td>0.138443</td><td>1.4656</td>
  <!-- -->
  <td>0.04005241</td><td>0.02583313</td><td>1.5504</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.2051926</td><td>0.1414261</td><td>1.4508</td>
  <!-- -->
  <td>0.03764153</td><td>0.02333069</td><td>1.6133</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.1964493</td><td>0.1325378</td><td>1.4822</td>
  <!-- -->
  <td>0.03736115</td><td>0.02357101</td><td>1.5850</td>
 </tr>
 <tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.2015152</td><td>0.1374689666666667</td><td>1.4658</td>
  <!-- -->
  <td>0.0383516966666667</td><td>0.0242449433333333</td><td>1.58184</td>
 </tr>
</table>

### 2048×2048 RGBA32(mipmap:enable) Filesize：21845KB
<table>
<tr>  <td></td>
<td  colspan="3">iPhone5c</td>
<td  colspan="3">iPhone6S</td>
</tr>
<tr>
  <td></td>
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
  
  <td>Load from AssetBundle(sec)</td>
  <td>Load from RawData(sec)</td>
  <td>The effect of "RawData"(rate)</td>
</tr>

 <tr>
 <td>1st</td>
  <!-- -->
  <td>0.3412313</td><td>0.2151585</td><td>1.5859</td>
  <!-- -->
  <td>0.06794071</td><td>0.04671097</td><td>1.4544</td>
 </tr>
 <tr>
 <td>2nd</td>
  <!-- -->
  <td>0.3287811</td><td>0.2084236</td><td>1.5774</td>
  <!-- -->
  <td>0.06585503</td><td>0.05086327</td><td>1.2947</td>
 </tr>
 <tr>
 <td>3rd</td>
  <!-- -->
  <td>0.3230667</td><td>0.2092361</td><td>1.5440</td>
  <!-- -->
  <td>0.06612396</td><td>0.05685425</td><td>1.1630</td>
 </tr>
 <tr>
 <td>Ave.</td>
  <!-- -->
  <td>0.3310263666666667</td><td>0.2109393666666667</td><td>1.5692</td>
  <!-- -->
  <td>0.0666399</td><td>0.0514761633333333</td><td>1.2945</td>
 </tr>
</table>


# loading time by devices

<table>
<tr>  <td></td>
<td>Nexus7 2013</td><td>Galaxy S6</td>
<td>iPhone5c</td><td>iPhone6S</td>
</tr>
<tr>
<td>Texture2D.LoadRawData<br/>2048×2048 RGBA32(mipmap:enable)<br/>Filesize：16384KB</td>
<td> 0.2454630 sec</td><td>0.0777527 sec</td>
<td> 0.1374689 sec</td><td>0.0242449 sec</td>
</tr>
<tr>
<td>AssetBundle<br/>2048×2048 RGBA32(mipmap:disable)<br/>FileSize：16384KB</td>
<td>0.4213867 sec</td><td>0.1482623 sec</td>
<td>0.2015152 sec</td><td>0.0383516 sec</td>
</tr>



<tr>
<td>Texture2D.LoadRawData<br/>2048×2048 RGBA32(mipmap:enable)<br/>Filesize：21845KB</td>
<td> 0.456309 sec</td><td>0.1367810 sec</td>
<td> 0.2109393 sec</td><td>0.0514761 sec</td>
</tr>
<tr>
<td>AssetBundle<br/>2048×2048 RGBA32(mipmap:disable)<br/>Filesize：16384KB</td>
<td>0.6056823 sec</td><td>0.18949 sec</td>
<td>0.3310263 sec</td><td>0.066639 sec</td>
</tr>

</table>
