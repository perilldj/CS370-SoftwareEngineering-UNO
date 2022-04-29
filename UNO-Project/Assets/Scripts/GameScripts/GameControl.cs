using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private GameObject turnIndicator;
    private TMP_Text turnIndicatorText;
    private Image turnIndicatorImage;

    [SerializeField]
    private List<Vector2> enemyPositions;
    private List<Enemy> enemies = new List<Enemy>();

    [SerializeField]
    private GameObject deck;
    private Deck deckScript;
    private Vector2 deckPos = new Vector2(-1.0f, 0.0f); // I hate hard coding positions like this, but it will do for now

    private Pile pile;
    private Vector2 pileLoc = new Vector2(1.0f, 0.0f);

    private Hand playerHand;

    private int opponentCount;

    private CardControl currentCard = null;

    private const int INITIAL_CARD_COUNT = 7;
    private int turnDirection = 1;
    private int turnIndex = 0;
    private int playerIndex;
    private bool gameEnabled = true;
    private bool turnComplete = false;

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

        turnIndicator = Instantiate(turnIndicator);
        turnIndicatorImage = turnIndicator.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        turnIndicatorText = turnIndicator.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        hidePlayerTurnIndicator();

        for(int i = 0; i < enemyPositions.Count; i++) {
            enemies.Add(new Enemy(enemyPositions[i], enemyPrefab, pile));
        }

        StartCoroutine(initializeGame());
        
    }

    private void hidePlayerTurnIndicator() {
        turnIndicatorImage.enabled = false;
        turnIndicatorText.enabled = false;
    }

    private void showPlayerTurnIndicator() {
        turnIndicatorImage.enabled = true;
        turnIndicatorText.enabled = true;
    }

    private IEnumerator initializeGame() {

        yield return new WaitForSeconds(2.0f);

        for(int i = 0; i < enemies.Count; i++) {
            for(int j = 0; j < INITIAL_CARD_COUNT; j++) {
                enemies[i].addCard(deckScript.drawCard());
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        for(int j = 0; j < INITIAL_CARD_COUNT; j++) {
            playerHand.addCard(deckScript.drawCard()); //might need a player class later
            yield return new WaitForSeconds(0.1f);
        }

        System.Random ran = new System.Random();
        playerIndex = enemies.Count;
        turnIndex = ran.Next(0, playerIndex + 1);

        StartCoroutine(gameLoop());

    }

    private IEnumerator gameLoop() {

        while(gameEnabled) {

            turnComplete = false;

            yield return new WaitForSeconds(1.0f);

            if(turnIndex == playerIndex) {
                showPlayerTurnIndicator();
                playerHand.setCanMove(true);

                while(!pile.canMove(playerHand)) {
                    playerHand.addCard(deckScript.drawCard());
                    yield return new WaitForSeconds(0.8f);
                }

                yield return new WaitUntil(() => turnComplete == true);
                playerHand.setCanMove(false);
                hidePlayerTurnIndicator();

            } else {

                Enemy enemy = enemies[turnIndex];

                enemy.setRedBackground();
                enemy.getHand().setCanMove(true);

                yield return new WaitForSeconds(1.0f);

                while(!pile.canMove(enemy.getHand())) {
                    enemy.addCard(deckScript.drawCard());
                    yield return new WaitForSeconds(0.8f);
                }

                enemy.attemptRandomMove();
                enemy.getHand().setCanMove(false);
                enemy.setWhiteBackground();

            }

            turnIndex += turnDirection;
            if(turnIndex > playerIndex)
                turnIndex = turnIndex - playerIndex - 1;
            if(turnIndex < 0)
                turnIndex = playerIndex;

        }

    }

    public void completedMove() {
        turnComplete = true;
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
                if(!cardControl.getIsHovering())
                    cardControl.setIsHovering(true);
            }
        } else
            if(currentCard != null && currentCard.getIsHovering())
                currentCard.setIsHovering(false);

    }

    /*
        Method: reversePlayed()
        Description: Called whenever a reverse is played, reverses the direction of the spinner.
    */

    public void reversePlayed() {
        if(directionController.getIsSpinningClockwise()) {
            directionController.spinCounterClockwise();
            turnDirection = -1;
        } else {
            directionController.spinClockwise();
            turnDirection = 1;
        }
    }

    /*
        Method: skipPlayed()
        Description: Called whenever a skip is played, momentarily speeds up the angular velocity of the spinner.
    */

    public void skipPlayed() {
        directionController.doSpeedup();
        turnIndex += turnDirection;
    }

}
