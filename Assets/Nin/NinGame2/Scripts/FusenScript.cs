using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FusenScript : MonoBehaviour
{
    private float filled = 0;
    private float t = 0;

    [SerializeField]
    private SpriteRenderer balloonSpriteRenderer;
    [SerializeField]
    private Sprite[] balloonSprites;

    [SerializeField]
    private SpriteRenderer girlSpriteRenderer;
    [SerializeField]
    private Sprite[] girlSprites;

    private Image progressBar;
    public void Awake()
    {
        progressBar = transform.GetComponent<Image>();
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        progressBar.fillOrigin = 0;
    }

    public void SetProgressValue(float value)
    {
        progressBar.fillAmount = value;
    }

    void Start()
    {
        balloonSpriteRenderer.sprite = balloonSprites[0];
        girlSpriteRenderer.sprite = girlSprites[0];
    }

    void Update()
    {
        t += Time.deltaTime;

        if (t < 3.5f)
        {
            if (Input.GetMouseButton(0) && filled < 102f)
            {
                filled += 2f * Time.deltaTime * 60f;
            }
            else if(filled > 1f)
            {
                filled -= 1f * Time.deltaTime * 60f;
            }
            SetProgressValue(filled * 0.01f);
            if(filled <= 10f)
            {
                balloonSpriteRenderer.sprite = balloonSprites[0];
            }
            else if(filled > 10f && filled <= 50f)
            {
                balloonSpriteRenderer.sprite = balloonSprites[1];
            }
            else
            {
                balloonSpriteRenderer.sprite = balloonSprites[2];
            }
        }
        else
        {
            if(filled >= 80f)
            {
                balloonSpriteRenderer.sprite = balloonSprites[3];
            }
            if(filled <= 50f || filled >= 80f)
            {
                girlSpriteRenderer.sprite = girlSprites[1];
            }
            else
            {
                GameManager.Clear();
            }
        }
    }
}
