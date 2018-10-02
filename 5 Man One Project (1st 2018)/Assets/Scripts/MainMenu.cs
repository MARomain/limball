using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour {

    public GameObject selectedObjectMainMenu;
    public EventSystem eventSystem;
    private bool buttonSelected;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            eventSystem.SetSelectedGameObject(selectedObjectMainMenu);
            buttonSelected = true;
        }
        //eventSystem.SetSelectedGameObject(selectedObjectMainMenu, null);
    }
}
