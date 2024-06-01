using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamipakkuCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.name == "StrewCollider")
        {
        //ゲームクリア
        GameManager.Clear();
        Debug.Log("!");
        }
    }
}
