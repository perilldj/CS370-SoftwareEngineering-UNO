using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    Class: GameControl : MonoBehavior
    Description: Main game class, controlls the flow of the game and will interface with visual aspects and
                 networking aspects of the game.
    Author: perilldj
*/

public class GameControl : MonoBehaviour {

    [SerializeField]
    private GameObject cardPrefab;

    [SerializeField]
    private GameObject directionIndicatorObject;
    private SpinDirectionController directionController;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject backgroundObject;
    private BackgroundController backgroundController;

    [SerializeField]
    private GameObject soundController; //TODO: Figure out sounds

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private List<Vector2> enemyPositions;

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

        /* Creates a deck */
        deck = Instantiate(deck);
        deckScript = deck.GetComponent<Deck>();

        /* Creates the player's hand */
        playerHand = new Hand();
        deckScript.hand = playerHand;

        /* Initial deck setup (includes shuffling) */
        deckScript.initializeDeck();
        deckScript.setDeckPos(deckPos);

        /* Set up spinner */
        directionIndicatorObject = Instantiate(directionIndicatorObject);
        directionController = directionIndicatorObject.GetComponent<SpinDirectionController>();
        directionController.spinClockwise();

        /* Sets up the pile and adds the top card of the deck to it */
        Card firstCard = deckScript.drawCard();
        pile = new Pile(pileLoc, firstCard, this);
        playerHand.pile = pile;

        /* Sets up the background color controller */
        backgroundObject = Instantiate(backgroundObject);
        backgroundController = backgroundObject.GetComponent<BackgroundController>();
        backgroundController.setCamera(cam);

        backgroundController.setBackgroundColor(firstCard.getCardClass());

        pile.setBackgroundController(backgroundController);

        RandomNames.clearUsedNames();

        Enemy enemy = new Enemy(new Vector2(-7.0f,0), enemyPrefab);
        enemy.getHand().addCard(deckScript.drawCard());
        enemy.getHand().addCard(deckScript.drawCard());
        enemy.getHand().addCard(deckScript.drawCard());
        enemy.getHand().addCard(deckScript.drawCard());
        enemy.getHand().addCard(deckScript.drawCard());
        enemy.getHand().addCard(deckScript.drawCard());
        enemy.getHand().addCard(deckScript.drawCard());
        enemy.getHand().addCard(deckScript.drawCard());
    }

    // Update is called once per frame
    void Update() {

        /* Calculates if the mouse is hovering over an object 
           by shooting a ray downwards from the mouse's position and
           returns whatever it hit, if it hits anything. */
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit) {  //If a hit occurs, check if it is a card to hover
            GameObject selectedObject = hit.collider.gameObject;
            CardControl cardControl = selectedObject.GetComponent<CardControl>();
            if(cardControl != null) {
                if(currentCard != null)
                    currentCard.setIsHovering(false);
                currentCard = cardControl;
                cardControl.setIsHovering(true);
            }
        } else
            if(currentCard != null)
                    currentCard.setIsHovering(false);

    }

    /*
        Method: reversePlayed()
        Description: Called whenever a reverse is played, reverses the direction of the spinner.
    */

    public void reversePlayed() {
        if(directionController.getIsSpinningClockwise())
            directionController.spinCounterClockwise();
        else
            directionController.spinClockwise();
    }

    /*
        Method: skipPlayed()
        Description: Called whenever a skip is played, momentarily speeds up the angular velocity of the spinner.
    */

    public void skipPlayed() {
        directionController.doSpeedup();
    }

}
