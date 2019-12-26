using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public GameObject ball;

    float ttime = 1000f;
    float compare = 0.2f;
    Vector3 lastBallLoc;
    float scale = 1f;
    float targetScale = 7.5f;
    float rotSpeed = 100f;
    bool throwOn = false;
    float rotCounter = 1000f;

    public GameObject UIManagerScript;
    UIManager UIscript;

    // Start is called before the first frame update
    void Start()
    {           
        UIscript = UIManagerScript.GetComponent<UIManager>();        
    }

    // Update is called once per frame
    void Update()
    {
        //if (ball == null  && ScoreManager.gameStarted)
        //{
        //    ball = GameObject.FindGameObjectWithTag("Ball");
            
        //}

        if (ball != null)
        {            
            lastBallLoc = ball.transform.position;            
        }


        //if (ttime <= Time.time && ScoreManager.gameStarted == true)
        //{            
        //    if ((lastBallLoc.x >= ball.transform.position.x - compare && lastBallLoc.x <= ball.transform.position.x + compare) || (lastBallLoc.z >= ball.transform.position.z - compare && lastBallLoc.z <= ball.transform.position.z + compare))
        //    {
        //        throwOn = true;
        //        rotCounter = Time.time + 1;
        //        Counter();
        //        Debug.Log("1");
        //    }
        //    lastBallLoc = ball.transform.position;            
        //}

        if (throwOn == true)
        {
            if (transform.localScale.x <= targetScale)
            {
                transform.localScale += new Vector3(0.1f, 0, 0);
            }
            transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
            Debug.Log("2");
        }

        if (Time.time >= rotCounter)
        {
            throwOn = false;
            transform.localScale = new Vector3(0.8f, transform.localScale.y, transform.localScale.z);
            Counter();
            Debug.Log("3");
        }


    }

    private void Counter ()
    {
        ttime = Time.time + 3;
    }

    private void Throw ()
    {
        transform.localScale = new Vector3(targetScale, transform.localScale.y, transform.localScale.z);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 180, 0), rotSpeed * Time.deltaTime);
    }
}
