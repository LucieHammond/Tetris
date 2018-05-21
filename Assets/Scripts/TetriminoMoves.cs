﻿using System;
using System.Collections;
using UnityEngine;

public class TetriminoMoves : MonoBehaviour
{
	public bool isActive { get; set; }
    public Vector3 rotationPoint;
	public float timePerRaw = 1;
   
	private bool rightMove;
	private bool leftMove;
	private bool downMove;
	private bool usedSoftDrop = false;
    
	private float timeSinceLastFall = 0f;
    private float timeLeftPressed = 0f;
    private float timeRightPressed = 0f;
	private float timeDownPressed = 0f;
    
	private TetriminoCollisions collisionManager;
    
    private void Start()
    {
		collisionManager = GetComponent<TetriminoCollisions>();
    }
    
    private void Update()
    {
		if (isActive)
		{
			bool regularFall = false;
			if (timeSinceLastFall > timePerRaw)
			{
				MoveDown();
				regularFall = true;
				timeSinceLastFall -= timePerRaw;
			}
			timeSinceLastFall += Time.deltaTime;
				
			MoveWithInputs(regularFall);
		}

    }

    private void MoveWithInputs(bool regularFall)
    {
		rightMove = false; leftMove = false; downMove = false;

		SmoothReaction(KeyCode.LeftArrow, ref timeLeftPressed, MoveLeft);
        
		SmoothReaction(KeyCode.RightArrow, ref timeRightPressed, MoveRight);
        
		if (!regularFall)
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
				timePassed -= (code == KeyCode.DownArrow)? 0.05f : 0.09f;
				if (code == KeyCode.DownArrow)
					usedSoftDrop = true;
				action();
            }
        }
        else if (Input.GetKeyUp(code))
            timePassed = 0f;
	}

    private void MoveRight()
	{
		if (collisionManager.CheckRight()) {
			transform.Translate(1, 0, 0, Space.World);
			rightMove = true;
		}
	}
    
	private void MoveLeft()
    {
		if (collisionManager.CheckLeft()) {
			transform.Translate(-1, 0, 0, Space.World);
			leftMove = true;
		}
    }
    
    private void MoveDown()
	{
		int xMove = (rightMove ? 1 : 0) - (leftMove ? 1 : 0);
		if (collisionManager.CheckBottom(xMove))
		{
			transform.Translate(0, -1, 0, Space.World);
			downMove = true;
		}
		else {
			isActive = false;
            collisionManager.Freeze(usedSoftDrop);
		}
	}

    private void Rotate()
	{
		int xMove = (rightMove ? 1 : 0) - (leftMove ? 1 : 0);
		int yMove = (downMove ? -1 : 0);
		if (collisionManager.CheckRotation(xMove, yMove))
		{
			transform.RotateAround(transform.position + transform.rotation * rotationPoint, Vector3.back, 90f);
			for (int i = 0; i < 4; i++){
				transform.GetChild(i).Rotate(0, 0, 90);
			}
		}
	}
}
