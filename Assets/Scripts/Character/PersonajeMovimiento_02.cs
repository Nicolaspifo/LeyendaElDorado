using UnityEngine;
using UnityEngine.InputSystem;

public class PersonajeMovimiento_02 : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadCaminar = 5f;
    public float velocidadCorrer = 9f;
    [Header("Rotación")]
    public float suavizadoRotacion = 10f;
    [Tooltip("Ángulo a partir del cual el giro es casi instantáneo")]
    public float anguloGiroRapido = 100f;
    [Tooltip("Velocidad mínima de rotación en giros pequeños")]
    public float velocidadGiroMinima = 5f;
    public float aceleracion = 15f;

    [Header("Salto")]
    public float fuerzaSalto = 5f;
    [Tooltip("Distancia al suelo para considerar que está pisando")]
    public float groundCheckDistance = 0.15f;
    public LayerMask groundLayers;


    [Header("Cámara")]
    public Transform camaraTransform;

    [Header("Input")]
    public InputActionReference move;
    public InputActionReference saltar;
    public InputActionReference correr;

    private Rigidbody rb;
    private Vector2 moveDirection;
    private bool estaMoviendo = false;

    private Quaternion rotacionObjetivo;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rotacionObjetivo = transform.rotation;
    }

    private void OnEnable()
    {
        saltar.action.Enable();
        saltar.action.performed += SaltarAccion;
    }

    private void OnDisable()
    {
        saltar.action.performed -= SaltarAccion;
        saltar.action.Disable();
    }

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();
        estaMoviendo = moveDirection.magnitude > 0.1f;

        // La rotación visual se actualiza en Update
        if (estaMoviendo && camaraTransform != null)
        {
            Vector3 forward = camaraTransform.forward;
            Vector3 right   = camaraTransform.right;
            forward.y = 0f;
            right.y   = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 direccion = (forward * moveDirection.y + right * moveDirection.x).normalized;
            if (direccion.sqrMagnitude > 0.01f)
                rotacionObjetivo = Quaternion.LookRotation(direccion);
        }

        float angulo = Quaternion.Angle(transform.rotation, rotacionObjetivo);
        float t = Mathf.Clamp01(angulo / anguloGiroRapido);
        float velocidadGiro = Mathf.Lerp(velocidadGiroMinima, suavizadoRotacion, t);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacionObjetivo,
            velocidadGiro * Time.deltaTime
        );
}

    private void FixedUpdate()
    {
        Mover();
    }

    private void Mover()
    {
        if (camaraTransform == null) return;

        Vector3 forward = camaraTransform.forward;
        Vector3 right   = camaraTransform.right;
        forward.y = 0f;
        right.y   = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 direccion = (forward * moveDirection.y + right * moveDirection.x).normalized;

        bool corriendo = correr.action.IsPressed();
        float velocidadObjetivo = corriendo ? velocidadCorrer : velocidadCaminar;

        // Velocidad horizontal actual
        Vector3 velHorizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Velocidad deseada en horizontal
        Vector3 velDeseada = direccion * (estaMoviendo ? velocidadObjetivo : 0f);

        // Aplicar fuerza proporcional a la diferencia 
        Vector3 diferencia = velDeseada - velHorizontal;
        rb.AddForce(diferencia * aceleracion, ForceMode.Acceleration);
    }

    private void SaltarAccion(InputAction.CallbackContext context)
    {
        if (EnSuelo())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.VelocityChange);
        }
        
    }

    private bool EnSuelo()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance + 0.05f,
            groundLayers
        );
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * (groundCheckDistance + 0.05f)
        );
        Gizmos.color = EnSuelo() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f);
    }
}