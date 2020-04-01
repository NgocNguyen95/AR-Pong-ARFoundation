using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : IntEventInvoker
{
    [SerializeField] Button restartBTN;
    [SerializeField] Text scoreTXT;

    // Start button reference
    [SerializeField] Button StartButton;

    // getting power up references
    [SerializeField] Image crossHair;
    [SerializeField] Button getPowerUpButton;

    // using freeze power up references
    [SerializeField] Button activatePowerUpButton;
    [SerializeField] Button freezeButton;

    Vector2 centerScreen;

    bool isPowerUpRespawned = false;
    bool isFreezePowerUpActivated = false;

    int playerGotFreeze;

    private void Awake()
    {
        // Add listener for place board event
        EventManager.AddListener(EventName.BoardPlacedEvent, FirstLoad);

        // add as invoker for game started event
        unityEvents.Add(EventName.GameStartedEvent, new GameStartedEvent());
        EventManager.AddInvoker(EventName.GameStartedEvent, this);

        // add listener for game over event
        EventManager.AddListener(EventName.GameOverEvent, HandleGameOverEvent);

        // add as invoker of restart game event
        unityEvents.Add(EventName.RestartGameEvent, new RestartGameEvent());
        EventManager.AddInvoker(EventName.RestartGameEvent, this);

        // add listener for power up respawned event
        EventManager.AddListener(EventName.PowerUpRespawnedEvent, HandlePowerUpRespawnedEvent);

        // add as invoker of the power up taken event
        unityEvents.Add(EventName.PowerUpTakenEvent, new PowerUpTakenEvent());
        EventManager.AddInvoker(EventName.PowerUpTakenEvent, this);

        // add listener for power up taken event
        EventManager.AddListener(EventName.PowerUpTakenEvent, HandlePowerUpTakenEvent);

        // add as invoker of the player be freeze selected event
        unityEvents.Add(EventName.PlayerBeFreezeSelectedEvent, new PlayerBeFreezeSelectedEvent());
        EventManager.AddInvoker(EventName.PlayerBeFreezeSelectedEvent, this);
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

        // power up support
        crossHair.gameObject.SetActive(false);
        getPowerUpButton.gameObject.SetActive(false);
        centerScreen = new Vector2 (Screen.width / 2, Screen.height / 2);
        getPowerUpButton.onClick.AddListener(TakePowerUp);

        // using freeze power up support
        activatePowerUpButton.gameObject.SetActive(false);
        freezeButton.gameObject.SetActive(false);
        activatePowerUpButton.onClick.AddListener(ActivateFreezePowerUp);
        freezeButton.onClick.AddListener(delegate { FreezeThePaddle(playerGotFreeze); });
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
        if (isPowerUpRespawned)
        {
            Ray ray = Camera.main.ScreenPointToRay(centerScreen);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("PowerUp"))
                {
                    getPowerUpButton.gameObject.SetActive(true);
                }
                else
                {
                    getPowerUpButton.gameObject.SetActive(false);
                }
            }
        }

        if (isFreezePowerUpActivated)
        {
            Ray ray = Camera.main.ScreenPointToRay(centerScreen);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string colliderTag = hit.collider.tag;
                if (colliderTag.Substring(0, 4).Equals("Goal"))
                {
                    freezeButton.interactable = true;
                    playerGotFreeze = int.Parse(colliderTag[colliderTag.Length - 1].ToString());
                }
                else
                {
                    freezeButton.interactable = false;
                }
            }
        }
    }

    /// <summary>
    /// Restart button onClick listener
    /// </summary>
    void RestartGame ()
    {
        Debug.Log("Game restarted");
        unityEvents[EventName.RestartGameEvent].Invoke(0);
        restartBTN.gameObject.SetActive(false);
    }

    /// <summary>
    /// Handle the board place event
    /// </summary>
    /// <param name="unused">unused</param>
    void FirstLoad (int unused)
    {
        StartButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handle game over event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleGameOverEvent (int unused)
    {
        Time.timeScale = 0;
        restartBTN.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handle the power up respawned event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandlePowerUpRespawnedEvent (int unused)
    {
        crossHair.gameObject.SetActive(true);
        isPowerUpRespawned = true;
    }

    /// <summary>
    /// Take the power up when get power up on click
    /// </summary>
    void TakePowerUp()
    {        
        getPowerUpButton.gameObject.SetActive(false);
        isPowerUpRespawned = false;
        unityEvents[EventName.PowerUpTakenEvent].Invoke(1);

        activatePowerUpButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Activate the freeze power up when button onClick
    /// </summary>
    void ActivateFreezePowerUp()
    {
        activatePowerUpButton.gameObject.SetActive(false);
        Time.timeScale = 0.1f;

        crossHair.gameObject.SetActive(true);
        freezeButton.gameObject.SetActive(true);
        freezeButton.interactable = false;
        isFreezePowerUpActivated = true;
    }

    /// <summary>
    /// Freeze the chosen player
    /// </summary>
    /// <param name="playerGotFreeze">number of player who chose to be freeze
    /// </param>
    void FreezeThePaddle (int playerGotFreeze)
    {
        Time.timeScale = 1;

        crossHair.gameObject.SetActive(false);
        freezeButton.gameObject.SetActive(false);
        isFreezePowerUpActivated = false;

        unityEvents[EventName.PlayerBeFreezeSelectedEvent].Invoke(playerGotFreeze);
    }

    /// <summary>
    /// Handle the event power up taken
    /// </summary>
    /// <param name="unused">not in used</param>
    void HandlePowerUpTakenEvent(int unused)
    {
        crossHair.gameObject.SetActive(false);
    }
}
