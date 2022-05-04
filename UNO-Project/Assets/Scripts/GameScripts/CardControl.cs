using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: CardControl : MonoBehavior
    Description: Used to control the behavior of the physical GameObject that represents a card on screen.

    Methods:   Start();
               Update();
               public doLerpPos(Vector2 targetPos, float duration);
               public doLerpScale(float startScale, float goalScale, float duration);
               public void setOwningCard(Card card);
               public Card getOwningCard();
               public void setIsHovering(bool val);
               public void setCanRise(bool val);
               public void setCanPlay(bool val);
               private void OnMouseDown();
               public void stopHover();


    Author: perilldj
*/

public class CardControl : MonoBehaviour {

    /* On screen objects */
    private GameObject cardClassObject;
    private GameObject cardTypeObject;
    private SpriteRenderer cardClassRenderer;
    private SpriteRenderer cardTypeRenderer;
    private bool isFaceDown = false;

    private Hand currentHand;
    private Deck deck;

    private int card_class;
    private int card_type;
    private int id;
    private bool isEnemy;

    private const float RISE_SPEED = 3.0f;
    private const float MAX_RISE_HEIGHT = 0.35f;
    private bool isHovering = false;
    private float direction = -1.0f;
    private float currentRiseHeight = 0;
    private bool canRise = false;
    private bool lockHover = false;

    private bool playEnable = true;

    //On screen data
    private Vector2 position;
    private int layer = 0;
    private float scale = 0.3f;

    void Start() { }

    void Update() {

        /* Controls the motion of the card when the mouse is hovering over it */
        if(canRise) {
            Vector2 pos = getCardPos();
            currentRiseHeight += (RISE_SPEED * Time.deltaTime * direction); //Increases the height of teh card by RISE_SPEED * Time.deltaTime * direction

            /* Clamps currentRiseHeight so it remains in bounds */
            if(currentRiseHeight > MAX_RISE_HEIGHT)
                currentRiseHeight = MAX_RISE_HEIGHT;
            else if(currentRiseHeight < 0.0f)
                currentRiseHeight = 0.0f;

            //Updates transform position. NOTE: Does not change the Card.cs position value, it doesn't need to.
            transform.position = new Vector3(pos.x, pos.y + currentRiseHeight, getCardZ());
        }

    }

    public void create(int card_class, int card_type, int id, bool isEnemy, Hand hand, Deck deck) {
        this.deck = deck;
        setCurrentHand(hand);
        setCardClass(card_class);
        setCardType(card_type);
        setCardID(id);
        this.isEnemy = isEnemy;
        createOnScreenCard();
    }

    /*
        Method: destroy()
        Description: destroys the on screen card in its entirety.
    */

    public void destroy() {
        GameObject.Destroy(this);
        GameObject.Destroy(cardClassObject);
        GameObject.Destroy(cardTypeObject);
    }

    public void doLerpPos(Vector2 targetPos, float duration, bool disableHover) {
        StartCoroutine(lerpPos(targetPos, duration, disableHover));
    }

    public void doLerpScale(float startScale, float goalScale, float duration) {
        StartCoroutine(lerpScale(startScale, goalScale, duration));
    }

    /*
        Method: IEnumerator lerpPos(Vector2 targetPos, float duration)
        Description: When called the card will slide to the target position for the given duration in seconds
    */

    private IEnumerator lerpPos(Vector2 targetPos, float duration, bool disableHover) {
        float time = 0;
        setCardPos(transform.position);
        Vector2 startPos = transform.position;
        setCanRise(false);
        while(time < duration) { //A loop will occur independantly every frame after the method is called
            setCardPos(Vector2.Lerp(startPos, targetPos, time / duration)); //Applies lerp
            time += Time.deltaTime; //Increments elapsed time by deltaTime
            yield return null;
        }
        if(!disableHover)
            setCanRise(true);
        setIsHovering(false);
        setCardPos(targetPos);
        
    }

    /*
        Method: IEnumerator lerpScale(float startScale, float goalScale, float duration)
        Description: When called the card will scale to the target size for the given duration in seconds.
    */

