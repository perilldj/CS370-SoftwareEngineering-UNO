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

    private Card owningCard;
    private bool isHovering = false;

    private const float RISE_SPEED = 3.0f;
    private const float MAX_RISE_HEIGHT = 0.35f;

    private float direction = -1.0f;

    private float currentRiseHeight = 0;
    
    private bool canRise = false;
    private bool lockHover = false;
    private bool playEnable = true;

    void Start() { }

    void Update() {

        /* Controls the motion of the card when the mouse is hovering over it */
        if(canRise) {
            Vector2 pos = owningCard.getCardPos();
            currentRiseHeight += (RISE_SPEED * Time.deltaTime * direction); //Increases the height of teh card by RISE_SPEED * Time.deltaTime * direction

            /* Clamps currentRiseHeight so it remains in bounds */
            if(currentRiseHeight > MAX_RISE_HEIGHT)
                currentRiseHeight = MAX_RISE_HEIGHT;
            else if(currentRiseHeight < 0.0f)
                currentRiseHeight = 0.0f;

            //Updates transform position. NOTE: Does not change the Card.cs position value, it doesn't need to.
            transform.position = new Vector3(pos.x, pos.y + currentRiseHeight, owningCard.getCardZ());
        }

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
        owningCard.setCardPos(transform.position);
        Vector2 startPos = transform.position;
        setCanRise(false);
        while(time < duration) { //A loop will occur independantly every frame after the method is called
            owningCard.setCardPos(Vector2.Lerp(startPos, targetPos, time / duration)); //Applies lerp
            time += Time.deltaTime; //Increments elapsed time by deltaTime
            yield return null;
        }
        if(!disableHover)
            setCanRise(true);
        setIsHovering(false);
        owningCard.setCardPos(targetPos);
        
    }

    /*
        Method: IEnumerator lerpScale(float startScale, float goalScale, float duration)
        Description: When called the card will scale to the target size for the given duration in seconds.
    */

    private IEnumerator lerpScale(float startScale, float goalScale, float duration) {
        float time = 0;
        owningCard.setCardScale(startScale);
        while(time < duration) { //A loop will occur independantly every frame after the method is called
            owningCard.setCardScale(Mathf.Lerp(startScale, goalScale, time / duration)); //Applies lerp
            time += Time.deltaTime; //Increments elapsed time by deltaTime
            yield return null;
        }
        owningCard.setCardScale(goalScale);
    }

    /*
        Method: setOwningCard(Card card)
        Description: Sets a reference to the owning Card object.
    */

    public void setOwningCard(Card card) {
        owningCard = card;
    }
    
    /*
        Method: getOwningCard()
        Description: Returns reference to the owning Card object.
    */

    public Card getOwningCard() {
        return owningCard;
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

    /*
        Method: setCanRise(bool val)
        Description: Sets the ability to hover all together.
    */

    public void setCanRise(bool val) {
        canRise = val;
    }

    /*
        Method: OnMouseDown()
        Description: Triggered when this card is clicked, makes an attempt to play the card.
    */

    private void OnMouseDown() {
            owningCard.onCardClick();
    }

    /*
        Method: stopHover()
        Description: Stop the ability to hover, while still allowing it to fall back down to its resting position.
    */

    public void stopHover() {
        direction = -1.0f;
        lockHover = true;
    }

    public bool getIsHovering() {
        return isHovering;
    }

    public void setPlayEnable(bool val) {
        playEnable = val;
        Debug.Log("canPlay set to: " + val + "\nCard info: C: " + owningCard.getCardClass() + " T: " + owningCard.getCardType());
    }

}
