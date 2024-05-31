using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timekeeper : MonoBehaviour
{
    bool f;
    bool onetime;
    public static float time=0;
    void Start()
    {
        //
        Application.targetFrameRate = 60;
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
            time += Time.deltaTime;
            if (onetime)
            {
                GetComponent<AudioSource>().Play();
                onetime = false;
            }
        if (time >= 4)
        {
            time = 0;
        }
    }
}
