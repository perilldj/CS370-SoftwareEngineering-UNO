using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDirectionController : MonoBehaviour
{

    private const float BASE_SPIN_SPEED = 50.0f;
    private float spinSpeed = BASE_SPIN_SPEED;
    private bool isSpinningClockwise = false;
    private bool speedup = false;
    private float decelleration = 500.0f;
    private const float ACCELLERATED_SPIN_SPEED = 600.0f;

    private float transitionSpeed = 2.0f;
    private float currentWidth = -1.5f;

    public void spinClockwise() {
        if(!isSpinningClockwise) {
            isSpinningClockwise = true;
        }
    }

    public void spinCounterClockwise() {
        if(isSpinningClockwise) {
            isSpinningClockwise = false;
        }
    }

    public bool getIsSpinningClockwise() {
        return isSpinningClockwise;
    }

    public void doSpeedup() {
        speedup = true;
        spinSpeed = ACCELLERATED_SPIN_SPEED;
    }

    // Start is called before the first frame update
    void Start() { 
        
    }

    // Update is called once per frame
    void Update() {

        if(speedup) {
            spinSpeed -= decelleration * Time.deltaTime;
            if(spinSpeed < BASE_SPIN_SPEED) {
                spinSpeed = BASE_SPIN_SPEED;
                speedup = false;
            }
        }

        if(isSpinningClockwise)
            transform.Rotate(0.0f, 0.0f, spinSpeed * Time.deltaTime);
        else
            transform.Rotate(0.0f, 0.0f, -spinSpeed * Time.deltaTime);

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
