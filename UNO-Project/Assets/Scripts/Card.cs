using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardTypes {

    /* Card Classes */
    public const int BLUE_CARD   = 0,
                     RED_CARD    = 1,
                     GREEN_CARD  = 2,
                     YELLOW_CARD = 3,
                     WILD_CARD   = 4,
                     PLUS_FOUR_CARD = 5;

    /* Card Types */
    public const int ONE_CARD = 1,
                     TWO_CARD = 2,
                     THREE_CARD = 3,
                     FOUR_CARD = 4,
                     FIVE_CARD = 5,
                     SIX_CARD = 6,
                     SEVEN_CARD = 7,
                     EIGHT_CARD = 8,
                     NINE_CARD = 9,
                     PLUS_TWO_CARD = 10,
                     REVERSE_CARD = 11,
                     SKIP_CARD = 12;

}

public class Card : MonoBehaviour {
    
    private Sprite class_sprite;
    private Sprite card_sprite;

    private int cardClass;
    private int cardType;

    public void createOnScreenCard() {

    }

    public void setCardClass(int cClass) {
        cardClass = cClass;
    }

    public void setCardType(int cType) {
        cardType = cType;
    }

    public int getCardClass() {
        return cardClass;
    }

    public int getCardType() {
        return cardType;
    }

}
