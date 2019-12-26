using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The score manager of the game
/// </summary>
public class ScoreManager : IntEventInvoker
{
    // start appear on the board
    [SerializeField] GameObject[] StarPlayers = new GameObject[5];

    // initial score for all player
    int[] scorePlayers;
    bool needNewBall = true;

    void Start()
    {
        // add listener for goal event
        EventManager.AddListener(EventName.GoalEvent, HandleGoalEvent);

        // add as invoker of knocked out event
        unityEvents.Add(EventName.KnockedOutEvent, new KnockedOutEvent());
        EventManager.AddInvoker(EventName.KnockedOutEvent, this);

        // add as invoker for game over event
        unityEvents.Add(EventName.GameOverEvent, new GameOverEvent());
        EventManager.AddInvoker(EventName.GameOverEvent, this);

        // add as invoker of respawn ball event
        unityEvents.Add(EventName.RespawnBallEvent, new RespawnBallEvent());
        EventManager.AddInvoker(EventName.RespawnBallEvent, this);

        scorePlayers = new int[5] { 0, 3, 3, 3, 3 };
    }

    void Update()
    {
        
    }

    /// <summary>
    /// Handle goal event
    /// </summary>
    /// <param name="goalOfPlayerGotHit">goal number of the player got hit</param>
    void HandleGoalEvent(int goalOfPlayerGotHit)
    {
        scorePlayers[goalOfPlayerGotHit] -= 1;

        if (scorePlayers[goalOfPlayerGotHit] == 0)
        {
            Destroy(StarPlayers[goalOfPlayerGotHit].gameObject);

            // check which condition to invoke which event
            if (goalOfPlayerGotHit == 1 || AllBotKnockedOut())
            {
                needNewBall = false;
                unityEvents[EventName.GameOverEvent].Invoke(0);
            }
            unityEvents[EventName.KnockedOutEvent].Invoke(goalOfPlayerGotHit);
        }
        else
        {
            Destroy(StarPlayers[goalOfPlayerGotHit].transform.GetChild(0).gameObject);
        }

        // check whether or not new ball is needed
        if (needNewBall)
        {
            unityEvents[EventName.RespawnBallEvent].Invoke(0);
        }
    }

    private void OnDestroy()
    {
        EventManager.RemoveInvoker(EventName.KnockedOutEvent, this);
        EventManager.RemoveInvoker(EventName.GameOverEvent, this);
    }

    /// <summary>
    /// Check whether or not all bots are knocked out
    /// </summary>
    /// <returns>true if all bot knocked out, otherwise return false</returns>
    bool AllBotKnockedOut()
    {
        if (scorePlayers[2] == 0 && scorePlayers[3] == 0 && scorePlayers[4] == 0)
        {
            return true;
        }

        return false;
    }
}
