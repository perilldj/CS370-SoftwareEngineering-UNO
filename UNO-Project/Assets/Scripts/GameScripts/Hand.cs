using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: Hand
    Description: A hand of cards for a player. It contains to Vector2 positions for cards to be between on screen
                 Also contains a list of cards that go on screen. Designed for multiple to exist in game

    Methods: addCard(Card card);

    Needed additions: Constructor to set the two Vector2s.
                      Ability to remove a specific card from hand.
                      Differentiate between player and opponent hands to draw cards face up or face down on screen.
                      Calculate the angle so when the defined line for the hand orients the cards correctly.

    Author: perilldj
*/

public class Hand {
    
    /* List of cards in hand */
    private List<Card> cards = new List<Card>();

    /* Both vectors create a line for the cards to be between */
    private Vector2 p1 = new Vector2(-7.5f, -3.5f);
    private Vector2 p2 = new Vector2(7.5f, -3.5f);

    public Deck deck;

    int handSize = 0;

    /*
        Method: addCard(Card card)
        Description: When called, the given card is added to the hand, it will also create an on-screen card representation and sets it
                     to the correct position by calling createOnScreenCard() and ajustCardPositions().
    */

    public void addCard(Card card) {
        handSize++;                 //Increment handSize
        cards.Add(card);            //Add card to hand
        card.createOnScreenCard();  //Add card to screen
        ajustCardPositions();       //Update positions of the cards
    }

    public void removeCard(int index) {
        cards[index].destroy();
        cards.RemoveAt(index);
    }

    /*
        Method: ajustCardPositions()
        Description: This function is called internally when addCard(Card card) is called. It updates the positions
                     of every card in your hand so they are evenly placed between two points.

        Known issues: The algorithm isn't entirely complete it, it just uses two arbitrary horizontal points
                      instead of the two Vector2s created for the hand.
    */

    private void ajustCardPositions() {
        float spacing = (1.0f / (float)(handSize + 1)) * 15.0f;  //Calculates horizontal spacing
        float currentX = -7.5f;
        int count = 0;
        foreach(Card card in cards) {                           //Loops through every card in hand
            currentX += spacing;                                //Increments x position by calculated spacing
            card.setCardPos(new Vector2(currentX, -3.5f));      //Sets card's calculated position
            card.setLayer(count);                               //Increments render layer (z coordinate)
            count++;                                            //Increments count
        }
    }

    public void playCard(int cardID) {

        Card card = null;
        int index = 0;
        for(int i = 0; i < handSize; i++) {
            card = cards[i];
            index = i;
            if(card.getCardID() == cardID)
                break;
        }

        if(deck.pile.attemptMove(card)) {
            removeCard(index);
            handSize--;
        }

    }

}
