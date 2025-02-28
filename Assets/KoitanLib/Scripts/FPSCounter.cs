using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    private float m_updateInterval = 0.5f;

    private float m_accum;
    private int m_frames;
    private float m_timeleft;
    private float m_fps;

    private void Start()
    {
        DebugTextManager.Display(() => "FPS: " + m_fps.ToString("f2") + "(" + (Time.deltaTime * 1000).ToString("00") + "ms)\n" , -3);
        DebugTextManager.Display(() => { return "Resolution: " + Screen.width + "×" + Screen.height + "\n"; }, -1);
        //ObserverGraph.observerValue = () => m_fps;
        RawImageGraph.observerValue = () => m_fps;
    }
    

    private void Update()
    {
        m_timeleft -= Time.deltaTime;
        m_accum += Time.timeScale / Time.deltaTime;
        m_frames++;

        if (0 < m_timeleft) return;

        m_fps = m_accum / m_frames;
        m_timeleft = m_updateInterval;
        m_accum = 0;
        m_frames = 0;
    }

    /*
    private void OnGUI()
    {
        if (Debug.isDebugBuild)
            GUILayout.Label("FPS: " + m_fps.ToString("f2") + "(" + (Time.deltaTime * 1000).ToString("00") + "ms)");
    }
    */
}