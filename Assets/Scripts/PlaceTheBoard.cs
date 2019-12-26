using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using System;

/// <summary>
/// Cast ray to demonstrate board game in real world
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceTheBoard : IntEventInvoker
{
    [SerializeField]
    [Tooltip("Instantiate this prefab on a plane at the ray")]
    GameObject board;
    Transform boardTransform;

    ARRaycastManager RaycastManager;
    ARPointCloudManager PointCloudManager;
    ARPlaneManager PlaneManager;
    ARSessionOrigin SessionOrigin;

    Vector2 centerScreenPos;
    static List<ARRaycastHit> listHits = new List<ARRaycastHit>();
    [SerializeField] Button PlaceHereButton;    
    Pose hitPose;
    bool boardIsPlaced = false;

    Timer delayRespawnNewBoardTimer;

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    GameObject spawnedObject { get; set; }

    private void Awake()
    {
        RaycastManager = GetComponent<ARRaycastManager>();
        PointCloudManager = GetComponent<ARPointCloudManager>();
        PlaneManager = GetComponent<ARPlaneManager>();
        SessionOrigin = GetComponent<ARSessionOrigin>();
    }

    // Start is called before the first frame update
    void Start()
    {
        centerScreenPos = new Vector2(Screen.width / 2, Screen.height / 2);

        // add listener for restart game event
        EventManager.AddListener(EventName.RestartGameEvent, HandleRestartGameEvent);

        // add listener for game over event
        EventManager.AddListener(EventName.GameOverEvent, HandleGameOverEvent);

        // create timer
        delayRespawnNewBoardTimer = gameObject.AddComponent<Timer>();
        delayRespawnNewBoardTimer.Duration = 0.5f;
        delayRespawnNewBoardTimer.AddTimerFinishedEventListener(HandleDelayRespawnNewBoardTimerFinishedEvent);

        PlaceHereButton.onClick.AddListener(PositionSelected);
    }

    // Update is called once per frame
    void Update()
    {
        if (!boardIsPlaced)
        {
            if (RaycastManager.Raycast(centerScreenPos, listHits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                hitPose = listHits[0].pose;
            
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(board);
                    SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hitPose.position, hitPose.rotation);
                }
                else
                {
                    SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hitPose.position);
                }                        
            }        
        }
        
    }

    private void PositionSelected()
    {
        PlaceBoard();

        // diactivate existings trackable
        foreach (ARPlane plane in PlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        foreach (ARPointCloud pointCloud in PointCloudManager.trackables)
        {
            pointCloud.gameObject.SetActive(false);
        }

        // dissable plane and point cloud detections
        PointCloudManager.enabled = !PointCloudManager.enabled;
        PlaneManager.enabled = !PlaneManager.enabled;

        PlaceHereButton.gameObject.SetActive(false);
    }

    void PlaceBoard()
    {
        Debug.Log("Board placed!");
        boardIsPlaced = true;
        boardTransform = GameObject.FindGameObjectWithTag("PlayArea").gameObject.transform;
    }

    /// <summary>
    /// Handle restart game event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleRestartGameEvent (int unused)
    {
        Time.timeScale = 1;
        delayRespawnNewBoardTimer.Run();
    }

    void HandleDelayRespawnNewBoardTimerFinishedEvent()
    {
        GameObject newBoard = Instantiate(board);
        newBoard.transform.position = boardTransform.position;
        newBoard.transform.rotation = boardTransform.rotation;

        PlaceBoard();
    }

    /// <summary>
    /// Handle game over event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleGameOverEvent(int unused)
    {
        Destroy(GameObject.FindGameObjectWithTag("PlayArea").gameObject);
    }
}
