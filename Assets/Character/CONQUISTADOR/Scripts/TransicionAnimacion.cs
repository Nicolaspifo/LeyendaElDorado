using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TransicionAnimacion : MonoBehaviour
{
    Animator anim;
    public InputActionReference Movimiento;
    public InputActionReference Saltar;
    public InputActionReference Intereactuar;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento
        if (Movimiento.action.IsPressed())
        {
            anim.SetBool("semueve", true);
        }
        else
        {
            anim.SetBool("semueve", false);
        }

        // Recoger
        if (Intereactuar.action.IsPressed())
        {
            anim.SetBool("recoger", true);
        }
        else
        {
            anim.SetBool("recoger", false);
        }

        // SALTO
        if (Saltar.action.IsPressed())
        {
            anim.SetBool("saltar", true);
        }
        else
        {
            anim.SetBool("saltar", false);
        }
    }
}