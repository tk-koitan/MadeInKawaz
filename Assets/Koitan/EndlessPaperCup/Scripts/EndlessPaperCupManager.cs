using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace EndlessPaperCup
{
    public class EndlessPaperCupManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject cupPrefab;
        private Cup[] cups;
        [SerializeField]
        private int cupCount = 3;
        private float cupY = 1.5f;
        [SerializeField]
        private float height = 3.5f;
        [SerializeField]
        private float cupMaxX = 5f;
        [SerializeField]
        private Transform centerObj;
        [SerializeField]
        private int moveCount = 4;
        [SerializeField]
        private float interval = 1f;
        private bool isMoving;
        private bool isOpen;
        [SerializeField]
        private TextMeshProUGUI countMesh;
        [SerializeField]
        private Transform item;
        [SerializeField]
        private float itemY;
        private int ansNum;
        private bool isCanSelect;
        private bool isSelected;
        [SerializeField]
        private GameObject itemEff, atariEff, atariEff2, hazureEff;        
        private AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {            
            cups = new Cup[cupCount];
            for (int i = 0; i < cupCount; i++)
            {
                cups[i] = Instantiate(cupPrefab, new Vector3(2 * cupMaxX * i / (cupCount - 1) - cupMaxX, cupY, 0), Quaternion.identity).GetComponent<Cup>();
                cups[i].id = i;
                cups[i].gameObject.SetActive(true);
            }
            ansNum = Random.Range(0, cupCount);
            cups[ansNum].isAnswer = true;
            item.position = new Vector3(cups[ansNum].transform.position.x, itemY, 0);
            //item.SetParent(cups[ansNum].itemTransform);
            //item.transform.localPosition = Vector3.zero;

            StartCoroutine(CupStart(cups[Random.Range(0, cupCount)].transform));
            countMesh.text = moveCount.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && isCanSelect && !isSelected)
            {
                Ray ray = new Ray();
                RaycastHit hit = new RaycastHit();
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //マウスクリックした場所からRayを飛ばし、オブジェクトがあればtrue 
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
                {
                    Cup cup = hit.collider.GetComponent<Cup>();
                    Debug.Log(cup.id);
                    CupOpen(cup);
                    isSelected = true;
                    if (cup.isAnswer)
                    {
                        GameManager.Clear();
                        atariEff.transform.position = cup.itemTransform.position;
                        atariEff.SetActive(true);
                        atariEff2.transform.position = cup.itemTransform.position;
                        atariEff2.SetActive(true);
                        itemEff.SetActive(true);
                    }
                    else
                    {
                        hazureEff.transform.position = cup.itemTransform.position;
                        hazureEff.SetActive(true);
                    }
                }
            }
        }

        void CupOpen(Cup cup)
        {
            //cup.transform.rotation = Quaternion.identity;
            Vector3 p = Camera.main.transform.position;
            p.y = cup.transform.position.y;
            cup.transform.LookAt(p);
            cup.transform.Rotate(0, 180, 0);
            cup.animator.Play("open");
        }

        void CupClose(Cup cup)
        {
            Vector3 p = Camera.main.transform.position;
            p.y = cup.transform.position.y;
            cup.transform.LookAt(p);
            cup.transform.Rotate(0, 180, 0);
            cup.animator.Play("close");
        }

        void CupOpend(Cup cup)
        {
            Vector3 p = Camera.main.transform.position;
            p.y = cup.transform.position.y;
            cup.transform.LookAt(p);
            cup.transform.Rotate(0, 180, 0);
            cup.animator.Play("idle_air");
        }

        void CupMove(Transform cupA, Transform cupB, bool reverse = false)
        {
            //isMoving = true;
            Vector3 center = (cupA.position + cupB.position) / 2f;
            centerObj.transform.position = center;
            cupA.SetParent(centerObj);
            cupB.SetParent(centerObj);
            centerObj.DORotate(new Vector3(0, 180 * (reverse ? -1 : 1), 0), interval).SetRelative()
                .OnComplete(() =>
                {
                    cupA.SetParent(null);
                    cupB.SetParent(null);
                    isMoving = false;
                });
        }

        IEnumerator CupStart(Transform cup)
        {
            //CupOpen(cup);
            //yield return new WaitForSeconds(1f);
            //CupClose(cup);
            CupOpen(cups[ansNum]);
            itemEff.SetActive(true);
            yield return new WaitForSeconds(2f);
            /*
            for (int i = 0; i < cupCount; i++)
            {
                cups[i].transform.DOMoveY(cupY, 1f).SetEase(Ease.OutCubic);
            }
            */
            itemEff.GetComponent<ParticleSystem>().Stop();
            CupClose(cups[ansNum]);
            yield return new WaitForSeconds(1f);
            item.gameObject.SetActive(false);
            itemEff.SetActive(false);
            while (moveCount > 0)
            {
                isMoving = true;
                moveCount--;
                int a = Random.Range(0, cupCount);
                int b = Random.Range(0, cupCount - 1);
                if (a == b) b++;
                CupMove(cups[a].transform, cups[b].transform, Random.Range(0, 1.0f) < 0.5f ? false : true);
                countMesh.text = moveCount.ToString();
                yield return new WaitForSeconds(interval);
            }
            //微妙にタイミングがずれる？
            //yield return new WaitForSeconds(interval);
            countMesh.text = "えらべ！";
            countMesh.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f).SetLoops(4, LoopType.Restart);
            item.position = new Vector3(cups[ansNum].transform.position.x, itemY, 0);
            item.gameObject.SetActive(true);
            isCanSelect = true;
        }
    }
}