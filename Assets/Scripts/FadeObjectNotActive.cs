using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectNotActive : MonoBehaviour {

    // At ObjectFocused, Triggered to
    private void OnEnable()
    {
        NRSRManager.ObjectFocused += FadeObject;
        NRSRManager.ObjectUnFocused += UnfadeObject;
    }

    private void OnDisable()
    {
        NRSRManager.ObjectFocused -= FadeObject;
        NRSRManager.ObjectUnFocused -= UnfadeObject;
    }

    void FadeObject()
    {
        if (gameObject.tag == "NRSRTools") { return; }
    }

    void UnfadeObject()
    {
        if (gameObject.tag == "NRSRTools") { return; }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
