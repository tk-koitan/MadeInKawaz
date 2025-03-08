using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Straw : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField] private float decreasRate = 0.1f;

    void Update()
    {
        if(Kamipakku.getButtonFlag() == true)
        {
            transform.Translate(0,this.speed * Time.deltaTime,0);
            //this.speed *= 0.98f;


            // revise not to depend on frame rate. (by y_y 2025_03_01
            var beta = 1f - Mathf.Pow(1 - decreasRate, 60f * Time.deltaTime);
            this.speed = Mathf.Lerp(this.speed, 0f, beta);

            Debug.Log(speed);
        }
        else if(Input.GetMouseButtonDown(0))
        {
            this.speed = -2.0f;
        }
    }
}
