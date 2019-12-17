using UnityEngine;
using System.Collections;
using UnityEditor;  //!< UnityEditorを使うよ！

public class SampleWindow : EditorWindow    //!< EditorWindowを継承してね！
{
    [SerializeField]
    public static string statement;
    [SerializeField]
    private string m_statement { get; set; }
    [SerializeField]
    public static GameType gameType;

    //! MenuItem("メニュー名/項目名") のフォーマットで記載してね
    [MenuItem("Custom/SampleWindow")]
    static void ShowWindow()
    {
        // ウィンドウを表示！
        EditorWindow.GetWindow<SampleWindow>();
    }

    /**
     * ウィンドウの中身
     */
    void OnGUI()
    {
        m_statement = EditorGUILayout.TextField("めいれいぶん", m_statement);

        gameType = (GameType)EditorGUILayout.EnumPopup("あいうえお", gameType);        
    }

    [MenuItem("Custom/Play")]
    private static void Play()
    {
        EditorApplication.isPlaying = true;        
        statement = EditorPrefs.GetString("statement", "めいれいぶん");
        Debug.Log(statement);
    }

    [MenuItem("Custom/Save")]
    private static void Save()
    {
        EditorPrefs.SetString("statement", statement);
    }

    private void OnValidate()
    {
        Debug.Log("save");
        EditorUtility.SetDirty(this);
    }
}