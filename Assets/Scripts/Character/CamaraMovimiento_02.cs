using UnityEngine;

public class CamaraMovimiento_02 : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Distancia")]
    public float distancia = 5f;
    public float altura = 2f;

    [Header("Rotación")]
    public float sensibilidad = 3f;
    public float limiteVerticalMin = -20f;
    public float limiteVerticalMax = 60f;

    [Header("Suavizado")]
    [Tooltip("Qué tan rápido sigue la posición del personaje (mayor = más pegado)")]
    public float smoothPosition = 8f;
    [Tooltip("Suavizado adicional solo en el eje horizontal (X/Z) para movimiento lateral")]
    public float smoothLateral = 12f;

    private float rotacionX = 0f;
    private float rotacionY = 0f;
    private bool rotandoConMouse = false;

    // Posición suavizada del target, separada por eje
    private Vector3 targetSmoothed;

    private void Start()
    {
        rotacionY = transform.eulerAngles.y;
        rotacionX = transform.eulerAngles.x;

        if (target != null)
            targetSmoothed = target.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            rotandoConMouse = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            rotandoConMouse = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (rotandoConMouse)
        {
            rotacionY += Input.GetAxis("Mouse X") * sensibilidad;
            rotacionX -= Input.GetAxis("Mouse Y") * sensibilidad;
            rotacionX = Mathf.Clamp(rotacionX, limiteVerticalMin, limiteVerticalMax);
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Suavizar la posición del target de forma independiente por eje:
        targetSmoothed.x = Mathf.Lerp(targetSmoothed.x, target.position.x, smoothLateral * Time.deltaTime);
        targetSmoothed.z = Mathf.Lerp(targetSmoothed.z, target.position.z, smoothLateral * Time.deltaTime);
        targetSmoothed.y = Mathf.Lerp(targetSmoothed.y, target.position.y, smoothPosition * Time.deltaTime);

        // La rotación se aplica directamente sin suavizado
        Quaternion rotacion = Quaternion.Euler(rotacionX, rotacionY, 0f);

        Vector3 offsetLocal = new Vector3(0, altura, -distancia);
        Vector3 posicionDeseada = targetSmoothed + rotacion * offsetLocal;

        // La cámara se teleporta a la posición calculada — el suavizado
        transform.position = posicionDeseada;

        transform.LookAt(targetSmoothed + Vector3.up * altura * 0.5f);
    }
}