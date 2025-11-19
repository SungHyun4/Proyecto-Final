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

