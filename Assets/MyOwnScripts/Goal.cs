using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : IntEventInvoker
{
    bool isGoal = true;

    // Start is called before the first frame update
    void Start()
    {
        // add as invoker for goal event
        unityEvents.Add(EventName.GoalEvent, new GoalEvent());
        EventManager.AddInvoker(EventName.GoalEvent, this);

        // add listener for knocked out event
        EventManager.AddListener(EventName.KnockedOutEvent, HandleKnockedOutEvent);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (isGoal)
        {
            if (col.gameObject.tag == "Ball")
            {
                Destroy(col.gameObject);

                // get the number of which goal got hit
                int goalNumberGotHit = int.Parse(gameObject.name[gameObject.name.Length - 1].ToString());

                // invoke the goal event
                unityEvents[EventName.GoalEvent].Invoke(goalNumberGotHit);
            }
        }        
    }
    
    /// <summary>
    /// Handle knocked out event
    /// </summary>
    /// <param name="playerKnockedOut">player who knocked out</param>
    void HandleKnockedOutEvent (int playerKnockedOut)
    {
        string numOfGoal = "Goal" + playerKnockedOut.ToString();

        if (gameObject.tag.Equals(numOfGoal))
        {
            isGoal = false;
        }
    }
}
