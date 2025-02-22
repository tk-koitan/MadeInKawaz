using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SpriteMove : MonoBehaviour
{
    private float elapsedTime = 0;
    SpriteRenderer sprite;
    Rigidbody2D rigidbody2D;
    Animator animator;
    public bool stopDancing = false;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!stopDancing)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 1)
            {
                int mode = Random.Range(0, 2);   
                if( (0b01& mode) > 0) 
                {
                    this.transform.DOScale(Random.Range(0.5f, 2f), 0.5f);
                    this.transform.DORotate(RandomVectorAngle(), 0.1f);
                }
                else elapsedTime = 0;
            }
            // sprite.color = Random.ColorHSV();
        }


        

    }
    private Vector3 RandomVectorAngle()
    {
        return new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), 0);
    }
    void OnMouseDown()
    {
            sprite.color = Color.white;
            this.transform.DOScale(1, 0.1f);
            this.transform.DORotate(new Vector3(0, 0, 0), 0.1f);
            rigidbody2D.isKinematic = true;
            rigidbody2D.linearVelocity = Vector3.zero;
            stopDancing = true;
            animator.enabled = false;

    }
}
