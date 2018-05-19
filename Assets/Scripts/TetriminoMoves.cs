using System;
using System.Collections;
using UnityEngine;

public class TetriminoMoves : MonoBehaviour
{

    public Vector3 rotationPoint;

    private bool isActive = false;
	private bool hasMoved = false;
    private bool leftMoveActive = true;
    private bool rightMoveActive = true;
    private float timeLeftPressed = 0f;
    private float timeRightPressed = 0f;
	private float timeDownPressed = 0f;

	private TetriminoCollisions collisionManager;

    // Use this for initialization
    private void Start()
    {
		collisionManager = GetComponent<TetriminoCollisions>();
    }

    public void Activate()
	{
		isActive = true;
        StartCoroutine(Fall());
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
			SmoothReaction(KeyCode.LeftArrow, ref timeLeftPressed, MoveLeft);
        }

        if (rightMoveActive)
        {
			SmoothReaction(KeyCode.RightArrow, ref timeRightPressed, MoveRight);
        }

		SmoothReaction(KeyCode.DownArrow, ref timeDownPressed, MoveDown);
        
		if (Input.GetKeyDown(KeyCode.UpArrow))
			Rotate();
    }
    
    private void SmoothReaction(KeyCode code, ref float timePassed, Action action)
	{
		if (Input.GetKeyDown(code))
            action();
        else if (Input.GetKey(code))
        {
            timePassed += Time.deltaTime;
            if (timePassed > 0.5)
            {
                action();
                timePassed -= 0.1f;
            }
        }
        else if (Input.GetKeyUp(code))
            timePassed = 0f;
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
