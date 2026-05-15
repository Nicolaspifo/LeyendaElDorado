
using UnityEngine;
using UnityEngine.UI;

public class NavegacionInventario : MonoBehaviour
{
    public GameObject PersonajesCanvas;
    public GameObject PersonajePrefabCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void crearPersonajeUI(string nombreNPC)
    {
        GameObject nuevoPersonajeUI = Instantiate(PersonajePrefabCanvas, PersonajesCanvas.transform);

        GameObject hijoImagen = nuevoPersonajeUI.transform.GetChild(0).gameObject;
        hijoImagen.SetActive(true);
        

        

        Image imagenUi = hijoImagen.GetComponent<Image>();

        // Ruta dentro de Resources (SIN "Assets/Resources" y SIN extensión)

        string rutaSprite = "portraits/" + nombreNPC;

        Texture2D nuevoSprite = Resources.Load<Texture2D>(rutaSprite);

        if (nuevoSprite != null)
        {
            // Create a Sprite from the loaded Texture2D and assign it to the UI Image
            Sprite runtimeSprite = Sprite.Create(nuevoSprite, new Rect(0, 0, nuevoSprite.width, nuevoSprite.height), new Vector2(0.5f, 0.5f));
            // Preserve the current RectTransform size of the child image
            RectTransform rt = imagenUi.rectTransform;
            Vector2 targetSize = rt.sizeDelta;

            imagenUi.sprite = runtimeSprite;

            // Re-apply the original size so the sprite matches the hijoImagen dimensions
            rt.sizeDelta = targetSize;
        }
        else
        {
            Debug.LogWarning("No se encontró el sprite en la ruta: " + rutaSprite);
        }
    }
}
