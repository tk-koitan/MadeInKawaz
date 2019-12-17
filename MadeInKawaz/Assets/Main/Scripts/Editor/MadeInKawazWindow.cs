using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

/// <summary>
/// Editorスクリプト
/// </summary>
public class MadeInKawaz : EditorWindow
{
    // 設定用のScriptableObject
    public static GamePackage setting;
    private static float timeScale = 1;

    [MenuItem("MadeInKawaz/TestPlaySetting")]
    public static void Open()
    {
        GetWindow<MadeInKawaz>();

        LoadSetting();

        timeScale = EditorPrefs.GetFloat("timeScale", 1);
    }

    private void OnGUI()
    {
        GUILayout.Label("テストプレイ設定");

        // ここから変更を検知
        EditorGUI.BeginChangeCheck();

        // とりあえず入力値を仮の変数に入れる
        var statement = EditorGUILayout.TextField("めいれいぶん", setting.statement);
        var gameType = (GameType)EditorGUILayout.EnumPopup("ゲームのながさ", setting.gameType);
        timeScale = EditorGUILayout.FloatField("時間スケール", timeScale);

        if (EditorGUI.EndChangeCheck())
        { // 変更を検知した場合、設定ファイルに戻す
            UnityEditor.Undo.RecordObject(setting, "Edit MadeInKawazSettingWindow"); // こうするとRedo/Undoが簡単に実装
            setting.statement = statement;
            setting.gameType = gameType;
            EditorPrefs.SetFloat("timeScale", timeScale);
            EditorUtility.SetDirty(setting); // Dirtyフラグを立てることで、Unity終了時に勝手に.assetに書き出す
        }
    }

    private void Update()
    {
        Repaint(); // Undo.RecordObjectを使うときは入れたほうが更新描画が正しく動く
    }

    [MenuItem("MadeInKawaz/TestPlay")]
    private static void TestPlay()
    {
        LoadSetting();
        EditorPrefs.SetBool("testPlayFlag", true);
        EditorApplication.isPlaying = true;                
    }

    private static void LoadSetting()
    {
        // 設定を保存(読み込み)するパス ScriptableObjectは.assetを付けないと正しく読んでくれません
        var path = "Assets/Main/Games/TestPlayPackage.asset";
        setting = AssetDatabase.LoadAssetAtPath<GamePackage>(path);

        if (setting == null)
        { // ロードしてnullだったら存在しないので生成
            setting = ScriptableObject.CreateInstance<GamePackage>(); // ScriptableObjectはnewではなくCreateInstanceを使います
            AssetDatabase.CreateAsset(setting, path);
        }

        string[] strs = EditorSceneManager.GetActiveScene().path.Split('/');

        setting.sceneName = strs[strs.Length - 1].Replace(".unity", string.Empty);
    }
}