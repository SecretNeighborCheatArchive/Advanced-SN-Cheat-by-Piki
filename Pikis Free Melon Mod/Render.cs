using System;
using UnityEngine;

public static class Render
{
    public static GUIStyle StringStyle { get; set; } = new GUIStyle(GUI.skin.label);
    public static Color Color
    {
        get
        {
            return GUI.color;
        }
        set
        {
            GUI.color = value;
        }
    }

    public static void DrawLine(Vector2 from, Vector2 to, Color color)
    {
        Render.Color = color;
        Render.DrawLine(from, to);
    }

    public static void DrawLine(Vector2 from, Vector2 to)
    {
        float num = Vector2.SignedAngle(from, to);
        GUIUtility.RotateAroundPivot(num, from);
        Render.DrawBox(from, Vector2.right * (from - to).magnitude, false);
        GUIUtility.RotateAroundPivot(-num, from);
    }

    public static void DrawLine3d(Vector2 from, Vector3 to, Color color)
    {
        Render.Color = color;
        Vector3 point = Camera.main.WorldToScreenPoint(to);
        point.y = Screen.height - point.y;
        if (point.z > 0) Render.DrawLine(from, point);
    }

    public static void DrawBox(Vector2 position, Vector2 size, Color color, bool centered = true)
    {
        Color c = GUI.color;
        Render.Color = color;
        Render.DrawBox(position, size, centered);
        Render.Color = c;
    }

    public static void DrawBox(Vector2 position, Vector2 size, bool centered = true)
    {
        if (centered)
        {
            position -= size / 2f;
        }
        GUI.DrawTexture(new Rect(position, size), Texture2D.whiteTexture, 0);
    }

    public static void DrawString(Vector2 position, string label, Color color, bool centered = true)
    {
        Render.Color = color;
        Render.DrawString(position, label, centered);
    }

    public static void DrawString(Vector2 position, string label, bool centered = true)
    {
        GUIContent guicontent = new GUIContent(label);
        Vector2 vector = Render.StringStyle.CalcSize(guicontent);
        GUI.Label(new Rect(centered ? (position - vector / 2f) : position, vector), guicontent);
    }
}
