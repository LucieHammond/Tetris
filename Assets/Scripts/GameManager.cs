using System.Collections.Generic;
using UnityEngine;
using Application;

public class GameManager : MonoBehaviour
{
	public GameObject[] tetriminos;
	public Vector3 spawnPoint = new Vector3(3, 22, 0);
	public bool[][] playGrid { get; set; }

	private Queue<GameObject> nextPieces = new Queue<GameObject>();
	private System.Random random = new System.Random();

	// Use this for initialization
	private void Start () {

		playGrid = new bool[22][];
        for (int i = 0; i < playGrid.Length; i++)
        {
            playGrid[i] = new bool[10];
        }

		spawnPoint = transform.position + spawnPoint;
		SpawnTetrimino();
	}
    
	// Update is called once per frame
	private void Update () {
		
	}

	private void RandomGenerator()
    {
        GameObject[] nextBag = random.Shuffle(tetriminos);
        for (int i = 0; i < nextBag.Length; i++)
            nextPieces.Enqueue(nextBag[i]);
    }

    private void SpawnTetrimino()
	{
		if (nextPieces.Count <= 3)
		{
			RandomGenerator();
		}
		GameObject tetrimino = nextPieces.Dequeue();
        GameObject newPiece = Instantiate(tetrimino, spawnPoint, Quaternion.identity);
		newPiece.GetComponent<TetriminoCollisions>().gameManager = this;
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
