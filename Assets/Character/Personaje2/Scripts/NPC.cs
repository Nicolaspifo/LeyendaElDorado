using UnityEngine;

public class NPC : MonoBehaviour
{
    public string npcName;
    [TextArea(2,5)] public string[] dialogos;

    public GameObject interactionUI;

    public void ShowIndicator(bool show)
    {
        if (interactionUI != null)
            interactionUI.SetActive(show);
    }


}
