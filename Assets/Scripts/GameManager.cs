using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Settings settings;
    [SerializeField]
    SceneController sceneController;
    [SerializeField]
    GameObject explosion, spaceship;
    [SerializeField]
    float renderDistance, explosionTime;
    [SerializeField]
    Text scoreText, highscoreText;
    [SerializeField]
    Color accentColor;
    [SerializeField]
    int maxBoosters;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    LineRenderer lineRenderer;
    [SerializeField]
    float rockSizeToSpawnBoosterThreshold;
    [SerializeField]
    GameObject booster;
    [SerializeField]
    float boosterSpawnChance;
    [SerializeField]
    UIManager uIManager;
    [SerializeField]
    GameObject ui;
    [SerializeField]
    Text coinsText, newHighscoreText;
    [SerializeField]
    int laserCost, shieldCost, turboCost;
    [SerializeField]
    Color disabledButtonTint;
    [SerializeField]
    Image shieldButtonBackground, lasersButtonBackground, turboButtonBackground;

    public int EnemyLayer { get; private set; }
    public int PlayerLayer { get; private set; }
    public GameObject Spaceship { get => spaceship; }
    public float RenderDistance { get => renderDistance; }
    public int ShieldCost { get => shieldCost; }
    public int LaserCost { get => laserCost; }
    public int TurboCost { get => turboCost; }
    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = Utils.IntToString(score);
            if (score > Highscore)
            {
                Highscore = score;
                scoreText.color = accentColor;
                newHighscoreText.gameObject.SetActive(true);
                newHighscore = true;
            }
        }
    }
    public int Coins
    {
        get => coins;
        set
        {
            coins = value;
            coinsText.text = Utils.IntToString(coins);
            settings.Coins = coins;
        }
    }
    public int Highscore
    {
        get => highscore;
        private set
        {
            highscore = value;
            highscoreText.text = Utils.IntToString(highscore);
            settings.Highscore = highscore;
        }
    }
    public int Boosters
    {
        get => boosters;
        set
        {
            boosters = Mathf.Clamp(value, 0, maxBoosters);
            uIManager.BoosterBarVal = boosters;

            if (boosters >= laserCost)
                lasersButtonBackground.color = Color.white;
            else
                lasersButtonBackground.color = disabledButtonTint;

            if (boosters >= shieldCost)
                shieldButtonBackground.color = Color.white;
            else
                shieldButtonBackground.color = disabledButtonTint;

            if (boosters >= turboCost)
                turboButtonBackground.color = Color.white;
            else
                turboButtonBackground.color = disabledButtonTint;
        }
    }

    public float RockSizeToSpawnBoosterThreshold
    {
        get => rockSizeToSpawnBoosterThreshold;
        private set => rockSizeToSpawnBoosterThreshold = value;
    }
    public GameObject Booster
    {
        get => booster;
        private set => booster = value;
    }
    public float BoosterSpawnChance
    {
        get => boosterSpawnChance;
        private set => boosterSpawnChance = value;
    }
    public UIManager UIManager
    {
        get => uIManager;
        private set => uIManager = value;
    }

    static GameManager instance = null;
    int score = 0, coins = 0, highscore = 0;
    int boosters;
    bool newHighscore = false;

    private void Start()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

        EnemyLayer = LayerMask.NameToLayer("Enemy");
        PlayerLayer = LayerMask.NameToLayer("Player");

        Explosion.explosion = explosion;
        Explosion.explosionTime = explosionTime;

        highscoreText.color = accentColor;

        Boosters = 0;

        ReadSettings();
    }

    private void ReadSettings()
    {
        highscore = settings.Highscore;
        Highscore = highscore;

        coins = settings.Coins;
        Coins = coins;
    }

    public void EndGame()
    {
        Coins += Score;
        if (!string.IsNullOrWhiteSpace(settings.Nickname))
            DreamloManager.Instance.AddEntry(new DreamloManager.Entry
            {
                key = settings.Nickname.Substring(0, Mathf.Min(15, settings.Nickname.Length)),
                firstValue = highscore,
                secondValue = (int)Application.platform
            });
        DisplayMenu();
        SaveGame();
    }

    public void SaveGame()
    {
        settings.Save();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        DisplayMenu();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        HideMenu();
    }

    public void RestartGame()
    {
        sceneController.MainLevel();
    }

    private void DisplayMenu()
    {
        menu.SetActive(true);
    }

    private void HideMenu()
    {
        menu.SetActive(false);
    }

    public static GameManager Get()
    {
        return instance;
    }

    public void DrawLine(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
