using System.Collections.Generic;
using UnityEngine;
using Application;

public class GameManager : MonoBehaviour
{
	public GameObject[] tetriminos;
	public Vector3 spawnPoint = new Vector3(3, 22, 0);
	public bool[][] playGrid { get; set; }
	public Transform[] nextPoints;

	private System.Random random = new System.Random();
	private Queue<GameObject> generatedQueue = new Queue<GameObject>();
	private List<GameObject> nextPieces = new List<GameObject>();

	// Use this for initialization
	private void Start () {

		playGrid = new bool[22][];
        for (int i = 0; i < playGrid.Length; i++)
        {
            playGrid[i] = new bool[10];
        }

		spawnPoint = transform.position + spawnPoint;

		RandomGenerator();
		for (int i = 0; i < 3; i++)
		{
			GameObject tetrimino = generatedQueue.Dequeue();
			nextPieces.Add(Instantiate(tetrimino, nextPoints[i].position, Quaternion.identity));
			nextPieces[i].GetComponent<TetriminoCollisions>().gameManager = this;
		}

		SpawnTetrimino();
	}

	private void RandomGenerator()
    {
        GameObject[] nextBag = random.Shuffle(tetriminos);
        for (int i = 0; i < nextBag.Length; i++)
            generatedQueue.Enqueue(nextBag[i]);
    }

    private void SpawnTetrimino()
	{
		if (generatedQueue.Count <= 0)
			RandomGenerator();

		GameObject newPiece = nextPieces[0];
		nextPieces.RemoveAt(0);
		for (int i = 0; i < 2; i++)
		{
			nextPieces[i].transform.position = nextPoints[i].position;
		}
		GameObject tetrimino = generatedQueue.Dequeue();
		nextPieces.Add(Instantiate(tetrimino, nextPoints[2].position, Quaternion.identity));
		nextPieces[2].GetComponent<TetriminoCollisions>().gameManager = this;

		newPiece.transform.position = spawnPoint;
		newPiece.GetComponent<TetriminoMoves>().Activate();
	}

	public void OnLanding()
    {
		CheckLines();
        SpawnTetrimino();
    }
    
	private List<int> CheckLines()
	{
		List<int> completedLines = new List<int>();
		for (int i = 0; i < playGrid.Length; i++)
		{
			bool completed = true;
			for (int j = 0; j < playGrid[i].Length; j++)
			{
				if (!playGrid[i][j])
				{
					completed = false;
					break;
				}
			}
			if (completed)
				completedLines.Add(i);
		}
		return completedLines;
	}
}
