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

    private Queue<Card> deck = new Queue<Card>();   //Queue for deck to be stored
    private Hand hand = new Hand();                 //Reference to player's hand (A better way for multiple hands needs to be made)

    /*
        Method: Start()
        Description: Initializes Deck by calling initializeDeckFunction
    */

    void Start() {
        initializeDeck();
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

        List<Card> orderedDeck = new List<Card>();
        List<Card> shufledDeck;

        for(int i = 0; i < 6; i++) {        //This doesn't create the cards in correct proportions, way to many wild and +4s
            for(int j = 6; j < 18; j++) {   //I need to rewrite this so it creates all the cards in the correct proportions -perilldj
                Card newCard = new Card(); 
                newCard.setCardClass(i);
                newCard.setCardType(j);
                newCard.deck = this;
                orderedDeck.Add(newCard);
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

}
