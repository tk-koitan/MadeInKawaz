using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(timekeeper.time);
        if (timekeeper.time >= 1.75f)
        {
            Debug.Log("Camera Moved");
            transform.position = new Vector3(30, 0, -10);
        }
    }
}
