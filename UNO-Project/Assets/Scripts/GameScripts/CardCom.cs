using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardCom : MonoBehaviour {

    private Hand hand;
    private Card card;

    private void OnMouseDown() {
        hand.playCard(card);
    }

}
