using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlType
{
    public string Name;
    public string MenuDisplayName;
    public string MoveForvardAxisRawName;
    public string RotateAxisRawName;
    public string ShootButtonName;

    public static readonly ControlType keyboard = new ControlType()
    {
        Name = "keyboard",
        MenuDisplayName = "Управление: клавиатура",
        MoveForvardAxisRawName = "Vertical",
        RotateAxisRawName = "Horizontal",
        ShootButtonName = "Fire1"
    };
    public static readonly ControlType keyboardMouse = new ControlType()
    {
        Name = "keyboardMouse",
        MenuDisplayName = "Управление: клавиатура + мышь",
        MoveForvardAxisRawName = "Vertical2",
        RotateAxisRawName = "[MOUSE]",
        ShootButtonName = "Fire2"
    };

    private ControlType() { }
}
