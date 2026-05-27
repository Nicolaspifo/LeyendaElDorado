using TMPro;
using UnityEngine;
using System.Collections;

public class ContadorNotasObjetos : MonoBehaviour
{
    
    public int ContadorNotas = 0;
    public int ContadorObjetos = 0;
    public GameObject UIDiarioNotas;
    public TMP_Text contadorNotasText;
    public GameObject UITextoNotificacionNotas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AgregarNota()
    {
        ContadorNotas ++;
        int index = ContadorNotas - 1;
        contadorNotasText.text = ContadorNotas.ToString();

        if (UIDiarioNotas != null && index < UIDiarioNotas.transform.childCount)
            UIDiarioNotas.transform.GetChild(index).gameObject.SetActive(true);
        else
            Debug.LogWarning("No hay suficientes hijos en UIDiarioNotas para la nota " + ContadorNotas);

        // ACTIVAR NOTIFICACIÓN
        if (UITextoNotificacionNotas != null)
        {
            // Activa el objeto
            UITextoNotificacionNotas.SetActive(true);

            // Busca el hijo llamado "TextoNotificacionNotas"
            Transform textoHijo = UITextoNotificacionNotas.transform.Find("TextoNotificacionNotas");

            if (textoHijo != null)
            {
                TMP_Text textoTMP = textoHijo.GetComponent<TMP_Text>();

                if (textoTMP != null)
                {
                    // Mantiene el texto que ya tiene
                    textoTMP.text = ContadorNotas + "/6";
                }
            }

            // Desactivar después de 5 segundos
            StartCoroutine(DesactivarNotificacion());
        }

        Debug.Log("Nota recogida. Total: " + ContadorNotas);
    }

    IEnumerator DesactivarNotificacion()
    {
        yield return new WaitForSeconds(5f);

        if (UITextoNotificacionNotas != null)
        {
            UITextoNotificacionNotas.SetActive(false);
        }
    }
    public void AgregarObjeto()
    {
        ContadorObjetos ++;
    }
}
