using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SampleBGMPlayer : MonoBehaviour
{
    private AudioSource player;
    private bool isFading;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        player.pitch = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeCountManager.RemainingTime <= 0.1f && !isFading && GameManager.mode != GameManager.PlayMode.None)
        {
            isFading = true;
            player.DOFade(0, 0.1f);
        }
    }
}
