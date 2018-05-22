using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

	public Slider musicSlider;
	public Slider soundsSlider;
	public Text musicVolume;
	public Text soundsVolume;
	private AudioSource audioSounds;
    
	private void Start()
	{
		audioSounds = GetComponent<AudioSource>();
        
		float musicVol = Settings.musicVolume * 10 / 0.8f;
		musicSlider.value = musicVol;
		musicVolume.text = musicVol.ToString();
        
		float soundsVol = Settings.soundsVolume * 10;
		soundsSlider.value = soundsVol;
		soundsVolume.text = soundsVol.ToString();
	}

	public void OnChangeMusicVolume()
	{
		musicVolume.text = musicSlider.value.ToString();
		float volume = musicSlider.value * 0.08f;

		Settings.musicVolume = volume;
		MusicPlayer.ChangeMusicVolume(volume);
	}

    public void OnChangeSoundsVolume()
	{
		soundsVolume.text = soundsSlider.value.ToString();
		float volume = soundsSlider.value * 0.1f;

		Settings.soundsVolume = volume;
		audioSounds.volume = volume;
		audioSounds.Play();
	}
}
