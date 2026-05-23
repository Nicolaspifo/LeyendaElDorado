using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuPrincipal : MonoBehaviour
{   
    [Header("Panel de Registro de Usuario")]
    public GameObject panelMenuPrincipal;

    [Header("Pantalla de Introducción")]
    public GameObject panelIntroduccion;           // Panel que contiene la imagen de intro
    public Image[] imagenesIntroduccion;           // Array de las 4 imágenes de introducción
    public TextMeshProUGUI textoPresionarTecla;    // Texto "Presiona ESPACIO para continuar"
    public float velocidadFadeIntro = 1.0f;        // Velocidad del fade in de la intro

    [Header("Variación de Color del Fondo")]
    public Image imagenFondoMenuPrincipal;
    public Color colorA = Color.white;
    public Color colorB = Color.gray;
    public float velocidadColor = 1.0f;
    
    // Referencia a los gestores
    private bool mostrandoIntroduccion = false;  // Nueva variable para controlar la intro
    
    // Variables para el sistema de introducción secuencial
    private int imagenActualIntro = 0;  // Índice de la imagen actual
    private bool mostrandoImagenIntro = false;  // Para evitar múltiples pulsaciones durante la animación
    
    private void Start()
    {

        // Configurar panel de introducción
        if (panelIntroduccion != null)
        {
            panelIntroduccion.SetActive(false);
        }
        
        // Configurar imágenes de introducción (todas invisibles al inicio)
        ConfigurarImagenesIntroduccion();
        
        // Asegurarse de que el menú principal esté visible por defecto
        if (panelMenuPrincipal != null)
        {
            panelMenuPrincipal.SetActive(true);
        }
    }

    private void Update()
    {
        AnimarFondo();
        
        // NUEVO: Detectar teclas para la introducción secuencial
        if (mostrandoIntroduccion && !mostrandoImagenIntro)
        {
            if (Input.GetKeyDown(KeyCode.Space) || 
                Input.GetKeyDown(KeyCode.Return) || 
                Input.GetKeyDown(KeyCode.UpArrow) || 
                Input.GetKeyDown(KeyCode.DownArrow) || 
                Input.GetKeyDown(KeyCode.LeftArrow) || 
                Input.GetKeyDown(KeyCode.RightArrow))
            {
                SiguienteImagenIntroduccion();
            }
        }
    }

    private void AnimarFondo()
    {
        // 2. Color cíclico
        if (imagenFondoMenuPrincipal != null)
        {
            float t = (Mathf.Sin(Time.time * velocidadColor) + 1f) / 2f;
            imagenFondoMenuPrincipal.color = Color.Lerp(colorA, colorB, t);
        }
    }
    
    // NUEVO: Configurar todas las imágenes de introducción como invisibles
    private void ConfigurarImagenesIntroduccion()
    {
        if (imagenesIntroduccion != null && imagenesIntroduccion.Length > 0)
        {
            foreach (Image imagen in imagenesIntroduccion)
            {
                if (imagen != null)
                {
                    Color color = imagen.color;
                    color.a = 0f;
                    imagen.color = color;
                }
            }
        }
    }

    
    // Método para resetear el estado de la introducción
    private void ResetearIntroduccion()
    {
        imagenActualIntro = 0;
        mostrandoImagenIntro = false;
        ConfigurarImagenesIntroduccion();
        
        // Resetear el texto también
        if (textoPresionarTecla != null)
        {
            Color colorTexto = textoPresionarTecla.color;
            colorTexto.a = 0f;
            textoPresionarTecla.color = colorTexto;
        }
    }
    
    // Método para mostrar la pantalla de introducción
    private void MostrarPantallaIntroduccion()
    {
        if (panelIntroduccion != null && imagenesIntroduccion != null && imagenesIntroduccion.Length > 0)
        {
            if (panelMenuPrincipal != null)
                panelMenuPrincipal.SetActive(false);

            ResetearIntroduccion();
            panelIntroduccion.SetActive(true);
            mostrandoIntroduccion = true;

            // ← Cambiar música al iniciar la introducción
            if (SimpleAudioSystem.Instance != null)
                SimpleAudioSystem.Instance.EnterMusicZone("MusicaIntro");

            StartCoroutine(MostrarTextoInstruccion());
        }
        else
        {
            SceneManager.LoadScene("ejemplo");
        }
    }

    // Corrutina para mostrar el texto de instrucción
    private IEnumerator MostrarTextoInstruccion()
    {
        if (textoPresionarTecla != null)
        {
            // Cambiar el texto para indicar que puede presionar para ver la primera imagen
            textoPresionarTecla.text = "Presiona cualquier tecla para comenzar la historia...";
            
            // Fade in del texto
            float tiempo = 0f;
            while (tiempo < 1f)
            {
                tiempo += Time.deltaTime * velocidadFadeIntro * 1f;
                
                Color colorTexto = textoPresionarTecla.color;
                colorTexto.a = Mathf.Clamp01(tiempo);
                textoPresionarTecla.color = colorTexto;
                
                yield return null;
            }
            
            // Asegurar alpha completo del texto
            Color colorTextoFinal = textoPresionarTecla.color;
            colorTextoFinal.a = 1f;
            textoPresionarTecla.color = colorTextoFinal;
        }
    }
    
    // Método para mostrar la siguiente imagen en la secuencia
    private void SiguienteImagenIntroduccion()
    {
        // Si estamos mostrando una imagen, ignorar la entrada
        if (mostrandoImagenIntro) return;
        
        // Si ya mostramos todas las imágenes, continuar al juego
        if (imagenActualIntro >= imagenesIntroduccion.Length)
        {
            ContinuarAlJuego();
            return;
        }
        
        // Mostrar la imagen actual
        StartCoroutine(MostrarImagenConFadeIn(imagenActualIntro));
    }
    
    // Corrutina para mostrar una imagen específica con fade in
    private IEnumerator MostrarImagenConFadeIn(int indiceImagen)
    {
        mostrandoImagenIntro = true;
        
        if (indiceImagen < imagenesIntroduccion.Length && imagenesIntroduccion[indiceImagen] != null)
        {
            Image imagenActual = imagenesIntroduccion[indiceImagen];
            
            // Fade in de la imagen
            float tiempo = 0f;
            while (tiempo < 1f)
            {
                tiempo += Time.deltaTime * velocidadFadeIntro;
                
                Color colorImg = imagenActual.color;
                colorImg.a = Mathf.Clamp01(tiempo);
                imagenActual.color = colorImg;
                
                yield return null;
            }
            
            // Asegurar alpha completo
            Color colorImgFinal = imagenActual.color;
            colorImgFinal.a = 1f;
            imagenActual.color = colorImgFinal;
        }
        
        // Incrementar contador para la siguiente imagen
        imagenActualIntro++;
        
        // Actualizar texto según el progreso
        ActualizarTextoProgreso();
        
        mostrandoImagenIntro = false;
    }
    
    // Método para actualizar el texto según el progreso
    private void ActualizarTextoProgreso()
    {
        if (textoPresionarTecla != null)
        {
            if (imagenActualIntro < imagenesIntroduccion.Length)
            {
                textoPresionarTecla.text = $"({imagenActualIntro}/{imagenesIntroduccion.Length})";
            }
            else
            {
                textoPresionarTecla.text = "¿Estás listo para comenzar tu aventura?";
            }
        }
    }
    
    // Método para continuar al juego
    private void ContinuarAlJuego()
    {
        mostrandoIntroduccion = false;

        // Detener el overlay de intro antes de cambiar de escena
        if (SimpleAudioSystem.Instance != null)
            SimpleAudioSystem.Instance.ExitMusicZone("MusicaIntro");

        SceneManager.LoadScene("ejemplo");
    }
    // Métodos de navegación
    // Ahora muestra la introducción en lugar de ir directamente al juego
    public void IniciarPartido()
    {
        MostrarPantallaIntroduccion();
    }

    public void IrAMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void Salir()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}