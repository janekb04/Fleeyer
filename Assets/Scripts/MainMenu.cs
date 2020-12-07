using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Settings settings;

    [SerializeField]
    GameObject resetDialog;
    [SerializeField]
    GameObject settingsDialog;

    [SerializeField]
    InputField nickname;

    static MainMenu instance = null;

    private void Start()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

        HideResetDialog();
        HideSettings();

        if (string.IsNullOrWhiteSpace(settings.Nickname))
            ShowSettings();
    }

    public void ShowResetDialog()
    {
        resetDialog.SetActive(true);
    }

    public void HideResetDialog()
    {
        resetDialog.SetActive(false);
    }

    public void ShowSettings()
    {
        settingsDialog.SetActive(true);
    }

    public void HideSettings()
    {
        settingsDialog.SetActive(false);
    }

    public void UpdateSettings()
    {
        nickname.text = settings.Nickname;
    }
}
