using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemBomb : MonoBehaviour
{
    [SerializeField] private GameObject bombPrefab; // drag the prefab asset here
    [SerializeField] private int cost = 500;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private ShopManager shopManager; // Assign your ShopManager here
    [SerializeField] private Transform playerTransform; // Player reference for bomb spawn

    private bool sold = false;

    private void Awake()
    {
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyBomb);
    }

    private void BuyBomb()
    {
        if (sold) return;
        float chosenRadius = 5f;
        if (shopManager.BuyAbility("Bomb", cost, chosenRadius))
        {
            sold = true;
            buttonText.text = "OUT";
            buyButton.interactable = false;

            // Spawn the bomb at the player’s position
            SpawnBombAtPlayer();

            // Remove the item from UI
            if (bombPrefab != null)
                Destroy(bombPrefab);

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Debug.Log("Not enough currency for " + buttonText.text);
        }
    }

    private void SpawnBombAtPlayer()
    {
        if (bombPrefab == null || playerTransform == null) return;

        // Slightly offset spawn to avoid overlapping player
        Vector3 spawnPos = playerTransform.position + Vector3.up * 0.5f + playerTransform.forward * 0.5f;

        GameObject bombInstance = Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        // Copy all BombController settings from prefab
        BombController template = bombPrefab.GetComponent<BombController>();
        BombController bc = bombInstance.GetComponent<BombController>();
        if (bc != null && template != null)
        {
            bc.InitializeFrom(template);
        }
    }
}

