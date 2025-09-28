using UnityEngine;

public class ShopManager : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject gameUICanvas;
    [SerializeField] private PlayerMovement player; // Reference to your player

    [Header("Dash")]
    [SerializeField] private GameObject dashVFXPrefab;


    [Header("Kunai")]
    [SerializeField] private GameObject kunaiPrefab;

    [Header("Bomb")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject bombExplosionVFX;
    [SerializeField] public LayerMask bombDamageLayer;
    [SerializeField] public bool isOpen = false;

    private void Start()
    {
        shopUI.SetActive(false);
    }

    public void Interact(GameObject player)
    {
        if (!isOpen) { OpenShop(); }
    }

    public string GetInteractText()
    {
        return isOpen ? "Press ESC to Close Shop" : "Press E to Trade";
    }
    public bool BuyAbility(string abilityName, int cost)
    {
        return BuyAbility(abilityName, cost, 0f); // forwards to the radius version
    }

    public bool BuyAbility(string abilityName, int cost, float chosenRadius)
    {
        if (!player.SpendGem(cost))
        {
            Debug.Log("Not enough Blood to buy " + abilityName);
            return false;
        }

        switch (abilityName)
        {
            case "Dash":
                player.UnlockAbility<DashAbility>(null, dashVFXPrefab, 0f, 0);
                Debug.Log("Bought Dash Rune for " + cost + " Blood");
                break;

            case "Kunai":
                player.UnlockAbility<KunaiAbility>(kunaiPrefab, null, 0f, 0);
                Debug.Log("Bought Kunai Rune for " + cost + " Blood");
                break;

            case "Bomb":
                player.UnlockAbility<BombAbility>(bombPrefab, bombExplosionVFX, chosenRadius, 40);
                Debug.Log("Bought Bomb Rune for " + cost + " Blood");
                break;

            default:
                Debug.LogWarning("Ability not recognized: " + abilityName);
                return false;
        }

        return true;
    }


    public void OpenShop()
    {
        shopUI.SetActive(true);
        gameUICanvas.SetActive(false);
        isOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        gameUICanvas.SetActive(true);
        isOpen = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (isOpen && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseShop();
        }
    }

    
}
