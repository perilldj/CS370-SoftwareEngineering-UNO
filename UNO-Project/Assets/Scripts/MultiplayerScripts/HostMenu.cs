using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class HostMenu : MonoBehaviourPunCallbacks {
    
    public TMP_Text hostInfo; 

    void Awake() {
        updateText();
    }

    public void startGame() {

        if(!ClientInfo.isHost)
            return;

        if(ClientInfo.numOfPlayers >= 2) {
            Debug.Log(base.photonView);
            base.photonView.RPC("RPC_StartGame", RpcTarget.Others);
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public void exitLobby() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        ClientInfo.numOfPlayers += 1;
        updateText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        ClientInfo.numOfPlayers += -1;
        updateText();
    }

    private void updateText() {
        if(hostInfo != null)
            hostInfo.text = "Room Created!\nPassword: " + ClientInfo.roomPassword + "\n# Player Count: " + ClientInfo.numOfPlayers;
    }

    [PunRPC]
    public void RPC_StartGame() {
        PhotonNetwork.LoadLevel("GameScene");
    }

}
