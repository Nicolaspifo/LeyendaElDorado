//NavegacionInventario.cs

using UnityEngine;

public class NavegacionInventario : MonoBehaviour
{
    public GameObject PersonajesCanvas;
    public DiarioMuiscaNavegacion diarioNavegacion; // ← NUEVO: referencia al diario

    // Mapa de nombre NPC → índice (debe coincidir con el orden en diariosPersonajes)
    private static readonly System.Collections.Generic.Dictionary<string, int> indicesPersonajes
        = new System.Collections.Generic.Dictionary<string, int>
    {
        { "Cacique HUE",          0 },
        { "Guerrero NYKY",        1 },
        { "Sacerdote CHYKY",     2 },
        { "Artista CHISAKUI",      3 },
        { "Agricultora CHIBCHAZHUM",         4 },
        { "Esclava BAIA", 5 }
    };

    void Start() { }

    public void crearPersonajeUI(string nombreNPC)
    {
        if (PersonajesCanvas == null) return;

        // Activa el personaje en el inventario
        Transform hijo = PersonajesCanvas.transform.Find(nombreNPC);
        if (hijo != null)
        {
            hijo.gameObject.SetActive(true);

            // Oculta la imagen de "no descubierto" y habilita el botón
            Transform imagenBloqueo = hijo.Find("Image");
            if (imagenBloqueo != null) imagenBloqueo.gameObject.SetActive(false);

            BotonBasicoInventario boton = hijo.GetComponent<BotonBasicoInventario>();
            if (boton != null) boton.estaDescubierto = true; // ← NUEVO
        }
        

        // Registra en el diario que este personaje ya se puede consultar
        if (diarioNavegacion != null && indicesPersonajes.TryGetValue(nombreNPC, out int idx))
        {
            diarioNavegacion.RegistrarPersonajeDescubierto(idx);
        }
    }
}