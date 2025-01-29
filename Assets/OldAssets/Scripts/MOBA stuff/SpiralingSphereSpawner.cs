using UnityEngine;

public class SpiralingSphereSpawner : MonoBehaviour
{
    public GameObject spherePrefab; // Reference to the sphere prefab
    public float spawnInterval = 0.1f; // Interval between spawns
    public float spiralSpeed = 1.0f; // Speed of the spiraling motion
    public float fallSpeed = 1.0f; // Speed of the downward motion
    public float spiralRadius = 1.0f; // Radius of the spiral

    private float timeSinceLastSpawn = 0.0f;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnSpiralingSphere();
            timeSinceLastSpawn = 0.0f;
        }
    }

    void SpawnSpiralingSphere()
    {
        GameObject sphere = Instantiate(spherePrefab, transform.position, Quaternion.identity);
        sphere.AddComponent<SpiralingMovement>().Initialize(spiralSpeed, fallSpeed, spiralRadius);
    }
}

public class SpiralingMovement : MonoBehaviour
{
    private float spiralSpeed;
    private float fallSpeed;
    private float spiralRadius;
    private float angle = 0.0f;

    public void Initialize(float spiralSpeed, float fallSpeed, float spiralRadius)
    {
        this.spiralSpeed = spiralSpeed;
        this.fallSpeed = fallSpeed;
        this.spiralRadius = spiralRadius;
    }

    void Update()
    {
        angle += spiralSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * spiralRadius;
        float z = Mathf.Sin(angle) * spiralRadius;
        transform.position += new Vector3(x, -fallSpeed * Time.deltaTime, z);

        // Optional: Destroy the sphere after it falls below a certain height
        if (transform.position.y < -10.0f)
        {
            Destroy(gameObject);
        }
    }
}
