using UnityEngine;
using System;
using UnityEngine.UI;
    using TMPro;
using UnityEngine.InputSystem.Interactions;

public class RebindButtons : MonoBehaviour
{
    public string actionName; // e.g. Attack ~ Item ~ Interact
    public Text labelText; // Ui text showing "Attack : M1"
    public Button rebindButton;

    private bool waiting = false;

    private void Start()
    {
        if (labelText != null)
            labelText.text = actionName + ": " + InputBindings.GetKey(actionName);

        rebindButton.onClick.AddListener(StartRebind);
    }

    private void Update()
    {
        if (!waiting) return;

        // check all KeyCodes

        foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(k))
            {
                InputBindings.SetKey(actionName, k);
                if (labelText != null) labelText.text = actionName + ": " + k.ToString();
                waiting = false;
                rebindButton.GetComponentInChildren<Text>().text = "Rebind";
                return;
            }
        }
    }
    public void StartRebind()
    {
        waiting = true;
        if (rebindButton != null) { rebindButton.GetComponentInChildren<Text>().text = "Press any key..."; }

    }
}
