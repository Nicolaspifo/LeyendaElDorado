// BotonInteractivo.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BotonInteractivo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{   
    [Header("Configuración de Texto")]
    public TextMeshProUGUI texto;

    [Header("Colores")]
    public Color colorNormal = Color.white;
    public Color colorHover = Color.yellow;
    public Color colorPresionado = Color.red;

    [Header("Escala")]
    private Vector3 escalaOriginalTexto;
    private Vector3 escalaOriginalImagen;
    public float escalaHover = 1.1f; // 10% más grande

    [Header("Audio")]
    public AudioClip sonidoBoton;
    public bool usarSonidoGlobal = true;

    // ← VARIABLE PARA LA IMAGEN (se buscará automáticamente)
    private Image imagenBoton;

    void Start()
    {
        // Guardar la escala original del texto
        if (texto != null)
            escalaOriginalTexto = texto.rectTransform.localScale;
        
        // BUSCAR LA IMAGEN
        imagenBoton = GetComponent<Image>();
        if (imagenBoton != null)
            escalaOriginalImagen = imagenBoton.rectTransform.localScale;
        else
            escalaOriginalImagen = Vector3.one; // Valor por defecto si no hay imagen
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = colorHover;
            texto.rectTransform.localScale = escalaOriginalTexto * escalaHover;
        }
        
        // ESCALAR LA IMAGEN SI EXISTE
        if (imagenBoton != null)
        {
            imagenBoton.rectTransform.localScale = escalaOriginalImagen * escalaHover;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = colorNormal;
            texto.rectTransform.localScale = escalaOriginalTexto;
        }
        
        // ← RESTAURAR ESCALA DE LA IMAGEN
        if (imagenBoton != null)
        {
            imagenBoton.rectTransform.localScale = escalaOriginalImagen;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = colorPresionado;
        }
        
        ReproducirSonido();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (texto != null)
        {
            texto.color = colorHover;
        }
    }
    
    private void ReproducirSonido()
    {
        if (usarSonidoGlobal && SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.PlayButtonSound();
        }
        else if (sonidoBoton != null && SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.sfxSource.PlayOneShot(sonidoBoton);
        }
    }
}