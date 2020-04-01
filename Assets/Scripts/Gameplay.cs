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
    [SerializeField] GameObject BlockWallPrefab;
    GameObject powerUp;

    [SerializeField] GameObject[] Players = new GameObject[5];

    Timer ballRespawnDelayTimer;
    Timer powerUpRespawnTimer;
    Timer enemyTakePowerUpTimer;
    Timer enemyUsePowerUpTimer;

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

        // add this as invoker for power up taken event
        unityEvents.Add(EventName.PowerUpTakenEvent, new PowerUpTakenEvent());
        EventManager.AddInvoker(EventName.PowerUpTakenEvent, this);

        // create timer for ball respawn delay
        ballRespawnDelayTimer = gameObject.AddComponent<Timer>();
        ballRespawnDelayTimer.Duration = 0.5f;
        ballRespawnDelayTimer.AddTimerFinishedEventListener(HandleBallRespawnDelayTimerFinishedEvent);

        // create timer for power up respawn
        powerUpRespawnTimer = gameObject.AddComponent<Timer>();
        powerUpRespawnTimer.Duration = RandomPowerUpRespawnDuration();
        powerUpRespawnTimer.AddTimerFinishedEventListener(HandlePowerUpRespawnTimerFinished);

        // create timer for enemies take powerUp
        enemyTakePowerUpTimer = gameObject.AddComponent<Timer>();
        enemyTakePowerUpTimer.Duration = 0.3f;
        enemyTakePowerUpTimer.AddTimerFinishedEventListener(HandleEnemyTakePowerUpTimerFinished);

        // create timer for enemis use powerUp
        enemyUsePowerUpTimer = gameObject.AddComponent<Timer>();
        enemyUsePowerUpTimer.Duration = 3f;
        enemyUsePowerUpTimer.AddTimerFinishedEventListener(EnemyUsePowerUp);
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
        return Random.Range(20, 40);
    }

    void HandlePowerUpRespawnTimerFinished()
    {
        powerUp = Instantiate(PowerUpPrefab, PlayArea.transform);
        unityEvents[EventName.PowerUpRespawnedEvent].Invoke(0);

        enemyTakePowerUpTimer.Run();
    }

    /// <summary>
    /// Handle power up taken event
    /// </summary>
    /// <param name="playerTookPowerUp">player number who took the power up</param>
    void HandlePowerUpTakenEvent (int playerTookPowerUp)
    {
        Destroy(powerUp.gameObject);
        Time.timeScale = 1;
        powerUpRespawnTimer.Duration = RandomPowerUpRespawnDuration();
        powerUpRespawnTimer.Run();

        if (playerTookPowerUp == 1)
        {
            enemyTakePowerUpTimer.Stop();
        }
        else
        {
            enemyUsePowerUpTimer.Run();
        }
    }

    void HandleEnemyTakePowerUpTimerFinished ()
    {
        unityEvents[EventName.PowerUpTakenEvent].Invoke(0);
    }

    void EnemyUsePowerUp()
    {
        Instantiate(BlockWallPrefab, PlayArea.transform);
    }
}
