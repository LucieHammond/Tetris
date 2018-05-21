using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoCollisions : MonoBehaviour {

	enum RotationState
	{
		Normal,
        FirstQuarter,
        Reverse,
        ThirdQuarter,
	}

	public Transform[] squares;
	public int rotationSpaceSize;
	public PlayfieldState playfield { private get; set; }

	public GameObject[][] playGrid { private get; set; }
	private Vector3 playfieldStart = new Vector3(-5, -10, 0);
	private RotationState rotationState = RotationState.Normal;

	// Use this for initialization
	private void Start () {
		playGrid = playfield.playGrid;
	}

	public bool CheckRight(int afterMoveY = 0)
	{
		Vector3 coords;
		foreach (Transform square in squares) {
			coords = square.position - playfieldStart;
			if (coords.x >= 9)
				return false;
			if (playGrid[(int) coords.y + afterMoveY][(int)coords.x + 1])
				return false;
		}
		return true;
	}
    
	public bool CheckLeft(int afterMoveY = 0)
    {
        Vector3 coords;
		foreach (Transform space in squares)
        {
            coords = space.position - playfieldStart;
            if (coords.x <= 1)
                return false;
			if (playGrid[(int) coords.y + afterMoveY][(int)coords.x - 1])
                return false;
        }
        return true;
    }
    
	public bool CheckBottom(int afterMoveX = 0)
    {
        Vector3 coords;
		foreach (Transform space in squares)
        {
            coords = space.position - playfieldStart;
			if (coords.y <= 1)
                return false;
			if (playGrid[(int)coords.y - 1][(int)coords.x + afterMoveX])
                return false;
        }
        return true;
    }
    
	public bool CheckRotation(int afterMoveX = 0, int afterMoveY = 0)
	{
		Vector3 start = transform.position - playfieldStart + new Vector3(afterMoveX, afterMoveY);
		int startX = (int)(start.x + 0.5f);
        int startY = (int)(start.y - 0.5f);

		switch (rotationState)
        {
            case RotationState.FirstQuarter:
				startX -= rotationSpaceSize;
                break;
            case RotationState.Reverse:
				startX -= rotationSpaceSize;
				startY += rotationSpaceSize;
                break;
            case RotationState.ThirdQuarter:
				startY += rotationSpaceSize;
                break;
		}

		if (startX < 0 || startX + rotationSpaceSize > 10 || startY - rotationSpaceSize < -1) return false;
        
		for (int x = startX; x < startX + rotationSpaceSize; x++)
		{
			for (int y = startY; y < startY - rotationSpaceSize; y++)
			{
				if (playGrid[y][x]) {
					return false;
				}
			}
		}

		int number = ((int) rotationState + 1) % 4;
		rotationState = (RotationState) number;
		return true;
	}

    public void Freeze(bool usedSoftDrop)
	{
		Vector3 coords;
		foreach (Transform square in squares)
		{
			coords = square.position - playfieldStart;
			playGrid[(int)coords.y][(int)coords.x] = square.gameObject;
		}
		StartCoroutine(playfield.OnLanding(usedSoftDrop));

	}
}
