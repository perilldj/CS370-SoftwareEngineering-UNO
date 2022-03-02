using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: Hand
    Description: A hand of cards for a player. It contains to Vector2 positions for cards to be between on screen
                 Also contains a list of cards that go on screen. Designed for multiple to exist in game

    Functions: addCard(Card card);

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
    private Vector2 p1 = new Vector2(-4.0f, -2.0f);
    private Vector2 p2 = new Vector2(4.0f, -2.0f);

    int handSize = 0;

    /*
        Function: addCard(Card card)
        Description: When called, the given card is added to the hand, it will also create an on-screen card representation and sets it
                     to the correct position by calling createOnScreenCard() and ajustCardPositions().

        Author: perilldj
    */

    public void addCard(Card card) {
        handSize++;                 //Increment handSize
        cards.Add(card);            //Add card to hand
        card.createOnScreenCard();  //Add card to screen
        ajustCardPositions();       //Update positions of the cards
    }

    /*
        Function: ajustCardPositions()
        Description: This function is called internally when addCard(Card card) is called. It updates the positions
                     of every card in your hand so they are evenly placed between two points.

        Known issues: The algorithm isn't entirely complete it, it just uses two arbitrary horizontal points
                      instead of the two Vector2s created for the hand.

        Author: perilldj
    */

    private void ajustCardPositions() {
        float spacing = (1.0f / (float)(handSize + 1)) * 8.0f;  //Calculates horizontal spacing
        float currentX = -4.0f;
        int count = 0;
        foreach(Card card in cards) {                           //Loops through every card in hand
            currentX += spacing;                                //Increments x position by calculated spacing
            card.setCardPos(new Vector2(currentX, -2.0f));      //Sets card's calculated position
            card.setLayer(count);                               //Increments render layer (z coordinate)
            count++;                                            //Increments count
        }
    }

}
