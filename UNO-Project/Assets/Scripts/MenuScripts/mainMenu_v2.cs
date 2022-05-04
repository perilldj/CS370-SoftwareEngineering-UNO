using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu_v2 : MonoBehaviour
{

    void Start() {
        Application.runInBackground = true;
    }

    public void PlayGame () {
        ClientInfo.isMultiplayer = false;
        ClientInfo.isHost = false;
        SceneManager.LoadScene("GameScene");
    }

    public void JoinMultiplayer() {
        SceneManager.LoadScene("Connecting");
    }

    public void Setting () {
        SceneManager.LoadScene("Setting");
    }
    
    public void QuitGame () {
        Application.Quit();
    }
}
