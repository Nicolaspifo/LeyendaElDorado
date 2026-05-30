using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.InputSystem;

public class CinematicaFinal : MonoBehaviour
{
    [Header("Imágenes")]
    
    public Image[] imagenes;           // Arrastra cada imagen en orden

    [Header("Input")]
    public InputActionReference avanzarCinematica; // ← asigna tu acción en el Inspector

    [Header("Tiempo mínimo por imagen")]
    public float tiempoMinimoPorImagen = 2f;

private float tiempoMostradoImagen = 0f;

    [Header("Audio")]
    public string zonaMusicaFinal = "Final_01";

    [Header("Texto")]
    public TextMeshProUGUI textoProgreso;
    public float tiempoEsperaTexto = 5f;

    [Header("Fade")]
    public Image panelFadeNegro;       // Image negra que cubre toda la pantalla
    public float velocidadFade = 1.5f;

    [Header("Escena Final")]
    public string escenaAlTerminar = "MenuPrincipal";
    
    private int imagenActual = 0;
    private bool procesando = false;

    void OnEnable()
    {
        if (SimpleAudioSystem.Instance != null)
        SimpleAudioSystem.Instance.EnterMusicZone(zonaMusicaFinal);
        
        imagenActual = 0;
        procesando = false;

        // Ocultar todas las imágenes
        foreach (var img in imagenes)
            if (img != null) SetAlpha(img, 0f);

        // Panel negro completamente transparente al inicio
        if (panelFadeNegro != null)
        {
            SetAlpha(panelFadeNegro, 0f);
            panelFadeNegro.gameObject.SetActive(true);
        }

        ActualizarTexto();
        StartCoroutine(MostrarPrimera());
    }

    void Update()
    {
        if (procesando) return;

        bool presiono = avanzarCinematica != null && avanzarCinematica.action.WasPressedThisFrame();

        if (presiono && Time.time >= tiempoMostradoImagen + tiempoMinimoPorImagen)
            StartCoroutine(Avanzar());
    }

    IEnumerator MostrarPrimera()
    {
        procesando = true;
        yield return StartCoroutine(FadeInImagen(imagenes[0]));
        tiempoMostradoImagen = Time.time; // ← registrar

        if (textoProgreso != null) SetAlpha(textoProgreso, 0f);

        yield return new WaitForSeconds(tiempoEsperaTexto);

        if (textoProgreso != null)
        {
            textoProgreso.text = "Presiona cualquier tecla para finalizar la ceremonia...";
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * velocidadFade;
                SetAlpha(textoProgreso, Mathf.Clamp01(t));
                yield return null;
            }
        }

        procesando = false;
    }

    IEnumerator Avanzar()
    {
        procesando = true;
        imagenActual++;

        if (imagenActual >= imagenes.Length)
        {
            yield return StartCoroutine(FadeOutPantalla());
            yield break;
        }
        yield return StartCoroutine(FadeInImagen(imagenes[imagenActual]));
        tiempoMostradoImagen = Time.time;

        // Penúltima imagen
        bool esPenultima = imagenActual == imagenes.Length - 2;
        if (esPenultima && SimpleAudioSystem.Instance != null)
            StartCoroutine(FadeOutAudio(15f));

        // Última imagen → mostrar y pasar al menú automáticamente
        bool esUltima = imagenActual == imagenes.Length - 1;
        if (esUltima)
        {
            ActualizarTexto();
            if (textoProgreso != null) SetAlpha(textoProgreso, 0f);
            yield return new WaitForSeconds(8f); // tiempo en pantalla de agradecimiento
            yield return StartCoroutine(FadeOutPantalla());
            yield break;
        }

        ActualizarTexto();
        procesando = false;
    }

    IEnumerator FadeOutAudio(float duracion)
    {
        if (SimpleAudioSystem.Instance == null) yield break;

        // Buscar la fuente overlay activa manualmente
        AudioSource source = SimpleAudioSystem.Instance.musicSource1.isPlaying
            ? SimpleAudioSystem.Instance.musicSource1
            : SimpleAudioSystem.Instance.musicSource2;

        // También bajar overlays por si acaso
        foreach (AudioSource overlay in SimpleAudioSystem.Instance.overlayAudioSources)
        {
            if (overlay != null && overlay.isPlaying)
            {
                StartCoroutine(FadeOutSource(overlay, duracion));
            }
        }

        // Bajar fuente principal también
        float volumenInicial = source.volume;
        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            source.volume = Mathf.Lerp(volumenInicial, 0f, t / duracion);
            yield return null;
        }
        source.volume = 0f;
        source.Stop();
    }

    IEnumerator FadeOutSource(AudioSource src, float duracion)
    {
        float volumenInicial = src.volume;
        float t = 0f;
        while (t < duracion)
        {
            t += Time.deltaTime;
            src.volume = Mathf.Lerp(volumenInicial, 0f, t / duracion);
            yield return null;
        }
        src.volume = 0f;
        src.Stop();
    }

    void ActualizarTexto()
    {
        if (textoProgreso == null) return;

        // Siempre visible desde la segunda imagen en adelante
        SetAlpha(textoProgreso, 1f);

        if (imagenActual < imagenes.Length-1)
            textoProgreso.text = $"({imagenActual + 1}/{imagenes.Length-1})";
        else
            textoProgreso.text = $"({imagenes.Length}/{imagenes.Length-1})";
    }
    void SetAlpha(TextMeshProUGUI t, float a)
    {
        Color c = t.color;
        c.a = a;
        t.color = c;
    }

    IEnumerator FadeInImagen(Image img)
    {
        if (img == null) yield break;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFade;
            SetAlpha(img, Mathf.Clamp01(t));
            yield return null;
        }
    }

    IEnumerator FadeOutImagen(Image img)
    {
        if (img == null) yield break;
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * velocidadFade;
            SetAlpha(img, Mathf.Clamp01(t));
            yield return null;
        }
    }

    IEnumerator FadeInPantalla()
    {
        if (panelFadeNegro == null) yield break;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFade;
            SetAlpha(panelFadeNegro, Mathf.Clamp01(t));
            yield return null;
        }
    }

    IEnumerator FadeOutPantalla()
    {
        if (panelFadeNegro == null) yield break;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFade;
            SetAlpha(panelFadeNegro, Mathf.Clamp01(t));
            yield return null;
        }

        // Detener TODO el audio antes de cambiar escena
        if (SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.StopAllOverlays();
            SimpleAudioSystem.Instance.StopMusic();
        }

        SceneManager.LoadScene(escenaAlTerminar);
    }

    void SetAlpha(Graphic g, float a)
    {
        Color c = g.color;
        c.a = a;
        g.color = c;
    }
}