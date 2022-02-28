using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour {
   
    private Queue<Card> deck;

    void Start() {
        initializeDeck();
    }

    void initializeDeck() {

        List<Card> orderedDeck = new List<Card>();
        List<Card> shufledDeck = new List<Card>();

        for(int i = 0; i < 6; i++) {
            for(int j = 1; j < 13; j++) {
                Card newCard = new Card();
                newCard.setCardClass(i);
                newCard.setCardType(i);
                orderedDeck.Add(newCard);
            }
        }

        var rand = new System.Random();
        shufledDeck = orderedDeck.OrderBy(x => rand.Next()).ToList();

        foreach(Card card in shufledDeck) {
            deck.Enqueue(card);
            Debug.Log("Class: " + card.getCardClass() + " Type: " + card.getCardType());
        }

    }

    void OnMouseDown() {
        Debug.Log("Sprite Clicked");
    }

}
