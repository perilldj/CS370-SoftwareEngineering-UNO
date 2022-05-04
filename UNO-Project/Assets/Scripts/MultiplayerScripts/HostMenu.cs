using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class HostMenu : MonoBehaviourPunCallbacks {
    
    public TMP_Text hostInfo; 

    void Start() {
        updateText();
        if(ClientInfo.isHost) {
            ClientInfo.players.Add(PhotonNetwork.LocalPlayer);
            ClientInfo.playerNames.Add(ClientInfo.username);
        }
    }

    public void startGame() {

        if(!ClientInfo.isHost)
            return;

        if(ClientInfo.numOfPlayers >= 2) {;
            string[] names = ClientInfo.playerNames.ToArray();
            ClientInfo.isMultiplayer = true;
            PhotonNetwork.LocalPlayer.NickName = "0";
            base.photonView.RPC("RPC_SetupPlayers", RpcTarget.Others, new object[] {string.Join(" ", names)});
            base.photonView.RPC("RPC_StartGame", RpcTarget.Others);
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public void exitLobby() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {

        if(!ClientInfo.isHost)
            return;

        if(ClientInfo.numOfPlayers > 5) {
            base.photonView.RPC("RPC_Kick", newPlayer);
        } else {      
            ClientInfo.numOfPlayers += 1;
            initializePlayer(newPlayer, ClientInfo.numOfPlayers - 1);
            updateText();
        }

    }

    private void initializePlayer(Player player, int id) {
        ClientInfo.players.Add(player);
        ClientInfo.playerNames.Add("");
        base.photonView.RPC("RPC_RequestPlayerName", player, new object[] {id});
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {

        if(!ClientInfo.isHost)
            return;

        ClientInfo.numOfPlayers += -1;
        updateText();
        int id = int.Parse(otherPlayer.NickName);
        ClientInfo.playerNames.RemoveAt(id);
        ClientInfo.players.RemoveAt(id);
        for(int i = 0; i < ClientInfo.players.Count; i++) {
            ClientInfo.players[i].NickName = i.ToString();
        }
    }

    private void updateText() {
        if(hostInfo != null)
            hostInfo.text = "Room Created!\nPassword: " + ClientInfo.roomPassword + "\n# Player Count: " + ClientInfo.numOfPlayers;
    }

    [PunRPC]
    public void RPC_Kick() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }

    [PunRPC]
    public void RPC_StartGame() {
        ClientInfo.isMultiplayer = true;
        PhotonNetwork.LoadLevel("GameScene");
    }

    [PunRPC]
    public void RPC_SetNumberOfPlayers(int val) {
        ClientInfo.numOfPlayers = val;
    }

    [PunRPC]
    public void RPC_RequestPlayerName(int id) {
        PhotonNetwork.LocalPlayer.NickName = id.ToString();
        base.photonView.RPC("RPC_SendPlayerName", RpcTarget.MasterClient, new object[] {ClientInfo.username, id}); 
    }

    [PunRPC]
    public void RPC_SendPlayerName(string name, int id) {
        ClientInfo.playerNames[id] = name;
    }

    [PunRPC]
    public void RPC_SetupPlayers(string allNames) {
        string[] nameList = allNames.Split(' ');
        ClientInfo.playerNames.Clear();
        for(int i = 0; i < nameList.Length; i++) {
            ClientInfo.playerNames.Add(nameList[i]);
        }
    }

}
