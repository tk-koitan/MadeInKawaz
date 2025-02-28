using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;

public class DebugPopupManager : MonoBehaviour
{
    [SerializeField]
    private GameObject textPopupBox;
    private DebugPopup popup;
    public static Queue<PopupElement> popupQueue = new Queue<PopupElement>();
    private bool isPop = false;            
    public Func<string> message { get; }
    // Start is called before the first frame update
    void Start()
    {
        //DebugTextManager.Display(() => "PopupQueueCount:" + popupQueue.Count.ToString() + "\n", -100);
        popup = textPopupBox.GetComponent<DebugPopup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (popup.openState == DebugPopup.OpenState.Closed && popupQueue.Count > 0)
        {
            popup.SetPopup(popupQueue.Dequeue().message);
        }
    }

    public static void ShowPopup(Func<string> message)
    {
        //TextMeshProUGUI tmpText = Instantiate(text);
        popupQueue.Enqueue(new PopupElement(message));
    }

    public class PopupElement
    {        
        public float duration { get; }
        public Vector2 size { get; }
        public Func<string> message { get; }
        public PopupElement(Func<string> m)
        {
            message = m;            
        }
    }
}
