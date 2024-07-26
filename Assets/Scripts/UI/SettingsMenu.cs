using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider sensSlider;
    private float minSens = 0.005f;
    private float maxSens = 0.8f;

    void Start(){
        UpdateSlider();
    }

    public void UpdateSlider(){
        sensSlider.value = InverseLerp(PlayerPrefs.GetFloat("Sens"), minSens, maxSens);
    }

    public void SetSens(){
        float val = Mathf.Lerp(minSens, maxSens, sensSlider.value);
        PlayerPrefs.SetFloat("Sens", val);
    }

    public static float InverseLerp(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }
}
