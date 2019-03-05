using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRSRManager : Singleton<NRSRManager> {
    //array of renderer, list of objects filtered
    public Renderer[] ObjInScene;
    public List<GameObject> FilterObjInScene = new List<GameObject>();

    public int totalNumObj = 0;
    public int prevNumObj = 0;
    public int numVisibleObj = 0;
    public int numUnFilterObj = 0;

    public static GameObject FocusedObject; // Target object: TransformUITools
    public delegate void OnObjectFocused();
    public static event OnObjectFocused ObjectFocused;
    public static event OnObjectFocused ObjectUnFocused;

    public RaycastHit hitInfo;
    public static bool holdSelectedObject_LookingAtTransformTool;
    public static bool holdSelectedObject_UsingTransformTool;
    LayerMask layerMask;
    public static Vector3 menuPosition;

    void Start () {
        layerMask = ~(LayerMask.GetMask("TransformTool"));
    }

    // Update is called once per frame

    private void Update()
    {
        //Debug.Log("Time: " + Time.frameCount);
        if (holdSelectedObject_UsingTransformTool) { return; }

        RayCastToHoldFocusedObject();
        menuPosition = hitInfo.point;

        // If looking at transform tool:
        // If Focused Object is null
        if (holdSelectedObject_LookingAtTransformTool) {
            if (ObjectFocused != null) { ObjectFocused(); }
            return;
        }
        else {
            FocusedObject = null;
            if (ObjectUnFocused != null) { ObjectUnFocused(); }
        }
    }

    private void FixedUpdate() {
        ObjInScene = FindObjectsOfType<Renderer>();
        totalNumObj = ObjInScene.Length;

        // if new objects appear, filtering start
        if (totalNumObj != prevNumObj) {
            FilterObjInScene.Clear();
            numUnFilterObj = 0;

            for (int i = 0; i < ObjInScene.Length; i++) {
                // if gameobject's tag is not NRSRTools, apply filtering
                if (ObjInScene[i].gameObject.tag != "NRSRTools") {
                    FilterObjInScene.Add(ObjInScene[i].gameObject);
                }
                else {
                    numUnFilterObj++;
                }
            }

            numVisibleObj = FilterObjInScene.Count;

            foreach (GameObject go in FilterObjInScene) {
                if (go.transform.root.gameObject.GetComponent<BBox>() == null) {
                    //Debug.Log("Root Obj" + go.transform.root.name);
					BBox box = go.transform.root.gameObject.AddComponent<BBox>();
                    go.transform.root.gameObject.AddComponent<FadeObjectNotActive>();
                    box.isRootObject = true;
                }
            }
        }
    }

    public void RayCastToHoldFocusedObject() {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward,
                            out hitInfo, Mathf.Infinity, layerMask)) {
            if (hitInfo.transform == null) {
                NRSRManager.holdSelectedObject_LookingAtTransformTool = false;
                return;
            }
            if (FocusedObject != null) {
                if (FocusedObject.transform.root.name == hitInfo.transform.root.name) {
                    NRSRManager.holdSelectedObject_LookingAtTransformTool = true;
                }
                else {
                    NRSRManager.holdSelectedObject_LookingAtTransformTool = false;
                }
            }
        }
        else {
            NRSRManager.holdSelectedObject_LookingAtTransformTool = false;
        }
    }

    public static void SendFocusedObjectToManager(GameObject go) {
        FocusedObject = go;
    }

    public static void ClearFocusedObjectFromManager() {
        FocusedObject = null;
    }

}
