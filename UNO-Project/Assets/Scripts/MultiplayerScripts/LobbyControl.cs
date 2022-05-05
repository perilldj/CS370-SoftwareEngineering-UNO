using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

/*

    public class LobbyControl : MonoBehaviourPunCallbacks
    Author: perilldj
    Description: Manages taking inpus from the fields required to chose a username, create a room, or join a room.

*/

public class LobbyControl : MonoBehaviourPunCallbacks {

    public TMP_InputField nameInput;
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    void Start() {

        ClientInfo.numOfPlayers = 1;
        ClientInfo.username = "";
        ClientInfo.roomPassword = "";
        ClientInfo.playerNames.Clear();
        ClientInfo.players.Clear();

    }

    public void exitLobby() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }

    public void createRoom() {
        ClientInfo.username = nameInput.text;
        ClientInfo.roomPassword = createInput.text;
        /* Ensures that the required fields for creating a room are not empty */
        if(!isNullOrEmpty(ClientInfo.username) && !isNullOrEmpty(ClientInfo.roomPassword)) {
            ClientInfo.isHost = true;
            PhotonNetwork.CreateRoom(ClientInfo.roomPassword); //Creates room
        }
    }

    public void joinRoom() {
        ClientInfo.username = nameInput.text;
        ClientInfo.roomPassword = joinInput.text;
        /* Ensures that the required fields for joining a room are not empty */
        if(!isNullOrEmpty(ClientInfo.username) && !isNullOrEmpty(ClientInfo.roomPassword)) {
            ClientInfo.isHost = false;
            PhotonNetwork.JoinRoom(joinInput.text); //Joins a room (provided there is one with the provided code)
        }
    }

    private bool isNullOrEmpty(string s) {
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
