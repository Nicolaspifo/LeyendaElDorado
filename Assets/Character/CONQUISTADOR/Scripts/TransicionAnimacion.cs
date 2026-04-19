using UnityEngine;

public class TransicionAnimacion : MonoBehaviour
{
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame  
    void Update()
    {
        if (Input.GetKey("w"))
        {
            anim.SetBool("semueve", true);

        }

        if (!Input.GetKey("w"))
        {
            anim.SetBool("semueve", false);

        }

        if (Input.GetKey("e"))
        {
            anim.SetBool("recojerr", true);

        }

        if (!Input.GetKey("e"))
        {
            anim.SetBool("recojerr", false);

        }
    }

}