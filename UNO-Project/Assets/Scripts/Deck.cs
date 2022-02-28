using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour {
   
    [SerializeField]
    private List<Texture2D> cardSprites;

    private Queue<Card> deck = new Queue<Card>();

    void Start() {
        initializeDeck();
    }

    void initializeDeck() {

        List<Card> orderedDeck = new List<Card>();
        List<Card> shufledDeck;

        for(int i = 0; i < 6; i++) {
            for(int j = 1; j < 13; j++) {
                Card newCard = new Card();
                newCard.setCardClass(i);
                newCard.setCardType(j);
                newCard.deck = this;
                orderedDeck.Add(newCard);
            }
        }

        var rand = new System.Random();
        shufledDeck = orderedDeck.OrderBy(x => rand.Next()).ToList();

        foreach(Card card in shufledDeck) {
            deck.Enqueue(card);
        }

    }

     public Texture2D getTexture(int index) {
        return cardSprites[index];
    }

    void OnMouseDown() {
        Debug.Log("AAA");
        deck.Dequeue().createOnScreenCard();
    }

}
