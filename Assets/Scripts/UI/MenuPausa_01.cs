using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPausa_01 : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject menuPausaUI;
    public GameObject panelAjustes; // Panel hijo con los sliders de audio
    public InputActionReference pauseButton;

    void Start()
    {
        menuPausaUI.SetActive(false);
        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }

    private void OnEnable() => pauseButton.action.Enable();
    private void OnDisable() => pauseButton.action.Disable();

    void Update()
    {
        if (pauseButton.action.triggered)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isPaused = true;
        menuPausaUI.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        menuPausaUI.SetActive(false);
        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }

    public void AbrirAjustes()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(true);
    }

    public void CerrarAjustes()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }

    public void IrMenuPrincipal()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MenuPrincipal");
    }
}