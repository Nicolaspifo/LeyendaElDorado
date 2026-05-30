using UnityEngine;

public class IndicadorNPC : MonoBehaviour
{
    public float interactionRange = 3f;
    public GameObject interactionUI;

    [HideInInspector] public bool dialogoActivo = false;

    public InteraccionNPC interaccion;

    [Header("World Canvas")]
    // Prefab for a world-space canvas that will appear above the NPC
    public GameObject worldCanvasPrefab;
    // Local offset from the NPC's transform where the canvas will appear
    public Vector3 canvasOffset = new Vector3(0f, 0.57f, 0f);

    // Spawned instance of the world canvas (if any)
    GameObject spawnedWorldCanvas;

    void Update()
    {
        DetectPlayerLooking();

        if (spawnedWorldCanvas != null && Camera.main != null)
        {
            spawnedWorldCanvas.transform.LookAt(Camera.main.transform);
            spawnedWorldCanvas.transform.Rotate(0, 180, 0);
        }
            
    }
    
    void DetectPlayerLooking()
    {
        if (interaccion == null) return;

        NPC npc = interaccion.GetNPCDetectado();
        bool shouldShow = (npc != null && npc.gameObject == gameObject);

        if (dialogoActivo) shouldShow = false;

        if (interactionUI != null)
            interactionUI.SetActive(shouldShow);

        if (shouldShow && worldCanvasPrefab != null && spawnedWorldCanvas == null)
        {
            spawnedWorldCanvas = Instantiate(worldCanvasPrefab, transform);
            spawnedWorldCanvas.transform.localPosition = canvasOffset;
            spawnedWorldCanvas.transform.localRotation = Quaternion.identity;
        }
        else if (!shouldShow && spawnedWorldCanvas != null)
        {
            Destroy(spawnedWorldCanvas);
            spawnedWorldCanvas = null;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
