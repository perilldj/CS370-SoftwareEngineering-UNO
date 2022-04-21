using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
    Class: Deck : MonoBehaviour
    Description: This script is attachable to a sprite that acts as a deck when clicked.
                 the script will manage everything to do with the deck including shuffling,
                 creating cards, and managing card textures. It also handles drawing cards
                 from this deck.

    Methods:    void Start();
                public Texture2D getTexture(int index);
                public void initializeDeck();
                public Card drawCard();
                public void setDeckPos(Vector2 pos);
        

    Author: perilldj
*/

public class Deck : MonoBehaviour {
   
    [SerializeField] /* List to load in all sprites */
    private List<Texture2D> cardSprites;

    [SerializeField]
    private GameObject cardPrefab;

    private Queue<Card> deck = new Queue<Card>();   //Queue for deck to be stored
    public Hand hand;                               //Reference to player's hand (A better way for multiple hands needs to be made)
    public Pile pile;

    private int idCount = 0; //Counter to assign unique IDs to the cards

    void Start() { }

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

    public void initializeDeck() {

        List<Card> orderedDeck = new List<Card>();
        List<Card> shufledDeck;

        /* Adds every type of card in the correct proportions */
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
        hand.addCard(drawCard());
    }

    /*
        Method: drawCard()
        Description: Draws and returns a card from the deck.
    */

    public Card drawCard() {
        return deck.Dequeue();
    }

    /*
        Method: addCard(List<Card> deck, int cardClass, int cardType)
        Description: Creates a card with specified class and type, and adds it to a list of cards.
    */

    private void addCard(List<Card> deck, int cardClass, int cardType) {
        Card newCard = new Card(cardPrefab, hand, idCount);
        idCount++;
        newCard.setCardClass(cardClass);
        newCard.setCardType(cardType);
        newCard.deck = this;
        deck.Add(newCard);
    }

    /*
        Method: setDeckPos(Vector2 pos)
        Description: Sets on screen position of the deck.
    */

    public void setDeckPos(Vector2 pos) {
        transform.position = pos;
    }

}
