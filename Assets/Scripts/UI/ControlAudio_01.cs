//ControlAudio_01.cs

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControlAudio_01 : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Sliders (para cargar valores)")]
    public Slider sliderGeneral;
    public Slider sliderMusica;
    public Slider sliderEfectos;

    void Start()
    {
        // Cargar valores guardados (defecto 1 = volumen máximo)
        CargarVolumenes();
    }

    public void ControlarAudioGeneral(float valor)
    {
        audioMixer.SetFloat("VolumenAudioGeneral", Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("VolGeneral", valor);
    }

    public void ControlarAudioMusica(float valor)
    {
        audioMixer.SetFloat("VolumenAudioMusica", Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("VolMusica", valor);
    }

    public void ControlarAudioEfectos(float valor)
    {
        audioMixer.SetFloat("VolumenAudioEfectos", Mathf.Log10(Mathf.Max(valor, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("VolEfectos", valor);
    }

    void CargarVolumenes()
    {
        float gen = PlayerPrefs.GetFloat("VolGeneral", 1f);
        float mus = PlayerPrefs.GetFloat("VolMusica", 1f);
        float efx = PlayerPrefs.GetFloat("VolEfectos", 1f);

        ControlarAudioGeneral(gen);
        ControlarAudioMusica(mus);
        ControlarAudioEfectos(efx);

        if (sliderGeneral != null) sliderGeneral.value = gen;
        if (sliderMusica != null) sliderMusica.value = mus;
        if (sliderEfectos != null) sliderEfectos.value = efx;
    }
}