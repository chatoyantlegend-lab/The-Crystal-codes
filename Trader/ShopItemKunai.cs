using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemKunai : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // drag the prefab instance here
    [SerializeField] private int cost = 500;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private ShopManager shopManager; // Assign your ShopManager here

    private bool sold = false;

    private void Awake()
    {
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyKunai);
    }

    public void BuyKunai()
    {
        if (sold) return; // Already purchased

        if (shopManager.BuyAbility("Kunai", cost, 0f))
        {
            sold = true;
            buttonText.text = "OUT";
            buyButton.interactable = false;

            // Remove the item prefab from hierarchy
            if (itemPrefab != null)
            {
                Destroy(itemPrefab);
            }


            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            Debug.Log("Not enough currency for " + buttonText.text);
        }
    }
}
