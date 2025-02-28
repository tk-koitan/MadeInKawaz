using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TimeCountManager : MonoBehaviour
{
    public static TimeCountManager Instance { get; private set; }
    public static float RemainingTime { get; private set; }
    public static bool IsRunning { get; private set; }

    [SerializeField]
    private TextMeshProUGUI textMesh;
    [SerializeField]
    private Image bombImg;
    [SerializeField]
    private Sprite[] bombSprs;
    public float time;
    [SerializeField]
    private Image timeMeterImg;

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
        Instance.bombImg.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (IsRunning)
        {
            RemainingTime -= Time.deltaTime;
            if (RemainingTime > 0)
            {
                int nowCount = Mathf.CeilToInt(RemainingTime);
                if (nowCount <= 3 && nowCount != count)
                {
                    textMesh.text = nowCount.ToString("0");
                    textMesh.transform.localScale = Vector3.one * 0.1f;
                    textMesh.transform.DOScale(Vector3.one, 0.2f);
                }
                count = nowCount;
            }
            else
            {
                textMesh.text = string.Empty;
                bombImg.sprite = bombSprs[1];
            }
        }
        */
        time += Time.deltaTime;
        if (RemainingTime > 0)
        {
            RemainingTime -= Time.deltaTime;
            if(RemainingTime < 0)
            {
                RemainingTime = 0;
            }
        }

        timeMeterImg.rectTransform.sizeDelta = new Vector2(1024 * RemainingTime / 8, 32);
    }

    public static void CountDownStart(float time)
    {
        RemainingTime = time;
        time = 0;
        IsRunning = true;
        Instance.textMesh.text = string.Empty;
        Instance.timeMeterImg.rectTransform.sizeDelta = new Vector2(1024 * RemainingTime / 8, 32);
        Sequence seq = DOTween.Sequence()
            .AppendInterval(RemainingTime - 2f)
            .AppendCallback(() =>
            {
                Instance.bombImg.gameObject.SetActive(true);
                Instance.bombImg.sprite = Instance.bombSprs[0];
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                Instance.textMesh.text = "3";
                Instance.textMesh.transform.localScale = Vector3.one * 0.1f;
                Instance.textMesh.transform.DOScale(Vector3.one, 0.1f);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                Instance.textMesh.text = "2";
                Instance.textMesh.transform.localScale = Vector3.one * 0.1f;
                Instance.textMesh.transform.DOScale(Vector3.one, 0.1f);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                Instance.textMesh.text = "1";
                Instance.textMesh.transform.localScale = Vector3.one * 0.1f;
                Instance.textMesh.transform.DOScale(Vector3.one, 0.1f);
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                Instance.textMesh.text = string.Empty;
                Instance.bombImg.sprite = Instance.bombSprs[1];
            })
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                Instance.bombImg.gameObject.SetActive(false);
            });
    }
}
