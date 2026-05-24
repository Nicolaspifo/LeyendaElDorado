using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    [TextArea(2,5)] public string[] dialogos;

    public AudioClip DialogoSonido;
    public bool usarSonidoGlobal = true;

    public GameObject interactionUI;
    public bool yainteractuado = false;

    public void ShowIndicator(bool show)
    {
        if (interactionUI != null)
            interactionUI.SetActive(show);
    }

    public void ReproducirSonidoDialogo()
    {
        if (usarSonidoGlobal && SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.PlayButtonSound();
        }
        else if (DialogoSonido != null && SimpleAudioSystem.Instance != null)
        {
            SimpleAudioSystem.Instance.sfxSource.PlayOneShot(DialogoSonido);
        }
    }


}
