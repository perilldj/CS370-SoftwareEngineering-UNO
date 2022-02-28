using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardTypes {

    public const int NONE = 18;

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
    
    private SpriteRenderer class_sprite;
    private SpriteRenderer type_sprite;

    private GameObject class_object;
    private GameObject type_object;

    private int cardClass;
    private int cardType;

    public Deck deck;

    private Vector2 position;
    private int layer;
    private float scale = 0.3f;

    public void createOnScreenCard() {

        Texture2D class_texture = deck.getTexture(cardClass);
        Texture2D type_texture;

        if(cardClass == CardTypes.WILD_CARD || cardClass == CardTypes.PLUS_FOUR_CARD)
            type_texture = deck.getTexture(CardTypes.NONE);
        else
            type_texture = deck.getTexture(cardType);

        class_object = new GameObject();
        type_object = new GameObject();

        class_sprite = class_object.AddComponent<SpriteRenderer>() as SpriteRenderer;
        type_sprite = type_object.AddComponent<SpriteRenderer>() as SpriteRenderer;

        class_sprite.sprite = Sprite.Create(class_texture, new Rect(0.0f, 0.0f, class_texture.width, class_texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        type_sprite.sprite = Sprite.Create(type_texture, new Rect(0.0f, 0.0f, type_texture.width, type_texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        Debug.Log(class_sprite);

        setCardScale(scale);
        setCardPos(Vector2.zero);

    }

    public void setCardPos(Vector2 pos) {
        position = pos;
        if(class_object != null && type_object != null) {
            class_object.transform.position = new Vector3(position.x, position.y, 0.01f * -layer);
            type_object.transform.position = new Vector3(position.x, position.y, 0.01f * -layer - 0.001f);
        }
    }

    public void setLayer(int val) {
        layer = val;
        if(class_object != null && type_object != null) {
            class_object.transform.position = new Vector3(position.x, position.y, 0.01f * -layer);
            type_object.transform.position = new Vector3(position.x, position.y, 0.01f * -layer - 0.001f);
        }
    }

    public void setCardScale(float val) {
        scale = val;
        if(class_object != null && type_object != null) {
            class_object.transform.localScale = new Vector3(scale, scale, 1);
            type_object.transform.localScale = new Vector3(scale, scale, 1);
        }
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