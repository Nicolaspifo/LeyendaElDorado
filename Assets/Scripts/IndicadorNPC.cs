using UnityEngine;

public class IndicadorNPC : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject interactionUI;

    public InteraccionNPC interaccion;



    void Update()
    {
        DetectPlayerLooking();
    }



    void DetectPlayerLooking()
    {
        if (interaccion == null) return;

        NPC npc = interaccion.GetNPCDetectado();

        bool shouldShow = (npc != null && npc.gameObject == gameObject);

        if (interactionUI != null)
            interactionUI.SetActive(shouldShow);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
