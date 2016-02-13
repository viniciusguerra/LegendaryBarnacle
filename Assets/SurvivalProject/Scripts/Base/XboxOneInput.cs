using UnityEngine;
using System.Collections;
using System.ComponentModel;
using Base;
using System;

namespace Base
{    
    public static class XboxOneEnumDescription
    {
        public static string GetDescription(this XboxOneButton val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetDescription(this XboxOneAxis val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

    public delegate void ButtonHeld();
}

public enum XboxOneButton
{
    [Description("a")]
    A,
    [Description("b")]
    B,
    [Description("x")]
    X,
    [Description("y")]
    Y,
    [Description("lb")]
    LB,
    [Description("rb")]
    RB,
    [Description("view")]
    View,
    [Description("menu")]
    Menu,
    [Description("ls")]
    LS,
    [Description("rs")]
    RS
}

public enum XboxOneAxis
{
    [Description("lx")]
    LeftThumbX,
    [Description("ly")]
    LeftThumbY,
    [Description("rx")]
    RightThumbX,
    [Description("ry")]
    RightThumbY,
    [Description("lt")]
    LT,
    [Description("rt")]
    RT,
    [Description("dpadx")]
    DPadX,
    [Description("dpady")]
    DPadY
}

public class XboxOneInput : Singleton<XboxOneInput>
{
    private static readonly XboxOneAxis[] axisToFlip = { XboxOneAxis.LeftThumbY, XboxOneAxis.RightThumbY };

    public bool GetButtonDown(XboxOneButton button)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetButtonDown(buttonName);
    }

    public bool GetButtonUp(XboxOneButton button)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetButtonUp(buttonName);
    }

    public bool GetButton(XboxOneButton button)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetButton(buttonName);
    }

    public void OnButtonHeld(XboxOneButton button, float time, ButtonHeld successAction, ButtonHeld failAction)
    {
        StartCoroutine(ButtonHeldCoroutine(button, time, successAction, failAction));
    }

    private IEnumerator ButtonHeldCoroutine(XboxOneButton button, float time, ButtonHeld successAction, ButtonHeld failAction)
    {
        bool success = true;
        float counter = 0;

        do
        {
            if (GetButtonUp(button))
            {
                success = false;
                break;
            }
            else
                counter += Time.deltaTime;

            yield return null;

        } while (counter < time);        

        if (success)
        {
            successAction.Invoke();
        }
        else
        {
            if(failAction != null)
                failAction.Invoke();
        }
    }

    public float GetAxis(XboxOneAxis axis)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + axis.GetDescription();

        return Input.GetAxis(buttonName) * (Array.Exists(axisToFlip, x => x == axis) ? -1 : 1);
    }

    //public bool GetButtonDown(XboxOneButton button, int player)
    //{
    //    string playerPrefix = "p" + player + "_";
    //    string buttonName = playerPrefix + button.GetDescription();

    //    return Input.GetButtonDown(buttonName);
    //}

    //public bool GetButtonUp(XboxOneButton button, int player)
    //{
    //    string playerPrefix = "p" + player + "_";
    //    string buttonName = playerPrefix + button.GetDescription();

    //    return Input.GetButtonUp(buttonName);
    //}

    //public bool GetButton(XboxOneButton button, int player)
    //{
    //    string playerPrefix = "p" + player + "_";
    //    string buttonName = playerPrefix + button.GetDescription();

    //    return Input.GetButton(buttonName);
    //}

    //public float GetAxis(XboxOneAxis axis, int player)
    //{
    //    string playerPrefix = "p" + player + "_";
    //    string buttonName = playerPrefix + axis.GetDescription();

    //    return Input.GetAxis(buttonName);
    //}
}
