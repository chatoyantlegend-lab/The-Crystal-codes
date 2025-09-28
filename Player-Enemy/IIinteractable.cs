using UnityEngine;

public interface IInteractable
{
    // Called when the player presses the Interact key
    void Interact(UnityEngine.GameObject player);

    // Called to get the text for the UI prompt
    string GetInteractText();
}

