using UnityEngine;
using UnityEngine.InputSystem;

public class RecoleccionNotas : MonoBehaviour
{
    public InputActionReference Interactuar;

    public float RangoDeInteraccion = 2f;

    public Transform player; // Referencia al player

    public ContadorNotasObjetos contador;

    [Header("Audio")]
    public AudioClip sonidoBoton;
    public bool usarSonidoGlobal = true;

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
        float radio = 1f;

        // Empezar un poco detrás del jugador
        Vector3 origen = player.position - player.forward * radio * 1f;

        Ray ray = new Ray(origen, player.forward);

        RaycastHit hit;

        if (Physics.SphereCast(ray, radio, out hit, RangoDeInteraccion + radio))
        {
            if (hit.collider.gameObject == gameObject)
            {
                contador.AgregarNota();

                ReproducirSonido();

                Destroy(gameObject);
            }
        }
    }

    private void ReproducirSonido()
    {
        if (usarSonidoGlobal && SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.PlayButtonSound();
        }
        else if (sonidoBoton != null && SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.sfxSource.PlayOneShot(sonidoBoton);
        }
    }
}