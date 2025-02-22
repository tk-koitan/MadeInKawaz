using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D; (removed because of build error By y_y: 2025_02_22
using UnityEngine;

public class Sensei : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Sprite> senseiSprite = new List<Sprite>();
    public GameEvent gameEvent;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gameEvent.senseiNoKigen);
        gameObject.GetComponent<SpriteRenderer>().sprite = senseiSprite[(int)(gameEvent.senseiNoKigen * (senseiSprite.Count - 1))];
    }
}
