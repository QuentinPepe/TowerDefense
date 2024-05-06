using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class OptionVolume : MonoBehaviour
{
    [SerializeField] private List<Slider> sliders;
    [SerializeField] private List<string> groupNames;
    [SerializeField] private AudioMixer audioMixer;
    private void Start()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            int index = i;
            sliders[i].onValueChanged.AddListener(value => HandleSliderValueChanged(value, groupNames[index]));
        }

        for (int i = 0; i < sliders.Count; i++)
        {
            sliders[i].value = audioMixer.GetFloat(groupNames[i], out float value) ? Mathf.Pow(10, value / 20) : 1;
        }
    }

    private void HandleSliderValueChanged(float value, string groupName)
    {
        audioMixer.SetFloat(groupName, Mathf.Log10(value) * 20);
    }
}