using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public float inputBufferTime = 0.2f;

    private Vector3 lastDirection = Vector3.forward;
    private float bufferTimer = 0f;



    //Nuevo input system
    private Rigidbody rb;

    private Vector2 MoveDirection;

    public InputActionReference move;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MoveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(MoveDirection.x, 0f, MoveDirection.y).normalized;

        // ✅ SIEMPRE guardar dirección si hay input (aunque sea rápido)
        if (direction.magnitude > 0)
        {
            lastDirection = direction;
            bufferTimer = inputBufferTime;

            // Movimiento
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else
        {
            bufferTimer -= Time.deltaTime;
        }

        // ✅ Rotación SIEMPRE basada en la última dirección válida
        if (lastDirection.magnitude > 0)
        {
            float angle = Mathf.Atan2(lastDirection.x, lastDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    // 👇 MÉTODO DEL NUEVO INPUT SYSTEM

}