using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy2;

    bool isFreeze2 = false;
    bool isFreeze3 = false;
    bool isFreeze4 = false;

    float speed = 10;
    float movAmount = 0.05f;

    bool[] knockedPlayers = { false, false, false, false, false };    

    // getting the ball coordinates
    private Vector3 ballCoord;
    [SerializeField]GameObject ball;
    int i = 0;

    public bool move = true;

    Timer freezeEffectTimer;
    int playerFrozen;

    // Start is called before the first frame update
    void Start()
    {
        // add listener for ball respawned event
        EventManager.AddListener(EventName.BallRespawnedEvent, HandleBallRespawnedEvent);

        // add listener for the knocked out event
        EventManager.AddListener(EventName.KnockedOutEvent, HandleKnockedOutEvent);

        // add listener for the player be freeze selected event
        EventManager.AddListener(EventName.PlayerBeFreezeSelectedEvent, HandlePlayerBeFreezeSelectedEvent);

        // add timer for the freeze effect
        freezeEffectTimer = gameObject.AddComponent<Timer>();
        freezeEffectTimer.Duration = 2;
        freezeEffectTimer.AddTimerFinishedEventListener(HandleFreezeEffectTimerFinished);
    }

    // Update is called once per frame
    void Update()
    {
        //Reading the ball's coordinates
        if (ball != null)
        {
            ballCoord = ball.transform.position;
            i = 0;

            //Tracking the ball and moving enemies
            if (move && ball.GetComponent<Ball>().ballmoves)
            {
                //Enemy 1
                if (!knockedPlayers[3] && !isFreeze3)
                {
                    if (enemy3.transform.position.x <= ballCoord.x && enemy3.transform.position.x <= 2.5F)
                    {
                        enemy3.transform.position = new Vector3(
                            enemy3.transform.position.x + movAmount,
                            enemy3.transform.position.y,
                            enemy3.transform.position.z);
                    }

                    if (enemy3.transform.position.x >= ballCoord.x && enemy3.transform.position.x >= -2.5F)
                    {
                        enemy3.transform.position = new Vector3(
                            enemy3.transform.position.x - movAmount,
                            enemy3.transform.position.y,
                            enemy3.transform.position.z);
                    }
                }
                

                //Enemy 2
                if (!knockedPlayers[4] && !isFreeze4)
                {
                    if (enemy4.transform.position.z <= ballCoord.z && enemy4.transform.position.z <= 2.5F)
                    {
                        enemy4.transform.position = new Vector3(
                            enemy4.transform.position.x,
                            enemy4.transform.position.y,
                            enemy4.transform.position.z + movAmount);
                    }

                    if (enemy4.transform.position.z >= ballCoord.z && enemy4.transform.position.z >= -2.5F)
                    {
                        enemy4.transform.position = new Vector3(
                            enemy4.transform.position.x,
                            enemy4.transform.position.y,
                            enemy4.transform.position.z - movAmount);
                    }
                }
                

                //Enemy 3
                if (!knockedPlayers[2] && !isFreeze2)
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
            }
        }
    }

    /// <summary>
    /// Handle ball respawned event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleBallRespawnedEvent (int unused)
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
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

    /// <summary>
    /// Handle the player be freeze selected event
    /// </summary>
    /// <param name="playerGotFreeze">player who got freeze effect</param>
    void HandlePlayerBeFreezeSelectedEvent (int playerGotFreeze)
    {
        if (playerGotFreeze == 2)
        {
            isFreeze2 = true;
        }
        else if (playerGotFreeze == 3)
        {
            isFreeze3 = true;
        }
        else if (playerGotFreeze == 4)
        {
            isFreeze4 = true;
        }

        playerFrozen = playerGotFreeze;
        freezeEffectTimer.Run();
    }

    void HandleFreezeEffectTimerFinished ()
    {
        if (playerFrozen == 2)
        {
            isFreeze2 = false;
        }
        else if (playerFrozen == 3)
        {
            isFreeze3 = false;
        }
        else if (playerFrozen == 4)
        {
            isFreeze4 = false;
        }
    }
}
