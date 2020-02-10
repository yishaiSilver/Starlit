using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour {

    public AudioMixer audioMixer;
    public static float volumeGlobal;
    public static bool volumeChanged;

    public Slider volumeSlider;

    private void Start()
    {
        if (!volumeChanged)
        {
            SettingsMenu.volumeGlobal = 1f;
            volumeChanged = true;
        }

        setVolume(volumeGlobal);
        volumeSlider.value = SettingsMenu.volumeGlobal;
        Debug.Log("GV = " + SettingsMenu.volumeGlobal);
        
    }

    public void setVolume(float volume)
    {
        Debug.Log(volume);
        SettingsMenu.volumeGlobal = volume;
        AudioListener.volume = volume;
    }

    public void setFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;
    }
}
