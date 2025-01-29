using UnityEngine;

public class RestartCaller : MonoBehaviour
{
    private GameManagerJam gameManagerJam;

    // Start is called before the first frame update
    void Start()
    {
        // Find the object with the tag "GameController"
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");

        if (gameController != null)
        {
            // Get the GameManagerJam component from the GameController
            gameManagerJam = gameController.GetComponent<GameManagerJam>();

            if (gameManagerJam == null)
            {
                Debug.LogError("GameManagerJam component not found on GameController.");
            }
        }
        else
        {
            Debug.LogError("GameController object not found.");
        }
    }

    // This method can be called to trigger the restart
    public void CallRestart()
    {
        Debug.Log("Called Restart");
        if (gameManagerJam != null)
        {
            // Call the RestartGame method from the GameManagerJam component
            //gameManagerJam.RestartGame();
        }
        else
        {
            Debug.LogError("GameManagerJam component not available to call RestartGame.");
        }
    }
}
