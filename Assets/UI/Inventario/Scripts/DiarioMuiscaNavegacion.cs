//DiarioMuiscaNavegacion.cs

using UnityEngine;
using System.Collections.Generic;

public class DiarioMuiscaNavegacion : MonoBehaviour
{
    [Header("Diarios de cada personaje (en orden)")]
    public GameObject[] diariosPersonajes;
    // Asignar en inspector: 0=Cacique, 1=Guerrero, 2=Sacerdote, 3=Artista, 4=Agricultora, 5=Esclava

    [Header("Botones de Navegación")]
    public GameObject siguienteBoton;
    public GameObject anteriorBoton;

    private List<int> descubiertos = new List<int>();
    private int indiceActual = 0;

    void Start()
    {
        ActualizarBotones();
    }

    // Llamado desde NavegacionInventario cuando se descubre un personaje
    public void RegistrarPersonajeDescubierto(int indicePersonaje)
    {
        if (!descubiertos.Contains(indicePersonaje))
        {
            descubiertos.Add(indicePersonaje);
            descubiertos.Sort();
        }
    }

    // Llamado desde BotonBasicoInventario para abrir el diario de un personaje específico
    public void AbrirDiarioDePersonaje(int indicePersonaje)
    {
        
        int idx = descubiertos.IndexOf(indicePersonaje);
        if (idx < 0)
        {
            return;
        }
        indiceActual = idx;
        MostrarDiarioActual();
    }

    public void SiguientePersonaje()
    {
        if (indiceActual < descubiertos.Count - 1)
        {
            indiceActual++;
            MostrarDiarioActual();
        }
    }

    public void AnteriorPersonaje()
    {
        if (indiceActual > 0)
        {
            indiceActual--;
            MostrarDiarioActual();
        }
    }

    private void MostrarDiarioActual()
    {
        foreach (var d in diariosPersonajes)
            if (d != null) d.SetActive(false);

        if (descubiertos.Count == 0) return;

        int globalIdx = descubiertos[indiceActual];
        if (globalIdx >= 0 && globalIdx < diariosPersonajes.Length)
            diariosPersonajes[globalIdx].SetActive(true);

        ActualizarBotones();
    }

    private void ActualizarBotones()
    {
        if (anteriorBoton) anteriorBoton.SetActive(indiceActual > 0);
        if (siguienteBoton) siguienteBoton.SetActive(indiceActual < descubiertos.Count - 1);
    }
    
    public void SincronizarDesdeBoton(int indicePersonaje)
    {
        if (!descubiertos.Contains(indicePersonaje))
            descubiertos.Add(indicePersonaje);

        descubiertos.Sort();
        indiceActual = descubiertos.IndexOf(indicePersonaje);
        ActualizarBotones(); // solo mueve botones, no toca SetActive de diarios
    }
}