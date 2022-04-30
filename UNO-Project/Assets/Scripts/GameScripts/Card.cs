using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: CardTypes (Static)
    Description: IDs for cards to use instead of their numerical values
    Author: perilldj
*/

public static class CardTypes {

    /* Transparent Texture */
    public const int NONE = 18;
    public const int BACK_CARD = 19;

    /* Card Classes */
    public const int BLUE_CARD   = 0,
                     RED_CARD    = 1,
                     GREEN_CARD  = 2,
                     YELLOW_CARD = 3,
                     WILD_CARD   = 4,
                     PLUS_FOUR_CARD = 5;

    /* Card Types */
    public const int ONE_CARD = 6,
                     TWO_CARD = 7,
                     THREE_CARD = 8,
                     FOUR_CARD = 9,
                     FIVE_CARD = 10,
                     SIX_CARD = 11,
                     SEVEN_CARD = 12,
                     EIGHT_CARD = 13,
                     NINE_CARD = 14,
                     PLUS_TWO_CARD = 15,
                     REVERSE_CARD = 16,
                     SKIP_CARD = 17;

}

/*
    Class: Card
    Description: Object to represent a card, is not the physical card, but it serves as a way to hold information
                 about cards within a shuffled deck.

    Author: perilldj
*/

public class Card {

    //IDs for card class and type
    private int cardClass;
    private int cardType;

    //Unique identifier for the card
    private int id;

    public Card(int id) {
        this.id = id;
    }

    /*
        Method: setCardClass(int cClass)
        Description: Sets the class of the card
    */

    public void setCardClass(int cClass) {
        cardClass = cClass;
    }

    /*
        Method: setCardType(int cType)
        Description: Sets the type of the card.
    */

    public void setCardType(int cType) {
        cardType = cType;
    }

    /*
        Method: getCardClass()
        Description: Returns the class ID of the card.
    */

    public int getCardClass() {
        return cardClass;
    }

    /*
        Method: getCardType()
        Description: Returns the type ID of the card.
    */

    public int getCardType() {
        return cardType;
    }

    /*
        Method: getCardID()
        Description: Returns the unique ID of the card.
    */

    public int getCardID() {
        return id;
    }

}
