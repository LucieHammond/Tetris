using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Application;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
	public GameObject[] tetriminos;
	public Transform spawnPoint;
	public Transform[] nextPoints;
	public Transform holdPoint;
	public GameObject FrontMessageBackground;
	public Text FrontMessageText;

	private System.Random random = new System.Random();
	private Queue<GameObject> generatedQueue = new Queue<GameObject>();
	private GameObject currentPiece;
	private List<GameObject> nextPieces = new List<GameObject>();
	private GameObject holdPiece;
    
	private PlayfieldState playfieldState;

	// Use this for initialization
	private void Start () {

		playfieldState = GetComponent<PlayfieldState>();

		RandomGenerator();
		for (int i = 0; i < 3; i++)
		{
			GameObject tetrimino = generatedQueue.Dequeue();
			nextPieces.Add(Instantiate(tetrimino, nextPoints[i].position, Quaternion.identity));
			nextPieces[i].GetComponent<TetriminoCollisions>().playfield = playfieldState;
		}

		StartCoroutine(StartCountDown());
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			SwitchWithHold();
	}

	private void RandomGenerator()
    {
        GameObject[] nextBag = random.Shuffle(tetriminos);
        for (int i = 0; i < nextBag.Length; i++)
            generatedQueue.Enqueue(nextBag[i]);
    }

    private IEnumerator StartCountDown()
	{
		FrontMessageBackground.SetActive(true);
		FrontMessageBackground.transform.localScale = Vector3.one;
		FrontMessageText.enabled = true;
		FrontMessageText.fontSize = 100;
		for (int number = 3; number > 0; number--) {
			FrontMessageText.text = number.ToString();
			yield return new WaitForSeconds(1);
		}
        
		FrontMessageBackground.SetActive(false);
		FrontMessageText.enabled = false;
		SpawnTetrimino();
	}

    public void SpawnTetrimino()
	{
		if (generatedQueue.Count <= 0)
			RandomGenerator();

		currentPiece = nextPieces[0];
		nextPieces.RemoveAt(0);
		for (int i = 0; i < 2; i++)
		{
			nextPieces[i].transform.position = nextPoints[i].position;
		}
		GameObject tetrimino = generatedQueue.Dequeue();
		nextPieces.Add(Instantiate(tetrimino, nextPoints[2].position, Quaternion.identity));
		nextPieces[2].GetComponent<TetriminoCollisions>().playfield = playfieldState;

		currentPiece.transform.position = spawnPoint.position;
		currentPiece.GetComponent<TetriminoMoves>().isActive = true;
	}
    
    private void SwitchWithHold()
	{
		// Store temporarily the currentPiece
        GameObject tempPiece = currentPiece;

		// Take the hold piece or the next one as currentPiece
        if (holdPiece)
        {
            currentPiece = holdPiece;
            currentPiece.transform.position = tempPiece.transform.position;
            currentPiece.GetComponent<TetriminoMoves>().isActive = true;
        }
        else
        {
            SpawnTetrimino();
			currentPiece.transform.position = tempPiece.transform.position;
        }

		// Place old current piece in the Hold box
		holdPiece = tempPiece;
		holdPiece.transform.position = holdPoint.position;
        holdPiece.transform.rotation = Quaternion.identity;
        holdPiece.GetComponent<TetriminoMoves>().isActive = false;
	}
}
