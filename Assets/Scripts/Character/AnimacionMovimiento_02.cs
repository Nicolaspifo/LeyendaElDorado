using UnityEngine;
using UnityEngine.InputSystem;

public class AnimacionMovimiento_02 : MonoBehaviour
{
    Animator anim;

    public InputActionReference movimiento;
    public InputActionReference saltar;
    public InputActionReference interactuar;
    public InputActionReference correr; 
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Movimiento — leer el Vector2 y ver si tiene magnitud
        Vector2 moveInput = movimiento.action.ReadValue<Vector2>();
        bool estaMoviendo = moveInput.magnitude > 0.1f;
        bool estaCorriendo = correr.action.IsPressed();

        anim.SetBool("semueve", estaMoviendo);

        // Salto — usar triggered para que sea un impulso
        anim.SetBool("saltar", saltar.action.IsPressed());

        // Recoger / Interactuar
        anim.SetBool("recoger", interactuar.action.IsPressed());

        // Acelerar animación al correr
        if (estaMoviendo && estaCorriendo)
            anim.speed = 2f;    // ajustable
        else
            anim.speed = 1f;      // velocidad normal
        }
}