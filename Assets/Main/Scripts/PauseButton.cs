using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    Camera managerCam;
    private Collider2D col;

    private void Start()
    {
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        col.enabled = !GameManager.IsGamePlaying;
        col = GetComponent<Collider2D>();
        if (Input.GetMouseButtonDown(0) && !GameManager.Instance.isPause && GameManager.Instance.isCanPause)
        {
            Ray ray = managerCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit)
            {
                if (hit.collider == col)
                {
                    GameManager.Instance.Pause();
                }
            }
        }
    }
}
