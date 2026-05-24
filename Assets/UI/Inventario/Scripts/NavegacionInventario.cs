
using UnityEngine;

public class NavegacionInventario : MonoBehaviour
{
    public GameObject PersonajesCanvas;
    

    void Start()
    {
    }

    public void crearPersonajeUI(string nombreNPC)
    {

        if (PersonajesCanvas == null)
        {
            return;
        }

        Transform hijo = PersonajesCanvas.transform.Find(nombreNPC);
        if (hijo != null)
        {
            hijo.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró un hijo con el nombre: " + nombreNPC + " dentro de " + PersonajesCanvas.name);
        }
    }

    
}
