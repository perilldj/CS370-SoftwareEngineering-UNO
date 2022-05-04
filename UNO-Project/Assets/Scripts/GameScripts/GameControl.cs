using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

/* 
    Class: GameControl : MonoBehavior
    Description: Main game class, controlls the flow of the game and will interface with visual aspects and
                 networking aspects of the game.
    Author: perilldj
*/

public class GameControl : MonoBehaviourPunCallbacks {

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
    private GameObject soundController;
    private SoundManager soundManager;

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
    private GameObject winScreen;

    [SerializeField]
    private GameObject deck;
    private Deck deckScript;
    private Vector2 deckPos = new Vector2(-1.0f, 0.0f);

    private Queue<Card> multiplayerDeck = new Queue<Card>();

    private Pile pile;
    private Vector2 pileLoc = new Vector2(1.0f, 0.0f);

    private List<ColorPicker> buttons = new List<ColorPicker>();

    private Hand playerHand;

    private int opponentCount;

    private CardControl currentCard = null;

    private const int INITIAL_CARD_COUNT = 7;
    private int numOfOpponents = 4; // Maximum is 4
    private int turnDirection = 1;
    private int turnIndex = 0;
    private int playerIndex;
    private bool gameEnabled = true;
    private bool turnComplete = false;

    private bool moveReceived = false;
    private int moveID = -1;
    private int moveCardID = -1;
    private int colorChange = -1;

    private bool colorSelected = false;
    private int selectedColor = 0;

    private int playerID = -1;

    private System.Random ran = new System.Random();

    // Start is called before the first frame update
    void Start() {

        Application.runInBackground = true;

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
        
        Card firstCard;

        if(ClientInfo.isMultiplayer) {

            firstCard = new Card(-1);
            firstCard.setCardClass(CardTypes.BLUE_CARD);
            firstCard.setCardType(CardTypes.ONE_CARD);
            pile = new Pile(pileLoc, firstCard, cardPrefab, this, deckScript);
            playerHand.pile = pile;
            playerHand.deck = deckScript;

        } else {
            firstCard = deckScript.drawCard();
            pile = new Pile(pileLoc, firstCard, cardPrefab, this, deckScript);
            playerHand.pile = pile;
            playerHand.deck = deckScript;
        }
        

        /* Sets up the background color controller */
        backgroundObject = Instantiate(backgroundObject);
        backgroundController = backgroundObject.GetComponent<BackgroundController>();
        backgroundController.setCamera(cam);

        backgroundController.setBackgroundColor(firstCard.getCardClass());

        pile.setBackgroundController(backgroundController);

        RandomNames.clearUsedNames();

        winScreen.SetActive(false);

        soundManager = soundController.GetComponent<SoundManager>();

        turnIndicator = Instantiate(turnIndicator);
        turnIndicatorImage = turnIndicator.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        turnIndicatorText = turnIndicator.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        hidePlayerTurnIndicator();

        if(ClientInfo.isMultiplayer) {

            playerID = int.Parse(PhotonNetwork.LocalPlayer.NickName);
            int enemyIndex = -1;
            for(int i = 0; i < ClientInfo.numOfPlayers; i++) {
                if(i != playerID) {
                    enemyIndex++;
                    enemies.Add(new Enemy(enemyPositions[enemyIndex], enemyPrefab, pile, deckScript, this, false));
                    enemies[enemyIndex].setName(ClientInfo.playerNames[i]);
                    enemies[enemyIndex].setID(i);
                }
            }

        } else {

            for(int i = 0; i < numOfOpponents; i++) {
                enemies.Add(new Enemy(enemyPositions[i], enemyPrefab, pile, deckScript, this, true));
            }

        }
        
        if(ClientInfo.isHost) {
            StartCoroutine(hostInitializeMultiplayerGame());
            return;
        }

        if(ClientInfo.isMultiplayer) 
            StartCoroutine(initializeMultiplayerGame());
        else
            StartCoroutine(initializeGame());
        
    }

    private int convertIDToIndex(int id) {

        for(int i = 0; i < enemies.Count; i++) {
            Debug.Log(i);
            if(enemies[i].getID() == id)
                return i;
        }

        if(playerID == id) {
            return ClientInfo.playerNames.Count - 1;
        }
        
        return -1;

    }

    private int convertIndexToID(int index) {
        if(index == playerIndex)
            return playerID;

        if(enemies[index] != null)
            return enemies[index].getID();

        return -1;
    }

