using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockWall : MonoBehaviour
{
    Timer disapearTimer;

    // Start is called before the first frame update
    void Start()
    {
        disapearTimer = gameObject.AddComponent<Timer>();
        disapearTimer.Duration = 5f;
        disapearTimer.AddTimerFinishedEventListener(DestroyTheWall);

        disapearTimer.Run();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyTheWall()
    {
        Destroy(this.gameObject);
    }
}
