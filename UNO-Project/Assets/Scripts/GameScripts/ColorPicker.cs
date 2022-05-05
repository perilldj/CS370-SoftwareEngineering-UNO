using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: ColorPicker
    Author: perilldj and kourb
    Description: Script for chosing a color when a +4 or wild card is played.
*/

public class ColorPicker : MonoBehaviour {

    public GameObject gameController;
    public int color;

    private GameControl gameControl;

    public GameObject gameScene;

    void Start() {
        gameControl = gameController.GetComponent<GameControl>();
        gameControl.addButton(this);
    }

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
