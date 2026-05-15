using UnityEngine;
using UnityEngine.InputSystem;

public class MenuInventario : MonoBehaviour
{
    public bool isInvetoryActive = false;
    public GameObject menuInventarioUI;
    public InputActionReference inventarioButton;
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
            if (isInvetoryActive)
            {
                ResumeGame();
            }
            else 
            {
                AbrirInventario();
            }
        }
    }

    void AbrirInventario()
    {
        Time.timeScale = 0f; // Detiene el tiempo del juego
        isInvetoryActive = true;
        menuInventarioUI.SetActive(true); // Muestra el menú de pausa

    }

    void ResumeGame()
    {
        Time.timeScale = 1f; // Reanuda el tiempo del juego
        isInvetoryActive = false;
        menuInventarioUI.SetActive(false);
    }
}
