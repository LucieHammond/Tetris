using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LastGameMemory{
    
	public static bool lastNotFinished = false;
	public static bool restoreGame = false;

	public static int score;
	public static int level;
	public static int[] linesDetails;
    
	public static int[][] playGrid;
	public static Queue<GameObject> generatedQueue;
    public static int currentPiece;
    public static List<int> nextPieces;
    public static int holdPiece = 0;

	public static void SaveLevels(int pScore, int pLevel, int[] pLinesDetails)
	{
		score = pScore;
		level = pLevel;
		linesDetails = pLinesDetails;
	}

    public static void SavePlayfield(GameObject[][] pPlayGrid)
	{
		playGrid = new int[pPlayGrid.Length][];
		for (int i = 0; i < playGrid.Length; i++)
		{
			playGrid[i] = new int[pPlayGrid[i].Length];
			for (int j = 0; j < playGrid[i].Length; j++)
			{
				if (pPlayGrid[i][j])
				    playGrid[i][j] = pPlayGrid[i][j].GetComponent<SquareBehaviour>().GetID();
			}
		}
	}

	public static void SavePieces(Queue<GameObject> pGeneratedQueue, GameObject pCurrentPiece, 
	                              List<GameObject> pNextPieces, GameObject pHoldPiece)
	{
		generatedQueue = pGeneratedQueue;
		currentPiece = pCurrentPiece.transform.GetChild(0).GetComponent<SquareBehaviour>().GetID();
        if (pHoldPiece)
		    holdPiece = pHoldPiece.transform.GetChild(0).GetComponent<SquareBehaviour>().GetID();
		nextPieces = new List<int>();
		for (int i = 0; i < pNextPieces.Count; i++)
		{
			nextPieces.Add(pNextPieces[i].transform.GetChild(0).GetComponent<SquareBehaviour>().GetID());
		}
	}

	public static void ResetMemory()
    {      
        playGrid = null;
        score = 0;
		level = 0;
		linesDetails = null;
		generatedQueue = null;
		currentPiece = 0;
		nextPieces = null;
		holdPiece = 0;
                      
        lastNotFinished = false;
		restoreGame = false;
    }
}
