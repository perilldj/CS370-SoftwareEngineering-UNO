using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public CardControl card;
    public GameObject colorPicker; 
    public GameObject gameController;
    public GameControl game;
    public Pile pile;

    public GameObject gameScene;
    // Start is called before the first frame update
    void Start()
    {
        colorPicker.SetActive(false);
    }

    // Update is called once per frame

    void Update() {
        
        if(pile.attemptMove(card)) {
            if (card.getCardClass() == CardTypes.WILD_CARD) {
                gameScene = GameObject.Find("GameController");
                gameScene.SetActive(false);
                colorPicker.SetActive(true);
                
            } 
        }
    }

}
