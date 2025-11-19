/// <summary>
/// Cambia a la siguiente escena cuando el jugador entra en el trigger.
/// - Requiere que el objeto tenga un collider con "Is Trigger" activado.
/// - Solo reacciona a objetos con el tag "Player".
/// - Carga la escena cuyo nombre está asignado en el inspector.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    public string sceneName = "Scene 2";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
