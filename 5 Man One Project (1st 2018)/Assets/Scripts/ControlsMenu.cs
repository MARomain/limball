using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ControlsMenu : MonoBehaviour {

    public GameObject selectedObjectControlsMenu;
    public EventSystem eventSystem;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        eventSystem.SetSelectedGameObject(selectedObjectControlsMenu);
    }
}
