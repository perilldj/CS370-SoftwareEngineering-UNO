using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyControl : MonoBehaviourPunCallbacks {

    public TMP_InputField nameInput;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void exitLobby() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }

    public void createRoom() {
        ClientInfo.username = nameInput.text;
        ClientInfo.roomPassword = createInput.text;
        if(!isNullOrEmpty(ClientInfo.username) && !isNullOrEmpty(ClientInfo.roomPassword)) {
            ClientInfo.isHost = true;
            PhotonNetwork.CreateRoom(ClientInfo.roomPassword);
        }
    }

    public void joinRoom() {
        ClientInfo.username = nameInput.text;
        ClientInfo.roomPassword = joinInput.text;
        if(!isNullOrEmpty(ClientInfo.username) && !isNullOrEmpty(ClientInfo.roomPassword)) {
            ClientInfo.isHost = false;
            PhotonNetwork.JoinRoom(joinInput.text);
        }
    }

    bool isNullOrEmpty(string s) {
        bool result;
        result = s == null || s == string.Empty;
        return result;
    }

    public override void OnJoinedRoom() {
        if(ClientInfo.isHost)
            PhotonNetwork.LoadLevel("HostMenu");
        else
            PhotonNetwork.LoadLevel("Waiting");
    }

}
