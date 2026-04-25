using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    public float speed = 5f;
    public float inputBufferTime = 0.2f;
    public float fuerzaSalto = 1f;

    private Vector3 lastDirection = Vector3.forward;
    private float bufferTimer = 0f;

    private Rigidbody rb;
    private Vector2 MoveDirection;

    public InputActionReference move;
    public InputActionReference Saltar;

    private bool enSuelo = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Saltar.action.performed += SaltarAccion;
    }

    private void OnDisable()
    {
        Saltar.action.performed -= SaltarAccion;
    }

    private void Update()
    {
        MoveDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(MoveDirection.x, 0f, MoveDirection.y).normalized;

        if (direction.magnitude > 0)
        {
            lastDirection = direction;
            bufferTimer = inputBufferTime;

            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
        else
        {
            bufferTimer -= Time.deltaTime;
        }

        if (lastDirection.magnitude > 0)
        {
            float angle = Mathf.Atan2(lastDirection.x, lastDirection.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, angle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    // 🔥 SALTO
    private void SaltarAccion(InputAction.CallbackContext context)
    {
        if (enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            enSuelo = false;
        }
    }

    // 👇 Detectar suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Suelo"))
        {
            enSuelo = true;
        }
    }
}