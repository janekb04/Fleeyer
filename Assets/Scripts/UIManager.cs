using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    HoldingButton shieldButton, lasersButton, turboButton;
    [SerializeField]
    Slider boosterBar;
    [SerializeField]
    HoldingButton leftButton, rightButton;
    [SerializeField]
    Image shieldButtonImage;

    static UIManager instance = null;
    bool shootButtonPressed = false;
    bool shieldButtonPressed = false;
    bool lasersButtonPressed = false;
    bool turboButtonPressed = false;
    float turn, prevVal = 0;

    public bool ShootButtonPressed
    {
        get
        {
            bool val = shootButtonPressed;
            shootButtonPressed = false;
            return val;
        }
        set => shootButtonPressed = value;
    }
    public bool ShieldButtonPressed
    {
        get
        {
            bool val = shieldButtonPressed;
            shieldButtonPressed = false;
            return val;
        }
        set => shieldButtonPressed = value;
    }
    public bool LasersButtonPressed
    {
        get
        {
            bool val = lasersButtonPressed;
            lasersButtonPressed = false;
            return val;
        }
        set => lasersButtonPressed = value;
    }
    public bool TurboButtonPressed
    {
        get
        {
            bool val = turboButtonPressed;
            turboButtonPressed = false;
            return val;
        }
        set => turboButtonPressed = value;
    }
    public float Turn => Convert.ToInt32(leftButton.Pressed) - Convert.ToInt32(rightButton.Pressed);

    public int BoosterBarVal
    {
        get => (int)boosterBar.value;
        set => boosterBar.value = value;
    }

    public float ShieldIconFill
    {
        get => shieldButtonImage.fillAmount;
        set => shieldButtonImage.fillAmount = value;
    }

    private void Start()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

#if UNITY_STANDALONE
        shieldButton.gameObject.SetActive(false);
        lasersButton.gameObject.SetActive(false);
        turboButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
#elif UNITY_ANDROID
        shieldButton.gameObject.SetActive(true);
        lasersButton.gameObject.SetActive(true);
        turboButton.gameObject.SetActive(true);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
#else
#error Unsupported platform
#endif
        //StartCoroutine(ResetTurn());
    }

    //IEnumerator ResetTurn()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForFixedUpdate();
    //        turn = 0.0f;
    //    }
    //}

}
