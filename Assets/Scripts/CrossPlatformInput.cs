using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CrossPlatformInput
{
    //static int touchID = -1;
    //static Vector2 startPos;

    //public static GameObject anchor; 

    public static bool Shoot
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Space)
                || Input.GetKeyDown(KeyCode.Joystick1Button0)
                || GameManager.Get().UIManager.ShootButtonPressed;
        }
    }

    public static bool Right
    {
        get
        {
            return (Input.GetAxisRaw("Horizontal") > 0) || (GameManager.Get().UIManager.Turn < 0);
        }
    }

    public static bool Left
    {
        get
        {
            return (Input.GetAxisRaw("Horizontal") < 0) || (GameManager.Get().UIManager.Turn > 0);
        }
    }

    public static bool Lasers
    {
        get
        {
            return Input.GetKeyDown(KeyCode.E)
                || Input.GetKeyDown(KeyCode.Joystick1Button1)
                || GameManager.Get().UIManager.LasersButtonPressed;
        }
    }

    public static bool Shield
    {
        get
        {
            return Input.GetKeyDown(KeyCode.S)
                || Input.GetKeyDown(KeyCode.Joystick1Button2)
                || GameManager.Get().UIManager.ShieldButtonPressed;
        }
    }

    public static bool Turbo
    {
        get
        {
            return Input.GetKeyDown(KeyCode.W)
                || Input.GetKeyDown(KeyCode.Joystick1Button3)
                || GameManager.Get().UIManager.TurboButtonPressed;
        }
    }


    //static Vector2 DeltaMovement()
    //{
    //    //bool touchOK = true;
    //    //if (touchID < 0 || touchID >= Input.touchCount || Input.GetTouch(touchID).phase == TouchPhase.Ended)
    //    //{
    //    //    touchOK = false;
    //    //    foreach (Touch touch in Input.touches)
    //    //        if (touch.fingerId != touchID)
    //    //        {
    //    //            touchID = touch.fingerId;
    //    //            startPos = touch.position;

    //    //            anchor.SetActive(true);
    //    //            anchor.transform.position = startPos;

    //    //            touchOK = true;
    //    //        }

    //    //    if (!touchOK)
    //    //    {
    //    //        anchor.SetActive(false);
    //    //        touchID = -1;
    //    //    }
    //    //}

    //    //try
    //    //{
    //    //    Vector2 fingerPosition = Input.GetTouch(touchID).position;
    //    //    GameManager.Get().DrawLine(Camera.main.ScreenToWorldPoint(fingerPosition), Camera.main.ScreenToWorldPoint(startPos));

    //    //    return fingerPosition - startPos;
    //    //}
    //    //catch (ArgumentException)
    //    //{
    //    //    return Vector2.zero;
    //    //}
    //}
}
