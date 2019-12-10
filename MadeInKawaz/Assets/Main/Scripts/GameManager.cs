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
    [SerializeField]
    private TextMeshProUGUI statementMesh;
    [SerializeField]
    private string sceneName;
    private AsyncOperation async;
    private Scene gameScene;
    [SerializeField]
    private RawImage transitionImage;
    [SerializeField]
    private RectTransform trasitionMask;
    [SerializeField]
    private TextMeshPro numberMesh;
    public int number { get; private set; }
    [SerializeField]
    private GamePackage[] games;
    private GamePackage currentGame;
    private GameType currentGameType;
    private bool isTestPlay = false;
    public PlayMode mode { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        mode = PlayMode.None;

#if UNITY_EDITOR
        isTestPlay = EditorPrefs.GetBool("testPlayFlag", false);
        if (isTestPlay)
        {
            Debug.Log("これはテストプレイです");
            EditorPrefs.SetBool("testPlayFlag", false);
            mode = PlayMode.Debug;
            Time.timeScale = EditorPrefs.GetFloat("timeScale", 1);
        }
#endif
        if (mode != PlayMode.None)
        {
            Initialization();
        }

        statementMesh.gameObject.SetActive(false);

        //デバッグ用
        //EndGame();
        DebugTextManager.Display(() => "TimeScale:" + Time.timeScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
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
    private void Initialization()
    {
        number = 1;
        numberMesh.text = number.ToString();
        if (isTestPlay)
        {
            currentGame = LoadGamePackage();
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("ManagerScene"));
            SceneManager.UnloadSceneAsync(currentGame.sceneName);
        }

        Sequence seq = DOTween.Sequence()
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                //次のシーン読み込み
                currentGame = LoadGamePackage();
                gameScene = SceneManager.GetSceneByName(sceneName);
                if (!gameScene.IsValid())
                {
                    async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    async.allowSceneActivation = false;
                }

                ClearActionQueue.Clear();
            })
            .AppendInterval(1.75f)
            .AppendCallback(() => ShowStatement())
            .AppendInterval(0.25f)
            .AppendCallback(() => StartGame());
    }

    [ContextMenu("StartGame")]
    private void StartGame()
    {
        //ShowStatement();
        TimeCountManager.CountDownStart(4);
        CameraZoomUp();
        Instance.async.allowSceneActivation = true;
        Sequence seq = DOTween.Sequence()
            .AppendInterval(4f)
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
                if (ClearFlag)
                {
                    MusicManager.audioSource.pitch = Time.timeScale;
                    MusicManager.Play(0);
                }
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                SceneManager.UnloadSceneAsync(Instance.sceneName);
                gameScene = SceneManager.GetSceneByName(sceneName);
                ClearFlag = false;
                EffectManager.StopEffect();
                currentGame = LoadGamePackage();
            })
            .AppendInterval(1.5f)
            .AppendCallback(() =>
            {
                gameScene = SceneManager.GetSceneByName(sceneName);
                if (!gameScene.IsValid())
                {
                    async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    async.allowSceneActivation = false;
                    Time.timeScale += 0.1f;
                }
                number++;
                numberMesh.text = number.ToString();
            })            
            .Append(numberMesh.transform.DOScale(0.5f,0.25f).SetRelative())
            .AppendInterval(0.25f)
            .Append(numberMesh.transform.DOScale(-0.5f, 0.25f).SetRelative())
            .AppendCallback(() => ShowStatement())
            .AppendInterval(0.25f)
            .AppendCallback(() => StartGame());
    }

    public static void Clear()
    {
        if (!ClearFlag)
        {
            ClearFlag = true;
            //EffectManager.EmitEffect(EffectCode.Kamihubuki);
            EffectManager.PlayEffect();
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
                Instance.trasitionMask.gameObject.SetActive(true);
                Instance.trasitionMask.transform.DOScale(30, 0.5f).SetEase(Ease.InQuad);
                Instance.transitionImage.transform.DOScale(0.2f, 0.5f).SetEase(Ease.InQuad);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
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
                Instance.trasitionMask.gameObject.SetActive(true);
                Instance.trasitionMask.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad);
                Instance.transitionImage.transform.DOScale(1, 0.5f).SetEase(Ease.OutQuad);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
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

        if (mode == PlayMode.Debug)
        {
            var path = "Assets/Main/Games/TestPlayPackage.asset";
            package = AssetDatabase.LoadAssetAtPath<GamePackage>(path);
        }
        else
        {
            //次のシーン読み込み
            int rand = UnityEngine.Random.Range(0, games.Length);
            package = games[rand];
        }

        sceneName = package.sceneName;
        statementMesh.text = package.statement;
        currentGameType = package.gameType;

        return package;
    }

    public enum PlayMode
    {
        Normal,
        Single,
        Debug,
        None
    }
}
