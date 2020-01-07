using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using DG.Tweening;

namespace g
{
    public class Goki : MonoBehaviour
    {
        [SerializeField]
        float speed = 1;
        [SerializeField]
        float power = 1;
        Vector3 target = Vector3.zero;
        [SerializeField]
        float width = 5;
        [SerializeField]
        float height = 5;
        SpriteRenderer spriteRenderer;
        private RaycastHit2D hit;
        bool isDead;
        Animator animator;
        Rigidbody2D rb;
        [SerializeField]
        float gravity;
        [SerializeField]
        GameObject shockEff;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            target = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), 0);
            transform.position = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), 0);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //マウスのポジションを取得してRayに代入
                hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (hit && !isDead)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    isDead = true;
                    transform.DORotate(new Vector3(0, 0, 180), 0.5f).SetRelative().SetEase(Ease.OutBack);
                    //Vector3 dir = Quaternion.AngleAxis(Random.Range(-90f, 90f), Vector3.up) * Vector3.up * power;
                    animator.speed = 0;
                    rb.gravityScale = gravity;
                    rb.velocity = new Vector2(Random.Range(-5, 5), 10);
                    //transform.DOMove(dir, 0.5f).SetRelative().SetEase(Ease.OutSine);
                    GameManager.Clear();
                }

                Instantiate(shockEff, ray.origin, Quaternion.identity);
            }

            //transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (!isDead)
            {
                if ((target - transform.position).magnitude < 1f)
                {
                    target = new Vector3(Random.Range(-width / 2, width / 2), Random.Range(-height / 2, height / 2), 0);
                    if (target.x - transform.position.x > 0)
                    {
                        spriteRenderer.flipX = false;
                    }
                    else
                    {
                        spriteRenderer.flipX = true;
                    }
                }
                else
                {
                    transform.Translate((target - transform.position).normalized * speed * Time.deltaTime);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            GizmosExtensions2D.DrawWireRect2D(Vector3.zero, width, height);
            Gizmos.DrawWireSphere(target, 1);
        }
    }

}

