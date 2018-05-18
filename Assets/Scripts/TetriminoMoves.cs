using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoMoves : MonoBehaviour
{

    public Vector3 rotationPoint;

    private bool isActive = true;
	private bool hasMoved = false;
    private bool leftMoveActive = true;
    private bool rightMoveActive = true;
    private float timeLeftPressed = 0f;
    private float timeRightPressed = 0f;

	private TetriminoCollisions collisionManager;

    // Use this for initialization
    private void Start()
    {
		collisionManager = GetComponent<TetriminoCollisions>();
        if (isActive)
        {
            StartCoroutine(Fall());
        }
    }

    // Update is called once per frame
    private void Update()
    {
		if (isActive)
		{
			if (hasMoved)
                CheckCollisions();
            MoveWithInputs();
		}

    }

    private void MoveWithInputs()
    {
        if (leftMoveActive)
        {
			if (Input.GetKeyDown(KeyCode.LeftArrow))
				MoveLeft();
            else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                timeLeftPressed += Time.deltaTime;
                if (timeLeftPressed > 0.5)
                {
					MoveLeft();
                    timeLeftPressed -= 0.1f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
                timeLeftPressed = 0f;
        }

        if (rightMoveActive)
        {
			if (Input.GetKeyDown(KeyCode.RightArrow))
				MoveRight();
            else if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                timeRightPressed += Time.deltaTime;
                if (timeRightPressed > 0.5)
                {
					MoveRight();
                    timeRightPressed -= 0.1f;
                }
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
                timeRightPressed = 0f;
        }

		if (Input.GetKeyDown(KeyCode.UpArrow))
			Rotate();

		if (Input.GetKeyDown(KeyCode.DownArrow))
			MoveDown();
    }

    private IEnumerator Fall()
    {
        WaitForSeconds timeWaiter = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return timeWaiter;
			MoveDown();
        }
    }

    private void MoveRight()
	{
		transform.Translate(1, 0, 0, Space.World);
		hasMoved = true;
	}
    
	private void MoveLeft()
    {
		transform.Translate(-1, 0, 0, Space.World);
		hasMoved = true;
    }

    private void MoveDown()
	{
		transform.Translate(0, -1, 0, Space.World);
		hasMoved = true;
	}

    private void Rotate()
	{
		if (collisionManager.CheckRotation())
		    transform.RotateAround(transform.position + transform.rotation * rotationPoint, Vector3.back, 90f);
	}

    private void CheckCollisions()
	{
		rightMoveActive = collisionManager.CheckRight();
		leftMoveActive = collisionManager.CheckLeft();
		if (!collisionManager.CheckBottom())
        {
            StopAllCoroutines();
			isActive = false;
			collisionManager.Freeze();
        }
		hasMoved = false;
	}
}
