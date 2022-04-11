using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinDirectionController : MonoBehaviour
{

    private const float SPIN_SPEED = 50.0f;
    private bool isSpinningClockwise = false;

    public void spinClockwise() {
        if(!isSpinningClockwise) {
            isSpinningClockwise = true;
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
    }

    public void spinCounterClockwise() {
        if(isSpinningClockwise) {
            isSpinningClockwise = false;
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    // Start is called before the first frame update
    void Start() { 
        //spinClockwise();
    }

    // Update is called once per frame
    void Update() {
        if(isSpinningClockwise)
            transform.Rotate(0.0f, 0.0f, SPIN_SPEED * Time.deltaTime);
        else
            transform.Rotate(0.0f, 0.0f, -SPIN_SPEED * Time.deltaTime);
    }
}
