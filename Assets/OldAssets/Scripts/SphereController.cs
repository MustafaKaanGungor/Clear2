using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    // Color dictionary to map color initials to their hexadecimal values
    private Dictionary<char, string> colorHexMap = new Dictionary<char, string>
    {
        { 'R', "#FF0000FF" },  // Red
        { 'B', "#003FFFFF" },  // Blue
        { 'G', "#24FF00FF" },  // Green
        { 'P', "#AC00FFFF" },  // Purple
        { 'W', "#FFFFFFFF" }   // White (added just in case)
    };

    private bool incrementedPerfect = false;  // Guard to check if PerfectCounter was incremented
    private bool incrementedDeath = false;    // Guard to check if DeathCounter was incremented

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is called when the sphere collides with another GameObject
    void OnCollisionEnter(Collision collision)
    {
        // Log the name of the GameObject the sphere collided with
        Debug.Log("Sphere collided with: " + collision.gameObject.name);

        // Find the highest parent or stop at "PatientModel"
        Transform highestParent = FindHighestParent(collision.gameObject.transform, "PatientModel");

        // Log the name of the highest parent or the found "PatientModel"
        Debug.Log("Highest parent or 'PatientModel' is: " + highestParent.name);

        // Find the highest parent or stop at "BedPrefab" for potential destruction or Perfect call
        Transform bedPrefabParent = FindHighestParent(collision.gameObject.transform, "BedPrefab");

        // If "PatientModel" was found, search for the child with the tag "shockNeedText"
        if (highestParent.name == "PatientModel")
        {
            Transform shockNeedTextObject = FindChildWithTag(highestParent, "shockNeedText");

            // If the child with the tag is found
            if (shockNeedTextObject != null)
            {
                Debug.Log("Found child with tag 'shockNeedText': " + shockNeedTextObject.name);

                // Find the TextMesh component on the child
                TextMesh textMeshComponent = shockNeedTextObject.GetComponent<TextMesh>();

                // If the TextMesh component is found, print its text
                if (textMeshComponent != null)
                {
                    string shockText = textMeshComponent.text;
                    Debug.Log("TextMesh text: " + shockText);

                    // Find the player GameObject with the tag "Player"
                    GameObject player = GameObject.FindGameObjectWithTag("Player");

                    if (player != null)
                    {
                        // Get the PlayerController component from the player GameObject
                        PlayerController playerController = player.GetComponent<PlayerController>();

                        if (playerController != null)
                        {
                            // Get the hexadecimal colors of the raw images
                            string[] rawImageHexColors = new string[]
                            {
                                ColorToHex(playerController.rawImage1.color),
                                ColorToHex(playerController.rawImage2.color),
                                ColorToHex(playerController.rawImage3.color)
                                //ColorToHex(playerController.rawImage4.color)
                            };

                            // Match the shockText letters with the corresponding rawImage colors
                            string[] letters = shockText.Split('.'); // Split the text by the dots

                            bool match = true;

                            for (int i = 0; i < letters.Length; i++)
                            {
                                char letter = letters[i][0]; // Get the first letter (e.g., 'B' for Blue)

                                if (colorHexMap.ContainsKey(letter))
                                {
                                    string expectedHex = colorHexMap[letter];
                                    string rawImageHex = rawImageHexColors[i];

                                    // Compare the expected color hex with the actual rawImage color hex
                                    if (expectedHex != rawImageHex)
                                    {
                                        Debug.Log($"Mismatch found: Expected {expectedHex} for {letter}, but got {rawImageHex}");
                                        match = false;
                                    }
                                    else
                                    {
                                        Debug.Log($"Match found: {letter} corresponds to {rawImageHex}");
                                    }
                                }
                                else
                                {
                                    Debug.Log($"Unknown color letter: {letter}");
                                    match = false;
                                }
                            }

                            if (match && !incrementedPerfect)  // Guard to prevent multiple increments
                            {
                                Debug.Log("All colors match the text!");
                                
                                // Change the text to "Perfect!" and the color to green
                                textMeshComponent.text = "Perfect!";
                                textMeshComponent.color = Color.green;
                                Debug.Log("TextMesh updated to 'Perfect!' in green.");

                                // Call the Perfect() method on the DestroyAfterTime script with a 2-second delay
                                if (bedPrefabParent != null)
                                {
                                    DestroyAfterTime destroyAfterTime = bedPrefabParent.GetComponent<DestroyAfterTime>();

                                    if (destroyAfterTime != null)
                                    {
                                        Debug.Log("Found script");

                                        // Increment the PerfectCounter variable only once
                                        playerController.PerfectCounter++;
                                        incrementedPerfect = true;  // Set the guard to true

                                        // Start the coroutine to call Perfect() after 2 seconds
                                        StartCoroutine(CallPerfectWithDelay(destroyAfterTime));                                    
                                    }
                                    else
                                    {
                                        Debug.Log("Could not find script");
                                    }
                                }
                                else
                                {
                                    Debug.Log("Could not find bedPrefabParent");
                                }
                            }
                            else if (!match && !incrementedDeath)  // Guard to prevent multiple increments
                            {
                                Debug.Log("Some colors do not match the text.");

                                // Change the text to "Ooof wrong setting Dead!" and the color to red
                                textMeshComponent.text = "Ooof wrong setting Dead!";
                                textMeshComponent.color = Color.red;
                                Debug.Log("TextMesh updated to 'Ooof wrong setting Dead!' in red.");

                                // Call the DestroyObject() method on the DestroyAfterTime script with a 2-second delay
                                if (bedPrefabParent != null)
                                {
                                    DestroyAfterTime destroyAfterTime = bedPrefabParent.GetComponent<DestroyAfterTime>();

                                    if (destroyAfterTime != null)
                                    {
                                        Debug.Log("Found script");

                                        // Increment the DeathCounter variable only once
                                        playerController.DeathCounter++;
                                        incrementedDeath = true;  // Set the guard to true

                                        // Start the coroutine to call DestroyObject() after 2 seconds
                                        StartCoroutine(CallDestroyWithDelay(destroyAfterTime));
                                    }
                                    else
                                    {
                                        Debug.Log("Could not find script");
                                    }
                                }
                                else
                                {
                                    Debug.Log("Could not find bedPrefabParent");
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("PlayerController component not found on player GameObject.");
                        }
                    }
                    else
                    {
                        Debug.Log("Player GameObject not found.");
                    }
                }
                else
                {
                    Debug.Log("No TextMesh component found on 'shockNeedText' object.");
                }
            }
            else
            {
                Debug.Log("No child with tag 'shockNeedText' found under 'PatientModel'.");
            }
        }
    }

    // Coroutine to call Perfect() with a 2-second delay
    IEnumerator CallPerfectWithDelay(DestroyAfterTime destroyAfterTime)
    {
        yield return new WaitForSeconds(2f);
        destroyAfterTime.Perfect();
        Debug.Log("Called Perfect() on BedPrefab after 2 seconds.");
    }

    // Coroutine to call DestroyObject() with a 2-second delay
    IEnumerator CallDestroyWithDelay(DestroyAfterTime destroyAfterTime)
    {
        yield return new WaitForSeconds(2f);
        destroyAfterTime.DestroyObject();
        Debug.Log("Called DestroyObject() on BedPrefab after 2 seconds.");
    }

    // Function to convert a Color to a hexadecimal string
    string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        int a = Mathf.RoundToInt(color.a * 255);

        return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    // Function to find the highest parent or stop at a specific parent name
    Transform FindHighestParent(Transform childTransform, string stopAtName)
    {
        while (childTransform.parent != null)
        {
            childTransform = childTransform.parent;

            if (childTransform.name == stopAtName)
            {
                Debug.Log($"Found '{stopAtName}' in hierarchy.");
                return childTransform;
            }
        }

        return childTransform;
    }

    // Function to find a child with a specific tag under a parent
    Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
        }

        return null;
    }
}
