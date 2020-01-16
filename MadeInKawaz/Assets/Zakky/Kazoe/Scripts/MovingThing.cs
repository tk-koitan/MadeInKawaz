using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingThing : MonoBehaviour
{
    //こいたんくんさんのゴキブリパクりました
    GameObject numberButton;

    [SerializeField]
    float speed = 100;
    [SerializeField]
    float width = 2000;
    [SerializeField]
    float height = 1000;

    SpriteRenderer spriteRenderer;
    Vector3 target = Vector3.zero;

    NumberButton numberButtonScript;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        numberButton = GameObject.Find("NumberButton");
        animator = GetComponent<Animator>();
        numberButtonScript = numberButton.GetComponent<NumberButton>();
        target = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2) + 3, 0);
        transform.position = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2) + 3, 0);
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        GetComponent<Animator>().speed = Random.Range(0.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (numberButtonScript.stageClear)
        {
            animator.speed = 2f;
            speed = 20f;
        }
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
