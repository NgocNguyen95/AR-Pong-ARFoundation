using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : IntEventInvoker
{
    public Button restartBTN;
    public Text scoreTXT;

    // Start button reference
    [SerializeField] Button StartButton;

    private void Awake()
    {
        // Add listener for place board event
        EventManager.AddListener(EventName.BoardPlacedEvent, FirstLoad);

        // add as invoker for game started event
        unityEvents.Add(EventName.GameStartedEvent, new GameStartedEvent());
        EventManager.AddInvoker(EventName.GameStartedEvent, this);

        // add listener for knocked out event
        EventManager.AddListener(EventName.KnockedOutEvent, HandleKnockedOutEvent);

        // add as invoker for game over event
        unityEvents.Add(EventName.GameOverEvent, new GameOverEvent());
        EventManager.AddInvoker(EventName.GameOverEvent, this);
    }

    // Start is called before the first frame update
    void Start()
    {
        restartBTN.onClick.AddListener(RestartGame);
        restartBTN.gameObject.SetActive(false);
        scoreTXT.gameObject.SetActive(false);

        // Start button
        StartButton.onClick.AddListener(StartGame);
        StartButton.gameObject.SetActive(false);        
    }

    // Event listener when StartButton OnClick
    void StartGame()
    {
        Time.timeScale = 1;
        StartButton.gameObject.SetActive(false);
        unityEvents[EventName.GameStartedEvent].Invoke(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RestartGame ()
    {
        Debug.Log("Game restarted");
        restartBTN.gameObject.SetActive(false);
        unityEvents[EventName.GameOverEvent].Invoke(0);
    }

    /// <summary>
    /// Handle the board place event
    /// </summary>
    /// <param name="unused">unused</param>
    void FirstLoad (int unused)
    {
        Time.timeScale = 0;
        StartButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handle knocked out event
    /// </summary>
    /// <param name="playerKnockedOut">player who is knocked out</param>
    void HandleKnockedOutEvent (int playerKnockedOut)
    {
        if (playerKnockedOut == 1)
        {
            EndGame();
        }
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    void EndGame()
    {
        Time.timeScale = 0;
        restartBTN.gameObject.SetActive(true);
    }
}
