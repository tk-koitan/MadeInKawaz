using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obake : MonoBehaviour
{
    private Transform tra;
    [SerializeField]
    private Transform rangeA;
    [SerializeField]
    private Transform rangeB;

    // Start is called before the first frame update
    void Start()
    {
        tra = this.transform;
        float x = Random.Range(rangeA.position.x, rangeB.position.x);
        float y = Random.Range(rangeA.position.y, rangeB.position.y);
        tra.position = new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
