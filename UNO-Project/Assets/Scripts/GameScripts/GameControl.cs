using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

    [SerializeField]
    private GameObject directionIndicatorObject;
    private SpinDirectionController directionController;

    [SerializeField]
    private GameObject backgroundController; //Camera????

    [SerializeField]
    private GameObject soundController; //TODO: Figure out sounds

    [SerializeField]
    private GameObject deck;
    private Vector2 deckLoc = new Vector2(1.0f, 0.0f); // I hate hard coding positions like this, but it will do for now

    private Pile pile;
    private Vector2 pileLoc = new Vector2(-1.0f, 0.0f);

    private Hand playerHand;

    private int opponentCount;
    private Hand[] enemyHands;

    // Start is called before the first frame update
    void Start() {
        Instantiate(deck);
        Instantiate(directionIndicatorObject);
        directionController = directionIndicatorObject.GetComponent<SpinDirectionController>();
        Debug.Log(directionController);
        directionController.spinCounterClockwise();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
