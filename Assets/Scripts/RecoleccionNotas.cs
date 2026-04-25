using UnityEngine;
using UnityEngine.InputSystem;

public class RecoleccionNotas : MonoBehaviour
{
    public InputActionReference Interactuar;

    public float RangoDeInteraccion = 2f;

    public Transform player; // Referencia al player

    public ContadorNotasObjetos contador;

    void Update()
    {
        if (Interactuar == null || Interactuar.action == null)
            return;

        if (Interactuar.action.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(player.position, player.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, RangoDeInteraccion))
        {
            if (hit.collider.gameObject == gameObject)
            {
                contador.AgregarNota();
                
                Destroy(gameObject);
            }
        }
    }
}