using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonKiller : MonoBehaviour
{
   

    
    private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject,0.25f);
         
       
    }
}
