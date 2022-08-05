using System;
using UnityEngine;

[Serializable]
public class Keys
{
    public KeyCode noclip = KeyCode.C;
    public KeyCode selfbuff = KeyCode.X;
    public KeyCode menu = KeyCode.Tab;
    public KeyCode select = KeyCode.LeftAlt;
    public KeyCode delete = KeyCode.Delete;
    public static Keys currentKeys = new Keys();
}
