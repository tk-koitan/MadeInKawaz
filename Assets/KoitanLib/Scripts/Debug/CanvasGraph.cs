using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasGraph : Graphic
{
    /// <summary>
    /// メッシュに設定したいテクスチャをここで指定
    /// </summary>
    [SerializeField]
    private Texture2D texture_;

    [SerializeField]
    private Vector2 offset;

    /// <summary>
    /// メッシュに設定するテクスチャの指定
    /// </summary>
    public override Texture mainTexture
    {
        get
        {
            // ここで設定したいテクスチャを返すようにする
            return texture_;
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        // 左上
        UIVertex lt = UIVertex.simpleVert;
        lt.position = new Vector3(-50, 50, 0);
        lt.uv0 = new Vector2(0, 1) + offset;

        // 右上
        UIVertex rt = UIVertex.simpleVert;
        rt.position = new Vector3(50, 50, 0);
        rt.uv0 = new Vector2(1, 1) + offset;

        // 右下
        UIVertex rb = UIVertex.simpleVert;
        rb.position = new Vector3(50, -50, 0);
        rb.uv0 = new Vector2(1, 0) + offset;

        // 左下
        UIVertex lb = UIVertex.simpleVert;
        lb.position = new Vector3(-50, -50, 0);
        lb.uv0 = new Vector2(0, 0) + offset;

        vh.AddUIVertexQuad(new UIVertex[] {
            lb, rb, rt, lt
        });        
    }
}
