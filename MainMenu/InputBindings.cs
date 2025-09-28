using System;
using System.Collections.Generic;
using UnityEngine;

public static class InputBindings
{
    private const string PREF_PREFIX = "bing_";
    private static Dictionary<string, KeyCode> bindings = new Dictionary<string, KeyCode>();

    //Call this once at startup
    public static void LoadDefaults()
    {
        // Default bindings here
        SetDefault("Attack", KeyCode.Mouse1);
        SetDefault("Abilities", KeyCode.Space);
        SetDefault("Interact", KeyCode.E);

        // Add more if needed

        // Load saved values

        var keys = new List<string>(bindings.Keys);
        foreach (var action in keys)
        {
            string keyString = PlayerPrefs.GetString(PREF_PREFIX + action, "");
            if (!string.IsNullOrEmpty(keyString))
            {
                if (Enum.TryParse(keyString, out KeyCode k))
                    bindings[action] = k;
            }
        }
    }

    private static void SetDefault(string action, KeyCode key)
    {
        if (!bindings.ContainsKey(action))
            bindings[action] = key;
    }

    public static KeyCode GetKey(string action)
    {
        if (bindings.ContainsKey(action)) return bindings[action];
        return KeyCode.None;
    }

    public static void SetKey(string action, KeyCode key) 
    {
        bindings[action] = key;
        PlayerPrefs.SetString(PREF_PREFIX + action, key.ToString());
    }

    public static bool GetKeyDown(string action)
    { 
        KeyCode k = GetKey(action);
        if (k == KeyCode.None) return false;
        return Input.GetKeyDown(k);
    }
    
}
