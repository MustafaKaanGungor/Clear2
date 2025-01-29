using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerController : UnitStats
{
    public GameObject validGroundPathPrefab, rectifiedGroundPathPrefab; // Prefabs for ground path markers
    public AudioClip validGroundPathAudio, rectifiedGroundPathAudio; // Audio clips for ground path markers
    public float groundMarkerDuration = 2f;
    public Vector3 markerPositionOffset = new Vector3(0, 0.1f, 0); // Offset for ground path markers
    public float textVisibleDuration = 1f; // Time for which the text is visible
    public TextMeshProUGUI debugText; // Reference to the TextMeshPro object

    public string[] allowableTags; // Public array for allowable tags

    private Camera mainCamera;
    private AudioSource cameraAudio;
    private NavMeshAgent targetAgent; // The target NavMeshAgent
    private float attackCooldownTimer = 0f;

    public float launchSpeed = 1000f; // Speed at which the sphere will be launched
    public GameObject spherePrefab; // Prefab for the sphere

    //colors
    
    //colors
    [SerializeField] private int selectedImage = 0;
    public RawImage rawImage1; // Reference to the RawImage
    public Color[] rawImage1Colors; // Array of colors
    private int rawImage1ColorIndex = 0; // Index to track the current color

    public RawImage rawImage2; // Reference to the RawImage
    public Color[] rawImage2Colors; // Array of colors
    private int rawImage2ColorIndex = 0; // Index to track the current color

    public RawImage rawImage3; // Reference to the RawImage
    public Color[] rawImage3Colors; // Array of colors
    private int rawImage3ColorIndex = 0; // Index to track the current color
    
    public RawImage rawImage4; // Reference to the RawImage
    public Color[] rawImage4Colors; // Array of colors
    private int rawImage4ColorIndex = 0; // Index to track the current color

    [SerializeField] GameObject imageSelect1;
    [SerializeField] GameObject imageSelect2;
    [SerializeField] GameObject imageSelect3;

    
    public int DeathCounter = 0;
    public int PerfectCounter = 0;
    public Text deathTextMeshPro;  // Reference to the TextMeshProUGUI component
    public Text perfectTextMeshPro;  // Reference to the TextMeshProUGUI component
    public Text livesSaved;  // Reference to the TextMeshProUGUI component
    public GameObject gameOverScreen;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private GameManagerJam gameManager;
    [SerializeField] private AudioManager audioManager;
    private bool isGameOver = false;


    void Start()
    {
        base.Start();
        Debug.Log("DerivedClass Start");

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera is missing!");
        }

        cameraAudio = mainCamera.GetComponent<AudioSource>();
        if (cameraAudio == null)
        {
            cameraAudio = mainCamera.gameObject.AddComponent<AudioSource>();
        }
    }

    public void ResetStats() {
        DeathCounter = 0;
        PerfectCounter = 0;
    }

    void Update()
    {

        if(DeathCounter >9)
        {

            uIManager.GameOver();
            gameManager.CleanGameArea();
            
            
        }
        deathTextMeshPro.text = DeathCounter.ToString();
        livesSaved.text = PerfectCounter.ToString();

        if (Input.GetMouseButtonDown(1) && currentUnitState != UnitState.Dead) // Right mouse button
        {
            Debug.Log("Right mouse button clicked");
            MoveAgentToClick();
        }

        if (currentUnitState == UnitState.Attacking && targetAgent != null)
        {
            RotateTowardsTarget(targetAgent.transform.position);

            // Check if the target has UnitStats and call AttackTarget if valid
            UnitStats targetStats = GetTopMostUnitStats(targetAgent.gameObject);
            if (targetStats != null)
            {
                AttackTarget(targetStats);
            }
        }

        if (targetAgent != null && currentUnitState == UnitState.Moving)
        {
            agent.SetDestination(targetAgent.transform.position);
        }

        if (attackCooldownTimer > 0f)
        {
            attackCooldownTimer -= Time.deltaTime;
        }

        PlayerSelectColors();

        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            // Ensure the array has colors
            if (rawImage1Colors.Length > 0)
            {
                // Cycle to the next color
                rawImage1ColorIndex = (rawImage1ColorIndex + 1) % rawImage1Colors.Length;

                // Get the next color from the array
                Color nextColor = rawImage1Colors[rawImage1ColorIndex];

                // Force the alpha to a fixed value
                nextColor.a = 1;

                // Apply the color to rawImage1
                rawImage1.color = nextColor;
            }
        }*/
        /*if (Input.GetKeyDown(KeyCode.W))
        {
            // Ensure the array has colors
            if (rawImage2Colors.Length > 0)
            {
                // Cycle to the next color
                rawImage2ColorIndex = (rawImage2ColorIndex + 1) % rawImage2Colors.Length;

                // Get the next color from the array
                Color nextColor = rawImage2Colors[rawImage2ColorIndex];

                // Force the alpha to a fixed value
                nextColor.a = 1;

                // Apply the color to rawImage2
                rawImage2.color = nextColor;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Ensure the array has colors
            if (rawImage3Colors.Length > 0)
            {
                // Cycle to the next color
                rawImage3ColorIndex = (rawImage3ColorIndex + 1) % rawImage3Colors.Length;

                // Get the next color from the array
                Color nextColor = rawImage3Colors[rawImage3ColorIndex];

                // Force the alpha to a fixed value
                nextColor.a = 1;

                // Apply the color to rawImage3
                rawImage3.color = nextColor;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Ensure the array has colors
            if (rawImage4Colors.Length > 0)
            {
                // Cycle to the next color
                rawImage4ColorIndex = (rawImage4ColorIndex + 1) % rawImage4Colors.Length;

                // Get the next color from the array
                Color nextColor = rawImage4Colors[rawImage4ColorIndex];

                // Force the alpha to a fixed value
                nextColor.a = 1;

                // Apply the color to rawImage4
                rawImage4.color = nextColor;
            }
        }*/

        // Check if the "P" key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Find the game object tagged as "Patient"
            GameObject patient = GameObject.FindWithTag("Patient");

            if (patient != null)
            {
                // Launch the sphere at the patient
                LaunchSphere(patient);
            }
            else
            {
                Debug.LogWarning("No GameObject found with tag 'Patient'.");
            }
        }
    }

    private void PlayerSelectColors() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            Color nextColor = rawImage1Colors[0];
            nextColor.a = 1;
            switch (selectedImage)
            {
                case 0:
                rawImage1.color = nextColor;
                selectedImage = 1;
                imageSelect1.SetActive(false);
                imageSelect2.SetActive(true);
                break;
                case 1:
                rawImage2.color = nextColor;
                selectedImage = 2;
                imageSelect2.SetActive(false);
                imageSelect3.SetActive(true);
                break;
                case 2:
                rawImage3.color = nextColor;
                selectedImage = 0;
                imageSelect3.SetActive(false);
                imageSelect1.SetActive(true);
                break;
            }
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            Color nextColor = rawImage1Colors[1];
            nextColor.a = 1;
            switch (selectedImage)
            {
                case 0:
                rawImage1.color = nextColor;
                selectedImage = 1;
                imageSelect1.SetActive(false);
                imageSelect2.SetActive(true);
                break;
                case 1:
                rawImage2.color = nextColor;
                selectedImage = 2;
                imageSelect2.SetActive(false);
                imageSelect3.SetActive(true);
                break;
                case 2:
                rawImage3.color = nextColor;
                selectedImage = 0;
                imageSelect3.SetActive(false);
                imageSelect1.SetActive(true);
                break;
            }
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            Color nextColor = rawImage1Colors[2];
            nextColor.a = 1;
            switch (selectedImage)
            {
                case 0:
                rawImage1.color = nextColor;
                selectedImage = 1;
                imageSelect1.SetActive(false);
                imageSelect2.SetActive(true);
                break;
                case 1:
                rawImage2.color = nextColor;
                selectedImage = 2;
                imageSelect2.SetActive(false);
                imageSelect3.SetActive(true);
                break;
                case 2:
                rawImage3.color = nextColor;
                selectedImage = 0;
                imageSelect3.SetActive(false);
                imageSelect1.SetActive(true);
                break;
            }
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            Color nextColor = rawImage1Colors[3];
            nextColor.a = 1;
            switch (selectedImage)
            {
                case 0:
                rawImage1.color = nextColor;
                selectedImage = 1;
                imageSelect1.SetActive(false);
                imageSelect2.SetActive(true);
                break;
                case 1:
                rawImage2.color = nextColor;
                selectedImage = 2;
                imageSelect2.SetActive(false);
                imageSelect3.SetActive(true);
                break;
                case 2:
                rawImage3.color = nextColor;
                selectedImage = 0;
                imageSelect3.SetActive(false);
                imageSelect1.SetActive(true);
                break;
            }
        }
        
    }

    void LaunchSphere(GameObject target)
    {
        // Set the launch position to (0, 100, 0)
        Vector3 launchPosition = new Vector3(0, 100, 0);

        // Instantiate a new sphere at the launch position
        GameObject sphere = Instantiate(spherePrefab, launchPosition, Quaternion.identity);
        //audioManager.PlaySound(audioManager.shock);
        // Ensure the sphere has a Rigidbody component
        Rigidbody rb = sphere.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = sphere.AddComponent<Rigidbody>();
        }

        // Calculate the direction from the launch position to the target's position
        Vector3 direction = (target.transform.position - launchPosition).normalized;

        // Launch the sphere towards the target with high speed
        rb.AddForce(direction * launchSpeed, ForceMode.Impulse);

        Debug.Log("Sphere launched at: " + target.name + " at position: " + target.transform.position);
    }

    void MoveAgentToClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Reset the targetAgent and targetRigidbody
            targetAgent = hit.collider.GetComponent<NavMeshAgent>();
            Rigidbody targetRigidbody = GetTopMostRigidbody(hit.collider.gameObject); // Find top-most Rigidbody

            if (targetAgent != null && targetAgent.gameObject != this.gameObject)
            {
                // Handle NavMeshAgent target
                HandleTarget(targetAgent.gameObject, targetAgent.transform.position);
            }
            else if (targetRigidbody != null && targetRigidbody.gameObject != this.gameObject)
            {
                // Check if the Rigidbody target has an allowed tag
                if (IsTagAllowed(targetRigidbody.tag))
                {
                    HandleTarget(targetRigidbody.gameObject, targetRigidbody.transform.position);
                }
                else
                {
                    // Add detailed information to the debug message
                    Debug.Log("Top-most Rigidbody does not have an allowable tag: " + targetRigidbody.tag 
                            + "\nGameObject Name: " + targetRigidbody.gameObject.name 
                            + "\nPosition: " + targetRigidbody.gameObject.transform.position 
                            + "\nLayer: " + LayerMask.LayerToName(targetRigidbody.gameObject.layer)
                            + "\nActive in Hierarchy: " + targetRigidbody.gameObject.activeInHierarchy
                            + "\nComponents attached: " + GetComponentNames(targetRigidbody.gameObject));
                }
            }
            else
            {
                // Handle movement to a ground point when no target is found
                agent.isStopped = false;
                agent.SetDestination(hit.point);
                SpawnGroundPathMarker(hit.point, true);
                PlayGroundPathAudio(true);
                SetUnitState(UnitState.Moving);
                StartCoroutine(CheckIfDestinationReached(null)); // No target for attacking
            }
        }
    }

    string GetComponentNames(GameObject obj)
    {
        // Get all components attached to the GameObject
        Component[] components = obj.GetComponents<Component>();
        string componentNames = "";

        // Loop through each component and add its type name to the string
        foreach (Component component in components)
        {
            componentNames += component.GetType().Name + ", ";
        }

        // Remove the last comma and space
        if (componentNames.Length > 2)
        {
            componentNames = componentNames.Substring(0, componentNames.Length - 2);
        }

        return componentNames;
    }

    Rigidbody GetTopMostRigidbody(GameObject obj)
    {
        // Traverse up the hierarchy until we find a Rigidbody or reach the root
        Transform currentTransform = obj.transform;

        while (currentTransform != null)
        {
            Rigidbody rb = currentTransform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                return rb; // Found a Rigidbody
            }

            currentTransform = currentTransform.parent; // Move up to the parent object
        }

        return null; // No Rigidbody found in the hierarchy
    }

    UnitStats GetTopMostUnitStats(GameObject obj)
    {
        // Traverse up the hierarchy until we find a UnitStats component or reach the root
        Transform currentTransform = obj.transform;

        while (currentTransform != null)
        {
            UnitStats stats = currentTransform.GetComponent<UnitStats>();
            if (stats != null)
            {
                return stats; // Found UnitStats
            }

            currentTransform = currentTransform.parent; // Move up to the parent object
        }

        return null; // No UnitStats found in the hierarchy
    }

    void HandleTarget(GameObject targetObject, Vector3 targetPosition)
    {
        Debug.Log("Target found: " + targetObject.name);

        // Update the destination to the target's position
        agent.isStopped = false;
        agent.SetDestination(targetPosition);
        SpawnGroundPathMarker(targetPosition, true);
        PlayGroundPathAudio(true);
        SetUnitState(UnitState.Moving);

        // Start the CheckIfDestinationReached coroutine with the target object
        StartCoroutine(CheckIfDestinationReached(targetObject));
    }

    bool IsTagAllowed(string tag)
    {
        // Check if the provided tag is in the list of allowable tags
        foreach (string allowableTag in allowableTags)
        {
            if (tag == allowableTag)
            {
                return true;
            }
        }
        return false;
    }

    void SpawnGroundPathMarker(Vector3 point, bool validClick)
    {
        GameObject prefab = validClick ? validGroundPathPrefab : rectifiedGroundPathPrefab;
        if (prefab == null) return;
        GameObject marker = Instantiate(prefab, point + markerPositionOffset, prefab.transform.rotation);
        Destroy(marker, groundMarkerDuration);
    }

    void PlayGroundPathAudio(bool validClick)
    {
        AudioClip audio = validClick ? validGroundPathAudio : rectifiedGroundPathAudio;
        if (audio == null) return;
        cameraAudio.PlayOneShot(audio);
    }

    void SpawnTextAbovePlayer(string text)
    {
        GameObject textObject = new GameObject("TextMesh");
        textObject.transform.position = agent.transform.position + Vector3.up * 2; // Adjust position above player

        TextMesh textMesh = textObject.AddComponent<TextMesh>();
        textMesh.text = text;
        textMesh.fontSize = 24;
        textMesh.color = Color.black;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;

        StartCoroutine(FaceCamera(textObject));

        Destroy(textObject, textVisibleDuration); // Destroy text after the specified duration
    }

    IEnumerator FaceCamera(GameObject textObject)
    {
        while (textObject != null)
        {
            textObject.transform.LookAt(mainCamera.transform);
            textObject.transform.Rotate(0, 180, 0); // Rotate to face the camera properly
            yield return null;
        }
    }

    private IEnumerator CheckIfDestinationReached(GameObject targetObject)
    {
        yield return new WaitForSeconds(0.1f); // Small delay to ensure the agent starts moving

        while (currentUnitState == UnitState.Moving || currentUnitState == UnitState.ReturningHome)
        {
            if (targetObject != null)
            {
                // Calculate the distance to the target
                float distance = Vector3.Distance(transform.position, targetObject.transform.position);

                // If within attack range, stop the agent and switch to attack
                if (distance <= attackRange)
                {
                    agent.isStopped = true;
                    RotateTowardsTarget(targetObject.transform.position);
                    Debug.Log("Within attack range. Attacking: " + targetObject.name);

                    SetUnitState(UnitState.Attacking);

                    // Perform attack logic here (e.g., applying damage, animations)
                    UnitStats targetStats = GetTopMostUnitStats(targetObject);
                    if (targetStats != null)
                    {
                        AttackTarget(targetStats);
                    }
                    else
                    {
                        Debug.Log("No Target Stats");
                    }

                    yield break;
                }

                // Continue moving towards the target
                agent.SetDestination(targetObject.transform.position);
            }

            // Check if the destination (target or point) is reached
            if (IsDestinationReached())
            {
                Debug.Log("Destination reached.");
                ResetAgentActions();
            }

            yield return null;
        }
    }

    private bool IsDestinationReached()
    {
        bool reached = !agent.hasPath || agent.remainingDistance <= (agent.stoppingDistance + 0.25f);
        if (reached)
        {
            Debug.Log("Destination reached.");
        }
        return reached;
    }

    private void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Check if the direction vector is not zero
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
        else
        {
            Debug.LogWarning("RotateTowardsTarget: Attempted to look at a zero direction vector.");
        }
    }

    void AttackTarget(UnitStats targetStats)
    {
        Debug.Log("Attacking Target");
        if (attackCooldownTimer <= 0f)
        {
            Debug.Log("CoolDown Good");
            attackCooldownTimer = 1f / attackSpeed; // Reset attack cooldown
            SetUnitState(UnitState.Attacking);

            // Apply damage to the target
            targetStats.TakeDamage(GetRandomAttackDamage(), DamageType.Physical);

            // Search for a GameObject with tag "Head" within the attacked GameObject's hierarchy
            GameObject head = FindHeadInHierarchy(targetStats.gameObject);

            if (head != null)
            {
                Debug.Log("Found Target Head");
                // Launch the sphere at the "Head" GameObject
                LaunchSphere(head);
                audioManager.PlaySound(audioManager.shock);
                Debug.Log("Launching sphere at Head inside: " + targetStats.gameObject.name);
            }
            else
            {
                Debug.LogWarning("No Head found in the hierarchy of the attacked object.");
            }
        }
    }


    GameObject FindPatientInHierarchy(GameObject targetObject)
    {
        // Search the target object's children for a GameObject with the tag "Patient"
        Transform[] children = targetObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.CompareTag("Patient"))
            {
                return child.gameObject; // Return the first "Patient" found
            }
        }

        return null; // No "Patient" found in the hierarchy
    }

    GameObject FindHeadInHierarchy(GameObject targetObject)
    {
        // Search the target object's children for a GameObject with the tag "Head"
        Transform[] children = targetObject.GetComponentsInChildren<Transform>(true); // Include inactive children

        Debug.Log("Searching for 'Head' tag in the hierarchy of: " + targetObject.name);
        Debug.Log("Number of children found: " + children.Length);

        // Check in children and grandchildren (all descendants)
        foreach (Transform child in children)
        {
            Debug.Log("Checking child: " + child.name + " with tag: " + child.tag);

            if (child.CompareTag("shockNeedText"))
            {
                Debug.Log("Found 'Head' tagged object: " + child.name);
                return child.gameObject; // Return the first "Head" found
            }
        }

        // If not found in children, check in the parent hierarchy
        Transform currentTransform = targetObject.transform;

        while (currentTransform != null)
        {
            Debug.Log("Checking parent: " + currentTransform.name + " with tag: " + currentTransform.tag);

            if (currentTransform.CompareTag("shockNeedText"))
            {
                Debug.Log("Found 'Head' tagged object in parent: " + currentTransform.name);
                return currentTransform.gameObject; // Return the first "Head" found in the parent hierarchy
            }

            currentTransform = currentTransform.parent; // Move up to the parent object
        }

        Debug.LogWarning("No 'Head' tag found in the hierarchy of: " + targetObject.name);
        return null; // No "Head" found in the hierarchy
    }





}
