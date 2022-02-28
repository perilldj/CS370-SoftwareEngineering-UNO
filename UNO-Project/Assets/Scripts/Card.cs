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

public class Card {
    
    private Sprite class_sprite;
    private Sprite type_sprite;

    private int cardClass;
    private int cardType;

    public Deck deck;

    public void createOnScreenCard() {
        Texture2D class_texture = deck.getTexture(cardClass);
        Texture2D type_texture = deck.getTexture(cardType);
        class_sprite = Sprite.Create(class_texture, new Rect(0.0f, 0.0f, class_texture.width, class_texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        type_sprite = Sprite.Create(type_texture, new Rect(0.0f, 0.0f, type_texture.width, type_texture.height), new Vector2(0.5f, 0.5f), 101.0f);
        Debug.Log(class_sprite);
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
