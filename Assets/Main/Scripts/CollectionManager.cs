using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CollectionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject app;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Vector3 backPos;
    [SerializeField]
    private GameObject appButton, canvas;
    [SerializeField]
    private TextMeshProUGUI titleText, explanationText, authorText, highScoreText;
    [SerializeField]
    private Image screenshotIm;
    [SerializeField]
    private GamePackageSet gamePackageSet;
    private GamePackage selectedGame;
    [SerializeField]
    private Vector3 upperRightPos;
    [SerializeField]
    private float width, height;
    [SerializeField]
    private int widthCount;
    // Start is called before the first frame update
    void Start()
    {
        app.SetActive(false);
        //button.onClick.AddListener(() => AppIn(button.gameObject));
        for (int i = 0; i < gamePackageSet.games.Length; i++)
        {
            GamePackage g = gamePackageSet.games[i];
            GameObject obj = Instantiate(appButton, canvas.transform);
            Button b = obj.GetComponent<Button>();
            Image im = obj.transform.GetChild(0).GetComponent<Image>();//プレハブの構造が変わると危険！
            obj.transform.localPosition = upperRightPos + new Vector3(width * (i % widthCount), -height * (i / widthCount));
            b.onClick.AddListener(() => AppIn(obj.transform.position, g));
            im.sprite = g.iconImage;
        }
        app.transform.SetAsLastSibling();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AppIn(Vector3 pos, GamePackage gamePackage)
    {
        app.transform.position = pos;
        backPos = pos;
        selectedGame = gamePackage;

        titleText.text = gamePackage.titleName;
        explanationText.text = gamePackage.explanation;
        authorText.text = "つくったひと : " + gamePackage.authorName;
        screenshotIm.sprite = gamePackage.screenshotImage;


        highScoreText.text = "ハイスコア：" + System.String.Format("{0, 3}", ScoreManager.Instance.GetScoreData(gamePackage.sceneName).Scores[0].ToString());

        app.SetActive(true);
        app.transform.localScale = Vector3.zero;
        app.transform.DOScale(1, 0.25f).SetEase(Ease.OutCubic);
        app.transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.OutCubic);
    }

    public void AppOut()
    {
        app.transform.localScale = Vector3.one;
        app.transform.DOScale(0, 0.25f).SetEase(Ease.InCubic);
        app.transform.DOMove(backPos, 0.25f).SetEase(Ease.InCubic).OnComplete(() => app.SetActive(false));
    }

    public void AppPlay()
    {
        GameManager.mode = GameManager.PlayMode.Single;
        GameManager.Instance.singleGame = selectedGame;
        GameManager.Instance.Initialization();
        SceneManager.UnloadSceneAsync("Library");
        //GameManager.TrasitionScene("Library", "ManagerScene");
    }

    public void BackTitle()
    {
        //GameManager.TrasitionScene("Library", "Title");
        SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Library");
    }
}
