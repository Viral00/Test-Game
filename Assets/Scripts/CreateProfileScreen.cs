using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UISystem;

public class CreateProfileScreen : UISystem.Screen
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

    public void Btn_backBtn()
    {
        ViewController.Instance.ChangeScreen(ScreenName.LoginScreen);
    }

    public void Btn_submit()
    {
        ViewController.Instance.ChangeScreen(ScreenName.VsSCreen);
    }
}
