using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour
{

    public GameObject gameController;
    public int color;

    private GameControl gameControl;

    public GameObject gameScene;

    // Start is called before the first frame update
    void Start() {
        gameControl = gameController.GetComponent<GameControl>();
        gameControl.addButton(this);
    }

    // Update is called once per frame

    void Update() { }

    public void ChooseBlue() {
        Debug.Log("Blue");
        gameControl.selectColor(color);
    }

    public void ChooseRed() {
        Debug.Log("Red");
        gameControl.selectColor(color);
    }

    public void ChooseGreen() {
        Debug.Log("Green");
        gameControl.selectColor(color);
    }

    public void ChooseYellow() {
        Debug.Log("Yellow");
        gameControl.selectColor(color);
    }

    public void deactivate() {
        gameObject.SetActive(false);
    }

    public void activate() {
        gameObject.SetActive(true);
    }

}
