using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Hitode
{
    public class Basket : MonoBehaviour
    {
        int hitodeNum;
        [SerializeField] int hitodeMax = 3;
        [SerializeField] TextMeshProUGUI text;
        // Start is called before the first frame update
        void Start()
        {
            hitodeNum = 0;
            text.text = "あと" + (hitodeMax - hitodeNum).ToString() + "こ";
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(hitodeNum < hitodeMax)
            {
                GameObject hitodeObj = collision.gameObject;
                HitodeContoroller hitode = hitodeObj.GetComponent<HitodeContoroller>();
                hitode.Catch();
                hitodeObj.transform.parent = transform;
                hitodeNum++;
                text.text = "あと" + (hitodeMax - hitodeNum).ToString() + "こ";
            }
            // 一定個数集めたらクリア
            if(hitodeNum == hitodeMax)
            {
                GameManager.Clear();
            }
        }
    }
}
