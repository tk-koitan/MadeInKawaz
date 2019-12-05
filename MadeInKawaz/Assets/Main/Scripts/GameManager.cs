using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool ClearFlag { get; private set; }
    public static Queue<Action> ClearActionQueue = new Queue<Action>();
    [SerializeField]
    private TextMeshProUGUI statement;
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private Camera managerCamera;
    private Camera gameCamera;
    private AsyncOperation async;
    private Scene gameScene;
    [SerializeField]
    private RawImage transitionImage;
    [SerializeField]
    private RectTransform trasitionMask;
    private AudioSource audioSource;
    [SerializeField]
    private GamePackage[] games;    
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

        Initialization();

        audioSource = GetComponent<AudioSource>();

        //デバッグ用
        EndGame();
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

    private void Initialization()
    {
        statement.gameObject.SetActive(false);
        Sequence seq = DOTween.Sequence()
            .AppendInterval(2f)
            .AppendCallback(() =>
            {
                //次のシーン読み込み
                int rand = UnityEngine.Random.Range(0, games.Length);
                sceneName = games[rand].sceneName;
                statement.text = games[rand].statement;
                gameScene = SceneManager.GetSceneByName(sceneName);
                if (!gameScene.IsValid())
                {
                    async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    async.allowSceneActivation = false;
                    Time.timeScale += 0.1f;
                }
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
                if(ClearFlag)
                {
                    audioSource.pitch = Time.timeScale;
                    audioSource.Play();
                }
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                SceneManager.UnloadSceneAsync(Instance.sceneName);
                Instance.gameCamera = null;
                gameScene = SceneManager.GetSceneByName(sceneName);
                ClearFlag = false;
                EffectManager.StopEffect();
                //次のシーン読み込み
                int rand = UnityEngine.Random.Range(0, games.Length);
                sceneName = games[rand].sceneName;
                statement.text = games[rand].statement;
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
            })
            .AppendInterval(1.75f)
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
        Instance.statement.gameObject.SetActive(true);
        Instance.statement.transform.localScale = Vector3.one * 10;
        Sequence seq = DOTween.Sequence()
            .Append(Instance.statement.transform.DOScale(Vector3.one, 0.25f))
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                Instance.statement.gameObject.SetActive(false);
            });
    }
}
