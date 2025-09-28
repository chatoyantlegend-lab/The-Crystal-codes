using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    private const string PREF_VOLUME = "masterVolume";

    private void Awake()
    {
       // DontDestroyOnLoad(gameObject); // This keeps music between scenes
       float vol = PlayerPrefs.GetFloat(PREF_VOLUME, 1f);
       AudioListener.volume = vol;
       if (musicSource != null && !musicSource.isPlaying) musicSource.Play();
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = Mathf.Clamp01(value);
        PlayerPrefs.SetFloat(PREF_VOLUME, AudioListener.volume);
    }
}
