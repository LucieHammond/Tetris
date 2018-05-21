using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingBehaviour : MonoBehaviour {

	public float waitingTime;
	public RectTransform loadingBar;
    
	void Start () {
		StartCoroutine(WaitAndLoadScene());
		loadingBar.sizeDelta = new Vector2(0, loadingBar.sizeDelta.y);
	}

    IEnumerator WaitAndLoadScene()
	{
		float delta = waitingTime / 100;
		for (int i = 1; i <= 100; i++){
			loadingBar.sizeDelta = new Vector2(i * 3, loadingBar.sizeDelta.y);
			yield return new WaitForSeconds(delta);
		}
		SceneManager.LoadScene("GameScene");
	}

}
