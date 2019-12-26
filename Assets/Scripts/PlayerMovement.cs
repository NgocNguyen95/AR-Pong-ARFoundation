using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public bool drag = false;
    RigidbodyConstraints originRBConstraints;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originRBConstraints = rb.constraints;
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            drag = false;            
            //rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {                
                //Debug.Log("Ray hit " + hit.collider.name);

                if (hit.collider.name.Equals("PlayerControlDetector"))
                {
                    drag = true;
                    //rb.constraints = originRBConstraints;

                    if (drag == true)
                    {
                        float screenDist = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                        Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, screenDist));
                        transform.position = new Vector3(pos_move.x, transform.position.y, transform.position.z);
                    }
                }                
            }            
        }

        //Bounds
        if (transform.position.x >= 2.5)
        {
            //Debug.Log("Block");
            transform.position = new Vector3(2.5F, transform.position.y, transform.position.z);
        }
        if (transform.position.x <= -2.5)
        {
            //Debug.Log("Block");
            transform.position = new Vector3(-2.5F, transform.position.y, transform.position.z);
        }
    }
}
