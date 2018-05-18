using System.Collections.Generic;
using UnityEngine;
using Application;

public class GameManager : MonoBehaviour
{

	public GameObject[] tetriminos;
	public Vector3 spawnPoint = new Vector3(3, 22, 0);
	private Queue<GameObject> nextPieces = new Queue<GameObject>();
	private System.Random random = new System.Random();

	// Use this for initialization
	private void Start () {
		spawnPoint = transform.position + spawnPoint;
		SpawnTetrimino();
	}
    
	// Update is called once per frame
	private void Update () {
		
	}

    private void SpawnTetrimino()
	{
		if (nextPieces.Count <= 3)
		{
			RandomGenerator();
		}
		GameObject tetrimino = nextPieces.Dequeue();
        Instantiate(tetrimino, spawnPoint, Quaternion.identity);
	}
    
	private void RandomGenerator()
	{
		GameObject[] nextBag = random.Shuffle(tetriminos);
		for (int i = 0; i < nextBag.Length; i++)
			nextPieces.Enqueue(nextBag[i]);
	}
}
