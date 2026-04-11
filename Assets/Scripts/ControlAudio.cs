using UnityEngine;
using UnityEngine.Audio;

public class ControlAudio : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void ControlarAudioGeneral(float sliderAudio)
    {
        audioMixer.SetFloat("VolumenAudioGeneral", Mathf.Log10(sliderAudio) * 20);
    }
    public void ControlarAudioMusica(float sliderAudio)
    {
        audioMixer.SetFloat("VolumenAudioMusica", Mathf.Log10(sliderAudio) * 20);
    }

    public void ControlarAudioEfectos(float sliderAudio)
    {
        audioMixer.SetFloat("VolumenAudioEfectos", Mathf.Log10(sliderAudio) * 20);
    }

}
