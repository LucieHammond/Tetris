using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoBehaviour : MonoBehaviour {

	public Vector3 rotationPoint;

	private bool isActive = true;
	private float timeLeftPressed = 0f;
	private float timeRightPressed = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		MoveWithInputs();
		
	}

    private void MoveWithInputs()
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
            transform.Translate(-1, 0, 0, Space.World);
		else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            timeLeftPressed += Time.deltaTime;
            if (timeLeftPressed > 0.5)
            {
                transform.Translate(-1, 0, 0, Space.World);
                timeLeftPressed -= 0.1f;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            timeLeftPressed = 0f;
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
            transform.Translate(1, 0, 0, Space.World);
		else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            timeRightPressed += Time.deltaTime;
            if (timeRightPressed > 0.5)
            {
                transform.Translate(1, 0, 0, Space.World);
                timeRightPressed -= 0.1f;
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            timeRightPressed = 0f;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            transform.RotateAround(transform.position + transform.rotation * rotationPoint, Vector3.back, 90f);
	}
}
