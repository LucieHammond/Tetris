using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetriminoCollisions : MonoBehaviour {

	public Transform[] squares;
    
	private bool[][] playGrid;
	private Vector3 playfieldStart = new Vector3(-5, -10, 0);

	// Use this for initialization
	private void Start () {
		playGrid = new bool[22][];
		for (int i = 0; i< playGrid.Length; i++){
			playGrid[i] = new bool[10];
		}
	}

    public bool CheckRight()
	{
		Vector3 coords;
		foreach (Transform square in squares) {
			coords = square.position - playfieldStart;
			if (coords.x >= 9)
				return false;
			if (playGrid[(int) coords.y][(int)coords.x + 1])
				return false;
		}
		return true;
	}
    
	public bool CheckLeft()
    {
        Vector3 coords;
		foreach (Transform space in squares)
        {
            coords = space.position - playfieldStart;
            if (coords.x <= 1)
                return false;
			if (playGrid[(int) coords.y][(int)coords.x - 1])
                return false;
        }
        return true;
    }
    
	public bool CheckBottom()
    {
        Vector3 coords;
		foreach (Transform space in squares)
        {
            coords = space.position - playfieldStart;
			if (coords.y <= 1)
                return false;
			if (playGrid[(int)coords.y - 1][(int)coords.x])
                return false;
        }
        return true;
    }

}
