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
    private static XboxOneAxis[] axisToFlip = { XboxOneAxis.LeftThumbY, XboxOneAxis.RightThumbY };

    public static bool GetButton(XboxOneButton button)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetButton(buttonName);
    }

    public static float GetAxis(XboxOneAxis axis)
    {
        string playerPrefix = "p1_";
        string buttonName = playerPrefix + axis.GetDescription();

        return Input.GetAxis(buttonName) * (System.Array.Exists(axisToFlip, x => x == axis) ? -1 : 1);
    }

    public static bool GetButton(XboxOneButton button, int player)
    {
        string playerPrefix = "p" + player + "_";
        string buttonName = playerPrefix + button.GetDescription();

        return Input.GetButton(buttonName);
    }

    public static float GetAxis(XboxOneAxis axis, int player)
    {
        string playerPrefix = "p" + player + "_";
        string buttonName = playerPrefix + axis.GetDescription();

        return Input.GetAxis(buttonName);
    }
}
