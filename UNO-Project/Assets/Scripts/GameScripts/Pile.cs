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
    private Vector2 pilePos;
    private int currentClass = CardTypes.BLUE_CARD;
    private int currentType = CardTypes.ONE_CARD;
    private BackgroundController backgroundController;
    private GameControl gameController;

    /* 
       Constructor: Pile(Vector2 pos)
       Description: Initializes visual component and sets position of pile
    */ 

    public Pile(Vector2 pos, Card card) {
        this.pilePos = pos;
        card.createOnScreenCard();
        setTopCard(card);
    }

    /*
        Method: public bool attemptMove(Card card)
        Description: Called to attempt a move, it will return false
                     if the move is illegal, true when it is legal
                     (will also execute the move if legal)
    */

    public bool attemptMove(Card card) {

        /* If card is a wild card */
        if(card.getCardClass() == CardTypes.WILD_CARD) {
            setTopCard(card);
            //Do wild
            return true;
        }

        /* If card is a +4 wild card */
        if(card.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
            setTopCard(card);
            //Do +4 wild
            return true;
        }

        if(topCard.getCardClass() == CardTypes.WILD_CARD || topCard.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
            setTopCard(card);
            return true;
        }

        /* If legal by card type */
        if(card.getCardType() == currentType) {
            setTopCard(card);
            return true;
        }

        /* If legal by a non special card class */
        if(card.getCardClass() == currentClass) {
            setTopCard(card);
            return true;
        }

        return false; // False returned for illegal move

    }

    /*
        Method: setTopCard(Card card)
        Description: Sets the visual component of the pile
    */

    private void setTopCard(Card card) {

        currentClass = card.getCardClass();
        currentType = card.getCardType();
        if(topCard != null)
            topCard.destroy();
        topCard = card;
        topCard.setCardPos(pilePos);
        topCardControl = card.getCardController();
        topCardControl.setCanPlay(false);
        topCardControl.stopHover();

        if(backgroundController != null)
            backgroundController.setBackgroundColor(card.getCardClass());

        switch(card.getCardType()) {
            case CardTypes.REVERSE_CARD : gameController.reversePlayed();
                break;
            case CardTypes.SKIP_CARD : gameController.skipPlayed();
                break;
            case CardTypes.PLUS_TWO_CARD : //Do +2
                break;
        }

    }

    public void setBackgroundController(BackgroundController backgroundController) {
        this.backgroundController = backgroundController;
    }

    public void setGameController(GameControl gameController) {
        this.gameController = gameController;
    }

}
