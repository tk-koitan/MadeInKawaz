using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{
    static MusicManager instance;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    public static AudioSource audioSource;
    public BGMSource[] bgm;
    public float startTime, endTime;
    private float currentTime;
    public bool isIntro = false;
    public int index = 0;
    public bool isloop;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isloop)
        {
            if (audioSource.time >= startTime)
            {
                isIntro = false;
            }
            if (audioSource.time >= endTime)
            {
                audioSource.time = audioSource.time - endTime + startTime;
                audioSource.Play();
            }
            if (!isIntro && audioSource.time <= 0)
            {
                audioSource.time = startTime;
                audioSource.Play();
            }
        }        
        //Debug.Log(audioSource.time);
    }

    public static void Play(int _index)
    {
        instance.startTime = instance.bgm[_index].startTime;
        instance.endTime = instance.bgm[_index].endTime;
        audioSource.clip = instance.bgm[_index].BGM;
        audioSource.time = 0;
        audioSource.Play();
        instance.isIntro = true;
        instance.index = _index;
        instance.isloop = instance.bgm[_index].isLoop;
    }

    public static void Play(BgmCode bgmCode)
    {
        Play((int)bgmCode);
    }

    public static void Stop()
    {
        instance.isIntro = true;
        audioSource.Stop();
    }

    public static void Resume()
    {
        instance.isIntro = true;
        audioSource.Play();
    }

    public static void FadeIn(float duration)
    {
        audioSource.DOFade(1, duration);
    }

    public static void FadeOut(float duration)
    {
        audioSource.DOFade(0, duration);
    }
}

public enum BgmCode
{
    CupBattle = 0,
    CupTitle = 1,
    CupClear = 2,
    CupResult = 3
}
