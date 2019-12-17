using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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
}
