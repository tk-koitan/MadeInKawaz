using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThingSpawner : MonoBehaviour
{
    //数え上げるやつ
    [SerializeField]
    GameObject movingThing;
    //正しい答え
    [System.NonSerialized]
    public int correctNumber;
    // Start is called before the first frame update
    void Awake()
    {
        correctNumber = Random.Range(0, 4) + 3;
        for (int i = 0; i < correctNumber; i++)
        {
            //いっぱいだす
            Instantiate(movingThing);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
