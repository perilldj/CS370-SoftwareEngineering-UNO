using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pile {

    private Card topCard;
    private int currentClass = CardTypes.BLUE_CARD;
    private int currentType = CardTypes.ONE_CARD;

    public Pile(Vector2 pos) {
        topCard.createOnScreenCard();
        topCard.setCardPos(pos);
    }

    public bool attempltMove(Card card) {

        if(card.getCardClass() == currentClass) {
            setTopCard(card);
            currentType = card.getCardType();
            return true;
        }

        if(card.getCardType() == currentType) {
            
            setTopCard(card);
            currentClass = card.getCardClass();

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

        if(card.getCardType() == CardTypes.WILD_CARD) {
            setTopCard(card);
            currentClass = card.getCardClass();
            currentType = card.getCardType();
            //Do wild
            return true;
        }

        if(card.getCardType() == CardTypes.PLUS_FOUR_CARD) {
            setTopCard(card);
            currentClass = card.getCardClass();
            currentType = card.getCardType();
            //Do +4 wild
            return true;
        }

        return false;

    }

    private void setTopCard(Card card) {
        topCard.setCardClass(card.getCardClass());
        topCard.setCardType(card.getCardType());
    }

}
