using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardControl : MonoBehaviour {

    private Card owningCard;
    private bool isHovering = false;

    private const float RISE_SPEED = 3.0f;
    private const float MAX_RISE_HEIGHT = 0.35f;
    private float direction = -1.0f;
    private float currentRiseHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {

        Vector2 pos = owningCard.getCardPos();
        currentRiseHeight += (RISE_SPEED * Time.deltaTime * direction);

        if(currentRiseHeight > MAX_RISE_HEIGHT)
            currentRiseHeight = MAX_RISE_HEIGHT;
        else if(currentRiseHeight < 0.0f)
            currentRiseHeight = 0.0f;

        transform.position = new Vector2(pos.x, pos.y + currentRiseHeight);

    }

    public void setOwningCard(Card card) {
        owningCard = card;
    }
    
    public Card getOwningCard() {
        return owningCard;
    }

    public void setIsHovering(bool val) {
        isHovering = val;
        if(isHovering)
            direction = 1.0f;
        else
            direction = -1.0f;
    }

    private void OnMouseDown() {

    }

}
