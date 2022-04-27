using System.Collections;
using System.Collections.Generic;  
using UnityEngine;
using TMPro;

public static class RandomNames {
    private static List<string> names = new List<string> {"Sophia", "Aiden", "Emma", "Jackson", "Isabella",
                                                          "Olivia", "Liam", "Ava", "Jacob", "Madison", "Noah",
                                                          "Emily", "Riley", "Ryan", "Alexander", "Layla", "Jack"};

    private static List<int> usedNames = new List<int>();
    private static System.Random ran = new System.Random();

    private static bool isNameUsed(int index) {
        for(int i = 0; i < usedNames.Count; i++)
            if(usedNames[i] == index)
                return true;
        return false;
    }

    public static void clearUsedNames() {
        usedNames.Clear();
    }

    public static string getRandomName() {

        int randomIndex = ran.Next(0, names.Count);

        int attempts = 25; //Insurance this doesn't get stuck in an infinite loop
        while(attempts > 0) {
            attempts--;
            if(isNameUsed(randomIndex))
                randomIndex = ran.Next(0, names.Count);
            else {
                usedNames.Add(randomIndex);
                return names[randomIndex];
            }
        }
        
        return ":)"; 

    }

}

public class Enemy {
    
    private bool isAI = true;

    private string name;

    private GameControl gameController;

    private TMP_Text nameText;
    private TMP_Text cardCount;

    private GameObject enemyObject;
    private Hand hand;

    private Vector2 uiLocation;
    private const float HAND_OFFSET_Y = -0.68f;
    private const float HAND_WIDTH = 3.0f;
    private const float CARD_SIZE = 0.1f;

    public Enemy(Vector2 pos, GameObject enemyPrefab) {

        enemyObject = GameObject.Instantiate(enemyPrefab);
        enemyObject.transform.position = new Vector3(pos.x, pos.y, 0.0f);

        // o_o
        nameText = enemyObject.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        cardCount = enemyObject.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();

        if(isAI) {
            name = RandomNames.getRandomName();
            nameText.text = name;
        }       

        uiLocation = pos;
        hand = new Hand();
        hand.setHandOffsetX(uiLocation.x);
        hand.setHandYPos(uiLocation.y + HAND_OFFSET_Y);
        hand.setHandWidth(HAND_WIDTH);
        hand.setCardSize(CARD_SIZE);
        hand.setIsEnemy(true);

    }

    public bool attemptRandomMove() {
        
        Card card;

        for(int i = 0; i < hand.getHandSize(); i++) {
            card = hand.get(i);
            if(hand.playCard(card.getCardID())) {
                updateCardCount();
                return true;
            }
        }

        return true;

    }

    public void updateCardCount() {
        cardCount.text = hand.getHandSize().ToString();
        Debug.Log(hand.getHandSize());
    }

    public void addCard(Card card) {
        hand.addCard(card);
        updateCardCount();
    }

    public Hand getHand() {
        return hand;
    }

}
