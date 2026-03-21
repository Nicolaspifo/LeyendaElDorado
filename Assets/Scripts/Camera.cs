using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // El objetivo a seguir (el jugador)
    public Vector3 offset =  new Vector3(0,5,-5);// La distancia entre la cámara y el objetivo
    public float smoothSpeed = 10.125f; // La velocidad de suavizado


    private void LateUpdate()
    {
        if (target == null) return; // Asegurarse de que el objetivo no sea nulo
        // Posición deseada de la cámara
        Vector3 desiredPosition = target.position + offset;
        // Suavizar la transición entre la posición actual y la deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        // Hacer que la cámara mire al objetivo
        transform.LookAt(target);
    }
}
