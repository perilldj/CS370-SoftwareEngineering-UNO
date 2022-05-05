using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

/*

    public class Chat : MonoBehaviourPunCallbacks
    Author: perilldj

*/

public class Chat : MonoBehaviourPunCallbacks {
    
    public TMP_InputField messageInput;
    public TMP_Text chatBox;
    public List<string> messages = new List<string>();
    int lines = 7;

    public void sendChat() {
        if(messageInput.text.Length > 30)
            return;
        string str = ClientInfo.username + ": " + messageInput.text;
        base.photonView.RPC("RPC_ReceiveMessage", RpcTarget.Others, new object[] {str});
        updateChatBox(str);
        messageInput.text = "";
    }

    [PunRPC]
    public void RPC_ReceiveMessage(string message) {
        updateChatBox(message);
    }

    private void updateChatBox(string message) {
        messages.Add(message);  
        if(messages.Count > lines)
            messages.RemoveAt(0);
        string str = "";
        for(int i = 0; i < messages.Count; i++)
            str += messages[i] + "\n";
        chatBox.text = str;
    }

}
