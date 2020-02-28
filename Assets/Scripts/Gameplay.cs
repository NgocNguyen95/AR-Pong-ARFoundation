using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : IntEventInvoker
{
    [SerializeField] GameObject BallPrefab;
    [SerializeField] GameObject PlayArea;

    [SerializeField] GameObject PowerUpPrefab;
    GameObject powerUp;

    [SerializeField] GameObject[] Players = new GameObject[5];

    Timer ballRespawnDelayTimer;
    Timer powerUpRespawnTimer;

    bool gameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // add listener for board placed event
        EventManager.AddListener(EventName.BoardPlacedEvent, HandleBoardPlacedEvent);

        // add listener for game started event
        EventManager.AddListener(EventName.GameStartedEvent, HandleGameStartedEvent);

        // add as listener for respawn ball event
        EventManager.AddListener(EventName.RespawnBallEvent, HandleRespawnBallEvent);

        // add listener for knocked out event
        EventManager.AddListener(EventName.KnockedOutEvent, HandleKnockedOutEvent);

        // add as invoker for power up respawned event
        unityEvents.Add(EventName.PowerUpRespawnedEvent, new PowerUpRespawnedEvent());
        EventManager.AddInvoker(EventName.PowerUpRespawnedEvent, this);

        // add listener for power up taken event
        EventManager.AddListener(EventName.PowerUpTakenEvent, HandlePowerUpTakenEvent);

        // create timer for ball respawn delay
        ballRespawnDelayTimer = gameObject.AddComponent<Timer>();
        ballRespawnDelayTimer.Duration = 0.5f;
        ballRespawnDelayTimer.AddTimerFinishedEventListener(HandleBallRespawnDelayTimerFinishedEvent);

        // create timer for power up respawn
        powerUpRespawnTimer = gameObject.AddComponent<Timer>();
        powerUpRespawnTimer.Duration = RandomPowerUpRespawnDuration();
        powerUpRespawnTimer.AddTimerFinishedEventListener(HandlePowerUpRespawnTimerFinished);
    }

    // Update is called once per frame
    void Update()
    {
         
    }
    
    /// <summary>
    /// Handles the BoardPlacedEvent
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleBoardPlacedEvent(int unused)
    {
        PlayArea = GameObject.FindGameObjectWithTag("PlayArea");
        
        // get all players and add to the players array
        for (int i = 1; i < Players.Length; i++)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player" + i);
            Players[i] = player;
        }
        gameOver = false;
    }  
    
    /// <summary>
    /// Handle the game started event       
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleGameStartedEvent(int unused)
    {
        RespawnBall();
        powerUpRespawnTimer.Run();
    }

    /// <summary>
    /// Respawn a ball when call and invoke ball respawned event
    /// </summary>
    void RespawnBall()
    {
        Instantiate(BallPrefab, PlayArea.transform);
    }

    /// <summary>
    /// Handle repsawn ball event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleRespawnBallEvent (int unused)
    {
        ballRespawnDelayTimer.Run();
    }

    /// <summary>
    /// Handle knocked out event
    /// </summary>
    /// <param name="playerKnockedOut">player who knocked out</param>
    void HandleKnockedOutEvent (int playerKnockedOut)
    {
        Destroy(Players[playerKnockedOut].gameObject);
    }

    void HandleBallRespawnDelayTimerFinishedEvent()
    {
        RespawnBall();
    }

    /// <summary>
    /// Take a random duration for power up timer
    /// </summary>
    /// <returns>return a random float number</returns>
    float RandomPowerUpRespawnDuration()
    {
        return Random.Range(30, 40);
    }

    void HandlePowerUpRespawnTimerFinished()
    {
        powerUp = Instantiate(PowerUpPrefab);
        unityEvents[EventName.PowerUpRespawnedEvent].Invoke(0);
    }

    /// <summary>
    /// Handle power up taken event
    /// </summary>
    /// <param name="unsued">unused</param>
    void HandlePowerUpTakenEvent (int unsued)
    {
        Destroy(powerUp.gameObject);
        Time.timeScale = 1;
        powerUpRespawnTimer.Duration = RandomPowerUpRespawnDuration();
        powerUpRespawnTimer.Run();
    }
}
