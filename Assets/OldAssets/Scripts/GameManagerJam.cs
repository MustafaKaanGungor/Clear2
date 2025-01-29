using UnityEngine;

public class GameManagerJam : MonoBehaviour
{
    [SerializeField] private GameObject bedPrefab;
    [SerializeField] private GameObject player;

    [SerializeField] private float xRange = 9f;
    [SerializeField] private float zRange = 9f;
    private float spawnHeight = 6.1f; //(20 feet is approximately 6.1 meters)
    private float spawnInterval = 4f;

    private bool gameStarted = false;

    [SerializeField] private UIManager uIManager;

    public void StartGame() //Game start is actually triggered by UIManager's GameStart()
    {//I know it is bad but I didnt wanted to create so many back to forth functions from here to there
        if (!gameStarted)
        {
            gameStarted = true;

            player.SetActive(true);

            InvokeRepeating("SpawnRandomPrefab", 0f, spawnInterval);// Start spawning prefabs repeatedly
        }
    }

    private void Update() {
        if(gameStarted) {
            //spawnInterval = 2000 - (player.GetComponent<PlayerController>().PerfectCounter * 5);

            
        }
    }

    void SpawnRandomPrefab()
    {
        // Generate a random position within the given range
        float randomX = Random.Range(-xRange, xRange);
        float randomZ = Random.Range(-zRange, zRange);

        // Set the spawn position
        Vector3 spawnPosition = new Vector3(randomX, spawnHeight, randomZ);

        // Instantiate the prefab at the random position
        Instantiate(bedPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("UYARI UYARI SPAWN INTERVAL DEĞİŞİYOR" + spawnInterval);
    }

    public void CleanGameArea() {
        CancelInvoke("SpawnRandomPrefab");

        // Destroy all objects with the tag "BedPrefab"
        GameObject[] bedPrefabs = GameObject.FindGameObjectsWithTag("BedPrefab");
        foreach (GameObject bed in bedPrefabs)
        {
            Destroy(bed);
        }

        // Destroy the player object with the tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.SetActive(false);
            player.GetComponent<PlayerController>().ResetStats();
        }
        // Reset game state
        gameStarted = false;
    }
}
