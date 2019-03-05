using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateToolSet : MonoBehaviour {

    public float scaleFactor = 0.4f;
    //GameObject cursor;
    
    bool isActive;

    public List<SpriteRenderer> rend;
    
	// Use this for initialization
	void Start () {
        //cursor = GameObject.Find("DefaultCursor");
        transform.localScale *= scaleFactor;
	}

    // At ObjectFocused, Triggered to enable renderers of toolset
    void ActivateThis()
    {
        foreach (SpriteRenderer spriteRend in rend) {
            spriteRend.enabled = true;
        }
    }

    void DeactivateThis()
    {
        foreach (SpriteRenderer spriteRend in rend) {
            spriteRend.enabled = false;
        }
    }

    private void OnEnable()
    {
        NRSRManager.ObjectFocused += ActivateThis;
        NRSRManager.ObjectUnFocused += DeactivateThis;
    }

    private void OnDisable()
    {
        NRSRManager.ObjectFocused -= ActivateThis;
        NRSRManager.ObjectUnFocused -= DeactivateThis;
    }
}
