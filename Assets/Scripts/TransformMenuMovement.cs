using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMenuMovement : MonoBehaviour {

    Bounds toolBounds; // Bounds of parent, children objects
    GameObject cursor; 
    Vector3 menuPosition;
    bool initialSetupComplete = true;
    private float x_diff;
    private float y_diff;
	
	// Update is called once per frame
	void Update () {
        menuPosition = NRSRManager.menuPosition;

        x_diff = transform.localPosition.x - menuPosition.x;
        y_diff = transform.localPosition.y - menuPosition.y;

        if (Mathf.Pow(x_diff, 2) > Mathf.Pow(toolBounds.extents.x / 3, 2) ||
            Mathf.Pow(y_diff, 2) > Mathf.Pow(toolBounds.extents.y / 3,  2) )
        {
            transform.position = Vector3.Lerp(transform.position, menuPosition, 0.02f);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,
                             cursor.transform.rotation * Quaternion.Euler(0, 0, 180), 1);

        transform.position = new Vector3(transform.position.x,
                                         transform.position.y,
                                         menuPosition.z - 0.1f);
	}

    private void OnEnable() {
        NRSRManager.ObjectFocused += Enabled_GetPosition;
        NRSRManager.ObjectUnFocused += Disabled_Reset;
        toolBounds = GetBoundsForAllChildren(gameObject);
        cursor = GameObject.Find("DefaultCursor");
    }

    private void OnDisable()
    {
        NRSRManager.ObjectFocused -= Enabled_GetPosition;
        NRSRManager.ObjectUnFocused -= Disabled_Reset;
    }

    void Disabled_Reset() {
        initialSetupComplete = true;
    }

    // When Object is Focused, transform position & rotation accordingly
    void Enabled_GetPosition()
    {
        if (initialSetupComplete)
        {
            transform.position = cursor.transform.position;
            transform.rotation = cursor.transform.rotation * Quaternion.Euler(0, 0, 180);
            initialSetupComplete = false;
        }
    }

    // Boundaries of the object
    public Bounds GetBoundsForAllChildren(GameObject findMyBounds)
    {
        Bounds result = new Bounds(Vector3.zero, Vector3.zero);

        foreach (Collider coll in findMyBounds.GetComponentsInChildren<Collider>()) {
            Debug.Log(coll.name);
            if (result.extents == Vector3.zero) {
                result = coll.bounds;
            }
            else {
                result.Encapsulate(coll.bounds);
            }
        }
        return result;
    }
}
