// SimpleAudioSystem.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleAudioSystem : MonoBehaviour
{
    // Referencia estática para acceso global fácil
    public static SimpleAudioSystem Instance { get; private set; }

    [System.Serializable]
    public class MusicTrack
    {
        public string zoneName;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Tooltip("Si está activado, esta música se reproducirá como overlay (encima de la música actual)")]
        public bool isOverlay = false;
    }
    
    [Header("Audio Mixer Groups")]
    public UnityEngine.Audio.AudioMixerGroup musicMixerGroup;
    public UnityEngine.Audio.AudioMixerGroup overlayMixerGroup;
    public UnityEngine.Audio.AudioMixerGroup sfxMixerGroup;

    [Header("Sonido de Botón")]
    public AudioClip buttonClickSound;

    [Header("Configuración")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    public float fadeTime = 1.0f;
    public List<MusicTrack> musicTracks = new List<MusicTrack>();
    
    [Header("Configuración de Escena")]
    [Tooltip("Si está activado, la música continuará entre escenas")]
    public bool persistMusicBetweenScenes = false;
    [Tooltip("Si está activado, se detendrá la música al cambiar de escena. Si no, solo se pausará")]
    public bool stopMusicOnSceneChange = true;

    [Header("Referencias")]
    // Usamos dos fuentes para hacer crossfade
    public AudioSource musicSource1;
    public AudioSource musicSource2;
    public AudioSource sfxSource;
    
    [Header("Overlay Audio Sources")]
    [Tooltip("Fuentes de audio adicionales para música overlay (se crearán automáticamente si está vacío)")]
    public List<AudioSource> overlayAudioSources = new List<AudioSource>();
    
    [Header("Configuración de Overlay")]
    [Range(0f, 1f)]
    [Tooltip("Multiplicador de volumen para música overlay (relativo al volumen principal)")]
    public float overlayVolumeMultiplier = 0.8f;

    // Tracking
    private string currentZone = "";
    private AudioSource activeSource;
    private AudioSource inactiveSource;
    private bool isFading = false;
    private string lastScene = "";
    
    // Tracking para overlays
    private Dictionary<string, AudioSource> activeOverlays = new Dictionary<string, AudioSource>();
    private List<AudioSource> availableOverlaySources = new List<AudioSource>();
    
    private void Awake()
    {
        // Configuración singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            //Debug.Log("[SimpleAudioSystem] Inicializado correctamente");

            // Suscribirse al evento de cambio de escena
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // Guardar la escena inicial
            lastScene = SceneManager.GetActiveScene().name;
        }
        else
        {
            //Debug.Log("[SimpleAudioSystem] Ya existe una instancia. Destruyendo este objeto.");
            Destroy(gameObject);
            return;
        }

        // Configurar fuentes de audio si no existen
        ConfigureAudioSources();
    }

    private void ConfigureAudioSources()
    {
        // Crear musicSource1 si no existe
        if (musicSource1 == null)
        {
            GameObject sourceObj1 = new GameObject("Music Source 1");
            sourceObj1.transform.SetParent(transform);
            musicSource1 = sourceObj1.AddComponent<AudioSource>();
            musicSource1.outputAudioMixerGroup = musicMixerGroup;
            musicSource1.playOnAwake = false;
            musicSource1.loop = true;
        }

        // Crear musicSource2 si no existe
        if (musicSource2 == null)
        {
            GameObject sourceObj2 = new GameObject("Music Source 2");
            sourceObj2.transform.SetParent(transform);
            musicSource2 = sourceObj2.AddComponent<AudioSource>();
            musicSource2.outputAudioMixerGroup = musicMixerGroup;
            musicSource2.playOnAwake = false;
            musicSource2.loop = true;
            musicSource2.volume = 0f; // Inicialmente silenciado
        }

        // Crear sfxSource si no existe
        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFX Source");
            sfxObj.transform.SetParent(transform);
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxMixerGroup;
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
        }

        // Configurar fuentes overlay si no existen
        ConfigureOverlayAudioSources();

        // Configuración inicial
        activeSource = musicSource1;
        inactiveSource = musicSource2;
    }

    private void ConfigureOverlayAudioSources()
    {
        // Si no hay fuentes overlay configuradas, crear algunas por defecto
        if (overlayAudioSources.Count == 0)
        {
            // Crear 3 fuentes overlay por defecto
            for (int i = 0; i < 3; i++)
            {
                GameObject overlayObj = new GameObject("Overlay Source " + (i + 1));
                overlayObj.transform.SetParent(transform);
                AudioSource overlaySource = overlayObj.AddComponent<AudioSource>();
                overlaySource.outputAudioMixerGroup = overlayMixerGroup;
                overlaySource.playOnAwake = false;
                overlaySource.loop = true;
                overlaySource.volume = 0f;
                overlayAudioSources.Add(overlaySource);
            }
        }
        // Aplicar mixer group a fuentes ya existentes (asignadas desde el inspector)
        foreach (AudioSource src in overlayAudioSources)
        {
            if (src != null && overlayMixerGroup != null)
                src.outputAudioMixerGroup = overlayMixerGroup;
        }

        // Inicializar la lista de fuentes disponibles
        availableOverlaySources.Clear();
        availableOverlaySources.AddRange(overlayAudioSources);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Al cargar una nueva escena
        string newSceneName = scene.name;
        //Debug.Log("[SimpleAudioSystem] Nueva escena cargada: " + newSceneName);

        // Verificar si la escena ha cambiado
        if (newSceneName != lastScene)
        {
            // Solo hacer algo si la escena es diferente
            lastScene = newSceneName;
            HandleSceneChange();
        }
    }

    // Método para manejar el cambio de escena
    private void HandleSceneChange()
    {
        // Si no queremos que la música persista entre escenas
        if (!persistMusicBetweenScenes)
        {
            if (stopMusicOnSceneChange)
            {
                // Detener completamente la música con fade out
                StopMusic();
                StopAllOverlays();
            }
            else
            {
                // Solo pausar la música
                PauseMusic();
                PauseAllOverlays();
            }
        }
    }

    private void OnDestroy()
    {
        // Desuscribirse del evento al destruir
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Método para reproducir un efecto de sonido
    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (clip == null || sfxSource == null) return;
        
        sfxSource.PlayOneShot(clip, masterVolume * volumeScale);
    }

    // Método principal para cambiar de zona musical
    public void EnterMusicZone(string zoneName)
    {
        // Buscar la pista correspondiente
        MusicTrack track = FindMusicTrack(zoneName);
        if (track == null)
        {
            //Debug.LogWarning("[SimpleAudioSystem] No se encontró la pista para la zona: " + zoneName);
            return;
        }

        // Verificar si es una pista overlay
        if (track.isOverlay)
        {
            StartOverlayMusic(zoneName, track);
        }
        else
        {
            // Si ya estamos en esta zona y no es overlay, no hacer nada
            if (zoneName == currentZone) return;
            
            currentZone = zoneName;
            // Iniciar transición normal
            StartCoroutine(FadeMusicCrossfade(track.clip, track.volume * masterVolume));
        }
    }

    // Método para salir de una zona musical (especialmente importante para overlays)
    public void ExitMusicZone(string zoneName)
    {
        // Buscar la pista correspondiente
        MusicTrack track = FindMusicTrack(zoneName);
        if (track == null) return;

        // Si es overlay, detenerlo
        if (track.isOverlay)
        {
            StopOverlayMusic(zoneName);
        }
        // Si es la zona principal actual, podrías implementar lógica para cambiar a otra zona o detener
    }

    // Método para iniciar música overlay
    private void StartOverlayMusic(string zoneName, MusicTrack track)
    {
        // Si ya está reproduciéndose este overlay, no hacer nada
        if (activeOverlays.ContainsKey(zoneName))
        {
            //Debug.Log("[SimpleAudioSystem] El overlay ya está activo: " + zoneName);
            return;
        }

        // Buscar una fuente overlay disponible
        if (availableOverlaySources.Count == 0)
        {
            //Debug.LogWarning("[SimpleAudioSystem] No hay fuentes overlay disponibles para: " + zoneName);
            return;
        }

        AudioSource overlaySource = availableOverlaySources[0];
        availableOverlaySources.RemoveAt(0);

        // Configurar y reproducir overlay
        overlaySource.clip = track.clip;
        overlaySource.volume = 0f;
        overlaySource.Play();

        // Agregar a overlays activos
        activeOverlays[zoneName] = overlaySource;

        // Calcular volumen final del overlay (más bajo que la música principal)
        float finalVolume = track.volume * masterVolume * overlayVolumeMultiplier;

        // Hacer fade in del overlay
        StartCoroutine(FadeInOverlay(overlaySource, finalVolume));

        //Debug.Log("[SimpleAudioSystem] Overlay iniciado: " + zoneName + " (Volumen: " + finalVolume + ")");
    }

    // Método para detener música overlay
    private void StopOverlayMusic(string zoneName)
    {
        if (!activeOverlays.ContainsKey(zoneName)) return;

        AudioSource overlaySource = activeOverlays[zoneName];
        
        // Hacer fade out y luego detener
        StartCoroutine(FadeOutOverlay(overlaySource, zoneName));
    }

    // Corrutina para fade in de overlay
    private System.Collections.IEnumerator FadeInOverlay(AudioSource source, float targetVolume)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / fadeTime);
            yield return null;
        }
        
        source.volume = targetVolume;
    }

    // Corrutina para fade out de overlay
    private System.Collections.IEnumerator FadeOutOverlay(AudioSource source, string zoneName)
    {
        float startVolume = source.volume;
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeTime);
            yield return null;
        }
        
        // Detener y limpiar
        source.Stop();
        source.clip = null;
        source.volume = 0f;
        
        // Devolver a fuentes disponibles
        availableOverlaySources.Add(source);
        activeOverlays.Remove(zoneName);
        
    }

    // Método para detener todos los overlays
    public void StopAllOverlays()
    {
        List<string> overlayKeys = new List<string>(activeOverlays.Keys);
        foreach (string zoneName in overlayKeys)
        {
            StopOverlayMusic(zoneName);
        }
    }

    // Método para pausar todos los overlays
    public void PauseAllOverlays()
    {
        foreach (AudioSource overlaySource in activeOverlays.Values)
        {
            if (overlaySource.isPlaying)
            {
                overlaySource.Pause();
            }
        }
    }

    // Método para reanudar todos los overlays
    public void ResumeAllOverlays()
    {
        foreach (AudioSource overlaySource in activeOverlays.Values)
        {
            if (!overlaySource.isPlaying && overlaySource.clip != null)
            {
                overlaySource.UnPause();
            }
        }
    }

    // Método alternativo que toma directamente un AudioClip
    public void PlayMusic(AudioClip clip, float volumeScale = 1f, bool isOverlay = false)
    {
        if (clip == null) return;
        
        //Debug.Log("[SimpleAudioSystem] Reproduciendo clip directamente: " + clip.name + (isOverlay ? " (OVERLAY)" : " (NORMAL)"));
        
        if (isOverlay)
        {
            // Crear un track temporal para overlay
            MusicTrack tempTrack = new MusicTrack
            {
                zoneName = "DirectPlay_" + clip.name + "_" + System.DateTime.Now.Ticks,
                clip = clip,
                volume = volumeScale,
                isOverlay = true
            };
            StartOverlayMusic(tempTrack.zoneName, tempTrack);
        }
        else
        {
            StartCoroutine(FadeMusicCrossfade(clip, volumeScale * masterVolume));
        }
    }

    // Método para buscar una pista por nombre de zona
    private MusicTrack FindMusicTrack(string zoneName)
    {
        foreach (MusicTrack track in musicTracks)
        {
            if (track.zoneName == zoneName)
            {
                return track;
            }
        }
        return null;
    }

    // Corrutina para crossfade entre fuentes de audio (música principal)
    private System.Collections.IEnumerator FadeMusicCrossfade(AudioClip newClip, float targetVolume)
    {
        // Si ya estamos en transición, no iniciar otra
        if (isFading)
        {
            //Debug.Log("[SimpleAudioSystem] Ya hay una transición en curso. Esperando...");
            yield return null;
        }

        isFading = true;
        
        // Preparar la fuente inactiva con el nuevo clip
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0f;
        inactiveSource.Play();

        // Realizar el crossfade
        float elapsedTime = 0f;
        float startVolumeActive = activeSource.volume;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeTime;
            
            // Fade out de la fuente activa
            activeSource.volume = Mathf.Lerp(startVolumeActive, 0f, t);
            
            // Fade in de la fuente inactiva
            inactiveSource.volume = Mathf.Lerp(0f, targetVolume, t);
            
            yield return null;
        }

        // Finalizar la transición
        activeSource.Stop();
        activeSource.clip = null;
        
        // Intercambiar las fuentes
        AudioSource temp = activeSource;
        activeSource = inactiveSource;
        inactiveSource = temp;
        
        // Asegurar volúmenes finales correctos
        activeSource.volume = targetVolume;
        inactiveSource.volume = 0f;
        
        isFading = false;
    }

    // Actualizar el volumen maestro
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        
        // Actualizar la fuente activa
        if (activeSource != null && activeSource.clip != null)
        {
            // Buscar el track actual para obtener su volumen específico
            float trackVolume = 1f;
            foreach (MusicTrack track in musicTracks)
            {
                if (track.clip == activeSource.clip)
                {
                    trackVolume = track.volume;
                    break;
                }
            }
            activeSource.volume = trackVolume * masterVolume;
        }
        
        // Actualizar overlays activos
        foreach (var overlay in activeOverlays)
        {
            string zoneName = overlay.Key;
            AudioSource overlaySource = overlay.Value;
            
            MusicTrack track = FindMusicTrack(zoneName);
            if (track != null)
            {
                overlaySource.volume = track.volume * masterVolume * overlayVolumeMultiplier;
            }
            else
            {
                // Para clips directos, usar el volumen actual ajustado
                float currentRelativeVolume = overlaySource.volume / (masterVolume * overlayVolumeMultiplier);
                overlaySource.volume = currentRelativeVolume * masterVolume * overlayVolumeMultiplier;
            }
        }
        
        //Debug.Log("[SimpleAudioSystem] Volumen maestro actualizado: " + masterVolume);
    }

    // Método para ajustar el multiplicador de volumen de overlay
    public void SetOverlayVolumeMultiplier(float multiplier)
    {
        overlayVolumeMultiplier = Mathf.Clamp01(multiplier);
        
        // Actualizar todos los overlays activos con el nuevo multiplicador
        foreach (var overlay in activeOverlays)
        {
            string zoneName = overlay.Key;
            AudioSource overlaySource = overlay.Value;
            
            MusicTrack track = FindMusicTrack(zoneName);
            if (track != null)
            {
                overlaySource.volume = track.volume * masterVolume * overlayVolumeMultiplier;
            }
        }
        
        //Debug.Log("[SimpleAudioSystem] Multiplicador de volumen overlay actualizado: " + overlayVolumeMultiplier);
    }

    // Silenciar/activar toda la música
    public void SetMusicMuted(bool muted)
    {
        if (activeSource != null)
        {
            activeSource.mute = muted;
        }
        if (inactiveSource != null)
        {
            inactiveSource.mute = muted;
        }
        
        // Silenciar overlays
        foreach (AudioSource overlaySource in activeOverlays.Values)
        {
            overlaySource.mute = muted;
        }
    }

    // Método para detener toda la música
    public void StopMusic()
    {
        StartCoroutine(FadeOut());
        StopAllOverlays();
    }

    // Método para pausar la música (se puede reanudar más tarde)
    public void PauseMusic()
    {
        if (activeSource != null && activeSource.isPlaying)
        {
            activeSource.Pause();
            //Debug.Log("[SimpleAudioSystem] Música pausada");
        }
        
        if (inactiveSource != null && inactiveSource.isPlaying)
        {
            inactiveSource.Pause();
        }
        
        PauseAllOverlays();
    }

    // Método para reanudar la música pausada
    public void ResumeMusic()
    {
        if (activeSource != null && !activeSource.isPlaying && activeSource.clip != null)
        {
            activeSource.UnPause();
            //Debug.Log("[SimpleAudioSystem] Música reanudada");
        }
        
        ResumeAllOverlays();
    }

    private System.Collections.IEnumerator FadeOut()
    {
        if (activeSource == null || !activeSource.isPlaying) yield break;
        
        float startVolume = activeSource.volume;
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            activeSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeTime);
            yield return null;
        }
        
        activeSource.Stop();
        activeSource.clip = null;
        currentZone = "";
    }

    // Método para reproducir un sonido de botón
    public void PlayButtonSound()
    {
        if (sfxSource != null && buttonClickSound != null)
        {
            sfxSource.PlayOneShot(buttonClickSound, masterVolume);
        }
    }
}