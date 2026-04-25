using UnityEngine;

public class TransicionAnimacion : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento
        if (Input.GetKey("w"))
        {
            anim.SetBool("semueve", true);
        }
        else
        {
            anim.SetBool("semueve", false);
        }

        // Recoger
        if (Input.GetKey("e"))
        {
            anim.SetBool("recoger", true);
        }
        else
        {
            anim.SetBool("recoger", false);
        }

        // SALTO
        if (Input.GetKey("space"))
        {
            anim.SetBool("saltar", true);
        }
        else
        {
            anim.SetBool("saltar", false);
        }
    }
}