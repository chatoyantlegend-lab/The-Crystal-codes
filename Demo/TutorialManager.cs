using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InteractiveTutorial : MonoBehaviour
{
    [Header("Slides in Order")]
    public GameObject moveSlide;     // "WASD to Move"
    public GameObject attackSlide;   // "Click to Attack"
    public GameObject interactSlide; // "E to Interact"
    public GameObject finalSlide;    // "Welcome to The Crystal..."

    [Header("Tutorial Objects")]
    public GameObject door;   // Door that unlocks at the end
    public GameObject zombie; // The tutorial zombie
    public HealingFountain fountain; // Fountain to interact with

    private PlayerControls controls;

    private bool hasMoved = false;
    private bool hasAttacked = false;
    private bool zombieKilled = false;
    private bool hasInteracted = false;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += ctx => hasMoved = true;
        controls.Player.Attack.performed += ctx => hasAttacked = true;
        controls.Player.Interact.performed += ctx => hasInteracted = true;
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= ctx => hasMoved = true;
        controls.Player.Attack.performed -= ctx => hasAttacked = true;
        controls.Player.Interact.performed -= ctx => hasInteracted = true;
        controls.Disable();
    }

    private void Start()
    {
        // Hide all slides first
        moveSlide.SetActive(false);
        attackSlide.SetActive(false);
        interactSlide.SetActive(false);
        finalSlide.SetActive(false);

        // Subscribe to zombie death
        if (zombie != null)
        {
            AttributesManager am = zombie.GetComponent<AttributesManager>();
            if (am != null)
                am.OnDeath += () => zombieKilled = true;
        }

        // Start tutorial flow
        StartCoroutine(TutorialFlow());
    }

    private IEnumerator TutorialFlow()
    {
        // Step 1: Show Move
        moveSlide.SetActive(true);
        yield return new WaitUntil(() => hasMoved);
        moveSlide.SetActive(false);

        // Step 2: Show Attack
        attackSlide.SetActive(true);
        yield return new WaitUntil(() => hasAttacked && zombieKilled);
        attackSlide.SetActive(false);

        // Step 3: Show Interact
        interactSlide.SetActive(true);
        yield return new WaitUntil(() => hasInteracted );
        interactSlide.SetActive(false);

        // Step 4: Final Slide
        finalSlide.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        finalSlide.SetActive(false);

        // Unlock door
        if (door != null) door.SetActive(false);

        Debug.Log("Tutorial finished!");
    }
}
