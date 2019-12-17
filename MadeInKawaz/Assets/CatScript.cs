using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    private float t = 0;
    private Vector3 startPosition;
    private Vector3 target;
    [SerializeField]
    private float timeToReachTarget = 6f;
    void Start()
    {
        startPosition = transform.position;
        target = new Vector3(-14, 0, 0);
    }
    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        transform.position = Vector3.Lerp(startPosition, target, t);
    }
}
