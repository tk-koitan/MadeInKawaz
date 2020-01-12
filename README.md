# MadeInKawaz
これは、Kawaz学生部が新歓用に制作したフレームワーク、及びそれを使用したゲームです。

## Requirements
- Unity 2019.2.8f1

## ミニゲームの仕様
- スマホでも遊べることを考慮して、入力はタッチ操作のみ
- ゲームの長さは、TimeScaleが1の状態で、[Short]が4秒、[Long]が8秒
- １つのミニゲームが終わるたびにTimeScaleが0.1増えます
- アスペクト比は16:9を想定していますが、一応他の比にも対応してます

## ミニゲームの追加方法
1. 新しくシーンを作ります。(一番上の階層に自分用のフォルダを作り、その中で作業してください)
2. 作ったシーンのCameraのCullingMaskの[ManagerScene]のチェックを外してください。
3. ミニゲームを実装してください。(後述の注意事項を確認してください)
4. 成功条件を実装します。

```bash
if(成功条件)
{
  GameManager.Clear();
}
```

※ メニューの[MadeInKawaz/TestPlaySetting]で[めいれいぶん]と[ゲームのながさ]を設定し、[MadeInKawaz/TestPlay]でテストプレイが可能です。

5. ミニゲームが完成したら、フォルダのAssets/Main/Gamesの場所で右クリック→Create/GamePackageでGamePackageを作成、設定してください。

### GamePackage設定項目
- SceneName … シーン名と同じにしてください(シーン読み込み時に使います)
- TitleName … タイトル名
- AuthorName … つくったひと
- DeveloperName … つくった団体
- Statement … めいれいぶん
- Explanation … 説明文
- GameType … ゲームのながさ
- IconImage … アイコン画像
- ScreenshotImage … スクリーンショット画像

6. 作ったGamePackageを同フォルダ内にあるSetAのGamesに追加します。
7. BuildSettingsに作ったシーンを追加します。
8. 完成！ [ごちゃまぜ]や[こべつ]で遊べるか確認しましょう。


## 注意事項
- ゲームオブジェクトのLayerに[ManagerScene]を設定しないでください。CullingMaskについても同様です。
- UIのOrder in Layerを100以上にすると意図しない挙動をする可能性があるので、100未満に設定してください。
- どんどんスピードが速くなるので、始めの難易度は簡単なくらいに調整してください。
- Time.timeScaleをいじらないでください。
- 座標の移動などには、移動量にTime.deltaTimeを掛けてください。
- Qキー、もしくは４本指タッチでデバッグの表示、非表示を切り替えます。

## Contributing
Contributions, issues and feature requests are welcome.

## Author
- Twitter: [@kawaz_student](https://twitter.com/kawaz_student), [@Jg2Bl](https://twitter.com/Jg2Bl)
- Github: [tk-koitan](https://github.com/tk-koitan)

## Show your support
Please STAR this repository if this software helped you!

## License
This software is released under the MIT License.
