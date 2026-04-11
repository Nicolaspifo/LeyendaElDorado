using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject menuPausaUI;
     void Start()
    {
        menuPausaUI.SetActive(false); // Asegúrate de que el menú de pausa esté desactivado al inicio
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // Detiene el tiempo del juego
        isPaused = true;
        menuPausaUI.SetActive(true); // Muestra el menú de pausa

    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        isPaused = false;
        menuPausaUI.SetActive(false);
    }

}
