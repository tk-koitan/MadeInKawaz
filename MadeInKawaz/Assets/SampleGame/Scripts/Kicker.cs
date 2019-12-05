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
        private GameObject arrow;
        [SerializeField]
        private float speed;
        [SerializeField]
        private float maxAngle;
        private bool isKicked;
        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, maxAngle);
            transform.DORotate(new Vector3(0, 0, -maxAngle), 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !isKicked)
            {
                isKicked = true;
                arrow.SetActive(false);
                ball.velocity = transform.rotation * Vector3.up * speed;
                audioSource.Play();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                isKicked = false;
                arrow.SetActive(true);
                ball.velocity = Vector2.zero;
                ball.transform.position = transform.position;
            }

            ball.transform.localScale = Vector3.one * (0.4f + (-2.5f - ball.transform.position.y) / 50f);
        }
    }
}