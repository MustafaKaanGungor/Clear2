using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientController : MonoBehaviour
{
    public TextMesh textMesh; // Reference to the legacy 3D Text Mesh component
    public char[] letters = { 'R', 'B', 'G', 'P' }; // Array of possible letters
    public string generatedString; // Public variable to store the generated string

    // Start is called before the first frame update
    void Start()
    {
        // Generate a random 4-letter string with a dot between each letter
        generatedString = GenerateRandomString();

        // Set the text of the Text Mesh component
        textMesh.text = generatedString;
    }

    // Method to generate a random 3-letter string with dots in between
    string GenerateRandomString()
    {
        string result = "";

        for (int i = 0; i < 3; i++)
        {
            // Pick a random letter from the array
            char randomLetter = letters[Random.Range(0, letters.Length)];

            // Append the letter to the result string
            result += randomLetter;

            // Append a dot if it's not the last letter
            if (i < 2)
            {
                result += ".";
            }
        }

        return result;
    }

}
