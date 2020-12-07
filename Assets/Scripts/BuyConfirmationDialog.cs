using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyConfirmationDialog : MonoBehaviour
{
    [SerializeField]
    GameObject GFX;
    [SerializeField]
    Text title, description;
    [SerializeField]
    Button buy;

    public void SetItem(Shop.ShopItem item, int idx)
    {
        title.text = "Buy " + item.name;
        description.text = "Would you like to buy " + item.name + " for " + Utils.IntToString(item.cost) + " coins?";
        buy.onClick.RemoveAllListeners();
        buy.onClick.AddListener(() => Shop.Instance.BuyItem(idx));
    }

    public void ShowDialog()
    {
        GFX.SetActive(true);
    }

    public void HideDialog()
    {
        GFX.SetActive(false);
    }
}
