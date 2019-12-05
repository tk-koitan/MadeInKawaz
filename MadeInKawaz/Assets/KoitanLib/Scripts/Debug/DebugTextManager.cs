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

        //debugText.text = str();
        if (Debug.isDebugBuild)
        {
            debugText.text = string.Empty;            
            foreach (DebugElement e in debugElements)
            {
                debugText.text += e.message();
            }
        }
    }

    public static void Display(Func<string> message,int priority = 0)
    {
        debugElements.Add(new DebugElement(message, priority));
        debugElements.Sort((a, b) => a.priority - b.priority);
    }

    public static void Display(object obj, int priority = 0)
    {
        Display(obj.ToString, priority);
    }

    public class DebugElement
    {
        public int priority { get; private set; }
        int indent;
        public Func<string> message { get; private set; }
        public List<DebugElement> elements = new List<DebugElement>();
        public bool isOpen { get; set; }

        public DebugElement(Func<string> s, int p = 0)
        {
            message = s;
            priority = p;
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
