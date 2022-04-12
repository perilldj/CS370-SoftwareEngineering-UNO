using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private GameObject directionIndicatorObject;
    private SpinDirectionController directionController;

    [SerializeField]
    private GameObject backgroundController; //Camera????

    [SerializeField]
    private GameObject soundController; //TODO: Figure out sounds

    [SerializeField]
    private GameObject deck;
    private Deck deckScript;
    private Vector2 deckPos = new Vector2(-1.0f, 0.0f); // I hate hard coding positions like this, but it will do for now

    private Pile pile;
    private Vector2 pileLoc = new Vector2(1.0f, 0.0f);

    private Hand playerHand;

    private int opponentCount;
    private Hand[] enemyHands;

    private CardControl currentCard = null;

    // Start is called before the first frame update
    void Start() {

        deck = Instantiate(deck);
        deckScript = deck.GetComponent<Deck>();

        playerHand = new Hand();
        deckScript.hand = playerHand;

        deckScript.initializeDeck();

        deckScript.setDeckPos(deckPos);
        directionIndicatorObject = Instantiate(directionIndicatorObject);
        directionController = directionIndicatorObject.GetComponent<SpinDirectionController>();
        directionController.spinClockwise();

        pile = new Pile(pileLoc, deckScript.drawCard());
        playerHand.pile = pile;

    }

    // Update is called once per frame
    void Update() {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit) {
            GameObject selectedObject = hit.collider.gameObject;
            CardControl cardControl = selectedObject.GetComponent<CardControl>();
            if(cardControl != null) {
                if(currentCard != null)
                    currentCard.setIsHovering(false);
                currentCard = cardControl;
                cardControl.setIsHovering(true);
            }
        } else {
            if(currentCard != null)
                    currentCard.setIsHovering(false);
        }

    }
}
