using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;

public class ChooseMultiLangScreen : UISystem.Screen
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
    }

    public void Btn_OnSelect()
    {
        ViewController.Instance.ChangeScreen(ScreenName.LoginScreen);
    }
}
