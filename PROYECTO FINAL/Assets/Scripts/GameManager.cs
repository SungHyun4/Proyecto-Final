using UnityEngine;
using TMPro; // si usas TextMeshProUGUI en la interfaz

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Totales acumulados")]
    public int totalScore = 0;
    public int totalFalls = 0;
    public int totalCollected = 0; // se mantiene por compatibilidad
    public float totalTime = 0f;

    [Header("Referencias UI (solo números)")]
    public TextMeshProUGUI scoreText;    // solo número del puntaje
    public TextMeshProUGUI fallsText;    // solo número de caídas

    [Header("Timer dividido (TMP)")]
    public TextMeshProUGUI minutesText;
    public TextMeshProUGUI secondsText;
    public TextMeshProUGUI millisecondsText;

    private bool countingTime = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (countingTime)
        {
            totalTime += Time.deltaTime;
            UpdateUI();
        }
    }

    // --- Métodos públicos ---
    public void AddScore(int amount)
    {
        totalScore += amount;
        if (totalScore < 0) totalScore = 0;
        UpdateUI();
    }

    public void AddFall()
    {
        totalFalls++;
        UpdateUI();
    }

    // se deja vacío para evitar errores si otros scripts lo llaman
    public void AddCollected()
    {
        totalCollected++;
        // no actualiza nada en pantalla
    }

    public void StopCountingTime() => countingTime = false;

    // --- Actualiza los textos del Canvas ---
    private void UpdateUI()
    {
        // solo números (ya tienes textos separados con etiquetas)
        if (scoreText != null)
            scoreText.text = totalScore.ToString();

        if (fallsText != null)
            fallsText.text = totalFalls.ToString();

        // Timer dividido
        int minutes = Mathf.FloorToInt(totalTime / 60f);
        int seconds = Mathf.FloorToInt(totalTime % 60f);
        int milliseconds = Mathf.FloorToInt((totalTime * 1000f) % 1000f);

        if (minutesText != null)
            minutesText.text = $"{minutes:00}";

        if (secondsText != null)
            secondsText.text = $"{seconds:00}";

        if (millisecondsText != null)
            millisecondsText.text = $"{milliseconds:000}";
    }
}
