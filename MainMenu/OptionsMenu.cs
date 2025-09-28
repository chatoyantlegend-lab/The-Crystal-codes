using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour 
{
    public Slider volumeSlider;
    public MenuAudioManager audioManager;

    private void OnEnable()
    {
        InputBindings.LoadDefaults();
        float vol = PlayerPrefs.GetFloat("masterVolume", 1f);
        if (volumeSlider != null )
        {
            volumeSlider.value = vol;
            volumeSlider.onValueChanged.RemoveAllListeners();
            volumeSlider.onValueChanged.AddListener((V) => audioManager.SetVolume(V));
            
        }
    }
}
