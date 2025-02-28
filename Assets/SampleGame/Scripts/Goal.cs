using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleGame
{
    public class Goal : MonoBehaviour
    {
        private AudioSource audioSource;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!GameManager.ClearFlag)
            {
                audioSource.Play();
                GameManager.Clear();
            }                        
        }
    }
}

