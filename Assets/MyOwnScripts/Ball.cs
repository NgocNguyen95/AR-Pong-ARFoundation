using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public float speed;
    Rigidbody rb;
    Vector3 randomDir;

    float timeToMove;

    public bool ballmoves;
    
    // Start is called before the first frame update
    void Start()
    {
        ballmoves = false;
        timeToMove = Time.time + 2f;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= timeToMove && !ballmoves)
        {            
            Move();
        }

        //For constant velocity
        if (ballmoves)
        {
            //Get current speed and direction
            Vector3 direction = rb.velocity;
            float currentSpeed = direction.magnitude;
            direction.Normalize();

            if (currentSpeed != speed)
            {
                rb.velocity = direction * speed;
            }
        }
    }

    void Move()
    {
        //Determines a random starting direction
        transform.position = new Vector3(0, 0.5f, 0);
        randomDir.x = Random.Range(-1F, 1F);
        randomDir.z = Random.Range(-1F, 1F);
        //randomDir.x = 1;
        randomDir.Normalize();
        rb.velocity = new Vector3(randomDir.x, randomDir.y, randomDir.z) * speed;

        ballmoves = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Cylinder")
        {
            gameObject.GetComponent<SphereCollider>().isTrigger = false;
        }
    }
}