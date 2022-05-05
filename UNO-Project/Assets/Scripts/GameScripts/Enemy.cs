using System.Collections;
using System.Collections.Generic;  
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*

    public static class RandomNames
    Author: perilldj
    Description: Used to provide unique names to the AIs in the singleplayer mode.

*/

public static class RandomNames {
    private static List<string> names = new List<string> {"Sophia", "Aiden", "Emma", "Jackson", "Isabella",
                                                          "Olivia", "Liam", "Ava", "Jacob", "Madison", "Noah",
                                                          "Emily", "Riley", "Ryan", "Alexander", "Layla", "Jack",
                                                          "James", "William", "David", "Lisa", "Daniel", "Amanda",
                                                          "Emma", "Justin", "Gary", "Brandon", "John"};

    private static List<int> usedNames = new List<int>();
    private static System.Random ran = new System.Random();

    private static bool isNameUsed(int index) {
        for(int i = 0; i < usedNames.Count; i++)
            if(usedNames[i] == index)
                return true;
        return false;
    }

    /*
        Method: clearUsedNames()
        Description: Clears the array of used names so that names that were previously chosen can be chose again.
    */

    public static void clearUsedNames() {
        usedNames.Clear();
    }

    /*
        Method: getRandomName()
        Description: Returns a random name from the above list of names. Will never return the same name twice unless
                     clearUsedNames() is called.
    */

    public static string getRandomName() {

        int randomIndex = ran.Next(0, names.Count);

        int attempts = 25; //Insurance so this doesn't get stuck in an infinite loop
        while(attempts > 0) {
            attempts--;
            if(isNameUsed(randomIndex))
                randomIndex = ran.Next(0, names.Count);
            else {
                usedNames.Add(randomIndex); //Adds chosen name to a list of used names
                return names[randomIndex];
            }
        }
        
        return ":)"; 

    }

}

/*

    public class Enemy
    Author: perilldj
    Description: A representation of an opponent in the game can either be AI controlled or controlled by
                 another player in multiplayer.

*/

public class Enemy {
    
    private bool isAI = true;
    private int id = -1;

    private string name;

    private GameControl gameController;

    private TMP_Text nameText;
    private TMP_Text cardCount;

    private Image nameBackground;
    private Image cardCountBackground;
    private Image cardHolderBackground;

    private GameObject enemyObject;
    private Hand hand;
    private Deck deck;

    private Vector2 uiLocation;
    private const float HAND_OFFSET_Y = -0.68f;
    private const float HAND_WIDTH = 3.0f;
    private const float CARD_SIZE = 0.1f;

    public Enemy(Vector2 pos, GameObject enemyPrefab, Pile pile, Deck deck, GameControl gameController, bool isAI) {

        this.isAI = isAI;

        this.deck = deck;
        this.gameController = gameController;

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
        hand.deck = deck;

        updateCardCount();  

    }

    /*
        Method: attemptRandomMove()
        Description: Iterates through each card in the enemy's hand. The first card that can be played legally
                     will be played through this method.
    */

    public bool attemptRandomMove() {
        
        CardControl card;

        for(int i = 0; i < hand.getHandSize(); i++) {
            card = hand.get(i);
            if(hand.playCard(card.getCardID(), true)) {
                updateCardCount();
                return true;
            }
        }

        return false;

    }

    /*
        Method: playCard(int cardID)
        Description: Playes a card to the deck.
        NOTE: Only called in multiplayer.
    */

    public void playCard(int cardID) {
        hand.setCanMove(true);
        hand.playCard(cardID, true);
        hand.setCanMove(false);
        updateCardCount();
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
        hand.addCard(card, gameController.getCardPrefab());
        updateCardCount();
    }

    public Hand getHand() {
        return hand;
    }

    public string getName() {
        return name;
    }

    public void setName(string name) {
        this.name = name;
        nameText.text = name;
    }

    public int getID() {
        return id;
    }

    public void setID(int id) {
        this.id = id;
    }

}
