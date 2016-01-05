using UnityEngine;
using System.Collections;
using System.ComponentModel;
using Base;

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

public static class XboxOneInput
{
    public static bool GetButton(XboxOneButton button)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetButton(buttonName);
    }

    public static float GetAxis(XboxOneAxis button)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetAxis(buttonName);
    }

    public static bool GetButton(int player, XboxOneButton button)
    {
        string playerPrefix = "p" + player + "_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetButton(buttonName);
    }

    public static float GetAxis(int player, XboxOneAxis button)
    {
        string playerPrefix = "p" + player + "_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetAxis(buttonName);
    }
}
