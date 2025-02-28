using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hitode
{
    public class HitodeGenerator : MonoBehaviour
    {
        [SerializeField] GameObject hitodePrefab;
        [SerializeField] Sprite[] hitodeSprites;

        Pool pool;

        float screenW, screenH;

        float timer;
        [SerializeField] float interval = 0.5f;
        // Start is called before the first frame update
        void Start()
        {
            screenH = Camera.main.orthographicSize + 0.5f;
            screenW = screenH * Camera.main.aspect + 0.5f;
            timer = 0.0f;

            pool = gameObject.GetComponent<Pool>();

            //for(int i = 0; i < 10; ++i)
            //{
            //    Generate();
            //}
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            if(timer >= interval)
            {
                Generate();
                timer = 0.0f;
            }
        }

        void Generate()
        {
            Vector3 generatePos = new Vector3(Random.Range(0.0f, screenW + 4.0f), screenH + 2.0f);
            Vector3 dir = new Vector3(Random.Range(-1.0f, -0.5f), -1.0f);
            GameObject newHitode = pool.GetInstance();
            newHitode.GetComponent<SpriteRenderer>().sprite = hitodeSprites[Random.Range(0, hitodeSprites.Length)];
            newHitode.GetComponent<HitodeContoroller>().Initialize(generatePos, dir, 30.0f);
        }
    }
}