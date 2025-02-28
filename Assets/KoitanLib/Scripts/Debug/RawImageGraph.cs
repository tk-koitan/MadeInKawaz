using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageGraph : MonoBehaviour
{
    private RawImage image;
    [SerializeField]
    private int maxRecordFrame = 300;
    private float[] y;
    [SerializeField]
    private int maxY = 80;
    [SerializeField]
    private int minY = 0;

    private Texture2D tex;
    private int width, height;
    private int currentFrame = 0;

    public static ObserverValue observerValue;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();

        y = new float[maxRecordFrame];

        // ①描画先のテクスチャ・スプライトを作成        
        width = maxRecordFrame;
        height = maxY - minY + 1;
        // テクスチャ生成
        tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        //ドット絵の表示
        //tex.filterMode = FilterMode.Point;
        //白で塗りつぶす
        Color[] cols = new Color[width * height];
        for (int i = 0; i < width * height; i++) cols[i] = Color.white;
        tex.SetPixels(cols);
        //60に線を引く
        cols = new Color[width];
        for (int i = 0; i < width; i++)
        {
            tex.SetPixel(i, 60, Color.green);
        }

        image.texture = tex;
        image.SetNativeSize();
        //image.rectTransform.sizeDelta = new Vector2(1920, height * 2);
    }

    // Update is called once per frame
    void Update()
    {
        if (observerValue != null)
        {
            tex.SetPixel(currentFrame, Mathf.FloorToInt(y[currentFrame]), Color.white);
            tex.SetPixel(currentFrame, 60, Color.green);
            y[currentFrame] = observerValue();
            tex.SetPixel(currentFrame, Mathf.FloorToInt(y[currentFrame]), Color.red);
            tex.Apply();
            currentFrame++;
            currentFrame %= maxRecordFrame;
            image.uvRect = new Rect((float)currentFrame / maxRecordFrame, 0, 1,1);
        }
    }

    public delegate float ObserverValue();
}
