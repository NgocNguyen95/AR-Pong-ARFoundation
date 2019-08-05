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
    Button PlaceHereButton;    
    Pose hitPose;
    bool boardIsPlaced = false;

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

        // add as invoker for BoardPlacedEvent
        unityEvents.Add(EventName.BoardPlacedEvent, new BoardPlacedEvent());
        EventManager.AddInvoker(EventName.BoardPlacedEvent, this);

        // add listener for game over event
        EventManager.AddListener(EventName.GameOverEvent, HandleGameOverEvent);
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

                    // finds and gets the place the board button
                    PlaceHereButton = GameObject.FindGameObjectWithTag("PlaceHereButton").GetComponent<Button>();
                    PlaceHereButton.onClick.AddListener(PositionSelected);
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
        boardTransform = spawnedObject.transform;

        unityEvents[EventName.BoardPlacedEvent].Invoke(0);
    }

    /// <summary>
    /// Handle game over event
    /// </summary>
    /// <param name="unused">unused</param>
    void HandleGameOverEvent (int unused)
    {
        spawnedObject = Instantiate(board);
        spawnedObject.transform.position = boardTransform.position;
        spawnedObject.transform.rotation = boardTransform.rotation;

        PlaceBoard();
    }
}
