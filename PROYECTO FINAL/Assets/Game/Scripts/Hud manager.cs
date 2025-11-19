/// <summary>
/// Administra el HUD principal del juego, asegurando que exista una única instancia.
/// Este objeto persiste entre escenas gracias a DontDestroyOnLoad,
/// evitando que el HUD se duplique al cambiar de nivel.
/// </summary>
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private static HUDManager instance;

    private void Awake()
    {
        // Si ya existe un HUD en otra escena, destruye este
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Este será el HUD único del juego
        instance = this;

        // Mantener HUD entre escenas
        DontDestroyOnLoad(gameObject);
    }
}
