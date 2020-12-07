using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    DreamloManager.EntryDownloadFinishAwaiter highscoreAwaiter;

    private void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

        Revert();
        FetchHighscore();
    }

    private void FetchHighscore()
    {
        highscoreAwaiter = DreamloManager.Instance.GetEntry(Nickname);
        highscoreAwaiter.OnFinish += HighscoreAwaiter_OnFinish;
    }

    public static Settings Instance { get; private set; } = null;

    private void HighscoreAwaiter_OnFinish(bool manual)
    {
        highscoreAwaiter.OnFinish -= HighscoreAwaiter_OnFinish;
        if (highscoreAwaiter.OK)
            Highscore = highscoreAwaiter.Data[0].firstValue;
        else
            Highscore = 0;
    }

    const string PlayerPrefsHighscoreID = "HIGHSCORE";
    int highscore;
    public int Highscore
    {
        get => highscore;
        set
        {
            highscore = value;
            PlayerPrefs.SetInt(PlayerPrefsHighscoreID, highscore);
        }
    }

    const string PlayerPrefsCoinsID = "COINS";
    int coins;
    public int Coins
    {
        get => coins;
        set
        {
            coins = value;
            PlayerPrefs.SetInt(PlayerPrefsCoinsID, coins);
        }
    }

    const string PlayerPrefsNicknameID = "NICKNAME";
    string nickname;
    public string Nickname
    {
        get => nickname;
        set
        {
            nickname = value;
            PlayerPrefs.SetString(PlayerPrefsNicknameID, nickname);
            FetchHighscore();
        }
    }

    public void SetDefault()
    {
        Highscore = 0;
        Coins = 0;
        Nickname = string.Empty;
    }

    public void Revert()
    {
        highscore = PlayerPrefs.GetInt(PlayerPrefsHighscoreID);
        coins = PlayerPrefs.GetInt(PlayerPrefsCoinsID);
        nickname = PlayerPrefs.GetString(PlayerPrefsNicknameID);
    }

    public void Save()
    {
        PlayerPrefs.Save();
    }
}
