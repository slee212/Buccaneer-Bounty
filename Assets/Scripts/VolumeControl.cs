using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        // Initialize the master volume slider if it exists
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
        }
    }

    private void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}