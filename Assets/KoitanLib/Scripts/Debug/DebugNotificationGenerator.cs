using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

public class DebugNotificationGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject textBox;
    private TextMeshProUGUI textMesh;
    private Func<string> ms;
    public static Queue<NotificationElement> nQueue = new Queue<NotificationElement>();
    private bool isOpen = false;
    public Transform parent { get; }
    public float duration { get; }
    public Vector2 size { get; }
    public Func<string> message { get; }

    void Start()
    {
        textMesh = textBox.GetComponentInChildren<TextMeshProUGUI>();
        textBox.SetActive(false);
        //DebugTextManager.Display(() => "NotificationQueueCount:" + nQueue.Count.ToString() + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOpen && nQueue.Count > 0)
        {
            NotificationElement e = nQueue.Dequeue();
            isOpen = true;
            //textBox.SetActive(true);            
            textMesh.text = e.message();
            ms = e.message;
            Sequence seq = DOTween.Sequence()
                .OnStart(() => textBox.SetActive(true))
                .Append(textBox.transform.DOLocalMoveX(-600, 0.3f).SetRelative().SetEase(Ease.OutCubic))
                .AppendInterval(e.duration)
                .Append(textBox.transform.DOLocalMoveX(600, 0.3f).SetRelative().SetEase(Ease.InCubic))
                .OnComplete(() =>
                {
                    isOpen = false;
                    textBox.SetActive(false);
                });
            //textBox.transform.DOMoveX(-400,)
        }

        if (isOpen)
        {
            textMesh.text = ms?.Invoke();
        }
    }

    public static void Notify(Func<string> message, float duration = 3f)
    {
        //TextMeshProUGUI tmpText = Instantiate(text);
        nQueue.Enqueue(new NotificationElement(message, duration));
    }

    public static void Notify(string str, float duration = 3f)
    {
        //TextMeshProUGUI tmpText = Instantiate(text);
        nQueue.Enqueue(new NotificationElement(() => str, duration));
    }

    public class NotificationElement
    {
        public float duration { get; }
        public Func<string> message { get; }
        public NotificationElement(Func<string> m, float d)
        {
            message = m;
            duration = d;
        }
    }
}
