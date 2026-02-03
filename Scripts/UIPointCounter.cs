using Godot;
using System;

public partial class UIPointCounter : RichTextLabel
{
    int red = 0;
    int blue = 0;
    int err = 0;

    private static UIPointCounter ins;

    public override void _Ready()
    {
        ins = this;
        UpdateText();
    }

    public static void AddBlue()
    {
        ins.blue++;
        ins.UpdateText();
    }
    public static void AddRed()
    {
        ins.red++;
        ins.UpdateText();
    }
    public static void AddErr()
    {
        ins.err++;
        ins.UpdateText();
    }

    private void UpdateText()
    {
        string txt = $"[color=0000ff]Blue[/color]:{blue.ToString("0000")}{System.Environment.NewLine}";
        txt += $"[color=d00000]Red[/color]:{red.ToString("0000")}{System.Environment.NewLine}";
        txt += $"[color=ffff00]Err[/color]:{err.ToString("0000")}{System.Environment.NewLine}";
        Text = txt;
    }
}// EOF CLASS
