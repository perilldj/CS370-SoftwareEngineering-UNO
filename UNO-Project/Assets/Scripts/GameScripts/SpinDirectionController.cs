using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class: SpinDirectionController : MonoBehaviour
    Description: Controlls the spin controller on screen in the game world.

    Methods: spinClockwise()
             spinCounterClockwise()
             getIsSpinningClockwise()
             doSpeedup()

    Author: perilldj
*/

public class SpinDirectionController : MonoBehaviour {

    private const float BASE_SPIN_SPEED = 50.0f;
    private float spinSpeed = BASE_SPIN_SPEED;
    private bool isSpinningClockwise = false;
    private bool speedup = false;
    private float decelleration = 500.0f;
    private const float ACCELLERATED_SPIN_SPEED = 600.0f;

    private float transitionSpeed = 8.0f;
    private float currentWidth = -1.5f;

    /*
        Method: spinClockwise()
        Description: Sets the spinner direction to clockwise.
    */

    public void spinClockwise() {
        if(!isSpinningClockwise) {
            isSpinningClockwise = true;
        }
    }

     /*
        Method: spinCounterClockwise()
        Description: Sets the spinner direction to counterclockwise.
    */

    public void spinCounterClockwise() {
        if(isSpinningClockwise) {
            isSpinningClockwise = false;
        }
    }

    /*
        Method: getIsSpinningClockwise()
        Description: Returns the direction of the spinner.
                     (True if clockwise, false if counterclockwise)
    */

    public bool getIsSpinningClockwise() {
        return isSpinningClockwise;
    }

    /*
        Method: doSpeedup()
        Description: Called when a skip is played, gives spinner a speed boost.
    */

    public void doSpeedup() {
        speedup = true;
        spinSpeed = ACCELLERATED_SPIN_SPEED;
    }

    // Start is called before the first frame update
    void Start() {  }

    // Update is called once per frame
    void Update() {

        /* If a speedup is ongoing (skip was played), reduce its spin speed by
           decelleration * Time.deltaTime */
        if(speedup) {
            spinSpeed -= decelleration * Time.deltaTime;
            if(spinSpeed < BASE_SPIN_SPEED) {
                spinSpeed = BASE_SPIN_SPEED;
                speedup = false;
            }
        }

        /* Do spin based upon direction */
        if(isSpinningClockwise)
            transform.Rotate(0.0f, 0.0f, spinSpeed * Time.deltaTime);
        else
            transform.Rotate(0.0f, 0.0f, -spinSpeed * Time.deltaTime);

        /* Changes width when the spinner changes directions */
        if(isSpinningClockwise) {
            currentWidth -= transitionSpeed * Time.deltaTime;
            if(currentWidth < -1.5f)
                currentWidth = -1.5f;
            transform.localScale = new Vector3(currentWidth, 1.5f, 1.5f);
        } else {
            currentWidth += transitionSpeed * Time.deltaTime;
            if(currentWidth > 1.5f)
                currentWidth = 1.5f;
            transform.localScale = new Vector3(currentWidth, 1.5f, 1.5f);
        }

    }
}
