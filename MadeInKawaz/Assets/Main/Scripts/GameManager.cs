using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool ClearFlag { get; private set; }
    public static Queue<Action> ClearActionQueue = new Queue<Action>();
    public static float timeScale = 1;
    [SerializeField]
    private TextMeshProUGUI statementMesh;
    [SerializeField]
    private TextMeshProUGUI highScoreMesh;
    [SerializeField]
    private string sceneName;
    private AsyncOperation async = new AsyncOperation();
    private Scene gameScene;
    [SerializeField]
    private RawImage transitionImage;
    [SerializeField]
    private Mask trasitionMask;
    [SerializeField]
    private RawImage screenImage;
    [SerializeField]
    private Camera renderCamera;
    [SerializeField]
    private Camera managerCammera;
    [SerializeField]
    private TextMeshPro numberMesh;
    public int number { get; private set; }
    [SerializeField]
    private GamePackageSet gamePackageSet;
    private GamePackage currentGame;
    private GameType currentGameType;
    private bool isTestPlay = false;
    private bool isPause;
    private bool isCanPause;
    public static PlayMode mode { get; set; } = PlayMode.None;
    public GamePackage singleGame;
    [SerializeField]
    private Animator backgrondAnim;
    [SerializeField]
    private Animator[] coinAnims;
    [SerializeField]
    private GameObject pauseUi;
    private int restNum;
    private bool FinishFlag;
    public static bool IsGamePlaying { get; private set; }
    private static Queue<int> gameQueue = new Queue<int>();
    public Texture2D tex;
    [SerializeField]
    private int speedUpIntervalCount = 1;
    private int intervalCount = 0;

    private void Awake()
    {
        //シングルトンはAwake内で呼び出し、それ以外はStartで
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

#if UNITY_EDITOR
        DebugNotificationGenerator.Notify("Qキーでデバッグ表示ON/OFF");
#else
        DebugNotificationGenerator.Notify("４本マルチタッチでデバッグ表示ON/OFF");
#endif

#if UNITY_EDITOR
        isTestPlay = EditorPrefs.GetBool("testPlayFlag", false);
        if (isTestPlay)
        {
            Debug.Log("これはテストプレイです😀");
            EditorPrefs.SetBool("testPlayFlag", false);
            mode = PlayMode.Debug;
            var path = "Assets/Main/Games/TestPlayPackage.asset";
            singleGame = AssetDatabase.LoadAssetAtPath<GamePackage>(path);
            Time.timeScale = EditorPrefs.GetFloat("timeScale", 1);
            //Initialization();
        }
#endif

        //Debug.Log(SceneManager.GetActiveScene().buildIndex);

        /*
        if (mode != PlayMode.None)
        {
            Initialization();
        }
        */

        //やばい実装、後で直す
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            mode = PlayMode.Normal;
        }
        else
        {
            //IsGamePlaying = true;
            //mode = PlayMode.None;            
        }

        if (mode != PlayMode.None)
        {
            Initialization();
        }

        statementMesh.gameObject.SetActive(false);

        //デバッグ用
        //EndGame();
        DebugTextManager.Display(() => "TimeScale:" + Time.timeScale + "\n");
        //DebugTextManager.Display(() => "progress:" + async.progress + "\n");
        //DebugTextManager.Display(() => "IsGamePlaying:" + IsGamePlaying.ToString() + "\n");
        DebugTextManager.Display(() => "mode:" + mode.ToString() + "\n");
        DebugTextManager.Display(() => "gameType:" + currentGameType.ToString() + "\n");
        //DebugTextManager.Display(() => "Loading:" + (async.progress * 100).ToString("D3") + "%\n");        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isCanPause && !isPause && mode != PlayMode.None)
        {
            Pause();
            MusicManager.Stop();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale += 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale -= 0.1f;
        }

        if (ClearFlag)
        {
            while (ClearActionQueue.Count > 0)
            {
                ClearActionQueue.Dequeue()();
            }
        }
    }

    [ContextMenu("Initialization")]
    public void Initialization()
    {
        if (!isTestPlay)
        {
            Time.timeScale = 1;
        }
        number = 1;
        restNum = 4;
        numberMesh.text = number.ToString();
        numberMesh.gameObject.SetActive(false);
        gameQueue.Clear();
        ClearFlag = false;
        numberMesh.transform.localScale = Vector3.one;
        highScoreMesh.text = "ハイスコア：" + System.String.Format("{0, 3}", ScoreManager.Instance.GetScoreData().Scores[0].ToString());
        if (isTestPlay)
        {
            currentGame = LoadGamePackage();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("ManagerScene"));
            SceneManager.UnloadSceneAsync(currentGame.sceneName);
        }
        currentGame = LoadGamePackage();

        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {
                FinishFlag = false;
                MusicManager.Play(BgmCode.Start);
                backgrondAnim.Play("Start", 0, 0);
                for (int i = 0; i < restNum; i++)
                {
                    coinAnims[i].gameObject.SetActive(true);
                    coinAnims[i].Play("CoinStart", 0, 0);
                    coinAnims[i].speed = 0;
                }
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                for (int i = 0; i < restNum; i++)
                {
                    coinAnims[i].speed = 1;
                }
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                MusicManager.Play(BgmCode.Ready);
                backgrondAnim.Play("Ready");
                foreach (Animator anim in coinAnims)
                {
                    anim.Play("CoinReady");
                }
                numberMesh.gameObject.SetActive(true);

                /*
                //次のシーン読み込み
                currentGame = LoadGamePackage();
                gameScene = SceneManager.GetSceneByName(sceneName);
                if (!gameScene.IsValid())
                {
                    async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    async.allowSceneActivation = false;
                }
                */

                ClearActionQueue.Clear();
                isCanPause = true;
            })
            .AppendCallback(() => StartGame());
    }

    [ContextMenu("StartGame")]
    private void StartGame()
    {
        //ShowStatement();        
        /*
        gameScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(gameScene);
        */
        //ゲームの時間を決める
        float waitTime = 4f;
        if (currentGameType == GameType.Short)
        {
            waitTime = 4f;
        }
        else if (currentGameType == GameType.Long)
        {
            waitTime = 8f;
        }

        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {
                /*
                number++;
                numberMesh.text = number.ToString();
                */
            })
            .Append(numberMesh.transform.DOScale(0.5f, 0.25f).SetRelative())
            .AppendInterval(0.25f)
            .Append(numberMesh.transform.DOScale(-0.5f, 0.25f).SetRelative())
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                ShowStatement();
                isCanPause = false;
                //シーンを非同期で読み込み
                gameScene = SceneManager.GetSceneByName(sceneName);
                if (!gameScene.IsValid())
                {
                    async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    async.allowSceneActivation = false;
                    //DebugTextManager.Display(() => "Loading:" + (async.progress * 100).ToString() + "%\n");
                }
            })
            .AppendInterval(0.15f)
            .AppendCallback(() =>
            {
                MusicManager.FadeOut(0.1f);
            })
            .AppendInterval(0.1f)
            .AppendCallback(() =>
            {
                TimeCountManager.CountDownStart(waitTime);
                CameraZoomUp();
                Instance.async.allowSceneActivation = true;
                IsGamePlaying = true;
            })
            .AppendInterval(waitTime)
            .OnComplete(() =>
            {
                EndGame();
            });
    }

    [ContextMenu("EndGame")]
    private void EndGame()
    {
        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {
                ClearActionQueue.Clear();
                CameraZoomOut();
                IsGamePlaying = false;
                MusicManager.audioSource.volume = 1f;
                if (ClearFlag)
                {
                    MusicManager.Play(BgmCode.Success);
                    backgrondAnim.Play("Clear");
                    for (int i = 0; i < restNum; i++)
                    {
                        coinAnims[i].Play("CoinClear");
                    }
                }
                else
                {
                    MusicManager.Play(BgmCode.Failure);
                    backgrondAnim.Play("Failure");
                    /*
                    for (int i = 0; i < restNum; i++)
                    {
                        coinAnims[i].Play("CoinClear");
                    }
                    foreach (Animator anim in coinAnims)
                    {
                        if (anim.gameObject.activeSelf)
                        {
                            anim.Play("CoinFailure");
                        }
                    }
                    */
                    coinAnims[restNum - 1].Play("CoinFailure");
                }
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                SceneManager.UnloadSceneAsync(Instance.sceneName);
                //シーン削除
                Scene scene = SceneManager.GetSceneByName("ManagerScene");
                // 取得したシーンのルートにあるオブジェクトを取得
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject obj in rootObjects)
                {
                    //Debug.Log(obj.name);
                    //レイヤーがManagerSceneでないオブジェクトを削除
                    if (obj.layer != 8)
                    {
                        Destroy(obj);
                    }
                }
                gameScene = SceneManager.GetSceneByName(sceneName);
                //EffectManager.StopEffect();
                currentGame = LoadGamePackage();
                isCanPause = true;
                //Debug.Log(currentGame.name);
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                //失敗処理
                if (!ClearFlag)
                {
                    coinAnims[restNum - 1].gameObject.SetActive(false);
                    restNum--;
                    if (restNum == 0)
                    {
                        FinishFlag = true;
                    }
                }
                ClearFlag = false;
                foreach (Animator anim in coinAnims)
                {
                    if (anim.gameObject.activeSelf)
                    {
                        anim.Play("CoinReady");
                    }
                }

                if (FinishFlag)
                {
                    Finish();
                }
                else
                {

                    backgrondAnim.Play("Ready");
                    number++;
                    numberMesh.text = number.ToString();
                    Time.timeScale += 0.1f;
                    MusicManager.audioSource.volume = 1f;
                    MusicManager.Play(BgmCode.Ready);
                    StartGame();

                    //SpeedUp();
                }
            });
    }

    private void SpeedUp()
    {
        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {
                backgrondAnim.Play("SpeedUp");
                isCanPause = false;
                MusicManager.audioSource.DOPitch(Time.timeScale + 0.1f, 4);
                MusicManager.Play(BgmCode.SpeedUp);
                DOTween.To(
                    () => Time.timeScale,
                    num => Time.timeScale = num,
                    0.1f,
                    4.0f
                ).SetEase(Ease.Linear).SetRelative();
            })
            .AppendInterval(4f)
            .AppendCallback(() =>
            {
                backgrondAnim.Play("Ready");
                number++;
                numberMesh.text = number.ToString();
                MusicManager.audioSource.volume = 1f;
                MusicManager.Play(BgmCode.Ready);
                StartGame();
            });
    }

    public void Finish()
    {
        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {
                Time.timeScale = 1;
                backgrondAnim.Play("Finish");
                isCanPause = false;
            })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                //Initialization();
                //SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
                // スコアを登録
                if (mode == PlayMode.Normal) ScoreManager.Instance.RegisterScore(number);
                else if (mode == PlayMode.Single) ScoreManager.Instance.RegisterScore(number, singleGame.sceneName);
                // リザルト画面を呼び出す
                SceneManager.LoadSceneAsync("ResultScene", LoadSceneMode.Additive);
                //mode = PlayMode.None;                

                // データをセーブする
                TadaLib.Save.SaveManager.Instance.Save();
            });
    }

    public static void Clear()
    {
        if (!ClearFlag && (IsGamePlaying || mode == PlayMode.None))
        {
            ClearFlag = true;
            EffectManager.EmitEffect(EffectCode.Kamihubuki);
            //EffectManager.PlayEffect();
        }
    }

    public static void CameraZoomUp()
    {
        /*
        Instance.gameCamera.orthographicSize = 100;
        Instance.gameCamera.DOOrthoSize(5, 0.5f);
        Instance.managerCamera.DOOrthoSize(0.1f, 0.5f);
        */
        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {
                //Instance.transitionImage.DOFade(0, 0.5f);
                //Instance.trasitionMask.DOSizeDelta(new Vector2(19200, 10800), 0.5f);
                Instance.renderCamera.enabled = true;
                Instance.trasitionMask.gameObject.SetActive(true);
                Instance.trasitionMask.enabled = true;
                Instance.trasitionMask.transform.DOScale(30, 0.5f).SetEase(Ease.InQuad);
                Instance.transitionImage.transform.DOScale(0.2f, 0.5f).SetEase(Ease.InQuad);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                Instance.renderCamera.enabled = false;
                Instance.managerCammera.enabled = false;
                Instance.trasitionMask.gameObject.SetActive(false);
            });
    }

    public static void CameraZoomOut()
    {
        /*
        Instance.gameCamera.DOOrthoSize(100, 0.5f);
        Instance.managerCamera.DOOrthoSize(5, 0.5f);
        */
        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {
                //Instance.transitionImage.DOFade(1, 0.5f);
                //Instance.trasitionMask.DOSizeDelta(new Vector2(1920, 1080), 0.5f);
                Instance.renderCamera.enabled = true;
                Instance.managerCammera.enabled = true;
                Instance.trasitionMask.gameObject.SetActive(true);
                Instance.trasitionMask.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad);
                Instance.transitionImage.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad);
            })
            .AppendInterval(0.6f)//少し遅らせる
            .AppendCallback(() =>
            {
                Instance.renderCamera.enabled = false;
                Instance.trasitionMask.gameObject.SetActive(false);
            });
    }

    public static void ShowStatement()
    {
        Instance.statementMesh.gameObject.SetActive(true);
        Instance.statementMesh.transform.localScale = Vector3.one * 10;
        Sequence seq = DOTween.Sequence()
            .Append(Instance.statementMesh.transform.DOScale(Vector3.one, 0.25f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                Instance.statementMesh.gameObject.SetActive(false);
            });
    }

    GamePackage LoadGamePackage()
    {
        GamePackage package;

        switch (mode)
        {
            case PlayMode.Normal:
                //次のシーン読み込み
                if (gameQueue.Count == 0)
                {
                    int[] indexList = new int[gamePackageSet.games.Length];
                    for (int i = 0; i < gamePackageSet.games.Length; i++)
                    {
                        indexList[i] = i;
                    }
                    for (int i = 0; i < gamePackageSet.games.Length; i++)
                    {
                        int rand = UnityEngine.Random.Range(i, indexList.Length);
                        int tmp = indexList[rand];
                        indexList[rand] = indexList[i];
                        indexList[i] = tmp;
                    }
                    for (int i = 0; i < gamePackageSet.games.Length; i++)
                    {
                        gameQueue.Enqueue(indexList[i]);
                    }
                }
                package = gamePackageSet.games[gameQueue.Dequeue()];
                break;
            default:
                package = singleGame;
                break;
        }

        sceneName = package.sceneName;
        statementMesh.text = package.statement;
        currentGameType = package.gameType;

        return package;
    }

    public void Pause()
    {
        isPause = true;
        timeScale = Time.timeScale;
        Time.timeScale = 0;
        pauseUi.SetActive(true);
    }

    public void Resume()
    {
        isPause = false;
        Time.timeScale = timeScale;
        pauseUi.SetActive(false);
        MusicManager.Resume();
    }

    public void ReturnTitle()
    {
        Time.timeScale = 1;
        isCanPause = false;
        isPause = false;
        pauseUi.SetActive(false);
        DOTween.KillAll();//やばそうな実装
        mode = PlayMode.None;
        SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
    }

    public static void ScreenTrasition(string fromScene, string toScene, float time = 0.5f)
    {
        Scene scene;
        AsyncOperation ao = null;
        Sequence seq = DOTween.Sequence()
                .OnStart(() =>
                {
                    Instance.screenImage.color = Color.white;
                    //シーンを非同期で読み込み
                    scene = SceneManager.GetSceneByName(toScene);
                    if (!scene.IsValid())
                    {
                        ao = SceneManager.LoadSceneAsync(toScene, LoadSceneMode.Additive);
                        ao.allowSceneActivation = false;
                    }
                })
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    //Instance.renderCamera.enabled = false;
                    Instance.screenImage.gameObject.SetActive(true);
                    Instance.screenImage.DOFade(0, time - 1f);
                    if (ao != null) ao.allowSceneActivation = true;
                    if (fromScene != "ManagerScene")
                    {
                        SceneManager.UnloadSceneAsync(fromScene);
                    }
                })
                .AppendInterval(time - 0.05f)
                .AppendCallback(() =>
                {
                    Instance.screenImage.gameObject.SetActive(false);
                    Instance.renderCamera.cullingMask = 0b00000001;//ManagerScene有効
                });
    }

    /*
    public static void TrasitionScene(string fromScene, string toScene, float time = 0.5f)
    {
        Instance.StartCoroutine(Instance.ScreenShot(fromScene, toScene, time));
    }

    IEnumerator ScreenShot(string fromScene, string toScene, float time = 0.5f)
    {
        Scene scene;
        AsyncOperation ao = null;

        yield return new WaitForEndOfFrame();
        // 画像生成
        tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        tex.alphaIsTransparency = true;
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
        tex.Apply();

        screenImage.gameObject.SetActive(true);
        screenImage.texture = tex;
        screenImage.color = Color.white;
        //シーンを非同期で読み込み
        if(toScene != "ManagerScene")
        {
            SceneManager.LoadSceneAsync(toScene, LoadSceneMode.Additive);
        }
        if (fromScene != "ManagerScene")
        {
            SceneManager.UnloadSceneAsync(fromScene);
        }
        Instance.screenImage.DOFade(0, time).OnComplete(() => screenImage.gameObject.SetActive(false));

        // byteデータに変換
        byte[] bytes = tex.EncodeToPNG();
        // 破棄
        //DestroyImmediate(tex);
        // 日付をファイル名にする
        DateTime dt = DateTime.Now;
        string dateStr = dt.Year + "_" + dt.Month + dt.Day + "_" + dt.Hour + dt.Minute + "_" + dt.Second;
        // 保存
        //File.WriteAllBytes(Application.dataPath + "/" + dateStr + ".png", bytes);
    }
    */

    public enum PlayMode
    {
        Normal,
        Single,
        Debug,
        None
    }
}
