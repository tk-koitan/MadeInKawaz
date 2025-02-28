using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TadaLib.Save;
using System.IO;

/// <summary>
/// データをセーブ・ロードするクラス
/// 
/// 以下使いかた
/// 1. namespace TadaLib.Save をusingする
/// 2. セーブしたいデータを持っているクラスの基底クラスをBaseSaverクラスにする
/// 3. 各クラスで
///     if (save_completed_)
///     {
///        save_completed_ = false;
///        SaveManager.Instance.RequestSave(() => { Save(kFileName); save_completed_ = true; });
///     }
///    を呼ぶ   save_completed_は，セーブ申請を多重に送らないようにするため
/// 4. SaveManager.Instance().Save() 関数を適当なタイミングで呼ぶ
/// 9. ロードはこのクラスを通さず，各クラスのLoad関数を呼ぶ
/// 
/// 結構いい設計だと思うんだけどどう？
/// </summary>

namespace TadaLib
{
    namespace Save
    {
        public class SaveManager : MonoBehaviour
        {
            #region field
            // シングルトンにする
            public static SaveManager Instance { private set; get; }

            // 今後セーブ予定のクラス
            private Queue<Action> save_action_queue_;
            #endregion

            private void Awake()
            {
                if (Instance == null)
                {
                    Instance = this;
                    save_action_queue_ = new Queue<Action>();
                    DontDestroyOnLoad(this);
                }
                else Destroy(gameObject);
            }

            // セーブ予定のデータをすべてセーブする
            public void Save()
            {
                if(save_action_queue_.Count >= 1)
                {
                    DebugNotificationGenerator.Notify("セーブしました");
                }

                while (save_action_queue_.Count >= 1)
                {
                    save_action_queue_.Dequeue()();
                }
            }

            // データを全削除する 保存ディレクトリがDataフォルダとは限らないので非常によくない
            public void DeleteAllData()
            {
                string path = Application.persistentDataPath + "/Documents/Data";
                Delete(path);
                Debug.Log("セーブデータを全削除しました");
            }

            // セーブしたいデータを持つクラスのセーブ関数を登録する

            public void RequestSave(Action save_method)
            {
                save_action_queue_.Enqueue(save_method);
            }

            private void Delete(string targetDirectoryPath)
            {
                if (!Directory.Exists(targetDirectoryPath))
                {
                    return;
                }

                //ディレクトリ以外の全ファイルを削除
                string[] filePaths = Directory.GetFiles(targetDirectoryPath);
                foreach (string filePath in filePaths)
                {
                    File.SetAttributes(filePath, FileAttributes.Normal);
                    File.Delete(filePath);
                }

                //ディレクトリの中のディレクトリも再帰的に削除
                string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
                foreach (string directoryPath in directoryPaths)
                {
                    Delete(directoryPath);
                }

                //中が空になったらディレクトリ自身も削除
                Directory.Delete(targetDirectoryPath, false);
            }
        }
    } // namespace Save
} // namespace TadaLib