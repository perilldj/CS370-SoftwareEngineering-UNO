using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: Pile
    Description: This script manages the pile of played cards. It enforces legal moves and changes its
                 appearance when a card is played.

    Methods:
        public Pile(Vector2 pos)
        public bool attemptMove(Card card)

    Author: perilldj
*/

public class Pile {

    private Card topCard;
    private CardControl topCardControl;
    private int currentClass = CardTypes.BLUE_CARD;
    private int currentType = CardTypes.ONE_CARD;

    /* 
       Constructor: Pile(Vector2 pos)
       Description: Initializes visual component and sets position of pile
    */ 

    public Pile(GameObject cardPrefab, Vector2 pos, Deck deck) {
        topCard = new Card(cardPrefab, null, -1);
        topCard.deck = deck;
        topCard.setCardClass(currentClass);
        topCard.setCardType(currentType);
        topCard.createOnScreenCard();
        topCard.setCardPos(pos);
        topCardControl = topCard.getCardObject().GetComponent<CardControl>();
        topCardControl.setCanRise(false);
    }

    /*
        Method: public bool attemptMove(Card card)
        Description: Called to attempt a move, it will return false
                     if the move is illegal, true when it is legal
                     (will also execute the move if legal)
    */

    public bool attemptMove(Card card) {

        /* If card is a wild card */
        if(card.getCardType() == CardTypes.WILD_CARD) {
            setTopCard(card);
            currentClass = card.getCardClass();
            currentType = card.getCardType();
            //Do wild
            return true;
        }

        /* If card is a +4 wild card */
        if(card.getCardType() == CardTypes.PLUS_FOUR_CARD) {
            setTopCard(card);
            currentClass = card.getCardClass();
            currentType = card.getCardType();
            //Do +4 wild
            return true;
        }

        /* If legal by a non special card class */
        if(card.getCardClass() == currentClass) {
            setTopCard(card);
            currentType = card.getCardType();
            return true;
        }

        /* If legal by card type */
        if(card.getCardType() == currentType) {

            setTopCard(card);
            currentClass = card.getCardClass();

            /* Checks for special card types and performs their functions */
            switch(card.getCardType()) {
                case CardTypes.REVERSE_CARD : //Do reverse
                    break;
                case CardTypes.SKIP_CARD : //Do skip
                    break;
                case CardTypes.PLUS_TWO_CARD : //Do +2
                    break;
            }

            return true;

        }

        return false; // False returned for illegal move

    }

    /*
        Method: setTopCard(Card card)
        Description: Sets the visual component of the pile
    */

    private void setTopCard(Card card) {
        topCard.setCardClass(card.getCardClass());
        topCard.setCardType(card.getCardType());
    }

}
