using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: Hand
    Description: A hand of cards for a player. It contains to Vector2 positions for cards to be between on screen
                 Also contains a list of cards that go on screen. Designed for multiple to exist in game

    Methods: addCard(Card card);
             removeCard(int index);
             playCard(int cardID);

    Author: perilldj
*/

public class Hand {
    
    /* List of cards in hand */
    private List<Card> cards = new List<Card>();

    private bool isPlayerHand = true;

    public Pile pile;

    private int handSize = 0;
    private float cardSize = 0.2f;

    private float handWidth = 7.5f;
    private float handOffsetX = 0.0f;
    private float handYPos = -3.5f;

    private bool isEnemy = false;

    /*
        Method: addCard(Card card)
        Description: When called, the given card is added to the hand, it will also create an on-screen card representation and sets it
                     to the correct position by calling createOnScreenCard() and ajustCardPositions().
    */

    public void addCard(Card card) {
        handSize++;                        //Increment handSize
        if(!isPlayerHand)
            card.getCardController().setCanPlay(false);
        cards.Add(card);                   //Add card to hand
        card.createOnScreenCard(isEnemy);  //Add card to screen
        ajustCardPositions();              //Update positions of the cards
    }

    /* 
        Method: removeCard(int index)
        Description: Removes a card from the hand at a given index.
    */

    public void removeCard(int index) {
        cards.RemoveAt(index);
    }

    /*
        Method: ajustCardPositions()
        Description: This function is called internally when addCard(Card card) is called. It updates the positions
                     of every card in your hand so they are evenly placed between two points.
    */

    private void ajustCardPositions() {
        float spacing = (1.0f / (float)(handSize + 1)) * handWidth;  //Calculates horizontal spacing
        float currentX = -(handWidth / 2);
        int count = 1;
        foreach(Card card in cards) {                           //Loops through every card in hand
            currentX += spacing;                                //Increments x position by calculated spacing
            card.getCardController().doLerpPos(new Vector2(currentX + handOffsetX, handYPos), 0.2f);
            card.getCardController().doLerpScale(card.getCardScale(), cardSize, 0.2f);
            card.setLayer(count);                               //Increments render layer (z coordinate)
            count++;                                            //Increments count
        }
    }

    /*
        Method: playCard(int cardId)
        Description: Attempts to play a card to the pile. If a player attempts to make an illegal move, the card will not
                    be played to the pile.
    */

    public void playCard(int cardID) {

        /* Finds the card in the hand with the given cardID */
        Card card = null;
        int index = 0;
        for(int i = 0; i < handSize; i++) {
            card = cards[i];
            index = i;
            if(card.getCardID() == cardID)
                break;
        }

        /* If the move attempt is successful remove it from the hand and make necessary ajustments */
        if(pile.attemptMove(card)) {
            removeCard(index);
            handSize--;
            ajustCardPositions();
        }

    }

    public void setIsPlayerHand(bool val) {
        isPlayerHand = val;
    }

    public void setCardSize(float val) {
        cardSize = val;
    }

    public void setHandOffsetX(float val) {
        handOffsetX = val;
    }

    public void setHandWidth(float val) {
        handWidth = val;
    }

    public void setHandYPos(float val) {
        handYPos = val;
    }

    public void setIsEnemy(bool val) {
        isEnemy = val;
    }

}
