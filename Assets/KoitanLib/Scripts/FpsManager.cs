using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsManager : MonoBehaviour
{
    private int framerate = 60;
    private int vSync = 0;    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        //DebugTextManager.Display(() => { return "Target FPS " + framerate.ToString() + "\n"; }, -2);
        //DebugTextManager.Display(() => { return "vSyncCount " + vSync.ToString() + "\n"; }, -2);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.UpArrow))
        {
            framerate++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            framerate--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            vSync++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            vSync--;
        }
        */
        Application.targetFrameRate = framerate; //60FPSに設定
        QualitySettings.vSyncCount = vSync;
    }

    /*
    private void OnGUI()
    {        
        GUILayout.Space(20);
        GUILayout.Label("Target FPS " + framerate.ToString());
        GUILayout.Label("vSyncCount " + vSync.ToString());
        GUILayout.Label("Particle Count " + ptcnt.ToString());        
    }
    */

    public void AddFramerate(int num)
    {
        framerate += num;
    }

    public void AddvSync(int num)
    {
        vSync += num;
    }
}
