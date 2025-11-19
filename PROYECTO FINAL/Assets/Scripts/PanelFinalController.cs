/// <summary>
/// Controla el panel final del juego.
/// - Muestra el puntaje, muertes y tiempo total al completar el nivel.
/// - Detiene el temporizador del GameManager.
/// - Permite reiniciar el nivel, volver al menú principal y salir del juego.
/// 
/// Este script debe estar asignado al panel final y conectado desde el inspector
/// con los textos y botones correspondientes.
/// </summary>
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalPanelController : MonoBehaviour
{
    [Header("Panel final")]
    public GameObject finalPanel;

    [Header("Textos TMP")]
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalDeathsText;
    public TextMeshProUGUI finalTimeText;

    private bool activated = false;

    public void ShowFinalPanel()
    {
        if (activated) return;
        activated = true;

        // Detener conteo del tiempo
        GameManager.Instance.StopCountingTime();

        // Activar el panel final
        finalPanel.SetActive(true);

        // --- Tomar valores directamente del GameManager ---
        int score = GameManager.Instance.totalScore;
        int deaths = GameManager.Instance.totalDeaths;
        float time = GameManager.Instance.totalTime;

        // Formato del tiempo
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        // Mostrar valores
        finalScoreText.text = score.ToString();
        finalDeathsText.text = deaths.ToString();
        finalTimeText.text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
