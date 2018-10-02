using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwitchMenu : MonoBehaviour {

    public GameObject OffCanvas;
    public GameObject OnCanvas;
    public GameObject FirstSelectedObject;
    public EventSystem eventSystem;

public void Switch()
    {
        OffCanvas.SetActive(false);
        OnCanvas.SetActive(true);
        eventSystem.SetSelectedGameObject(FirstSelectedObject, null);
    }
}
