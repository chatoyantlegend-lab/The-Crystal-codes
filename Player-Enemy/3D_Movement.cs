using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{

    [Header("Player Stats")]

    private Rigidbody rb;
    [SerializeField] private AttributesManager am;
    public float MoveSpeed = 4;
    public int gemCount;
    public TMP_Text text;
    public Ability activeAbility; // Single ability slot

    [Header("Footstep Sounds")]

    private Animator anim;

    public float stepInterval = 0.2f;
    public float stepTimer;
    public AudioClip[] footstepSounds;
    public AudioSource audioSource;

    private PlayerControls controls;
    private Vector2 moveInput;

     void Awake()
    {
        controls = new PlayerControls();

        // Movement input
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Ability input
        controls.Player.Ability.performed += ctx =>
        {
            if (activeAbility != null)
                activeAbility.TryActivate(gameObject);
        };
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Ability.performed += OnAbility;
    }
    void OnDisable()
    {
        controls.Disable();
        controls.Player.Ability.performed -= OnAbility;
    }

    private void OnAbility(InputAction.CallbackContext ctx)
    {
        if (activeAbility != null)
        {
            Debug.Log("Ability Used");
            activeAbility.TryActivate(gameObject);
        }
    }


    public bool SpendGem(int amount)
    {
        if (gemCount >= amount)
        {
            gemCount -= amount;
            Debug.Log("Bought Item for " + amount + " blood. Remaining: " + gemCount + " Blood");
            return true;
        }
        else
        {
            Debug.Log("Not Enough Blood.");
            return false;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        text.text = gemCount.ToString();
        anim = GetComponent<Animator>();
       
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;   

    }

    void Update()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        rb.linearVelocity = inputDir * MoveSpeed;

        bool isMoving = inputDir.magnitude > 0.1f;
        anim.SetBool("isWalking", isMoving);

        // footsteps
        if (isMoving)
        {
            
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f && footstepSounds.Length > 0)
            {
                AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                audioSource.PlayOneShot(clip);
                stepTimer = stepInterval;
            }
        }
        else { stepTimer = 0f; }

        // Rotate towards move direction
        if (inputDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        text.text = gemCount.ToString();
    }

    void OnTriggerEnter(Collider other)  // Items and Currency
    {
        if (other.CompareTag("Gem"))
        {
            gemCount++;
            PlayerPrefs.SetInt("gemAmount", gemCount);
            Destroy(other.gameObject);
            text.text = gemCount.ToString();
        }

        // You can add pickups like Boots/Shield here later
    }

    public void UnlockAbility<T>(GameObject prefab = null, GameObject vfxPrefab = null, float aoeRadius = 0f, int damage = 0)
    where T : Ability
    {
        if (activeAbility != null)
        {
            Destroy(activeAbility);
        }

        activeAbility = gameObject.AddComponent<T>();

        // Bomb-specific setup
        if (activeAbility is BombAbility ba)
        {
            ba.bombPrefab = prefab;
            ba.explosionVFXPrefab = vfxPrefab;
            ba.aoeRadius = aoeRadius;
            ba.bombDamage = damage;
        }
        // Kunai-specific setup
        else if (activeAbility is KunaiAbility ka)
        {
            ka.kunaiPrefab = prefab;
            //ka.kunaiDamage = damage;
            ka.cooldown = 3f;
        }
        // Dash-specific setup
        else if (activeAbility is DashAbility da)
        {
            da.dashForce = 150f; // example
            da.vfxDuration = 0.5f;     // example
            da.dashVFXPrefab = vfxPrefab;
        }

        Debug.Log("New Ability acquired: " + activeAbility.abilityName);
    }

    


}
