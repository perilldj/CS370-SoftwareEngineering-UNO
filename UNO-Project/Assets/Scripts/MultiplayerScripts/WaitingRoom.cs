using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class WaitingRoom : MonoBehaviour {
    
    public void exit() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }

    

}
