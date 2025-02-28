using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ClearTest : MonoBehaviour
{    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clear()
    {
        GameManager.Clear();
    }

    public void GameStart()
    {
        GameManager.mode = GameManager.PlayMode.Normal;
        GameManager.Instance.Initialization();
        SceneManager.UnloadSceneAsync("Title");
    }
}
