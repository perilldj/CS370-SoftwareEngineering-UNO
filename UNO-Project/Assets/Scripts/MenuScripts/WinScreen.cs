using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using Photon.Pun;
using Photon.Realtime;

public class WinScreen : MonoBehaviour {

    public void goMainMenu() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }
    
}
