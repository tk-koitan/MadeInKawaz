namespace SceneLauncher
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEditor.SceneManagement;
    using System.Linq;

    public class SceneLaunchWindow : EditorWindow
    {
        private SceneAsset[] sceneArray;
        private Vector2 scrollPos = Vector2.zero;

        [MenuItem("MadeInKawaz/Scene Launcher")]
        static void Open()
        {
            GetWindow<SceneLaunchWindow>("SceneLauncher");
        }

        void OnFocus()
        {
            this.ReloadScenes();
        }

        void OnGUI()
        {
            GUILayout.Label("※ボタンを押すと現在のシーンの変更が保存されます");

            if (this.sceneArray == null) { this.ReloadScenes(); }

            if (this.sceneArray.Length == 0)
            {
                EditorGUILayout.LabelField("シーンファイル(.unity)が存在しません");
                return;
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
            this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (var scene in scenes)
            {
                string[] strs = scene.path.Split('/');

                string sceneName = strs[strs.Length - 1].Replace(".unity", string.Empty);
                if (GUILayout.Button(sceneName))
                {
                    EditorApplication.SaveScene();//危険かも
                    EditorSceneManager.OpenScene(scene.path);
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// シーン一覧の再読み込み
        /// </summary>
        void ReloadScenes()
        {
            this.sceneArray = GetAllSceneAssets().ToArray();
        }

        /// <summary>
        /// プロジェクト内に存在するすべてのシーンファイルを取得する
        /// </summary>
        static IEnumerable<SceneAsset> GetAllSceneAssets()
        {
            return AssetDatabase.FindAssets("t:SceneAsset")
           .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
           .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)))
           .Where(obj => obj != null)
           .Select(obj => (SceneAsset)obj);
        }
    }
}