using UnityEngine;

public class BotonBasicoInventario : MonoBehaviour
{

    public GameObject MenuInventario;
    public GameObject DiarioPersonajeNPC;


    public void AbrirDiarioPersonajeNPC()
    {
        if (DiarioPersonajeNPC != null && MenuInventario != null)
        {
            if (MenuInventario.activeSelf && !DiarioPersonajeNPC.activeSelf)
            {
                DiarioPersonajeNPC.SetActive(true);
                MenuInventario.SetActive(false);
            }
            else
            {
                DiarioPersonajeNPC.SetActive(false);
                MenuInventario.SetActive(true);
            }

        }
        else
        {
            Debug.LogWarning("DiarioPersonajeNPC o MenuInventario no están asignados en el inspector.");
        }
    }
}
