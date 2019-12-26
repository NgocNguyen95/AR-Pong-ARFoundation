using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayArea : IntEventInvoker
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;

        // add as invoker for BoardPlacedEvent
        unityEvents.Add(EventName.BoardPlacedEvent, new BoardPlacedEvent());
        EventManager.AddInvoker(EventName.BoardPlacedEvent, this);

        unityEvents[EventName.BoardPlacedEvent].Invoke(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        gameObject.tag = "Untagged";
        EventManager.RemoveInvoker(EventName.BoardPlacedEvent, this);
    }
}
