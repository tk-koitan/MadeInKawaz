using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SampleGame;

namespace SampleGame
{
    public class Kicker : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D ball;
        [SerializeField]
        private GameObject gestureHand;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float maxAngle;
        private bool isKicked;
        private AudioSource audioSource;
        private Vector3 fromPos;

        // Start is called before the first frame update
        void Start()
        {
            /*
            transform.rotation = Quaternion.Euler(0, 0, maxAngle);
            transform.DORotate(new Vector3(0, 0, -maxAngle), 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            */
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 touchScreenPosition = Input.mousePosition;

            // 10.0fに深い意味は無い。画面に表示したいので適当な値を入れてカメラから離そうとしているだけ.
            touchScreenPosition.z = 10.0f;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(touchScreenPosition);
            if (Input.GetMouseButtonDown(0) && !isKicked)
            {
                /*
                isKicked = true;
                arrow.SetActive(false);
                ball.velocity = transform.rotation * Vector3.up * speed;
                audioSource.Play();
                */
                fromPos = mousePos;
            }

            if(Input.GetMouseButton(0) && !isKicked)
            {
                if((mousePos-fromPos).magnitude > 1)
                {
                    isKicked = true;
                    ball.linearVelocity = (mousePos - fromPos).normalized * speed;
                    audioSource.Play();
                    gestureHand.SetActive(false);
                }
            }

            ball.transform.localScale = Vector3.one * (0.4f + (-2.5f - ball.transform.position.y) / 50f);
        }
    }
}