using UnityEngine;
using UnityEngine.InputSystem;

public class RecoleccionNotas : MonoBehaviour
{
    public InputActionReference Interactuar;

    public float RangoDeInteraccion = 2f;

    public Transform player; // Referencia al player

    public ContadorNotasObjetos contador;
    public MenuAyuda menuAyuda;

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
        Collider[] colliders = Physics.OverlapSphere(player.position, RangoDeInteraccion);

        foreach (Collider col in colliders)
        {
            if (col.gameObject == gameObject)
            {
                contador.AgregarNota();
                ReproducirSonido();
                Destroy(gameObject);
                return;
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

    private void OnTriggerEnter(Collider other)
    {
        // Verificar que sea el jugador
        if (other.transform == player)
        {
            if (menuAyuda != null)
                menuAyuda.MostrarAyudaRecolectarSiNecesario();
        }
    }
}