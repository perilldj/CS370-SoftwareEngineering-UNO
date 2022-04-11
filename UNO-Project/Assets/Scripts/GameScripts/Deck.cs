using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
    Class: Deck : MonoBehaviour
    Description: This script is attachable to a sprite that acts as a deck when clicked.
                 the script will manage everything to do with the deck including shuffling,
                 creating cards, and managing card textures (may be changed)

    Methods:
        void Start()
        public Texture2D getTexture(int index)

    Author: perilldj
*/

public class Deck : MonoBehaviour {
   
    [SerializeField] /* List to load in all sprites */
    private List<Texture2D> cardSprites;

    [SerializeField]
    private GameObject cardPrefab;

    private Queue<Card> deck = new Queue<Card>();   //Queue for deck to be stored
    private Hand hand = new Hand();                 //Reference to player's hand (A better way for multiple hands needs to be made)
    public Pile pile;

    private int idCount = 0;

    /*
        Method: Start()
        Description: Initializes Deck by calling initializeDeckFunction
    */

    void Start() {
        initializeDeck();
        hand.deck = this;
    }

    /*
        Method: getTexture(int index)
        Description: Returns a Texture2D based on IDs from the CardTypes class.
    */

    public Texture2D getTexture(int index) {
        return cardSprites[index];
    }

    /*
        Method: initializeDeck()
        Description: Creates and shuffles a deck into the deck Queue.
    */

    private void initializeDeck() {

        pile = new Pile(cardPrefab, new Vector2(1.0f, 0.0f), this);

        List<Card> orderedDeck = new List<Card>();
        List<Card> shufledDeck;

        for(int i = 0; i < 4; i++) {

            addCard(orderedDeck, CardTypes.WILD_CARD, 18);
            addCard(orderedDeck, CardTypes.PLUS_FOUR_CARD, 18);

            for(int j = 6; j < 18; j++) {
                addCard(orderedDeck, i, j);
                addCard(orderedDeck, i, j);
            }

        }

        /* Shufles the deck */
        var rand = new System.Random();
        shufledDeck = orderedDeck.OrderBy(x => rand.Next()).ToList();

        /* For every card in the deck, push it into the queue */
        foreach(Card card in shufledDeck) {
            deck.Enqueue(card);
        }

    }

    /*
        Method: OnMouseDown()
        Description: Called when the sprite this script is added to is clicked. It adds a card
                     to the players hand while simultaneously removing it from the deck queue.
    */

    private void OnMouseDown() {
        hand.addCard(deck.Dequeue());
    }

    private void addCard(List<Card> deck, int cardClass, int cardType) {
        Card newCard = new Card(cardPrefab, null, idCount);
        idCount++;
        newCard.setCardClass(cardClass);
        newCard.setCardType(cardType);
        newCard.deck = this;
        deck.Add(newCard);
    }

}
