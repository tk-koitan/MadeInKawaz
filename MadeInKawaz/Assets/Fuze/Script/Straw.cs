using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Straw : MonoBehaviour
{
    [SerializeField]
    float speed;
    void Update()
    {
        if(Kamipakku.getButtonFlag() == true)
        {
            transform.Translate(0,this.speed * Time.deltaTime,0);
            this.speed *= 0.98f;
        }
        else if(Input.GetMouseButtonDown(0))
        {
            this.speed = -2.0f;
        }
    }
}
