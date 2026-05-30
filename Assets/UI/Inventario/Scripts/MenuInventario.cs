//MenuInventario.cs

using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInventario : MonoBehaviour
{
    public bool isInvetoryActive = false;
    public GameObject menuInventarioUI;
    public InputActionReference inventarioButton;
    public InputActionReference cancelarButton;
    void Start()
    {
        menuInventarioUI.SetActive(false); // Asegúrate de que el menú de pausa esté desactivado al inicio
    }

    private void OnEnable()
    {
        inventarioButton.action.Enable();
    }

    private void OnDisable()
    {
        inventarioButton.action.Disable();
    }



    // Update is called once per frame
    void Update()
    {

        if (inventarioButton.action.triggered )
        {
            if (cancelarButton.action.triggered || inventarioButton.action.triggered && isInvetoryActive)
            {
                ResumeGame();
            }
            else 
            {
                AbrirInventario();
            }
        }
        if (cancelarButton.action.triggered && isInvetoryActive)
        {
            ResumeGame();
        }

    }

    void AbrirInventario()
    {
        Time.timeScale = 0f;
        isInvetoryActive = true;
        menuInventarioUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isInvetoryActive = false;
        menuInventarioUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
