using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/*

    Class: Chat : MonoBehaviourPunCallbacks
    Author: perilldj

    Description: Script for managing the multiplayer chat function


*/

public class Chat : MonoBehaviourPunCallbacks {
    
    public TMP_InputField messageInput;
    public TMP_Text chatBox;
    public List<string> messages = new List<string>();

    private const int MAX_LINES = 15;
    private const int CHARACTER_LIMIT = 30;

    /*
        Method: sendChat()
        Description: When called, this method will take the message in the TMP_Input field and
                     send it to other opponents, also calls the updateChatBox method to add the message to
                     the local chat box.
    */

    public void sendChat() {
        if(messageInput.text.Length > CHARACTER_LIMIT) // Enforces character limit
            return;
        string str = ClientInfo.username + ": " + messageInput.text; //Compose message with player's name
        base.photonView.RPC("RPC_ReceiveMessage", RpcTarget.Others, new object[] {str}); //Send everyone the message
        updateChatBox(str); //Update local chat box
        messageInput.text = ""; //Wipe InputField
    }

    /*
        Method: RPC_ReceiveMessage(string message)
        Description: Function called remotely when another client sends a message.
    */

    [PunRPC]
    public void RPC_ReceiveMessage(string message) {
        updateChatBox(message);
    }

    /*
        Method: updateChatBox(string message)
        Description: Updates the on-screen chat box with messages received and sent alike.
    */

    private void updateChatBox(string message) {
        messages.Add(message);
        if(messages.Count > MAX_LINES) //If the number of messages exceed the max, remove the oldest message on screen
            messages.RemoveAt(0);
        string str = "";
        for(int i = 0; i < messages.Count; i++) //Compiles every message into one string
            str += messages[i] + "\n";
        chatBox.text = str; //Displays string in chatbox
    }

}
