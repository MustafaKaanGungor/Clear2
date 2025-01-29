using System.Collections;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float floatSpeed = 2f; // Speed at which the text floats up
    public float fadeDuration = 1f; // Duration for the text to fade out

    public TMP_Text m_TextComponent;

    private void Start()
    {
        if (m_TextComponent == null)
        {
            Debug.LogError("TMP_Text component not found!");
        }
    }

    public void Initialize(float damage)
    {
        if (m_TextComponent != null)
        {
            int roundedDamage = Mathf.RoundToInt(damage); // Round the damage to the nearest whole number
            m_TextComponent.text = roundedDamage.ToString();
            StartCoroutine(AnimateDamageText());
        }
        else
        {
            Debug.LogError("TMP_Text component is not assigned.");
        }
    }

    private IEnumerator AnimateDamageText()
    {
        float elapsed = 0.0f;
        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = initialPosition + Vector3.up; // Move up on the Y axis

        while (elapsed < fadeDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsed / fadeDuration);
            Color color = m_TextComponent.color;
            color.a = Mathf.Lerp(1, 0, elapsed / fadeDuration); // Fade out
            m_TextComponent.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
