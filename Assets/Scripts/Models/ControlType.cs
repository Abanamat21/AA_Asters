using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlType
{
    public static readonly ControlType keyboard = new ControlType() { 
        name = "keyboard", 
        menuDisplayName = "Управление: клавиатура",
        moveForvardAxisRawName = "Vertical",
        rotateAxisRawName = "Horizontal",
        shootButtonName = "Fire1"
    };
    public static readonly ControlType keyboardMouse = new ControlType() { 
        name = "keyboardMouse", 
        menuDisplayName = "Управление: клавиатура + мышь",
        moveForvardAxisRawName = "Vertical2",
        rotateAxisRawName = "[MOUSE]",
        shootButtonName = "Fire2"
    };
    
    private ControlType() { }

    public string name;
    public string menuDisplayName;
    public string moveForvardAxisRawName;
    public string rotateAxisRawName;
    public string shootButtonName;
}
