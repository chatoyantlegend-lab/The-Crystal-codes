using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private AttributesManager attributesManager;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (slider == null) slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        if (attributesManager == null)
            attributesManager = FindAnyObjectByType<AttributesManager>();

        if (attributesManager == null)
        {
            Debug.LogError("HealthBar : no attributesManager Assigned or found");
                return;
        }

        // initialize health slider
        slider.maxValue = attributesManager.maxHealth;
        slider.value = attributesManager.health;

        // subscribe
        attributesManager.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        if (attributesManager != null)
            attributesManager.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
