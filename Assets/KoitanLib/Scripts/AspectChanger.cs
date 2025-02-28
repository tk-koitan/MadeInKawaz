using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectChanger : MonoBehaviour
{
    int defaultWidth = Screen.width;
    int defaultHeight = Screen.height;
    // Start is called before the first frame update
    void Start()
    {
        //強制的にアスペクト比を16:9にする
        //Screen.SetResolution(Screen.width, Screen.width * 9 / 16, Screen.fullScreen);

        //DebugTextManager.Display(() => { return "Resolution: " + Screen.width + "×" + Screen.height + "\n"; }, -1);
        //DebugTextManager.Display(() => { return "FullScreen: " + Screen.fullScreen.ToString() + "\n"; }, -1);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //エディタ上では実行されないので注意
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Screen.SetResolution(640, 360, Screen.fullScreen);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Screen.SetResolution(1280, 720, Screen.fullScreen);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Screen.SetResolution(1440, 810, Screen.fullScreen);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
        */
    }

    /*
    private void OnGUI()
    {
        if (Debug.isDebugBuild)
        {
            GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Resolution: " + Screen.width + "×" + Screen.height);
            GUILayout.Label("FullScreen: " + Screen.fullScreen.ToString());
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
    }
    */

    public void ChangeAspect(int aspect)
    {
        Screen.SetResolution(Screen.width, Screen.width * (aspect % 100) / (aspect / 100), Screen.fullScreen);
    }

    public void ChangeResolution(int heightResolution)
    {
        Screen.SetResolution(heightResolution * 16 / 9, heightResolution * 16 / 9 * defaultHeight / defaultWidth, Screen.fullScreen);
    }

    public void ChangeDefaultResolution()
    {
        Screen.SetResolution(defaultWidth, defaultHeight, Screen.fullScreen);
    }
}
