using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour, IInteractable
{
    [Header("UI References")]
    public GameObject gameUI;   // your HUD canvas
    public GameObject endGameUI; // your win panel
    public AudioClip cheerSFX;
    [SerializeField][Range(0f, 1f)] private float sfxVolume = 1f;

    private bool gameEnded = false;

    public void Interact(GameObject player)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null && inventory.HasKey)
        {
            Debug.Log("Player opened the door. Game Over!");
            TriggerEndGame();
        }
        else
        {
            Debug.Log("You need a key to open this door!");
        }
    }

    public string GetInteractText()
    {
        return "Press E to open door";
    }


    public void TriggerEndGame()
    {
        if (gameEnded) return; gameEnded = true;

        // Hide game HUD
        if (gameUI != null)
            gameUI.SetActive(false);

        // Show end game panel
        if (endGameUI != null)
            endGameUI.SetActive(true);

        // Play cheer sound
        if (cheerSFX != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(cheerSFX, Camera.main.transform.position, sfxVolume);
        }

        // Optional: pause game
        Time.timeScale = 0f;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
