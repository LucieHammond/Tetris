using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Application;

public class GameManager : MonoBehaviour
{
	public GameObject[] tetriminos;
	public Vector3 spawnPoint = new Vector3(3, 22, 0);
	public GameObject[][] playGrid { get; set; }
	public Transform[] nextPoints;
	public FlashingBehaviour[] shiningLines;

	private System.Random random = new System.Random();
	private Queue<GameObject> generatedQueue = new Queue<GameObject>();
	private List<GameObject> nextPieces = new List<GameObject>();

	// Use this for initialization
	private void Start () {
    
		playGrid = new GameObject[22][];
        for (int i = 0; i < playGrid.Length; i++)
        {
			playGrid[i] = new GameObject[10];
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
    
	public IEnumerator OnLanding()
    {
		List<int> lines = CheckLines();
		if (lines.Count > 0)
		{
			foreach (int line in lines)
			    shiningLines[line].Flash();
			yield return new WaitForSeconds(0.9f);
			RemoveLines(lines);
		}
			
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

	private void RemoveLines(List<int> lines)
	{
		int offset = 0;
		for (int i = 0; i < playGrid.Length; i++)
		{
			if (lines.Contains(i))
			{
				for (int j = 0; j < playGrid[i].Length; j++)
				{
					Destroy(playGrid[i][j]);
				}    
				offset++;
			}
			else
			{
				for (int j = 0; j < playGrid[i].Length; j++)
                {
                    if (offset > 0 && playGrid[i][j])
                        playGrid[i][j].transform.Translate(0, -offset, 0, Space.World);
					playGrid[i - offset][j] = playGrid[i][j];
                }
			}
		}
	}
}
