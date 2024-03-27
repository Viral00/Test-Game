using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class CompanyAdvScreen : UISystem.Screen
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

    public void ChangeScreen()
    {
        ViewController.Instance.ChangeScreen(ScreenName.SplashScreen);
    }
   
}
