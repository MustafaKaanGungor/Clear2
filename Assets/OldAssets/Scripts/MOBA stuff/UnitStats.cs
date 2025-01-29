using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;


public enum DamageType
{
    Physical,
    Magical,
    True
}

public enum AttackType
{
    Melee,
    Ranged
}

public enum BasicSkillLvls
{
    ZERO,
    ONE,
    TWO,
    THREE,
    FOUR
}

public enum UltimateSkillLvls
{
    ZERO,
    ONE,
    TWO,
    THREE
}

// States
public enum UnitState
{
    Idle = 0,
    Moving = 1,
    Attacking = 2,
    ReturningHome = 3,
    Dead = 4
}

public class UnitStats : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public Image hpImage; // Reference to the HP image
    public NavMeshAgent agent;
    public Animator anim; // Reference to Animator component
    public Transform homeBase; // Home base position

    //anim target
    public Transform targetTransform; // The transform to scale
    public float endScale = 1f; // The maximum scale for x and z
    public float animationDuration = 0.2f; // Duration of the scaling animation

    // Health and Mana
    [Header("Health and Mana")]
    public float maxHealth;
    public float maxMana;
    public float currentHealth;
    public float currentMana;

    // General stats
    [Header("General Stats")]
    public string unitName; // Name of the unit
    public int moneyRaised; // Money raised by the unit
    public int killCount;
    public int deathCount;
    public int assistCount;
    public int lastHitCount;
    public int denyCount;
    public int killStreakCount;
    public int expBounty;
    public int goldBounty;

    // Skills
    [Header("SkillPoints")]
    public BasicSkillLvls skill_1_lvl;
    public BasicSkillLvls skill_2_lvl;
    public BasicSkillLvls skill_3_lvl;
    public UltimateSkillLvls ulti_lvl;

    // Attack stats
    [Header("Attack Stats")]
    public float minAttackDamage; // Minimum attack damage
    public float maxAttackDamage; // Maximum attack damage
    public float attackSpeed; // Attacks per second
    public float visionRange; 
    public float attackRange; // Base attack range of the unit
    public float projectileSpeed;
    public AttackType attackType; // Attack type: Melee or Ranged
    public float moveSpeed;
    public float spellAmp;
    public float manaRegen;
    public float criticalChance; // Chance to land a critical hit (0 to 1)
    public float criticalMultiplier; // Damage multiplier for critical hits

    // Defense stats
    [Header("Defense Stats")]
    public float armor;
    public float physicalResistance;
    public float magicResistance;
    public float statusResistance;
    public float slowResist;
    public float evasion;
    public float healthRegen;

    // Attributes
    [Header("Attributes")]
    public float strength;
    public float agility;
    public float intelligence;

    // Experience
    [Header("Experience")]
    public int experiencePoints;

    public UnitState currentUnitState;
    private Coroutine currentCoroutine; // Reference to the current running coroutine


    protected virtual void Start()
    {


        currentHealth=maxHealth;
        currentMana=maxMana;

        UpdateHealthUI();

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.Log("NavMeshAgent component is missing!");
        }

        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.Log("Animator component is missing!");
        }else
        {
            SetUnitState(UnitState.Idle);
            currentUnitState = UnitState.Idle;
        }

        if (hpImage == null)
        {
            Debug.Log("HP Image component is not assigned!");
        }
    }

    // Methods to modify stats
    public void TakeDamage(float damage, DamageType damageType)
    {
        float finalDamage = CalculateDamage(damage, damageType);
        //Debug.Log("UnitStat:TakeDamage"+damage+" "+finalDamage);
        currentHealth -= finalDamage;
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            Die();
        }
        ShowDamageText(finalDamage);
    }

    // Method to update the health UI
    protected void UpdateHealthUI()
    {
        //Debug.Log("Updating HealthUi");
        if (hpImage != null)
        {
            hpImage.fillAmount = currentHealth / maxHealth;
            //Debug.Log($"Updated HP bar: fill amount = {hpImage.fillAmount}");
        }
        else
        {
            Debug.LogError("HP Image is not assigned.");
        }
    }

    protected float CalculateDamage(float damage, DamageType damageType)
    {
        float reducedDamage = damage;
        switch (damageType)
        {
            case DamageType.Physical:
                // Debug.Log("CalculateDamage");
                // Debug.Log(damage);
                // Debug.Log(armor);
                reducedDamage -= armor;
                reducedDamage *= (1 - physicalResistance);
                break;
            case DamageType.Magical:
                reducedDamage *= (1 - magicResistance);
                reducedDamage *= (1 + spellAmp);
                break;
            case DamageType.True:
                // True damage is not reduced
                break;
        }

        // Critical hit calculation
        if (Random.value <= criticalChance)
        {
            reducedDamage *= criticalMultiplier;
            Debug.Log("Critical Hit!");
        }

        // Randomize damage dealt
        reducedDamage *= Random.Range(0.8f, 1.2f);

        // Ensure damage is not negative
        return Mathf.Max(reducedDamage, 0);
    }

    private void ShowDamageText(float damage)
    {
        // GameObject damageTextObject = Instantiate(damageTextPrefab, transform.position + Vector3.up, Quaternion.identity);
        // DamageText damageText = damageTextObject.GetComponent<DamageText>();
        // damageText.Initialize(damage);
    }

    // Method to get random attack damage within range
    protected float GetRandomAttackDamage()
    {
        return Random.Range(minAttackDamage, maxAttackDamage);
    }

    // Method to add money
    public void AddMoney(int amount)
    {
        moneyRaised += amount;
    }

    // Method to spend money
    public bool SpendMoney(int amount)
    {
        if (moneyRaised >= amount)
        {
            moneyRaised -= amount;
            return true;
        }
        return false;
    }

    // Public method to get the current health
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        // Add death animation or logic here
        Debug.Log("Player has died!");
        SetUnitState(UnitState.Dead);
        gameObject.tag = "Untagged"; // Change the tag to "Untagged"
        agent.isStopped = true; // Stop the NavMeshAgent

        // Remove the CapsuleCollider
        CapsuleCollider collider = GetComponent<CapsuleCollider>();
        if (collider != null)
        {
            Destroy(collider);
        }

        StartCoroutine(DestroyAfterDelay(8f, 1f)); // Sink over 8 seconds, then destroy
    }

    private IEnumerator DestroyAfterDelay(float delay, float sinkRate)
    {
        // Remove all colliders
        Collider[] colliders = GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }

        // Disable the child object named "Utils"
        Transform utilsChild = transform.Find("Utils");
        if (utilsChild != null)
        {
            utilsChild.gameObject.SetActive(false);
        }

        // Wait for 2 seconds before starting to sink
        yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < delay)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y - elapsedTime * sinkRate, startPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }


    public void SetUnitState(UnitState state)
    {
        currentUnitState = state;
        int visualState = state == UnitState.ReturningHome ? (int)UnitState.Moving : (int)state;
        anim.SetInteger("State", visualState);
    }

    public void ResetAgentActions()
    {
        agent.ResetPath();
        SetUnitState(UnitState.Idle);
    }



    // Start the scale animation
    public void PlayScaleAnim(float endScale)
    {
        if (targetTransform != null)
        {
            // Stop the current animation if it is running
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }

            // Reset the scale to the start
            targetTransform.localScale = new Vector3(0, 0, 0);

            // Start a new animation coroutine with the specified end scale
            currentCoroutine = StartCoroutine(ScaleCoroutine(endScale));
        }
        else
        {
            Debug.LogError("Target transform not assigned on " + gameObject.name);
        }
    }

    // Coroutine to handle the scaling animation
    private IEnumerator ScaleCoroutine(float endScale)
    {
        // Scale up
        yield return StartCoroutine(ScaleTo(endScale, animationDuration / 8));
        // Scale down
        yield return StartCoroutine(ScaleTo(0, animationDuration / 8));
    }

    // Coroutine to scale to a specific value over time
    private IEnumerator ScaleTo(float targetScale, float duration)
    {
        Vector3 initialScale = targetTransform.localScale; // Initial scale (current scale)
        Vector3 targetVector = new Vector3(targetScale, targetScale, 0); // Target scale (z always 0)

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            targetTransform.localScale = Vector3.Lerp(initialScale, targetVector, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = targetVector; // Ensure the final scale is set
    }


    // Visualize the detection and attack radii in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


}