    private void hidePlayerTurnIndicator() {
        turnIndicatorImage.enabled = false;
        turnIndicatorText.enabled = false;
    }

    private void showPlayerTurnIndicator() {
        turnIndicatorImage.enabled = true;
        turnIndicatorText.enabled = true;
    }

    private IEnumerator hostInitializeMultiplayerGame() {

        playerIndex = ClientInfo.playerNames.Count - 1;

        yield return new WaitForSeconds(0.1f);
        int id = ran.Next(0, ClientInfo.numOfPlayers);
        base.photonView.RPC("RPC_SetInitialMoveID", RpcTarget.Others, new object[] {id});
        turnIndex = convertIDToIndex(id);
        for(int i = 0; i < 500; i++) {
            Card card = deckScript.drawCard();
            multiplayerDeck.Enqueue(card);
            base.photonView.RPC("addCardToDrawQueue", RpcTarget.Others, new object[] {card.getCardClass(), card.getCardType(), card.getCardID()});
        }
        
        yield return new WaitForSeconds(2.9f);

        int index;
        for(int i = 0; i < ClientInfo.playerNames.Count; i++) {
            index = convertIDToIndex(i);
            if(index == playerIndex) {
                for(int j = 0; j < INITIAL_CARD_COUNT; j++) {
                    playerHand.addCard(multiplayerDeck.Dequeue(), cardPrefab);
                    yield return new WaitForSeconds(0.1f);
                }
            } else {
                for(int j = 0; j < INITIAL_CARD_COUNT; j++) {
                    Card card = multiplayerDeck.Dequeue();
                    enemies[index].addCard(card);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        
        StartCoroutine(hostGameLoop());

    }

    private IEnumerator initializeMultiplayerGame() {

        yield return new WaitForSeconds(3.0f);
    
        playerIndex = ClientInfo.playerNames.Count - 1;

        int index;
        for(int i = 0; i < ClientInfo.playerNames.Count; i++) {
            index = convertIDToIndex(i);
            if(index == playerIndex) {
                for(int j = 0; j < INITIAL_CARD_COUNT; j++) {
                    playerHand.addCard(multiplayerDeck.Dequeue(), cardPrefab);
                    yield return new WaitForSeconds(0.1f);
                }
            } else {
                for(int j = 0; j < INITIAL_CARD_COUNT; j++) {
                    Card card = multiplayerDeck.Dequeue();
                    enemies[index].addCard(card);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(multiplayerGameLoop());

    }

    private IEnumerator hostGameLoop() {
        CardControl playedCard;
        while(gameEnabled) {
            playedCard = null;
            turnComplete = false;
            if(turnIndex == playerIndex) {

                showPlayerTurnIndicator();

                while(!pile.canMove(playerHand)) {
                    playerHand.addCard(multiplayerDeck.Dequeue(), cardPrefab);
                    yield return new WaitForSeconds(0.8f);
                }

                playerHand.setCanMove(true);

                yield return new WaitUntil(() => turnComplete == true);
                playerHand.setCanMove(false);
                soundManager.playRandomCardSound();

                playedCard = pile.getTopCard(); 

                int winVal = checkWin();
                if(winVal != -1) {
                    base.photonView.RPC("RPC_ReceivePlayCard", RpcTarget.Others, new object[] {playerID, playedCard.getCardID(), pile.getCurrentClass()});
                    displayWin(winVal);
                    yield return new WaitForSeconds(1.0f);
                    break;
                }

                if(playedCard.getCardClass() == CardTypes.WILD_CARD)
                    soundManager.playSound(SoundID.COLOR_CHANGE);
                else if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    soundManager.playSound(SoundID.PLUS_FOUR);
                else if(playedCard.getCardType() == CardTypes.PLUS_TWO_CARD)
                    soundManager.playSound(SoundID.PLUS_TWO);

                
                if(playedCard.getCardClass() == CardTypes.WILD_CARD || playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
                    colorSelected = false;
                    for(int i = 0; i < buttons.Count; i++)
                        buttons[i].activate();
                    yield return new WaitUntil(() => colorSelected == true);
                    backgroundController.setBackgroundColor(selectedColor);
                    pile.setCurrentClass(selectedColor);
                }

                base.photonView.RPC("RPC_ReceivePlayCard", RpcTarget.Others, new object[] {playerID, playedCard.getCardID(), pile.getCurrentClass()});

                hidePlayerTurnIndicator();

            } else {

                Enemy enemy = enemies[turnIndex];

                enemy.setRedBackground();
                enemy.getHand().setCanMove(true);

                while(!pile.canMove(enemy.getHand())) {
                    enemy.addCard(multiplayerDeck.Dequeue());
                    yield return new WaitForSeconds(0.8f);
                }

                moveReceived = false;
                yield return new WaitUntil(() => moveReceived == true);
                if(enemy.getID() == moveID)
                    enemy.playCard(moveCardID);
                else
                    Debug.Log("ERROR: ID INCONSISTENT.");

                playedCard = pile.getTopCard();

                int winVal = checkWin();
                if(winVal != -1) {

                    displayWin(winVal);
                    for(int i = 0; i < ClientInfo.players.Count; i++) {
                    if(moveID != i)
                        base.photonView.RPC("RPC_ReceivePlayCard", ClientInfo.players[i], new object[] {moveID, moveCardID, colorChange});
                    }   
                    yield return new WaitForSeconds(1.0f);
                    break;
                }

                soundManager.playRandomCardSound();

                if(playedCard.getCardClass() == CardTypes.WILD_CARD)
                    soundManager.playSound(SoundID.COLOR_CHANGE);
                else if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    soundManager.playSound(SoundID.PLUS_FOUR);
                else if(playedCard.getCardType() == CardTypes.PLUS_TWO_CARD)
                    soundManager.playSound(SoundID.PLUS_TWO);

                if(playedCard.getCardClass() == CardTypes.WILD_CARD || playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
                    backgroundController.setBackgroundColor(colorChange);
                    pile.setCurrentClass(colorChange);
                }

                enemy.getHand().setCanMove(false);
                enemy.setWhiteBackground();

                for(int i = 0; i < ClientInfo.players.Count; i++) {
                    if(moveID != i)
                        base.photonView.RPC("RPC_ReceivePlayCard", ClientInfo.players[i], new object[] {moveID, moveCardID, colorChange});
                }

            }

            turnIndex += turnDirection;
            if(turnIndex > playerIndex)
                turnIndex = turnIndex - playerIndex - 1;
            if(turnIndex < 0)
                turnIndex = playerIndex - Mathf.Abs(turnIndex) + 1;

            playedCard = pile.getTopCard();
            if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD || playedCard.getCardType() == CardTypes.PLUS_TWO_CARD) {

                yield return new WaitForSeconds(0.5f);

                int numOfCards = 0;

                if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    numOfCards = 4;
                else
                    numOfCards = 2;

                for(int i = 0; i < numOfCards; i++) {
                    if(turnIndex == playerIndex)
                        playerHand.addCard(multiplayerDeck.Dequeue(), cardPrefab);
                    else
                        enemies[turnIndex].addCard(multiplayerDeck.Dequeue());
                    yield return new WaitForSeconds(0.4f);
                }

            }

        }
    }

    private IEnumerator multiplayerGameLoop() {
        CardControl playedCard;
        while(gameEnabled) {

            playedCard = null;
            turnComplete = false;
            moveReceived = false;

            Debug.Log("TURN: " + turnIndex);

            if(turnIndex == playerIndex) {

                showPlayerTurnIndicator();

                while(!pile.canMove(playerHand)) {
                    playerHand.addCard(multiplayerDeck.Dequeue(), cardPrefab);
                    yield return new WaitForSeconds(0.8f);
                }

                playerHand.setCanMove(true);

                yield return new WaitUntil(() => turnComplete == true);
                playerHand.setCanMove(false);
                soundManager.playRandomCardSound();

                playedCard = pile.getTopCard(); 

                int winVal = checkWin();
                if(winVal != -1) {
                    displayWin(winVal);
                    base.photonView.RPC("RPC_ReceivePlayCard", RpcTarget.MasterClient, new object[] {playerID, playedCard.getCardID(), pile.getCurrentClass()});
                    yield return new WaitForSeconds(1.0f);
                    break;
                }

                if(playedCard.getCardClass() == CardTypes.WILD_CARD)
                    soundManager.playSound(SoundID.COLOR_CHANGE);
                else if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    soundManager.playSound(SoundID.PLUS_FOUR);
                else if(playedCard.getCardType() == CardTypes.PLUS_TWO_CARD)
                    soundManager.playSound(SoundID.PLUS_TWO);

                
                if(playedCard.getCardClass() == CardTypes.WILD_CARD || playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
                    colorSelected = false;
                    for(int i = 0; i < buttons.Count; i++)
                        buttons[i].activate();
                    yield return new WaitUntil(() => colorSelected == true);
                    backgroundController.setBackgroundColor(selectedColor);
                    pile.setCurrentClass(selectedColor);
                }

                base.photonView.RPC("RPC_ReceivePlayCard", RpcTarget.MasterClient, new object[] {playerID, playedCard.getCardID(), pile.getCurrentClass()});

                hidePlayerTurnIndicator();

            } else {

                Enemy enemy = enemies[turnIndex];

                enemy.setRedBackground();
                enemy.getHand().setCanMove(true);

                while(!pile.canMove(enemy.getHand())) {
                    enemy.addCard(multiplayerDeck.Dequeue());
                    yield return new WaitForSeconds(0.8f);
                }

                moveReceived = false;
                yield return new WaitUntil(() => moveReceived == true);
                if(enemy.getID() == moveID)
                    enemy.playCard(moveCardID);
                else
                    Debug.Log("ERROR: ID INCONSISTENT.");


                int winVal = checkWin();
                if(winVal != -1) {

                    displayWin(winVal);
                    break;
                }

                soundManager.playRandomCardSound();
                playedCard = pile.getTopCard();

                if(playedCard.getCardClass() == CardTypes.WILD_CARD)
                    soundManager.playSound(SoundID.COLOR_CHANGE);
                else if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    soundManager.playSound(SoundID.PLUS_FOUR);
                else if(playedCard.getCardType() == CardTypes.PLUS_TWO_CARD)
                    soundManager.playSound(SoundID.PLUS_TWO);

                if(playedCard.getCardClass() == CardTypes.WILD_CARD || playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
                    backgroundController.setBackgroundColor(colorChange);
                    pile.setCurrentClass(colorChange);
                }

                enemy.getHand().setCanMove(false);
                enemy.setWhiteBackground();

            }

            turnIndex += turnDirection;
            if(turnIndex > playerIndex)
                turnIndex = turnIndex - playerIndex - 1;
            if(turnIndex < 0)
                turnIndex = playerIndex - Mathf.Abs(turnIndex) + 1;

            playedCard = pile.getTopCard();
            if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD || playedCard.getCardType() == CardTypes.PLUS_TWO_CARD) {

                yield return new WaitForSeconds(0.5f);

                int numOfCards = 0;

                if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    numOfCards = 4;
                else
                    numOfCards = 2;

                for(int i = 0; i < numOfCards; i++) {
                    if(turnIndex == playerIndex)
                        playerHand.addCard(multiplayerDeck.Dequeue(), cardPrefab);
                    else
                        enemies[turnIndex].addCard(multiplayerDeck.Dequeue());
                    yield return new WaitForSeconds(0.4f);
                }

            }

        }
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
            playerHand.addCard(deckScript.drawCard(), cardPrefab);
            yield return new WaitForSeconds(0.1f);
        }

        playerIndex = enemies.Count;
        turnIndex = ran.Next(0, playerIndex + 1);

        StartCoroutine(gameLoop());

    }

    private IEnumerator gameLoop() {

        CardControl playedCard;

        while(gameEnabled) {

            playedCard = null;
            turnComplete = false;

            yield return new WaitForSeconds(1.0f);

            if(turnIndex == playerIndex) {
                showPlayerTurnIndicator();
                playerHand.setCanMove(true);

                while(!pile.canMove(playerHand)) {
                    playerHand.addCard(deckScript.drawCard(), cardPrefab);
                    yield return new WaitForSeconds(0.8f);
                }

                yield return new WaitUntil(() => turnComplete == true);
                playerHand.setCanMove(false);
                soundManager.playRandomCardSound();

                int winVal = checkWin();
                if(winVal != -1) {
                    displayWin(winVal);
                    break;
                }

                playedCard = pile.getTopCard(); 
                if(playedCard.getCardClass() == CardTypes.WILD_CARD)
                    soundManager.playSound(SoundID.COLOR_CHANGE);
                else if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    soundManager.playSound(SoundID.PLUS_FOUR);
                else if(playedCard.getCardType() == CardTypes.PLUS_TWO_CARD)
                    soundManager.playSound(SoundID.PLUS_TWO);

                
                if(playedCard.getCardClass() == CardTypes.WILD_CARD || playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
                    colorSelected = false;
                    for(int i = 0; i < buttons.Count; i++)
                        buttons[i].activate();
                    yield return new WaitUntil(() => colorSelected == true);
                    backgroundController.setBackgroundColor(selectedColor);
                    pile.setCurrentClass(selectedColor);
                }

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

                int winVal = checkWin();
                if(winVal != -1) {
                    displayWin(winVal);
                    break;
                }

                soundManager.playRandomCardSound();
                playedCard = pile.getTopCard();

                if(playedCard.getCardClass() == CardTypes.WILD_CARD)
                    soundManager.playSound(SoundID.COLOR_CHANGE);
                else if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    soundManager.playSound(SoundID.PLUS_FOUR);
                else if(playedCard.getCardType() == CardTypes.PLUS_TWO_CARD)
                    soundManager.playSound(SoundID.PLUS_TWO);

                if(playedCard.getCardClass() == CardTypes.WILD_CARD || playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD) {
                    yield return new WaitForSeconds(0.75f);
                    int randomColor = ran.Next(0, 3);
                    backgroundController.setBackgroundColor(randomColor);
                    pile.setCurrentClass(randomColor);
                }

                enemy.getHand().setCanMove(false);
                enemy.setWhiteBackground();

            }

            turnIndex += turnDirection;
            if(turnIndex > playerIndex)
                turnIndex = turnIndex - playerIndex - 1;
            if(turnIndex < 0)
                turnIndex = playerIndex - Mathf.Abs(turnIndex) + 1;

            playedCard = pile.getTopCard();
            if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD || playedCard.getCardType() == CardTypes.PLUS_TWO_CARD) {

                yield return new WaitForSeconds(0.5f);

                int numOfCards = 0;

                if(playedCard.getCardClass() == CardTypes.PLUS_FOUR_CARD)
                    numOfCards = 4;
                else
                    numOfCards = 2;

                for(int i = 0; i < numOfCards; i++) {
                    if(turnIndex == playerIndex)
                        playerHand.addCard(deckScript.drawCard(), cardPrefab);
                    else
                        enemies[turnIndex].addCard(deckScript.drawCard());
                    yield return new WaitForSeconds(0.4f);
                }

            }

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
                cardControl.setIsHovering(true);
            }
        } else
            if(currentCard != null)
                currentCard.setIsHovering(false);

    }

    private int checkWin() {
        if(playerHand.getHandSize() == 0)
            return playerIndex;

        for(int i = 0; i < enemies.Count; i++) {
            if(enemies[i].getHand().getHandSize() == 0)
                return i;
        }

        return -1;
    }

    private void displayWin(int val) {
        winScreen.SetActive(true);
        TMP_Text winText = winScreen.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        
        if(val == playerIndex)
            winText.text = "YOU WIN!!!!";
        else
            winText.text = enemies[val].getName().ToUpper() + " WINS!";

        soundManager.playSound(SoundID.WIN_SOUND);
    }

    /*
        Method: reversePlayed()
        Description: Called whenever a reverse is played, reverses the direction of the spinner.
    */

    public void reversePlayed() {
        soundManager.playSound(SoundID.REVERSE_SOUND);
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
        soundManager.playSound(SoundID.SKIP_SOUND);
        directionController.doSpeedup();
        turnIndex += turnDirection;
    }

    public GameObject getCardPrefab() {
        return cardPrefab;
    }

    public void addButton(ColorPicker button) {
        buttons.Add(button);
        button.deactivate();
    }

    public void selectColor(int color) {
        colorSelected = true;
        selectedColor = color;
        for(int i = 0; i < buttons.Count; i++)
            buttons[i].deactivate();
    }

    [PunRPC]
    public void RPC_SetInitialMoveID(int id) {
        turnIndex = convertIDToIndex(id);
    }

    [PunRPC]
    public void RPC_ReceivePlayCard(int id, int cardID, int card_class) {
        moveReceived = true;
        moveID = id;
        moveCardID = cardID;
        colorChange = card_class;
    }

    [PunRPC]
    public void RPC_SendPlayCard(int id, int cardID, int card_class) {
        moveReceived = true;
        moveID = id;
        moveCardID = cardID;
        colorChange = card_class;
    }

    [PunRPC]
    public void addCardToDrawQueue(int card_class, int card_type, int cardID) {
        Card card = new Card(cardID);
        card.setCardClass(card_class);
        card.setCardType(card_type);
        multiplayerDeck.Enqueue(card);
    }

}
