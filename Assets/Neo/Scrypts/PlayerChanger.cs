using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerChanger : MonoBehaviour
{
    

    private GameObject[] DragonObjects;
    private SpriteRenderer spriteRenderer;

    // インスペクターからスプライトを受け取っておく
    [SerializeField]
    private Sprite newSprite;

    // Update is called once per frame
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        DragonObjects = GameObject.FindGameObjectsWithTag("Dragon");

        if (DragonObjects.Length == 0)  //Dragonタグがついてる残りが0になれば
        {
            spriteRenderer.sprite = newSprite;
            GameManager.Clear();
        }
    }
}
