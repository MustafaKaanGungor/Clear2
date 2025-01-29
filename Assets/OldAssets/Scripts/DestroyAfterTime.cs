using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Time in seconds before the GameObject is destroyed
    public float lifetime = 5f;

    private void Start()
    {
        StartDestroyTimer(lifetime);
    }

    public void StartDestroyTimer(float delay)
    {
        // Invoke the function to destroy the object after the specified delay
        Invoke("DestroyIdleObject", delay);
    }


    // Function to handle destruction of the GameObject
    public void DestroyIdleObject()
    {
        // Log the name of the GameObject that is about to be destroyed
        Debug.Log("Destroying GameObject: " + gameObject.name);

        // Check if the current GameObject's tag is "Sphere"
        if (gameObject.tag == "Sphere")
        {
            Debug.Log("GameObject has the 'Sphere' tag, not incrementing DeathCounter.");
        }
        else
        {
            // Find the GameObject with the tag "Player"
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Check if the player GameObject is found
            if (player != null)
            {
                // Get the PlayerController component from the player GameObject
                PlayerController playerController = player.GetComponent<PlayerController>();

                // Check if the PlayerController component is found
                if (playerController != null)
                {
                    // Increment the DeathCounter variable
                    playerController.DeathCounter++;
                }
            }
        }

        // Destroy the GameObject
        Destroy(gameObject);
    }




    // Function to handle destruction of the GameObject
    public void DestroyObject()
    {
        // Log the name of the GameObject that is about to be destroyed
        Debug.Log("Destroying GameObject: " + gameObject.name);

        // Destroy the GameObject
        Destroy(gameObject);
    }

    // Function to handle destruction of the GameObject
    public void Perfect()
    {
        Debug.Log("Perfect Called");
        // Log the name of the GameObject that is about to be destroyed
        Debug.Log("Destroying GameObject: " + gameObject.name);

        // Destroy the GameObject
        Destroy(gameObject);
    }

}
