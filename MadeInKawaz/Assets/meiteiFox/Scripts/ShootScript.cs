using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using UnityEngine.Experimental.PlayerLoop;

public class ShootScript : MonoBehaviour
{
    public GameObject Ink;
    int InkShootInterval = 10;
    GameObject[] Inks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        if (InkShootInterval > 0)
        {
            InkShootInterval--;
        }
        if (Input.GetMouseButton(0))
        {
            if (InkShootInterval == 0)
            {
                InkShootInterval = 3;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject nowInk = Instantiate(Ink, hit.point, Quaternion.identity);
                    var InksList = Inks.ToList();
                    InksList.Add(nowInk);
                    Inks = InksList.ToArray();
                }
            }
        }
//
    }
}
