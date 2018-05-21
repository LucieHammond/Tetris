using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour {

	public string sceneName = "";
	public bool disabled;

	private void Start()
	{
		if (disabled)
		{
			GetComponent<Button>().interactable = false;
		}
	}

	public void OnClick()
	{
		if (sceneName != "") 
		    SceneManager.LoadScene(sceneName);
	}
}
