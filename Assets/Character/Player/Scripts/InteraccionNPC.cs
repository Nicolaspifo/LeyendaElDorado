using UnityEngine;

public class InteraccionNPC : MonoBehaviour
{
    public float interactionRange = 2f; // distancia para interactuar
    public KeyCode interactionKey = KeyCode.E;

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange);

        foreach (Collider hit in hits)
        {
            NPC npc = hit.GetComponent<NPC>();

            if (npc != null)
            {
                // Dirección hacia el NPC
                Vector3 directionToNPC = (npc.transform.position - transform.position).normalized;

                // Ángulo entre hacia donde miras y el NPC
                float dot = Vector3.Dot(transform.forward, directionToNPC);

                // Si está mirando suficientemente hacia el NPC
                if (dot > 0.7f) // puedes ajustar este valor
                {
                    npc.Interact();
                    return;
                }
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