    private IEnumerator lerpScale(float startScale, float goalScale, float duration) {
        float time = 0;
        setCardScale(startScale);
        while(time < duration) { //A loop will occur independantly every frame after the method is called
            setCardScale(Mathf.Lerp(startScale, goalScale, time / duration)); //Applies lerp
            time += Time.deltaTime; //Increments elapsed time by deltaTime
            yield return null;
        }
        setCardScale(goalScale);
    }

    public void createOnScreenCard() {
        
        if(isEnemy) {
            setPlayEnable(false);
        }

        /* Gets cards on screen sprite objects */
        cardClassObject = transform.GetChild(0).gameObject;
        cardTypeObject = transform.GetChild(0).GetChild(0).gameObject;

        /* Gets card's class and type SpriteRenderers */
        cardClassRenderer = cardClassObject.GetComponent<SpriteRenderer>();
        cardTypeRenderer = cardTypeObject.GetComponent<SpriteRenderer>();

        updateCardTexture(isEnemy);

        /* Set scale and position */
        setCardScale(scale);
        setCardPos(deck.transform.position);

    }

    public void flipCard() {
        isFaceDown = !isFaceDown;
        updateCardTexture(isFaceDown);
    }

    private void updateCardTexture(bool side) {

        Texture2D class_texture, type_texture;
        
        if(!side) {
            
            class_texture = deck.getTexture(card_class);   //Get class texture
            /* If a card class is a wild card or a +4 card, there is no type necessary, this
            checks for that and gets the appropriate texture */
            if(card_class == CardTypes.WILD_CARD || card_class == CardTypes.PLUS_FOUR_CARD)
                type_texture = deck.getTexture(CardTypes.NONE);
            else
                type_texture = deck.getTexture(card_type);

        } else {

            class_texture = deck.getTexture(CardTypes.BACK_CARD);
            type_texture = deck.getTexture(CardTypes.NONE);

        }

        isFaceDown = !isFaceDown;
        cardClassRenderer.sprite = Sprite.Create(class_texture, new Rect(0.0f, 0.0f, class_texture.width, class_texture.height), new Vector2(0.5f, 0.5f), 100.0f);
        cardTypeRenderer.sprite = Sprite.Create(type_texture, new Rect(0.0f, 0.0f, type_texture.width, type_texture.height), new Vector2(0.5f, 0.5f), 100.0f);

    }

    /*
        Method: setIsHovering(bool val)
        Description: Sets the isHovering bool, also sets the direction of the hover, rise or fall.
    */

    public void setIsHovering(bool val) {
        if(!lockHover) {
            isHovering = val;
             if(isHovering)
                 direction = 1.0f;
            else
                direction = -1.0f;
        }
    }

    public void disableHover() {
        lockHover = true;
        direction = -1.0f;
    }

    public void enableHover() {
        lockHover = false;
    }

    /*
        Method: OnMouseDown()
        Description: Triggered when this card is clicked, makes an attempt to play the card.
    */

    private void OnMouseDown() {
        if(playEnable)
            currentHand.playCard(id, false);
    }


    /*
        GETTERS AND SETTERS
    */

    public int getCardID() {
        return id;
    }

    public void setCardID(int val) {
        id = val;
    }

    public int getCardClass() {
        return card_class;
    }

    public void setCardClass(int val) {
        card_class = val;
    }

    public int getCardType() {
        return card_type;
    }

    public void setCardType(int val) {
        card_type = val;
    }

    public void setCardPos(Vector2 pos) {
        position = pos;
        transform.position = new Vector3(pos.x, pos.y, layer * -0.02f);
    }

    public Vector2 getCardPos() {
        return position;
    }

    public void setCardScale(float scale) {
        this.scale = scale;
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public float getCardScale() {
        return scale;
    }

    public void setLayer(int layer) {
        this.layer = layer;
        setCardPos(position);
    }

    public int getLayer() {
        return layer;
    }

    public float getCardZ() {
        return layer * -0.02f;
    }

    public void setCurrentHand(Hand hand) {
        currentHand = hand;
    }

    public Hand getCurrentHand() {
        return currentHand;
    }

    public void setCanRise(bool val) {
        canRise = val;
    }

    public bool getCanRise() {
        return canRise;
    }

    public void setPlayEnable(bool val) {
        playEnable = val;
    }

    public bool getIsHovering() {
        return isHovering;
    }

}
