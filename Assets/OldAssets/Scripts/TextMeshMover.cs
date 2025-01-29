using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshMover : MonoBehaviour
{
    public Rigidbody parentRigidbody; // Reference to the Rigidbody of the parent ragdoll object
    public float initialMoveSpeed = 2f; // Initial speed of the oscillation
    public float maxMoveSpeed = 50f;    // Maximum speed at a given point
    public float bounceHeight = 0.5f;   // Height of the bounce above the parent
    public float xOffset = 0f;          // Static offset for the X axis
    public float yOffset = 0f;          // Static offset for the Y axis
    public float zOffset = 0f;          // Static offset for the Z axis
    public float accelerationRate = 2f; // Controls how fast the speed accelerates
    private float spawnTime;            // Time when the object was spawned

    // Start is called before the first frame update
    void Start()
    {
        // Record the time the object was spawned
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (parentRigidbody != null)
        {
            // Get the elapsed time since the object was spawned
            float elapsedTime = Time.time - spawnTime;

            // Calculate the move speed, using exponential growth for faster acceleration over time
            float moveSpeed = initialMoveSpeed * Mathf.Pow(accelerationRate, elapsedTime / 10f);

            // Clamp the moveSpeed to a maximum value to avoid it growing indefinitely
            moveSpeed = Mathf.Clamp(moveSpeed, initialMoveSpeed, maxMoveSpeed);

            // Get the world position from the Rigidbody's Y position
            float parentWorldYPosition = parentRigidbody.position.y;

            // Calculate the bounce using a sine wave
            float bounceOffset = Mathf.Sin(Time.time * moveSpeed) * bounceHeight;

            // Apply the position with static offsets on X, Y, and Z
            // Y position is the parent's Y position plus the bounce and the static Y offset
            transform.position = new Vector3(parentRigidbody.position.x + xOffset, parentWorldYPosition + bounceOffset + yOffset, parentRigidbody.position.z + zOffset);
        }
    }
}
