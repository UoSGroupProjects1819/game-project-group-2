using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuScript : MonoBehaviour {

	public void OpenMenu(GameObject menuToOpen)
    {
        menuToOpen.SetActive(true);
    }

    public void CloseMenu(GameObject menuToClose)
    {
        menuToClose.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
