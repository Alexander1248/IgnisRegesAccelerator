using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider sensSlider;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    private float minSens = 0.005f;
    private float maxSens = 0.8f;

    void Start(){
        UpdateSlider();
    }

    public void UpdateSlider(){
        sensSlider.value = InverseLerp(PlayerPrefs.GetFloat("Sens", 0.2f), minSens, maxSens);
        volumeSlider.value = PlayerPrefs.GetFloat("PlayerVolume", 0.8f);
        audioMixer.SetFloat("Volume", Mathf.Log10(volumeSlider.value)*20);
    }

    public void SetSens(){
        float val = Mathf.Lerp(minSens, maxSens, sensSlider.value);
        PlayerPrefs.SetFloat("Sens", val);
    }
    public void changeVolume(){
        PlayerPrefs.SetFloat("PlayerVolume", volumeSlider.value);

        audioMixer.SetFloat("Volume", Mathf.Log10(volumeSlider.value)*20);
    }

    public void setLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }

    public static float InverseLerp(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}
