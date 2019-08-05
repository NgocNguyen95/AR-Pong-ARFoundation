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
    int[] scorePlayers = new int[5]{ 0, 3, 3, 3, 3};    

    void Start()
    {
        // add listener for goal event
        EventManager.AddListener(EventName.GoalEvent, HandleGoalEvent);

        // add as invoker of knocked out event
        unityEvents.Add(EventName.KnockedOutEvent, new KnockedOutEvent());
        EventManager.AddInvoker(EventName.KnockedOutEvent, this);
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
            unityEvents[EventName.KnockedOutEvent].Invoke(goalOfPlayerGotHit);
        }
        else
        {
            Destroy(StarPlayers[goalOfPlayerGotHit].transform.GetChild(0).gameObject);
        }
    }
}
