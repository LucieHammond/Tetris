using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	private static MusicPlayer instance = null;

	private void Awake()
	{
		GetComponent<AudioSource>().volume = Settings.musicVolume;
		if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
	}

	public static void DestroyInstance()
	{
		if (instance != null)
		    Destroy(instance.gameObject);
	}

	public static void ChangeMusicVolume(float value)
    {
		instance.GetComponent<AudioSource>().volume = value;
    }
}
