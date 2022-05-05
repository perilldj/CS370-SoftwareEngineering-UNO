using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/*

    public class HostMenu : MonoBehaviourPunCallbacks
    Author: perilldj
    Description: Responsible for managing the joining and leaving of players, updating
                 players with the relevant information needed for starting the multiplayer
                 game in sync.

*/

public class HostMenu : MonoBehaviourPunCallbacks {
    
    public TMP_Text hostInfo; 

    void Start() {

        updateText();
        if(ClientInfo.isHost) {
            ClientInfo.players.Add(PhotonNetwork.LocalPlayer);
            ClientInfo.playerNames.Add(ClientInfo.username);
        }

    }

    /*
        Method: startGame()
        Description: When the host presses the start button, this method sets up all the connected
                     players with player names, then officially starts the game.
    */

    public void startGame() {

        if(!ClientInfo.isHost)  //If the client isn't the host (MasterClient) this function shouldn't be called.
            return;

        if(ClientInfo.numOfPlayers >= 2) { //Enforces 2 player minimum
            string[] names = ClientInfo.playerNames.ToArray(); //Converts list to array
            ClientInfo.isMultiplayer = true;
            PhotonNetwork.LocalPlayer.NickName = "0";
            base.photonView.RPC("RPC_SetupPlayers", RpcTarget.Others, new object[] {string.Join(" ", names)}); //Communicates all clients all usernames
            base.photonView.RPC("RPC_StartGame", RpcTarget.Others); //Tells all clients to start the game.
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    public void exitLobby() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu_v2");
    }

    /*
        Method: OnPlayerEnteredRoom(Player newPlayer)
        Description: Automatically called by photon, handles a connecting player
    */

    public override void OnPlayerEnteredRoom(Player newPlayer) {

        if(!ClientInfo.isHost)
            return;

        if(ClientInfo.numOfPlayers > 5) { //Enforces 5 player maximum
            base.photonView.RPC("RPC_Kick", newPlayer);
        } else {      
            ClientInfo.numOfPlayers += 1;
            initializePlayer(newPlayer, ClientInfo.numOfPlayers - 1); //Sets the players ID and fetches the players chosen username
            updateText();
        }

    }

    /*
        Method: initializePlayer(Player player, int id)
        Description: Gives the connecting player it's unique ID and requests the players chosen username.
    */

    private void initializePlayer(Player player, int id) {
        ClientInfo.players.Add(player);
        ClientInfo.playerNames.Add("");
        base.photonView.RPC("RPC_RequestPlayerName", player, new object[] {id});
    }

    /*
        Method: OnPlayerLeftRoom(Player otherPlayer)
        Description: Automatically called by photon whenever a player leaves the room
    */

    public override void OnPlayerLeftRoom(Player otherPlayer) {

        if(!ClientInfo.isHost)
            return;

        ClientInfo.numOfPlayers += -1;
        updateText();
        int id = int.Parse(otherPlayer.NickName);
        ClientInfo.playerNames.RemoveAt(id); //Removes the player from the relevant lists
        ClientInfo.players.RemoveAt(id);
        for(int i = 0; i < ClientInfo.players.Count; i++) { //Updates the client's ID
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

    /*
        Method: RPC_SetupPlayers(string allNames)
        Description: Splits provided names by spaces gives a client every username
                     of every player to create in the GameScene.
    */

    [PunRPC]
    public void RPC_SetupPlayers(string allNames) {
        string[] nameList = allNames.Split(' ');
        ClientInfo.playerNames.Clear();
        for(int i = 0; i < nameList.Length; i++) {
            ClientInfo.playerNames.Add(nameList[i]);
        }
        ClientInfo.numOfPlayers = ClientInfo.playerNames.Count;
    }

}
