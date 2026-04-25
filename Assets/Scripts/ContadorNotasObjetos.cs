using UnityEngine;

public class ContadorNotasObjetos : MonoBehaviour
{

    public int ContadorNotas = 0;
    public int ContadorObjetos = 0;
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
        Debug.Log("Nota recogida. Total: " + ContadorNotas);
    }
    public void AgregarObjeto()
    {
        ContadorObjetos ++;
    }
}
