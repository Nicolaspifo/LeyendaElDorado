using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuAyuda : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject ayudaCaminar;
    public GameObject ayudaCorrer;
    public GameObject ayudaInteractuar;
    public GameObject ayudaRecolectar;
    public GameObject notificacionObjeto;

    [Header("Input")]
    public InputActionReference cerrar; 

    [Header("Referencias")]
    public InteraccionNPC interaccionNPC;
    public RecoleccionNotas[] fragmentos;

    // Flags
    private bool moviendoMostrado = false;
    private bool correrMostrado = false;
    private bool interactuarMostrado = false;
    private bool recolectarMostrado = false;

    private GameObject panelActivo = null;
    private Coroutine cierreAutomatico = null;
    private float tiempoAparicion = 0f;
    private const float tiempoMinimo = 3f;

    void OnEnable()
    {
        ayudaCaminar.SetActive(false);
        ayudaCorrer.SetActive(false);      
        ayudaInteractuar.SetActive(false);
        ayudaRecolectar.SetActive(false);
        notificacionObjeto.SetActive(false);

        StartCoroutine(MostrarAyudaCaminar());
        StartCoroutine(MostrarAyudaCorrer());  
    }

    void Start() { }

    void Update()
    {
        if (panelActivo != null && panelActivo != notificacionObjeto)
        {
            bool presionoCerrar = cerrar != null && cerrar.action.WasPressedThisFrame();
            if (presionoCerrar && Time.realtimeSinceStartup >= tiempoAparicion + tiempoMinimo)
                CerrarPanelActivo();
        }

        if (!interactuarMostrado && interaccionNPC != null)
        {
            if (interaccionNPC.GetNPCDetectado() != null)
                MostrarPanel(ayudaInteractuar, ref interactuarMostrado);
        }
    }

    // Llamado desde ContadorNotasObjetos al recoger fragmento
    public void MostrarNotificacionObjeto()
    {
        if (cierreAutomatico != null) StopCoroutine(cierreAutomatico);
        notificacionObjeto.SetActive(true);
        // Sin timeScale, sin cursor, sin bloqueo — solo mostrar y ocultar
        cierreAutomatico = StartCoroutine(OcultarNotificacion());
    }

    IEnumerator OcultarNotificacion()
    {
        yield return new WaitForSecondsRealtime(5f);
        notificacionObjeto.SetActive(false);
    }

    IEnumerator MostrarAyudaCaminar()
    {
        yield return new WaitForSecondsRealtime(5f);
        MostrarPanel(ayudaCaminar, ref moviendoMostrado);
    }
    IEnumerator MostrarAyudaCorrer()
    {
        yield return new WaitForSecondsRealtime(20f);
        MostrarPanel(ayudaCorrer, ref correrMostrado);
    }
    
    public void MostrarAyudaRecolectarSiNecesario()
    {
        if (!recolectarMostrado)
            MostrarPanel(ayudaRecolectar, ref recolectarMostrado);
    }

    void MostrarPanel(GameObject panel, ref bool flag)
    {
        if (flag) return;
        flag = true;

        if (panelActivo != null && panelActivo != notificacionObjeto)
            panelActivo.SetActive(false);

        panelActivo = panel;
        tiempoAparicion = Time.realtimeSinceStartup;
        panel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CerrarPanelActivo()
    {
        if (panelActivo != null)
        {
            panelActivo.SetActive(false);
            panelActivo = null;
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void BotonCerrar()
    {
        if (Time.realtimeSinceStartup >= tiempoAparicion + tiempoMinimo) // ← y aquí
            CerrarPanelActivo();
    }
}