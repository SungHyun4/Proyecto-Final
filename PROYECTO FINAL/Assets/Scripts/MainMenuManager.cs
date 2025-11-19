/// <summary>
/// Controlador del Menú Principal.
/// Gestiona la navegación básica del menú:
/// - Iniciar el juego cargando la primera escena.
/// - Mostrar y ocultar el panel de instrucciones.
/// Este script se usa únicamente en la pantalla de inicio.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject instructionsPanel;   // El panel que muestra instrucciones

    // BOTÓN JUGAR
    public void PlayGame()
    {
        SceneManager.LoadScene("Scene 1");
    }

    // BOTÓN INSTRUCCIONES
    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    // BOTÓN VOLVER
    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }
}
