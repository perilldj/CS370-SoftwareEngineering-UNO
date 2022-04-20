using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour {
    
    private Camera cam;

    [SerializeField]
    private Color BLUE_BACKGROUND_COLOR; 

    [SerializeField]
    private Color RED_BACKGROUND_COLOR;

    [SerializeField]
    private Color GREEN_BACKGROUND_COLOR;

    [SerializeField]
    private Color YELLOW_BACKGROUND_COLOR;

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

        if(doLerp) {

            cam.backgroundColor = currentColor;
            alpha += transitionSpeed * Time.deltaTime;

            if(alpha > 1.0f) {
                alpha = 1.0f;
                doLerp = false;
            }

            currentColor = Color.Lerp(beginColor, goalColor, alpha);

        }

    }

    public void setBackgroundColor(int col) {
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

    private void setLerpColor(Color col1, Color col2) {
        beginColor = col1;
        goalColor = col2;
        alpha = 0.0f;
        currentColor = beginColor;
        doLerp = true;
    }

    public void setCamera(Camera cam) {
        this.cam = cam;
    }

}
