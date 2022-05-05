using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

/*
    Class: ConnectToServer : MonoBehaviourPunCallbacks
    Description: Code to initially connect to the server, automatically
                 joins the multiplayer lobby once successful.
*/

public class ConnectToServer : MonoBehaviourPunCallbacks {

    void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        SceneManager.LoadScene("Lobby");
    }

}
