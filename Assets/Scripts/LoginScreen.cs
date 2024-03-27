using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UISystem;

public class LoginScreen : UISystem.Screen
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Back()
    {
        base.Back();
    }

    public override void Disable()
    {
        base.Disable();
    }

    public override void Enable()
    {
        base.Enable();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public override void Redraw()
    {
        base.Redraw();
    }

    public override void Show()
    {
        base.Show();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public void Btn_GetOtp()
    {
        ViewController.Instance.ChangeScreen(ScreenName.VerifyOtpScreen);
    }

    public void Btn_Back()
    {
        ViewController.Instance.ChangeScreen(ScreenName.chooseMultiLangScreen);
    }
}
