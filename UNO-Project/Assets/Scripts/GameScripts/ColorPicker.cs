using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public Card card;
    public GameObject colorPicker; 
    public GameObject gameController;
    // Start is called before the first frame update
    void Start()
    {
        colorPicker.SetActive(false);
    }

    // Update is called once per frame

    void Update() {
        if (card.getCardClass() == CardTypes.WILD_CARD) {
            colorPicker.SetActive(true);
        }
        
    }

}
