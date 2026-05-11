using UnityEngine;

public class GameController : MonoBehaviour
{
    private int LimiteFPS = 60;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = LimiteFPS;
    }

    
}
