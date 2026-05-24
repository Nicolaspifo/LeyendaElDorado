using TMPro;
using UnityEngine;

public class ContadorNotasObjetos : MonoBehaviour
{

    public int ContadorNotas = 0;
    public int ContadorObjetos = 0;
    public GameObject UIDiarioNotas;
    public TMP_Text contadorNotasText;
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

        Debug.Log("Nota recogida. Total: " + ContadorNotas);
    }
    public void AgregarObjeto()
    {
        ContadorObjetos ++;
    }
}
