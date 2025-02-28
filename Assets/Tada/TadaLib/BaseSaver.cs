using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Security.Cryptography;

/// <summary>
/// ローカルファイルにJson形式のファイルを保存，ロードする基底クラス
/// </summary>

namespace TadaLib
{
    namespace Save
    {
        abstract public class BaseSaver<T> where T : BaseSaver<T>, new()
        {
            protected static T instance;
            // 最初にロードすることでインスタンスを生成するための制御
            private bool isLoaded = true;
            // セーブ中であるかどうか
            protected bool isSaving = false;

            // 保存するディレクトリ
            protected static string DIRECTORY_NAME = "Documents/Data";

            // セーブ申請を出してからセーブが完了されたかどうか 派生クラスで使う
            protected bool save_completed_ = true;

            // ロード後の操作
            protected virtual void SetLoadedStutas()
            {

            }

            // ロードする
            public T Load(string fileName)
            {
                //if (null == instance)
                {
                    string json = "";
                    if (File.Exists(GetFilePath(fileName)))
                    {
                        string tmp_json = File.ReadAllText(GetFilePath(fileName));
                        // 復号化
                        string jaming = GetSaveKey();
                        int cnt = -1;
                        int n = jaming.Length;
                        for (int i = 0; i < tmp_json.Length; ++i)
                        {
                            json += (char)((int)tmp_json[i] - (int)jaming[++cnt % n]);
                        }
                    }
                    if (json.Length > 0) LoadFromJSON(json);
                    else
                    {
                        //instance = new T();
                        //instance.isLoaded = true;
                        //instance.SetLoadedStutas();
                    }
                }
                return instance;
            }

            // セーブする
            public void Save(string fileName)
            {
                if (isLoaded)
                {
                    isSaving = true;
                    // 暗号化する 元のセーブデータにある文字列をASCIIコードで足し合わせる 通常127までだからオーバーフローしないよね？
                    string path = GetFilePath(fileName);
                    string json = JsonUtility.ToJson(this);
                    string jaming = GetSaveKey();
                    int cnt = -1;
                    int n = jaming.Length;
                    string new_json = "";
                    for(int i = 0; i < json.Length; ++i)
                    {
                        new_json += (char)((int)json[i] + (int)jaming[++cnt % n]);
                    }
                    File.WriteAllText(path, new_json);

                    Debug.Log(path + "にセーブしました");

                    // IOSの場合にデータをiCloudにバックアップさせないようにもしとく
#if UNITY_IOS
            UnityEngine.iOS.Device.SetNoBackupFlag(path);
#endif
                    isSaving = false;
                }
            }

            // リセットする
            public void Reset()
            {
                instance = null;
            }

            // ファイルを削除する
            public void Clear(string fileName)
            {
                if (File.Exists(GetFilePath(fileName))) File.Delete(GetFilePath(fileName));
                instance = null;
            }

            // JsonUnitilityを用いてインスタンスを取得
            public static void LoadFromJSON(string json)
            {
                try
                {
                    instance = JsonUtility.FromJson<T>(json);
                    instance.isLoaded = true;
                    instance.SetLoadedStutas();
                    Debug.Log(json + "を読み込み");
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                }
            }

            // パスを読み取る
            static string GetFilePath(string fileName)
            {
                string directoryPath = Application.persistentDataPath + "/" + DIRECTORY_NAME;
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                return directoryPath + "/" + fileName;
            }

            // 暗号化する まだ未使用
            static string GetSaveKey()
            {
                var provider = new SHA1CryptoServiceProvider();
                var hash = provider.ComputeHash(System.Text.Encoding.ASCII.GetBytes(typeof(T).FullName));
                return BitConverter.ToString(hash);
            }
        }
    } // namespace Save
} // namespace TadaLib