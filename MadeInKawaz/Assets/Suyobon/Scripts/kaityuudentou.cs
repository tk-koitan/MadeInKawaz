using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kaityuudentou : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Rigidbody2D rb;
    [SerializeField]
    private GameObject obake;
    [SerializeField]
    private GameObject flashLight;
    [SerializeField]
    private Sprite obake_mabusii;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 flashPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 5f));
        flashPos.z = 5f;
        rb.MovePosition(flashPos);

        if(Input.GetMouseButton(0))
        {
            GameObject clickObj = null;
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(ray, out hit))
            {
                clickObj = hit.collider.gameObject;

                if(clickObj = obake)
                {
                    obake.GetComponent<SpriteRenderer>().sprite = obake_mabusii;
                    obake.GetComponent<Animator>().SetBool("clear", true);
                    GetComponent<AudioSource>().Play();
                    GameManager.Clear();
                }
            }
            else
            {
                return;
            }
        }
    }
}
