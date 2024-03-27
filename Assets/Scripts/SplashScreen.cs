using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class SplashScreen : UISystem.Screen
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
        Helper.Execute(this, ChangeScreen, 2f);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public void ChangeScreen()
    {
        ViewController.Instance.ChangeScreen(ScreenName.chooseMultiLangScreen);
    }
}
