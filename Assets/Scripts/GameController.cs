using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class GameController : MonoBehaviour
{
    private int LimiteFPS = 60;
   
    public TextMeshProUGUI fpsText;
    public float updateInterval = 0.5f;
    private float accumulatedFrames = 0f;
    private float accumulatedTime = 0f;
    private float timeLeft;

    void Start()
    {
        timeLeft = updateInterval;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = LimiteFPS;
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        accumulatedTime += Time.timeScale / Time.deltaTime;
        accumulatedFrames++;
        if (timeLeft <= 0.0f)
        {
            float fps = accumulatedTime / accumulatedFrames;
            if (fpsText != null)
            {
                fpsText.text = " FPS: " + Mathf.RoundToInt(fps).
                ToString();
            }
            timeLeft = updateInterval;
            accumulatedTime = 0f;
            accumulatedFrames = 0f;
        }
    }

}
