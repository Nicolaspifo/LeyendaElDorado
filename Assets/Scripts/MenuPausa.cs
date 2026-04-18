using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPausa : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject menuPausaUI;
    public InputActionReference pauseButton;
    void Start()
    {
        menuPausaUI.SetActive(false); // Asegúrate de que el menú de pausa esté desactivado al inicio
    }

    private void OnEnable()
    {
        pauseButton.action.Enable();
    }

    private void OnDisable()
    {
        pauseButton.action.Disable();
    }



    // Update is called once per frame
    void Update()
    {
        
        if (pauseButton.action.triggered)
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
