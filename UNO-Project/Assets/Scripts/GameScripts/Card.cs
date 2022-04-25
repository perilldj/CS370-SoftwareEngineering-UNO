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

    Methods:   public void createOnScreenCard();
               public void onCardClick();
               public void destroy();
               public CardControl getCardController();
               public void setCardPos(Vector2 pos);
               public void setLayer(int val);
               public float getCardZ()
               public void setCardScale(float val);
               public void setCardClass(int cClass);
               public void setCardType(int cType);
               public int getCardClass();
               public int getCardType();
               public int getCardID();
               public GameObject getCardObject();

    Author: perilldj
*/

public class Card {

    //Components for on screen card
    private GameObject card;
    private GameObject cardClassObject;
    private GameObject cardTypeObject;
    private SpriteRenderer cardClassRenderer;
    private SpriteRenderer cardTypeRenderer;
    private CardControl cardControl = null;

    //IDs for card class and type
    private int cardClass;
    private int cardType;

    //Unique identifier for the card
    private int id;

    //Reference to the deck (To get card textures)
    public Deck deck;

    //Cards owning hand
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

        /* Creates on screen game object */
        card = GameObject.Instantiate(card);

        /* Gets the card's card control script */
        cardControl = card.GetComponent<CardControl>();
        cardControl.setOwningCard(this);

        /* Gets cards on screen sprite objects */
        cardClassObject = card.gameObject.transform.GetChild(0).gameObject;
        cardTypeObject = cardClassObject.gameObject.transform.GetChild(0).gameObject;

        /* Gets card's class and type SpriteRenderers */
        cardClassRenderer = cardClassObject.GetComponent<SpriteRenderer>();
        cardTypeRenderer = cardTypeObject.GetComponent<SpriteRenderer>();

        /* Creates sprites for the SpriteRenderers*/
        cardClassRenderer.sprite = Sprite.Create(class_texture, new Rect(0.0f, 0.0f, class_texture.width, class_texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        cardTypeRenderer.sprite = Sprite.Create(type_texture, new Rect(0.0f, 0.0f, type_texture.width, type_texture.height), new Vector2(0.5f, 0.5f), 100.0f);

        /* Set scale and position */
        setCardScale(scale);
        setCardPos(deck.transform.position);

    }

    /*
        Method: onCardClick()
        Description: Gets called from the CardControl script when the in game card is clicked.
                     Tells the owning hand (if it exists) that one of it's cards is attempting to be played.
    */

    public void onCardClick() {
        if(currentHand != null) {
            currentHand.playCard(id);
        }
    }

    /*
        Method: destroy()
        Description: destroys the on screen card in its entirety.
    */

    public void destroy() {
        GameObject.Destroy(card);
        GameObject.Destroy(cardClassObject);
        GameObject.Destroy(cardTypeObject);
    }

    /*
        Method: setCardPos(Vector2 pos)
        Description: Sets the position of the on-screen card.
    */
    public void setCardPos(Vector2 pos) {
        position = pos;
        card.transform.position = new Vector3(pos.x, pos.y, layer * -0.02f);
    }

    /*
        Method getCardController()
        Decsription: Returns the CardControl script attached to the in-game card.
    */

    public CardControl getCardController() {
        return cardControl;
    }

    /*
        Method getCardPos()
        Decsription: Returns a Vector2 which contains the position of the card.
    */

    public Vector2 getCardPos() {
        return position;
    }

    /*
        Method: setLayer(int val)
        Description: Sets the layer of the on-screen card (z level, a higher layer means its ontop of lower levels)
    */

    public void setLayer(int val) {
        layer = val;
        card.transform.position = new Vector3(position.x, position.y, layer * -0.02f);
    }

    /*
        Method: getCardZ()
        Description: Returns the Z position equivalent of the card's layer.
    */

    public float getCardZ() {
        return (float)layer * -0.02f;
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

    /*
        Method: getCardObject()
        Description: Returns a reference to the card's physical GameObject that it owns.
    */

    public GameObject getCardObject() {
        return card;
    }

}
