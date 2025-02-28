using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DebugTextManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI debugText;
    [SerializeField]
    GameObject debugCanvas;
    public static List<DebugElement> debugElements = new List<DebugElement>();
    // Start is called before the first frame update
    void Start()
    {
        debugText.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || (Input.touchCount == 4 && Input.touches[3].phase == TouchPhase.Began))
        {
            debugCanvas.SetActive(!debugCanvas.activeSelf);
        }

        if (Input.GetKey(KeyCode.W))
        {
            debugText.fontSize += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            debugText.fontSize -= 1;
        }

        //debugText.text = str();
        if (Debug.isDebugBuild)
        {
            debugText.text = string.Empty;
            for (int i = debugElements.Count - 1; i >= 0; i--)
            {
                //Triggerで削除
                if (debugElements[i].removeTrigger())
                {
                    debugElements.RemoveAt(i);
                }
                else
                {
                    debugText.text += debugElements[i].message();
                }
            }
            /*
            foreach (DebugElement e in debugElements)
            {
                debugText.text += e.message();
            }
            */
        }
    }

    public static DebugElement Display(Func<string> message, int priority = 0)
    {
        DebugElement elem = new DebugElement(message, priority);
        debugElements.Add(elem);
        debugElements.Sort((a, b) => b.priority - a.priority);
        return elem;
    }

    public static DebugElement Display(object obj, int priority = 0)
    {
        return Display(obj.ToString, priority);
    }

    public class DebugElement
    {
        public int priority { get; private set; }
        int indent;
        public Func<string> message { get; private set; }
        public List<DebugElement> elements = new List<DebugElement>();
        public bool isOpen { get; set; }
        public Func<bool> removeTrigger { get; private set; }

        public DebugElement(Func<string> s, int p = 0)
        {
            message = s;
            priority = p;
            removeTrigger = () => false;
        }

        public DebugElement AddRemoveTrigger(MonoBehaviour mono)
        {
            removeTrigger = () => mono != null ? false : true;
            return this;
        }
    }

    public void ResolutionChange(int resolution)
    {
        Screen.SetResolution(resolution / 10000, resolution % 10000, Screen.fullScreen);
    }

    public void FullScreenChange()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
