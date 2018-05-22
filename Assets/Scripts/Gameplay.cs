using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Application;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameplay : MonoBehaviour
{
	public GameObject[] tetriminos;
	public Transform spawnPoint;
	public Transform[] nextPoints;
	public Transform holdPoint;
	public GameObject frontMessageBackground;
	public Text frontMessageText;
	public AudioSource countDownSound;
	public AudioSource gameOverSound;
	public GameObject replayWindow;

	private System.Random random = new System.Random();
	private Queue<GameObject> generatedQueue = new Queue<GameObject>();
	private GameObject currentPiece;
	private List<GameObject> nextPieces = new List<GameObject>();
	private GameObject holdPiece;
	private bool gameOver = false;
	private bool isRestored = false;
    
	private PlayfieldState playfieldState;
	private LevelDesign levelDesign;
	private AudioSource musicSound;

	// Use this for initialization
	private void Start () {

		playfieldState = GetComponent<PlayfieldState>();
		levelDesign = GetComponent<LevelDesign>();
		musicSound = GetComponent<AudioSource>();
		musicSound.volume = Settings.musicVolume;
		gameOverSound.volume = Settings.soundsVolume;
		countDownSound.volume = Settings.soundsVolume;
		MusicPlayer.DestroyInstance();

        if (!isRestored)
		{
			RandomGenerator();
            for (int i = 0; i < 3; i++)
            {
                GameObject tetrimino = generatedQueue.Dequeue();
                nextPieces.Add(Instantiate(tetrimino, nextPoints[i].position, Quaternion.identity));
            }
		}

		StartCoroutine(StartCountDown());
	}

	private void Update()
	{
		if (gameOver)
		{
			if (Input.anyKeyDown) {
				replayWindow.SetActive(true);
			}
			return;
		}
		if (Input.GetKeyDown(KeyCode.Space))
			SwitchWithHold();

		if (Input.GetButtonDown("Alt"))
		{
			RecordGame();
			SceneManager.LoadScene("MenuScene");
		}
	}

	private void RandomGenerator()
    {
        GameObject[] nextBag = random.Shuffle(tetriminos);
        for (int i = 0; i < nextBag.Length; i++)
            generatedQueue.Enqueue(nextBag[i]);
    }

    private IEnumerator StartCountDown()
	{
		frontMessageBackground.SetActive(true);
		frontMessageBackground.transform.localScale = Vector3.one;
		frontMessageText.enabled = true;
		frontMessageText.fontSize = 100;
		for (int number = 3; number > 0; number--) {
			frontMessageText.text = number.ToString();
			countDownSound.Play();
			yield return new WaitForSeconds(1);
		}
        
		frontMessageBackground.SetActive(false);
		frontMessageText.enabled = false;
		musicSound.Play();

		if (!isRestored)
		    SpawnTetrimino();
		else
			currentPiece.GetComponent<TetriminoMoves>().isActive = true;
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
        
		currentPiece.transform.position = spawnPoint.position;
 		currentPiece.GetComponent<TetriminoMoves>().isActive = true;
		currentPiece.GetComponent<TetriminoMoves>().timePerRaw = levelDesign.GetTimePerRaw();
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
			currentPiece.GetComponent<TetriminoMoves>().timePerRaw = levelDesign.GetTimePerRaw();
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
		for (int i = 0; i < 4; i++)
        {
            holdPiece.transform.GetChild(i).rotation = Quaternion.identity;
        }
        holdPiece.GetComponent<TetriminoMoves>().isActive = false;
	}

    public void LaunchGameOver()
	{
		frontMessageBackground.SetActive(true);
        frontMessageBackground.transform.localScale = new Vector3(2, 1, 1);
        frontMessageText.enabled = true;
        frontMessageText.fontSize = 40;
		frontMessageText.text = "GAME OVER";
		gameOver = true;
		gameOverSound.Play();
		levelDesign.SendStatistics();
	}

	public void Replay()
    {
		SceneManager.LoadScene("GameScene");
    }

    public void GoToMenu()
    {
		SceneManager.LoadScene("MenuScene");
    }

	public void Restore(Queue<GameObject> pGeneratedQueue, GameObject pCurrentPiece,
	                    List<GameObject> pNextPieces, GameObject pHoldPiece)
	{
		generatedQueue = pGeneratedQueue;
		currentPiece = pCurrentPiece;
		nextPieces = pNextPieces;
		holdPiece = pHoldPiece;

		isRestored = true;
	}

    private void RecordGame()
	{
		LastGameMemory.lastNotFinished = true;
		LastGameMemory.SavePieces(generatedQueue, currentPiece, nextPieces, holdPiece);
		LastGameMemory.SavePlayfield(playfieldState.playGrid);
		levelDesign.SaveInMemory();
	}
}
