using UnityEngine;

public class IndicadorNPC : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject interactionUI;

    

    void Update()
    {
        DetectPlayerLooking();
    }

    

    void DetectPlayerLooking()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);

        bool shouldShow = false;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // Dirección desde jugador hacia NPC
                Vector3 directionToNPC = (transform.position - hit.transform.position).normalized;

                float dot = Vector3.Dot(hit.transform.forward, directionToNPC);

                if (dot > 0.7f)
                {
                    shouldShow = true;
                    break;
                }
            }
        }

        if (interactionUI != null)
            interactionUI.SetActive(shouldShow);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
