using System;
using System.Collections;
using UnityEngine;

public class TetriminoMoves : MonoBehaviour
{
	public bool isActive { get; set; }
    public Vector3 rotationPoint;
	public float speed // In nb of square fall per second
	{
		get { return 1 / timeInterval; }
		set { timeInterval = 1 / value; }
	}
   
	private bool hasMoved = false;
    private bool leftMoveActive = true;
    private bool rightMoveActive = true;
    
	private float timeInterval = 0.4f;
	private float timeSinceLastFall = 0f;
    private float timeLeftPressed = 0f;
    private float timeRightPressed = 0f;
	private float timeDownPressed = 0f;
    
	private TetriminoCollisions collisionManager;

    // Use this for initialization
    private void Start()
    {
		collisionManager = GetComponent<TetriminoCollisions>();
    }

    // Update is called once per frame
    private void Update()
    {
		if (isActive)
		{
			bool regularFall = false;
			if (timeSinceLastFall > timeInterval)
			{
				MoveDown();
				regularFall = true;
				timeSinceLastFall -= timeInterval;
			}
			timeSinceLastFall += Time.deltaTime;
				
			if (hasMoved)
                CheckCollisions();
			MoveWithInputs(regularFall);
		}

    }

    private void MoveWithInputs(bool regularFall)
    {
        if (leftMoveActive)
			SmoothReaction(KeyCode.LeftArrow, ref timeLeftPressed, MoveLeft);

        if (rightMoveActive)
			SmoothReaction(KeyCode.RightArrow, ref timeRightPressed, MoveRight);
        
		if (!regularFall || collisionManager.CheckBottom())
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
			isActive = false;
			collisionManager.Freeze();
        }
		hasMoved = false;
	}


}
