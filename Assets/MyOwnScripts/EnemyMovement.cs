using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    float speed = 10;
    float movAmount = 0.05f;

    bool[] knockedPlayers = { false, false, false, false, false };    

    //getting the ball coordinates
    private Vector3 ballCoord;
    GameObject ball;
    int i = 0;

    public bool move = true;

    // Start is called before the first frame update
    void Start()
    {
        // add listener for ball respawned event
        EventManager.AddListener(EventName.BallRespawnedEvent, HandleBallRespawnedEvent);

        // add listner for the knocked out event
        EventManager.AddListener(EventName.KnockedOutEvent, HandleKnockedOutEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (ball == null)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
        }
        
        //Reading the ball's coordinates
        if (ball != null)
        {
            ballCoord = ball.transform.position;
            i = 0;

            //Tracking the ball and moving enemies
            if (move && ball.GetComponent<Ball>().ballmoves)
            {
                //Enemy 1
                if (!knockedPlayers[3])
                {
                    if (enemy1.transform.position.x <= ballCoord.x && enemy1.transform.position.x <= 2.5F)
                    {
                        enemy1.transform.position = new Vector3(
                            enemy1.transform.position.x + movAmount,
                            enemy1.transform.position.y,
                            enemy1.transform.position.z);
                    }

                    if (enemy1.transform.position.x >= ballCoord.x && enemy1.transform.position.x >= -2.5F)
                    {
                        enemy1.transform.position = new Vector3(
                            enemy1.transform.position.x - movAmount,
                            enemy1.transform.position.y,
                            enemy1.transform.position.z);
                    }
                }
                

                //Enemy 2
                if (!knockedPlayers[4])
                {
                    if (enemy2.transform.position.z <= ballCoord.z && enemy2.transform.position.z <= 2.5F)
                    {
                        enemy2.transform.position = new Vector3(
                            enemy2.transform.position.x,
                            enemy2.transform.position.y,
                            enemy2.transform.position.z + movAmount);
                    }

                    if (enemy2.transform.position.z >= ballCoord.z && enemy2.transform.position.z >= -2.5F)
                    {
                        enemy2.transform.position = new Vector3(
                            enemy2.transform.position.x,
                            enemy2.transform.position.y,
                            enemy2.transform.position.z - movAmount);
                    }
                }
                

                //Enemy 3
                if (!knockedPlayers[2])
                {
                    if (enemy3.transform.position.z <= ballCoord.z && enemy3.transform.position.z <= 2.5F)
                    {
                        enemy3.transform.position = new Vector3(
                            enemy3.transform.position.x,
                            enemy3.transform.position.y,
                            enemy3.transform.position.z + movAmount);
                    }

                    if (enemy3.transform.position.z >= ballCoord.z && enemy3.transform.position.z >= -2.5F)
                    {
                        enemy3.transform.position = new Vector3(
                            enemy3.transform.position.x,
                            enemy3.transform.position.y,
                            enemy3.transform.position.z - movAmount);
                    }
                }                
            }
        }

        //Resetting accidental rotation
        //enemy1.transform.Rotate(0, 0, 0, Space.Self);
        //enemy2.transform.Rotate(0, 0, 0, Space.Self);
        //enemy3.transform.Rotate(0, 0, 0, Space.Self);
    }

    /// <summary>
    /// Handle ball respawned event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleBallRespawnedEvent (int unused)
    {
        ball = GameObject.FindWithTag("Ball");        
    }

    /// <summary>
    /// Handle the knocked out event
    /// </summary>
    /// <param name="playerKnockedOut">player who knocked out</param>
    void HandleKnockedOutEvent (int playerKnockedOut)
    {
        if (playerKnockedOut != 1)
        {
            knockedPlayers[playerKnockedOut] = true;
        }
    }
}
