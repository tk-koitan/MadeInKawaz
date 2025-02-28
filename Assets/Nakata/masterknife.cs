using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class masterknife : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(demo());
        IEnumerator demo()
        {
            yield return new WaitForSeconds(0.5f);
            cut();
            yield return new WaitForSeconds(0.25f);
            cut();
            yield return new WaitForSeconds(0.25f);
            cut();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timekeeper.time<=1.5f)
        {
            transform.Translate(7.998f*Time.deltaTime, 1.5f*Time.deltaTime, 0);
        }
    }
    void cut()
    {
        transform.Translate(0, -2, 0);
        StartCoroutine(Up());
        IEnumerator Up()
        {
            yield return new WaitForSeconds(0.1f);
            transform.Translate(0, 2, 0);
        }
    }
}
