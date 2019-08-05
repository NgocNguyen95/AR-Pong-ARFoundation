using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : IntEventInvoker
{
    [SerializeField] GameObject BallPrefab;
    [SerializeField] GameObject PlayArea;

    [SerializeField] GameObject[] Players = new GameObject[5];
    
    // Start is called before the first frame update
    void Start()
    {
        // add listener for board placed event
        EventManager.AddListener(EventName.BoardPlacedEvent, HandleBoardPlacedEvent);

        // add listener for game started event
        EventManager.AddListener(EventName.GameStartedEvent, HandleGameStartedEvent);

        // add as invoker for ball respawned event
        unityEvents.Add(EventName.BallRespawnedEvent, new BallRespawnedEvent());
        EventManager.AddInvoker(EventName.BallRespawnedEvent, this);

        // add as listener for goal event
        EventManager.AddListener(EventName.GoalEvent, HandleGoalEvent);

        // add listener for game over event
        EventManager.AddListener(EventName.GameOverEvent, HandleGameOverEvent);

        // add listener for knocked out event
        EventManager.AddListener(EventName.KnockedOutEvent, HandleKnockedOutEvent);
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
    }  
    
    /// <summary>
    /// Handle the game started event       
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleGameStartedEvent(int unused)
    {
        RespawnBall();
    }

    /// <summary>
    /// Respawn a ball when call and invoke ball respawned event
    /// </summary>
    void RespawnBall()
    {
        Instantiate(BallPrefab, PlayArea.transform);
        unityEvents[EventName.BallRespawnedEvent].Invoke(0);
    }

    /// <summary>
    /// Handle the goal event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleGoalEvent (int unused)
    {
        RespawnBall();
    }

    /// <summary>
    /// Handle game over event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleGameOverEvent (int unused)
    {
        Destroy(PlayArea.gameObject);
    }

    /// <summary>
    /// Handle knocked out event
    /// </summary>
    /// <param name="playerKnockedOut">player who knocked out</param>
    void HandleKnockedOutEvent (int playerKnockedOut)
    {
        Destroy(Players[playerKnockedOut].gameObject);
    }
}
