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
    public float tiempoMinimoPorImagenIntro = 2f; // ← nuevo
    private float tiempoMostradoImagenIntro = 0f;

    [Header("Variación de Color del Fondo")]
    public Image imagenFondoMenuPrincipal;
    public Color colorA = Color.white;
    public Color colorB = Color.gray;
    public float velocidadColor = 1.0f;
    
    [Header("Fade de Escena")]
    public Image imagenFadeNegro;
    public float velocidadFadeEscena = 1f;

    [Header("Créditos")]
    public GameObject panelCreditos;      // Panel padre que contiene todo
    public GameObject subpanel1;          // Primer panel de créditos
    public GameObject subpanel2;          // Segundo panel de créditos
    public GameObject botonSiguiente;     // Botón siguiente
    public GameObject botonAnterior;      // Botón anterior
    public float velocidadFadeCreditos = 1.5f;

    private int panelCreditosActual = 1;

    public void AbrirCreditos()
    {
        StartCoroutine(FadeACreditos());
    }
    
    // Referencia a los gestores
    private bool mostrandoIntroduccion = false;
    
    private int imagenActualIntro = 0; 
    private bool mostrandoImagenIntro = false; 
    
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

        // Asegurar que el fade empiece invisible
        if (imagenFadeNegro != null)
        {
            imagenFadeNegro.gameObject.SetActive(false);
            Color color = imagenFadeNegro.color;
            color.a = 0f;
            imagenFadeNegro.color = color;
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
                Input.GetKeyDown(KeyCode.W)|| 
                Input.GetKeyDown(KeyCode.A) || 
                Input.GetKeyDown(KeyCode.S) || 
                Input.GetKeyDown(KeyCode.D) || 
                Input.GetKeyDown(KeyCode.Mouse0))
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
        if (mostrandoImagenIntro) return;

        // Mínimo 2 segundos por imagen
        if (Time.time < tiempoMostradoImagenIntro + tiempoMinimoPorImagenIntro) return;

        if (imagenActualIntro >= imagenesIntroduccion.Length)
        {
            ContinuarAlJuego();
            return;
        }

        StartCoroutine(MostrarImagenConFadeIn(imagenActualIntro));
    }
        
    // Corrutina para mostrar una imagen específica con fade in
    private IEnumerator MostrarImagenConFadeIn(int indiceImagen)
    {
        mostrandoImagenIntro = true;

        if (indiceImagen < imagenesIntroduccion.Length && imagenesIntroduccion[indiceImagen] != null)
        {
            Image imagenActual = imagenesIntroduccion[indiceImagen];

            float tiempo = 0f;
            while (tiempo < 1f)
            {
                tiempo += Time.deltaTime * velocidadFadeIntro;
                Color colorImg = imagenActual.color;
                colorImg.a = Mathf.Clamp01(tiempo);
                imagenActual.color = colorImg;
                yield return null;
            }

            Color colorImgFinal = imagenActual.color;
            colorImgFinal.a = 1f;
            imagenActual.color = colorImgFinal;
        }

        tiempoMostradoImagenIntro = Time.time; // ← registrar cuando termina el fade
        imagenActualIntro++;
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
                textoPresionarTecla.text = "Estás listo para comenzar tu aventura?";
            }
        }
    }
    
    // Método para continuar al juego
    private void ContinuarAlJuego()
    {
        mostrandoIntroduccion = false;
        StartCoroutine(CambiarEscenaConFade());
    }

    private IEnumerator CambiarEscenaConFade()
    {
        // Detener audio con fade
        if (SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.StopMusic();
            SimpleAudioSystem.Instance.StopAllOverlays();
        }

        // Fade in de la imagen negra (pantalla se oscurece)
        if (imagenFadeNegro != null)
        {
            imagenFadeNegro.gameObject.SetActive(true);

            float tiempo = 0f;
            Color color = imagenFadeNegro.color;
            color.a = 0f;
            imagenFadeNegro.color = color;

            while (tiempo < 1f)
            {
                tiempo += Time.deltaTime * velocidadFadeEscena;
                color.a = Mathf.Clamp01(tiempo);
                imagenFadeNegro.color = color;
                yield return null;
            }
        }
        else
        {
            // Si no hay imagen, solo esperar el audio
            if (SimpleAudioSystem.Instance != null)
                yield return new WaitForSeconds(SimpleAudioSystem.Instance.fadeTime);
        }

        SceneManager.LoadScene("EscenaPrincipal");
    }

    private IEnumerator FadeACreditos()
    {
        // Fade out del menú principal
        CanvasGroup cgMenu = panelMenuPrincipal.GetComponent<CanvasGroup>();
        if (cgMenu == null) cgMenu = panelMenuPrincipal.AddComponent<CanvasGroup>();

        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * velocidadFadeCreditos;
            cgMenu.alpha = Mathf.Clamp01(t);
            yield return null;
        }
        cgMenu.alpha = 0f;
        panelMenuPrincipal.SetActive(false);
        cgMenu.alpha = 1f; // resetear para cuando vuelva

        // Mostrar créditos
        panelCreditosActual = 1;
        panelCreditos.SetActive(true);
        ActualizarSubpaneles();

        // Fade in de créditos
        CanvasGroup cgCreditos = panelCreditos.GetComponent<CanvasGroup>();
        if (cgCreditos == null) cgCreditos = panelCreditos.AddComponent<CanvasGroup>();
        cgCreditos.alpha = 0f;

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFadeCreditos;
            cgCreditos.alpha = Mathf.Clamp01(t);
            yield return null;
        }
        cgCreditos.alpha = 1f;
    }

    public void SiguienteCreditos()
    {
        panelCreditosActual = 2;
        ActualizarSubpaneles();
    }

    public void AnteriorCreditos()
    {
        panelCreditosActual = 1;
        ActualizarSubpaneles();
    }

    public void CerrarCreditos()
    {
        StartCoroutine(FadeVueltaMenu());
    }

    private IEnumerator FadeVueltaMenu()
    {
        // Fade out créditos
        CanvasGroup cgCreditos = panelCreditos.GetComponent<CanvasGroup>();
        if (cgCreditos == null) cgCreditos = panelCreditos.AddComponent<CanvasGroup>();

        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime * velocidadFadeCreditos;
            cgCreditos.alpha = Mathf.Clamp01(t);
            yield return null;
        }
        panelCreditos.SetActive(false);

        // Fade in menú
        panelMenuPrincipal.SetActive(true);
        CanvasGroup cgMenu = panelMenuPrincipal.GetComponent<CanvasGroup>();
        if (cgMenu == null) cgMenu = panelMenuPrincipal.AddComponent<CanvasGroup>();
        cgMenu.alpha = 0f;

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * velocidadFadeCreditos;
            cgMenu.alpha = Mathf.Clamp01(t);
            yield return null;
        }
        cgMenu.alpha = 1f;
    }

    private void ActualizarSubpaneles()
    {
        subpanel1.SetActive(panelCreditosActual == 1);
        subpanel2.SetActive(panelCreditosActual == 2);
        botonSiguiente.SetActive(panelCreditosActual == 1);
        botonAnterior.SetActive(panelCreditosActual == 2);
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