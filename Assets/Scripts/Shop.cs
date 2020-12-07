using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;
    [SerializeField]
    GameObject itemsView;
    [SerializeField]
    ShopItem[] shopItems;
    [SerializeField]
    Text coins;
    [SerializeField]
    BuyConfirmationDialog buyConfirmationDialog;

    public static Shop Instance { get; private set; } = null;
    Settings settings;
    Button[] buttons;
    string dataPath;

    private void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

        dataPath = Application.persistentDataPath + "\\shop.dat";

        settings = Settings.Instance;
        UpdateCoinText();

        ReadBoughtItems();

        int idx = 0;
        buttons = new Button[shopItems.Length];
        foreach (ShopItem item in shopItems)
        {
            GameObject instantiated = Instantiate(itemPrefab, itemsView.transform);
            instantiated.transform.Find("Title").GetComponent<Text>().text = item.name;
            instantiated.transform.Find("Description").GetComponent<Text>().text = item.description;
            instantiated.transform.Find("Coins").GetComponent<Text>().text = Utils.IntToString(item.cost);
            instantiated.transform.Find("Icon").GetComponent<Image>().sprite = item.icon;

            buttons[idx] = instantiated.GetComponent<Button>();

            ++idx;
        }

        UpdateItemButtons();
    }

    private void ReadBoughtItems()
    {
        try
        {
            StreamReader reader = new StreamReader(dataPath);
            foreach (ShopItem item in shopItems)
                item.bought = Convert.ToBoolean(reader.ReadLine());

            reader.Close();
            reader.Dispose();
        }
        catch (FileNotFoundException)
        {
        }
    }

    private void UpdateItemButtons()
    {
        for (int i = 0; i < buttons.Length; ++i)
        {
            if (shopItems[i].cost > settings.Coins || shopItems[i].bought)
                buttons[i].interactable = false;
            else
                buttons[i].onClick.AddListener(MakeShowBuyConfirmationCaller(i));
        }
    }

    UnityAction MakeShowBuyConfirmationCaller(int i)
    {
        return () => ShowBuyConfirmation(i);
    }

    private void UpdateCoinText()
    {
        coins.text = Utils.IntToString(settings.Coins);
    }

    public void BuyItem(int itemIdx)
    {
        settings.Coins -= shopItems[itemIdx].cost;
        shopItems[itemIdx].bought = true;
        settings.Save();
        WriteBoughtItems();

        GameManager manager = GameManager.Get();
        if (manager != null)
            manager.Coins = settings.Coins;

        UpdateCoinText();
        UpdateItemButtons();
    }

    private void WriteBoughtItems()
    {
        StreamWriter writer = new StreamWriter(dataPath, false);
        foreach (ShopItem item in shopItems)
            writer.WriteLine(item.bought);

        writer.Close();
        writer.Dispose();
    }

    void ShowBuyConfirmation(int itemIdx)
    {
        buyConfirmationDialog.SetItem(shopItems[itemIdx], itemIdx);
        buyConfirmationDialog.ShowDialog();
    }

    [System.Serializable]
    public class ShopItem
    {
        public string name, description;
        public int cost;
        public Sprite icon;

        internal bool bought = false;
    }
}
