using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoringBehaviour: MonoBehaviour {

	public Gameplay gameplay;
	public PlayfieldState playfield;
	public LevelDesign levels;

	public GameObject[] tetriminos;
	public GameObject[] squares;

	private float offsetX = -4.5f;
	private float offsetY = -9.5f; 

	private void Awake()
	{
		if (LastGameMemory.restoreGame) {
            
			RestoreGameplay();
			RestorePlayfield();
			levels.Restore();
		}

		LastGameMemory.ResetMemory();
	}

    private void RestoreGameplay()
	{
		// Current Piece
		GameObject currentPiece = Instantiate(tetriminos[LastGameMemory.currentPiece - 1],
		                                      gameplay.spawnPoint.position, 
		                                      Quaternion.identity);

		// Hold Piece
		GameObject holdPiece = null;
        if (LastGameMemory.holdPiece != 0)
			holdPiece = Instantiate(tetriminos[LastGameMemory.holdPiece - 1],
			                                   gameplay.holdPoint.position,
			                                   Quaternion.identity);

		// Next Pieces
		List<GameObject> nextPieces = new List<GameObject>();
		for (int i = 0; i < LastGameMemory.nextPieces.Count; i++)
		{
			GameObject newPiece = Instantiate(tetriminos[LastGameMemory.nextPieces[i] - 1],
											  gameplay.nextPoints[i].position,
											  Quaternion.identity);
			nextPieces.Add(newPiece);
		}

		gameplay.Restore(LastGameMemory.generatedQueue, currentPiece, nextPieces, holdPiece);
	}

	private void RestorePlayfield()
	{
		GameObject[][] playGrid = new GameObject[22][];
		for (int i = 0; i < playGrid.Length; i++)
		{
			playGrid[i] = new GameObject[10];
			for (int j = 0; j < playGrid[i].Length; j++)
			{
				if (LastGameMemory.playGrid[i][j] != 0)
					playGrid[i][j] = Instantiate(squares[LastGameMemory.playGrid[i][j] - 1],
					                            new Vector3(j + offsetX, i + offsetY, 0),
					                            Quaternion.identity);
			}
        }

		playfield.Restore(playGrid);
	}
}
