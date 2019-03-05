using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IFocusable: for focus enter and focus exit
// IInputHandler: for input down and input up
// ISourceStateHandler: for source detected and source lost
// IManipulationHandler: for started, cancelled, updated, completed manipulation

public class PlayTool : MonoBehaviour, IFocusable, IInputHandler, IInputClickHandler
{
    public Transform HostTransform; // main object for movement

    private int isClicked;

    private Vector3 manipulationEventData;
    private Vector3 manipulationDelta;

    private IInputSource currentInputSource;
    private uint currentInputSourceId;

    // Use this for initialization
    void Start()
    {
        // Temporary setup
        if (HostTransform == null) { HostTransform = transform; }
        isClicked = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnFocusEnter()
    {
    }

    public void OnFocusExit()
    {
    }

    // with input, start dragging
    public void OnInputDown(InputEventData eventData)
    {
    }

    public void OnInputUp(InputEventData eventData)
    {
    }


    public void OnInputClicked(InputClickedEventData eventData)
    {
        Debug.Log(NRSRManager.FocusedObject.transform.name);
        if (isClicked >= 1) { isClicked = 0; }
        else { isClicked += 1; }
        
        Sprite playSprite = Resources.Load("Materials/11-01", typeof(Sprite)) as Sprite;
        Sprite stopSprite = Resources.Load("Materials/65-01", typeof(Sprite)) as Sprite;

        if (NRSRManager.FocusedObject != null)
        {
            HostTransform = NRSRManager.FocusedObject.transform;
        }

        if (HostTransform.name == "FreeCharacter_model")
        {
            if (isClicked >= 1)
            {
                Debug.Log("Start");
                HostTransform.GetComponent<Animation>().Play();
                gameObject.GetComponent<SpriteRenderer>().sprite = stopSprite;
            }
            else
            {
                Debug.Log("Stop");
                HostTransform.GetComponent<Animation>().Stop();
                gameObject.GetComponent<SpriteRenderer>().sprite = playSprite;
            }
        }
    }
}
