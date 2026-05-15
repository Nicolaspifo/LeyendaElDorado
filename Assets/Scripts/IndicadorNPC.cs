using UnityEngine;

public class IndicadorNPC : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject interactionUI;

    public InteraccionNPC interaccion;

    [Header("World Canvas")]
    // Prefab for a world-space canvas that will appear above the NPC
    public GameObject worldCanvasPrefab;
    // Local offset from the NPC's transform where the canvas will appear
    public Vector3 canvasOffset = new Vector3(0.5f, 2f, 0f);

    // Spawned instance of the world canvas (if any)
    GameObject spawnedWorldCanvas;

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

        // Handle world canvas prefab: instantiate when player looks at this NPC,
        // destroy when the player looks away.
        if (shouldShow)
        {
            if (worldCanvasPrefab != null && spawnedWorldCanvas == null)
            {
                // Parent to the NPC so it follows its movement. Use localPosition for offset.
                spawnedWorldCanvas = Instantiate(worldCanvasPrefab, transform);
                spawnedWorldCanvas.transform.localPosition = canvasOffset;
                spawnedWorldCanvas.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            if (spawnedWorldCanvas != null)
            {
                Destroy(spawnedWorldCanvas);
                spawnedWorldCanvas = null;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
