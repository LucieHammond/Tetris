using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {

	public string sceneName = "";
	public bool checkLastMemory;

	public void Start()
	{
		if (checkLastMemory)
		{
			GetComponent<Button>().interactable = LastGameMemory.lastNotFinished;         
		}
	}

	public void OnClick()
	{
		if (sceneName != "")
		{
			if (checkLastMemory)
            {
                LastGameMemory.restoreGame = true;
            }
            SceneManager.LoadScene(sceneName);
		}
		    
	}
}
