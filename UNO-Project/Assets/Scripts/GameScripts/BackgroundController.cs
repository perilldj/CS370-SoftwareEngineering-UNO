using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: BackgroundController : MonoBehavior
    Description: Used to controll the changing of the background color base on the previous card that was played.
                 If the card color changes to yellow, the background changes to yellow, if it changes to blue, the
                 background changes to blue.

    Methods:   Start();
               Update();
               public void setBackgroundColor(int col);
               private void setLerpColor(Color col1, Color col2);
               public void setCamera(Camera cam);


    Author: perilldj
*/

public class BackgroundController : MonoBehaviour {
    
    //The background color is stored in the Camera object, we need it to control the color
    private Camera cam;

    /* Colors for each of the UNO color types */

    [SerializeField]
    private Color BLUE_BACKGROUND_COLOR; 

    [SerializeField]
    private Color RED_BACKGROUND_COLOR;

    [SerializeField]
    private Color GREEN_BACKGROUND_COLOR;

    [SerializeField]
    private Color YELLOW_BACKGROUND_COLOR;

    /* Variables required to perform a linear interpolation (lerp) between two colors */
    private bool doLerp = false;
    private float alpha = 0.0f;
    private float transitionSpeed = 2.0f;
    private Color beginColor;
    private Color currentColor;
    private Color goalColor;

    void Start() {
        currentColor = BLUE_BACKGROUND_COLOR;
    }

    void Update() {

        if(doLerp) {    //If a lerp is in progress

            cam.backgroundColor = currentColor;         //Set the background color to the current color
            alpha += transitionSpeed * Time.deltaTime;  //Update the progress of the lerp

            /* If the lerp has completed, stop the loop */
            if(alpha > 1.0f) {
                alpha = 1.0f;
                doLerp = false;
            }

            /* Calculate the next color with Lerp function */
            currentColor = Color.Lerp(beginColor, goalColor, alpha);

        }

    }

    /*
        Method: setBackgroundColor(int col)
        Description: Initiates the background color change.
    */

    public void setBackgroundColor(int col) {
        //Based on the integer provided (The card class value), the currect color will be set to transition to. 
        switch(col) {
            case CardTypes.BLUE_CARD : setLerpColor(currentColor, BLUE_BACKGROUND_COLOR);
                break;
            case CardTypes.RED_CARD : setLerpColor(currentColor, RED_BACKGROUND_COLOR);
                break;
            case CardTypes.GREEN_CARD : setLerpColor(currentColor, GREEN_BACKGROUND_COLOR);
                break;
            case CardTypes.YELLOW_CARD : setLerpColor(currentColor, YELLOW_BACKGROUND_COLOR);
                break;
            default:
                break;
        }
    }

    /*
        Method: setLerpColor(Color col1, Color col2)
        Description: Initiates the Lerp between the current color and the target color.
    */

    private void setLerpColor(Color col1, Color col2) {
        beginColor = col1;
        goalColor = col2;
        alpha = 0.0f;
        currentColor = beginColor;
        doLerp = true;
    }

    /*
        Method: setCamera(Camera cam)
        Description: Sets the reference to the camera provided.
    */

    public void setCamera(Camera cam) {
        this.cam = cam;
    }

}
