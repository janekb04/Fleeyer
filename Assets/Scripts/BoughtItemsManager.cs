using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoughtItemsManager : MonoBehaviour
{
    public static BoughtItemsManager Instance { get; private set; } = null;

    private void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;
    }
}