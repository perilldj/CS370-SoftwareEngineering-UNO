using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDirectionController : MonoBehaviour
{

    private const float SPIN_SPEED = 50.0f;
    private bool isSpinningClockwise = false;

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

    // Start is called before the first frame update
    void Start() { 
        
    }

    // Update is called once per frame
    void Update() {

        if(isSpinningClockwise)
            transform.Rotate(0.0f, 0.0f, SPIN_SPEED * Time.deltaTime);
        else
            transform.Rotate(0.0f, 0.0f, -SPIN_SPEED * Time.deltaTime);

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
