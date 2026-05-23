// MusicZone.cs
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MusicZone : MonoBehaviour
{
    [Header("Configuración de zona")]
    public string zoneName;
    
    [Header("Configuración de Overlay")]
    [Tooltip("Si está activado, esta música se reproducirá como overlay")]
    public bool isOverlay = false;
    [Tooltip("Si está activado y es overlay, la música se detendrá al salir de la zona")]
    public bool stopOnExit = false;
   
    [Header("Configuración Direct Play (Opcional)")]
    [Tooltip("Para reproducir un clip directamente sin usar las zonas del SimpleAudioSystem")]
    public AudioClip directPlayClip;
    [Range(0f, 1f)]
    public float volumeScale = 1f;
    
    // Variable para trackear si el jugador está en la zona
    private bool playerInZone = false;
   
    private void Awake()
    {
        // Verificar que el collider esté configurado como trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }
    }
   
    private void Start()
    {
        // Verificaciones iniciales
        if (SimpleAudioSystem.Instance == null)
        {
            //Debug.LogError("[MusicZone] No se encontró SimpleAudioSystem en la escena. Asegúrate que existe un GameObject con este componente.");
            return;
        }
        
        // Validar configuración
        if (string.IsNullOrEmpty(zoneName) && directPlayClip == null)
        {
            //Debug.LogError("[MusicZone] Configuración incompleta: se requiere zoneName o directPlayClip");
            return;
        }
       
        // Log de configuración
        string playMethod = !string.IsNullOrEmpty(zoneName) ? "Zona: " + zoneName : "Clip directo: " + directPlayClip.name;
        string overlayInfo = isOverlay ? " (OVERLAY)" : " (NORMAL)";
    }
   
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si es el jugador
        if (other.CompareTag("Player"))
        {
            if (SimpleAudioSystem.Instance == null)
            {
                //Debug.LogError("[MusicZone] SimpleAudioSystem no está disponible");
                return;
            }
           
            playerInZone = true;
           
            // Determinar qué método usar para reproducir música
            if (!string.IsNullOrEmpty(zoneName))
            {
                // Reproducir por zona
                SimpleAudioSystem.Instance.EnterMusicZone(zoneName);
            }
            else if (directPlayClip != null)
            {
                // Reproducir directamente el clip
                SimpleAudioSystem.Instance.PlayMusic(directPlayClip, volumeScale, isOverlay);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        // Verificar si es el jugador
        if (other.CompareTag("Player"))
        {
            if (SimpleAudioSystem.Instance == null)
            {
                //Debug.LogError("[MusicZone] SimpleAudioSystem no está disponible");
                return;
            }
            
            playerInZone = false;
            
            // Solo detener si es overlay y stopOnExit está activado
            if (isOverlay && stopOnExit)
            {
                if (!string.IsNullOrEmpty(zoneName))
                {
                    // Salir de zona por nombre
                    SimpleAudioSystem.Instance.ExitMusicZone(zoneName);
                }
               
            }
        }
    }
    
    // Método público para verificar si el jugador está en la zona
    public bool IsPlayerInZone()
    {
        return playerInZone;
    }
    
    // Método público para forzar el inicio de la música
    public void ForceStartMusic()
    {
        if (SimpleAudioSystem.Instance == null) return;
        
        if (!string.IsNullOrEmpty(zoneName))
        {
            SimpleAudioSystem.Instance.EnterMusicZone(zoneName);
        }
        else if (directPlayClip != null)
        {
            SimpleAudioSystem.Instance.PlayMusic(directPlayClip, volumeScale, isOverlay);
        }
    }
    
    // Método público para forzar la detención de la música
    public void ForceStopMusic()
    {
        if (SimpleAudioSystem.Instance == null) return;
        
        if (isOverlay && !string.IsNullOrEmpty(zoneName))
        {
            SimpleAudioSystem.Instance.ExitMusicZone(zoneName);
        }
    }
   
    // Método para visualizar la zona en el editor
    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col == null) return;
        
        // Guardar estado original
        Color originalColor = Gizmos.color;
        Matrix4x4 originalMatrix = Gizmos.matrix;
        
        // Establecer color
        Gizmos.color = isOverlay ? new Color(1f, 0.5f, 0f, 0.8f) : new Color(0f, 1f, 0.2f, 0.8f);
        
        if (col is BoxCollider boxCol)
        {
            // Calcular centro mundial
            Vector3 worldCenter = transform.TransformPoint(boxCol.center);
            
            // Calcular tamaño mundial (respetando escala)
            Vector3 worldSize = new Vector3(
                boxCol.size.x * transform.lossyScale.x,
                boxCol.size.y * transform.lossyScale.y,
                boxCol.size.z * transform.lossyScale.z
            );
            
            // Crear matriz que incluya rotación
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                worldCenter,           // Posición
                transform.rotation,    // Rotación del objeto
                worldSize              // Tamaño
            );
            
            // Aplicar matriz
            Gizmos.matrix = rotationMatrix;
            
            // Dibujar cubo unitario (la matriz lo escala, rota y posiciona)
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
        else if (col is SphereCollider sphereCol)
        {
            Vector3 worldCenter = transform.TransformPoint(sphereCol.center);
            float worldRadius = sphereCol.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
            
            Gizmos.matrix = Matrix4x4.identity;
            Gizmos.DrawWireSphere(worldCenter, worldRadius);
        }
        
        // Restaurar
        Gizmos.color = originalColor;
        Gizmos.matrix = originalMatrix;
    }
    
    // Método para mostrar información en el inspector
    private void OnDrawGizmosSelected()
    {
        // Información adicional cuando está seleccionado
        if (isOverlay)
        {
            Gizmos.color = Color.yellow;
            Vector3 pos = transform.position + Vector3.up * 2f;
        }
    }
}