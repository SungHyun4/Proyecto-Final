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
