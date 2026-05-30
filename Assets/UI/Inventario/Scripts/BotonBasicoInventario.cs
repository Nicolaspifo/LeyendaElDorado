using UnityEngine;

public class BotonBasicoInventario : MonoBehaviour
{
    public GameObject MenuInventario;
    public GameObject DiarioMuiscaGO;
    public DiarioMuiscaNavegacion diarioNavegacion;
    public int indicePersonaje = 0;

    [Header("Modo simple (mision, notas, etc.)")]
    public bool esSoloPanel = false;   // ← activa en el inspector para el panel de misión
    public GameObject panelSimple;     // ← el canvas/panel a abrir

    [HideInInspector] public bool estaDescubierto = false;

    public void AbrirDiarioPersonajeNPC()
    {
        // Modo panel simple — no necesita descubrimiento
        if (esSoloPanel)
        {
            if (panelSimple != null) panelSimple.SetActive(true);
            if (MenuInventario != null) MenuInventario.SetActive(false);
            return;
        }

        // Modo diario normal
        if (!estaDescubierto) return;

        if (DiarioMuiscaGO == null || MenuInventario == null)
        {
            Debug.LogWarning("Faltan referencias.");
            return;
        }

        if (diarioNavegacion != null)
            diarioNavegacion.RegistrarPersonajeDescubierto(indicePersonaje);

        if (DiarioMuiscaGO.transform.parent != null)
            DiarioMuiscaGO.transform.parent.gameObject.SetActive(true);

        DiarioMuiscaGO.SetActive(true);
        MenuInventario.SetActive(false);

        if (diarioNavegacion != null)
            diarioNavegacion.SincronizarDesdeBoton(indicePersonaje);
    }

    public void CerrarDiario()
    {
        // Modo panel simple
        if (esSoloPanel)
        {
            if (panelSimple != null) panelSimple.SetActive(false);
            if (MenuInventario != null) MenuInventario.SetActive(true);
            return;
        }

        // Modo diario normal
        if (DiarioMuiscaGO != null && DiarioMuiscaGO.transform.parent != null)
            DiarioMuiscaGO.transform.parent.gameObject.SetActive(false);

        if (MenuInventario != null) MenuInventario.SetActive(true);
    }
}