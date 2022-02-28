using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand {
    
    private List<Card> cards = new List<Card>();
    private Vector2 p1 = new Vector2(-4.0f, -2.0f);
    private Vector2 p2 = new Vector2(4.0f, -2.0f);
    int size = 0;

    public void addCard(Card card) {
        cards.Add(card);
        card.createOnScreenCard();
        ajustCardPositions();
    }

    /* Lots of magic numbers, I'll improve this soon - DP */
    private void ajustCardPositions() {
        size++;
        float spacing = (1.0f / (float)(size + 1)) * 8.0f;
        Debug.Log(spacing);
        float currentX = -4.0f;
        int count = 0;
        foreach(Card card in cards) {
            currentX += spacing;
            card.setCardPos(new Vector2(currentX, -2.0f));
            card.setLayer(count);
            count++;
        }
    }

}
