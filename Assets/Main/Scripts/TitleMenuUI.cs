using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleMenuUI : MonoBehaviour
{
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveXUI(float x)
    {
        rectTransform.DOLocalMoveX(x, 0.5f).SetEase(Ease.OutCubic);
    }

    public void GameStart()
    {
        GameManager.mode = GameManager.PlayMode.Normal;
        GameManager.Instance.Initialization();
        //SceneManager.UnloadSceneAsync("Title");
        //GameManager.ScreenTrasition("Title","ManagerScene",10f);
        SceneManager.UnloadSceneAsync("Title");
    }

    public void LibraryStart()
    {
        //GameManager.TrasitionScene("Title", "Library");
        SceneManager.LoadSceneAsync("Library", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Title");
    }
}
