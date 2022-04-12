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
    Description: Object for a card game, has the capability to be on or off screen and still retain it's data
                 A card has a class and a type, information of what makes a class and a type can be found in the CardTypes class.

    Methods:   createOnScreenCard();
               setCardPos(Vector2 pos);
               public void setLayer(int val);
               public void setCardScale(float val);
               public void setCardClass(int cClass);
               public void setCardType(int cType);
               public int getCardClass();
               public int getCardType();

    Author: perilldj

*/

public class Card {

    private GameObject card;

    private GameObject cardClassObject;
    private GameObject cardTypeObject;

    private SpriteRenderer cardClassRenderer;
    private SpriteRenderer cardTypeRenderer;

    //IDs for card class and type
    private int cardClass;
    private int cardType;

    private int id;

    //Reference to the deck (To get card textures)
    public Deck deck;

    public Hand currentHand = null;

    //On screen data
    private Vector2 position;
    private int layer = 0;
    private float scale = 0.3f;

    /*
        Method: createOnScreenCard()
        Description: Creates a visual card on-screen thats tied to this class.
    */

    public Card(GameObject cardPrefab, Hand currentHand, int id) {
        this.card = cardPrefab;
        this.currentHand = currentHand;
        this.id = id;
    }

    public void createOnScreenCard() {

        Texture2D class_texture = deck.getTexture(cardClass);   //Get class texture

        Texture2D type_texture;
        /* If a card class is a wild card or a +4 card, there is no type necessary, this
           checks for that and gets the appropriate texture */
        if(cardClass == CardTypes.WILD_CARD || cardClass == CardTypes.PLUS_FOUR_CARD)
            type_texture = deck.getTexture(CardTypes.NONE);
        else
            type_texture = deck.getTexture(cardType);

        card = GameObject.Instantiate(card);

        CardControl cardControl = card.GetComponent<CardControl>();
        cardControl.setOwningCard(this);

        cardClassObject = card.gameObject.transform.GetChild(0).gameObject;
        cardTypeObject = cardClassObject.gameObject.transform.GetChild(0).gameObject;

        cardClassRenderer = cardClassObject.GetComponent<SpriteRenderer>();
        cardTypeRenderer = cardTypeObject.GetComponent<SpriteRenderer>();

        /* Creates sprites for the SpriteRenderers*/
        cardClassRenderer.sprite = Sprite.Create(class_texture, new Rect(0.0f, 0.0f, class_texture.width, class_texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        cardTypeRenderer.sprite = Sprite.Create(type_texture, new Rect(0.0f, 0.0f, type_texture.width, type_texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        /* Set scale and position */
        setCardScale(scale);
        setCardPos(Vector2.zero);

    }

    public void destroy() {
        GameObject.Destroy(card);
        GameObject.Destroy(cardClassObject);
        GameObject.Destroy(cardTypeObject);
    }

    /*
        Method: setCardPos(Vector2 pos)
        Description: Sets the position of the on-screen card
    */
    public void setCardPos(Vector2 pos) {
        position = pos;
        card.transform.position = new Vector3(pos.x, pos.y, layer * -0.01f);
    }

    public Vector2 getCardPos() {
        return position;
    }

    /*
        Method: setLayer(int val)
        Description: Sets the layer of the on-screen card (z level, a higher layer means its ontop of lower levels)
    */

    public void setLayer(int val) {
        layer = val;
        card.transform.position = new Vector3(position.x, position.y, layer * -0.01f);
    }

    public float getCardZ() {
        return (float)layer * -0.01f;
    }

    /*
        Method: setCardScale(float val)
        Description: Sets the scale of the on-screen card.
    */

    public void setCardScale(float val) {
        scale = val;
        card.transform.localScale = new Vector3(val, val, val);
    }

    /*
        Method: setCardClass(int cClass)
        Description: Sets the class of the card
        Note: May need to be changed so it changes the on-screen card's texture as well.
    */

    public void setCardClass(int cClass) {
        cardClass = cClass;
    }

    /*
        Method: setCardType(int cType)
        Description: Sets the type of the card.
        Note: May need to be changed so it changes the on-screen card's texture as well.
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
