using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThing : MonoBehaviour
{
    //こいたんくんさんのゴキブリパクりました

    [SerializeField]
    float speed = 100;
    Vector3 target = Vector3.zero;
    [SerializeField]
    float width = 2000;
    [SerializeField]
    float height = 1000;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        target = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2) + 3, 0);
        transform.position = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2) + 3, 0);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((target - transform.position).magnitude < 1f)
        {
            target = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2) + 3, 0);
            if (target.x - transform.position.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
            //Debug.Log(target.x + " " + target.y);
        }
        else
        {
            transform.Translate((target - transform.position).normalized * speed * Time.deltaTime);
        }
    }
}
