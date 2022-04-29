using System.Collections;
using System.Collections.Generic;  
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    private Image nameBackground;
    private Image cardCountBackground;
    private Image cardHolderBackground;

    private GameObject enemyObject;
    private Hand hand;

    private Vector2 uiLocation;
    private const float HAND_OFFSET_Y = -0.68f;
    private const float HAND_WIDTH = 3.0f;
    private const float CARD_SIZE = 0.1f;

    public Enemy(Vector2 pos, GameObject enemyPrefab, Pile pile) {

        enemyObject = GameObject.Instantiate(enemyPrefab);
        enemyObject.transform.position = new Vector3(pos.x, pos.y, 0.0f);

        nameText = enemyObject.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        cardCount = enemyObject.gameObject.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>();

        nameBackground = enemyObject.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        cardCountBackground = enemyObject.gameObject.transform.GetChild(0).GetChild(1).GetComponent<Image>();
        cardHolderBackground = enemyObject.gameObject.transform.GetChild(0).GetChild(2).GetComponent<Image>();

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
        hand.pile = pile;

        updateCardCount();  

    }

    public bool attemptRandomMove() {
        
        Card card;

        for(int i = 0; i < hand.getHandSize(); i++) {
            card = hand.get(i);
            if(hand.playCard(card.getCardID())) {
                updateCardCount();
                card.flipCard();
                return true;
            }
        }

        return false;

    }

    public void setRedBackground() {
        Color redColor = new Color(1, 0, 0, 1);
        nameBackground.color = redColor;
        cardCountBackground.color = redColor;
        cardHolderBackground.color = redColor;
    }

    public void setWhiteBackground() {
        Color whiteColor = new Color(1, 1, 1, 1);
        nameBackground.color = whiteColor;
        cardCountBackground.color = whiteColor;
        cardHolderBackground.color = whiteColor;
    }

    public void updateCardCount() {
        cardCount.text = hand.getHandSize().ToString();
    }

    public void addCard(Card card) {
        hand.addCard(card);
        updateCardCount();
    }

    public Hand getHand() {
        return hand;
    }

}
