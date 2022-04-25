
using UnityEngine;
using UnityEngine.SceneManagement;

public class Events : MonoBehaviour
{
    /*
    [SerializeField]
    private Transform checkpoint;
    [SerializeField]
    private GameObject player;
    */
    // public CharacterController characterController;
    // public GameObject respawnPoint;
    public void Restart()
    {
        // Instantiate(player, checkpoint, Quaternion.identity);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // characterController.transform.position = respawnPoint.transform.position;
        // player.transform.position = checkpoint.transform.position;
        // Application.LoadLevel(1);
        // GetComponent<PlayerMovement>().transform.position = GetComponent<Checkpoint>().transform.position;
        // Checkpoint.DontDestroyOnLoad(this.gameObject);
        UIManager.instance.deathCounter++;
        UIManager.instance.updateDeathCounterUI();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
