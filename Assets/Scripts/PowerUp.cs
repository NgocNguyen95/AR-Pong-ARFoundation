using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //float initTime;
    Vector3 randPowerUpPosition;

    // always called before any Start functions and also just after a prefab is instantiated
    void Awake()
    {
        // get random x, y and z for position of gameObject
        GameObject floor = GameObject.FindGameObjectWithTag("Floor");
        Vector3 meshColliderFloorSize = floor.GetComponent<MeshCollider>().bounds.size;
        float randX = Random.Range(meshColliderFloorSize.x / 2, -meshColliderFloorSize.x / 2);
        float randY = Random.Range(4, 8);
        float randZ = Random.Range(meshColliderFloorSize.z / 2, -meshColliderFloorSize.z/ 2);

        randPowerUpPosition = new Vector3(randX, randY, randZ);
        Debug.Log(randPowerUpPosition);
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.1f;
        //initTime = Time.time;

        // set new random postition for gameObject
        transform.position = randPowerUpPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.time >= initTime + 1)
        //{
        //    Time.timeScale = 1;
        //    Destroy(gameObject);
        //}

        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * 10);
    }

    // All physics calculations and updates occur immediately after FixedUpdate
    void FixedUpdate()
    {

    }
}
