using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IFocusable: for focus enter and focus exit
// IInputHandler: for input down and input up
// ISourceStateHandler: for source detected and source lost
// IManipulationHandler: for started, cancelled, updated, completed manipulation

public class MoveTool : MonoBehaviour, IFocusable, IInputHandler, ISourceStateHandler, IManipulationHandler {
    public Transform HostTransform; // main object for movement

    [Range(0.01f, 1.0f)] public float PositionLerpSpeed = 0.2f;
    [Range(0.01f, 1.0f)] public float RotationLerpSpeed = 0.2f;
    public float DistanceScale = 8f;

    public bool IsDraggingEnabled = true;
    private bool isDragging;
    private bool isGazed;

    private Vector3 manipulationEventData;
    private Vector3 manipulationDelta;

    //private Camera mainCamera;
    private IInputSource currentInputSource;
    private uint currentInputSourceId;


    // Use this for initialization
    void Start() {
        // Temporary setup
        if (HostTransform == null) { HostTransform = transform; }
        //mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        // If there is focused object, set the object to it
        Quaternion currentRot = HostTransform.transform.rotation;
        if (NRSRManager.FocusedObject != null) {
            HostTransform = NRSRManager.FocusedObject.transform;
        }
        else { return; }
        
        // if there is dragging, move the object
        if (IsDraggingEnabled && isDragging) {
            HostTransform.position = Vector3.Lerp(HostTransform.position,
                                     HostTransform.position + manipulationDelta,
                                     PositionLerpSpeed);
        }
    }

    // not dragging yet? disable update for NRSRManager
    public void StartDragging() {
        if (IsDraggingEnabled && !isDragging) {
            NRSRManager.holdSelectedObject_UsingTransformTool = true;
        }
        else {
            return;
        }
    }

    // no longer dragging? enable update. if not, currentInputSource = null
    public void StopDragging() {
        if (!isDragging) {
            NRSRManager.holdSelectedObject_UsingTransformTool = false;
            return;
        }
        else {
            InputManager.Instance.PopModalInputHandler();
            isDragging = false;
            currentInputSource = null;
        }
    }

    public void OnFocusEnter()
    {
        if (IsDraggingEnabled && !isGazed) { isGazed = true; }
        else { return; }
    }

    public void OnFocusExit()
    {
        if (IsDraggingEnabled && isGazed) { isGazed = false; }
        else { return; }
    }

    // with input, start dragging
    public void OnInputDown(InputEventData eventData)
    {
        if (IsDraggingEnabled && !isDragging &&
            eventData.InputSource.SupportsInputInfo(eventData.SourceId,
            inputInfo: SupportedInputInfo.PointerPosition)) {
            Debug.Log("Input Down");
            InputManager.Instance.PushModalInputHandler(gameObject);
            isDragging = true;
            currentInputSource = eventData.InputSource;
            currentInputSourceId = eventData.SourceId;
            StartDragging();
        }
        else { return; }
    }

    public void OnInputUp(InputEventData eventData)
    {
        if (currentInputSource != null && eventData.SourceId == currentInputSourceId) {
            Debug.Log("Input Up");
            StopDragging();
        }
    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {

    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        if (currentInputSource != null && eventData.SourceId == currentInputSourceId) {
            Debug.Log("Source is Lost");
            StopDragging();
        }
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        Debug.LogFormat("Start Move: Sources: {0} \r\n" +
            "CummulativeDelta: {1} {2} {3} ",
                eventData.InputSource, eventData.CumulativeDelta.x,
                eventData.CumulativeDelta.y, eventData.CumulativeDelta.z);
        manipulationEventData = eventData.CumulativeDelta;
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        Vector3 delta = eventData.CumulativeDelta - manipulationEventData;
        manipulationDelta = delta * DistanceScale;
        manipulationEventData = eventData.CumulativeDelta;
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        Debug.LogFormat("Complete Move: Sources: {0} \r\n" +
    "CummulativeDelta: {1} {2} {3} ",
        eventData.InputSource, eventData.CumulativeDelta.x,
        eventData.CumulativeDelta.y, eventData.CumulativeDelta.z);
        manipulationDelta = Vector3.zero;
        manipulationEventData = Vector3.zero;
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        Debug.LogFormat("Cancel Scale: Sources: {0} \r\n" +
    "CummulativeDelta: {1} {2} {3} ",
        eventData.InputSource, eventData.CumulativeDelta.x,
        eventData.CumulativeDelta.y, eventData.CumulativeDelta.z);
    }
}
