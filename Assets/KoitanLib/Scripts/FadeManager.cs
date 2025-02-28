using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{    
    static FadeManager instance;
    public static FadeManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private Canvas fadeCanvas;

    public static RawImage image;
    private static bool is_fading = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(instance);
            DontDestroyOnLoad(fadeCanvas);
        }
        else Destroy(gameObject);

        image = GetComponent<RawImage>();       
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void FadeIn(float duration)
    {
        is_fading = false;
        image.DOFade(0, duration);
    }

    public static void FadeOut(float duration, string next_scene_name)
    {
        if (is_fading) return;
        is_fading = true;
        image.DOFade(1, duration).OnComplete(() => SceneManager.LoadScene(next_scene_name));
    }
}
