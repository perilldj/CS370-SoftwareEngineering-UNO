using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class ClientInfo : MonoBehaviour {
    
    public static bool isMultiplayer = false;
    public static bool isHost = false;
    public static int numOfPlayers = 1;
    public static string roomPassword;
    public static string username;

    public static List<string> playerNames = new List<string>();
    public static List<Player> players = new List<Player>();

}
