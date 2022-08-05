using Piki.UnityGUI.BasesAndInterfaces;
using Piki.UnityGUI.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Piki.UnityGUI.Elements
{
    public class ClassicBox : GUIElement
    {
        public static ClassicBox Create(Vector2 position, Vector2 size, string title = "", Color? color = null, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            ClassicBox b = new ClassicBox();
            b.position = position;
            b.size = size;
            b.title = title;
            b.color = color ?? Color.white;
            b.stickToElement = stickToElement;
            b.layer = layer;
            b.canvas = canvas;
            return b;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            Color col = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUI.Box(new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY), title);
            GUI.backgroundColor = col;
        }

        public Color color = default;
        public string title = string.Empty;
    }

    public class CheckBox : GUIElement
    {
        public static CheckBox Create(Vector2 position, Vector2 size, string title = "", bool isChecked = false, Action<bool> onCheckedChanged = null, Color? color = null, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            CheckBox b = new CheckBox();
            b.position = position;
            b.size = size;
            b.title = title;
            b.color = color ?? Color.white;
            b.stickToElement = stickToElement;
            b.OnCheckedChanged = onCheckedChanged;
            b.isChecked = isChecked;
            b.layer = layer;
            b.canvas = canvas;
            return b;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            Color col = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUIStyle style = GUI.skin.toggle;
            style.fontStyle = textStyle;
            style.fontSize = textSize;
            style.normal.textColor = textColor;
            bool result = GUI.Toggle(new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY), isChecked, title);
            if (isChecked != result)
            {
                isChecked = result;
                if (OnCheckedChanged != null) OnCheckedChanged.Invoke(result);
            }
            GUI.backgroundColor = col;
        }

        public Action<bool> OnCheckedChanged;
        public bool isChecked;
        public Color color = default;
        public string title = string.Empty;

        public FontStyle textStyle;

        public Color textColor;
        public int textSize;
        public Texture2D texture;
    }

    public class ClassicButton : GUIElement
    {
        public static ClassicButton Create(Vector2 position, Vector2 size, string title, Action onClick, Color? color = null, int textSize = 12, Color? textColor = null, FontStyle textStyle = FontStyle.Normal, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            ClassicButton b = new ClassicButton();
            b.position = position;
            b.size = size;
            b.title = title;
            b.color = color ?? Color.white;
            b.stickToElement = stickToElement;
            b.textSize = textSize;
            b.textColor = textColor ?? Color.white;
            b.textStyle = textStyle;
            b.OnClick = onClick;
            b.layer = layer;
            b.canvas = canvas;
            return b;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            Color col = GUI.backgroundColor;
            GUI.backgroundColor = color;
            GUIStyle style = GUI.skin.button;
            style.normal.textColor = textColor;
            style.fontSize = textSize;
            style.fontStyle = textStyle;
            if (GUI.Button(new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY), title, style) && canBeClicked && OnClick != null) OnClick.Invoke();
            GUI.backgroundColor = col;
        }

        public string title = string.Empty;
        public Color color = default;
        public bool canBeClicked = true;

        public FontStyle textStyle;
        public Color textColor;
        public int textSize;

        public Action OnClick;
    }

    public class TextBox : GUIElement
    {
        public static TextBox Create(Vector2 position, Vector2 size, string text = "", Texture2D texture = null, Color? color = null, int textSize = 12, Color? textColor = null, FontStyle textStyle = FontStyle.Normal, bool enabled = true, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            TextBox b = new TextBox();
            b.position = position;
            b.size = size;
            b.text = text;
            b.color = color ?? new Color(0.2f, 0.2f, 0.3f);
            b.texture = texture == null ? Texture2D.whiteTexture : texture;
            b.stickToElement = stickToElement;
            b.textSize = textSize;
            b.textColor = textColor ?? Color.white;
            b.textStyle = textStyle;
            b.layer = layer;
            b.enabled = enabled;
            b.canvas = canvas;
            b.style.richText = false;
            return b;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            Color col = GUI.backgroundColor;
            GUI.backgroundColor = color;
            style.fontSize = (int)(textSize * canvas.fontSizeMultiplier);
            text = GUI.TextField(new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY), text, style);
            GUI.backgroundColor = col;
        }

        public string text = string.Empty;
        public Color color = default;

        private GUIStyle style = new GUIStyle();
        public Texture2D texture
        {
            get => style.normal.background;
            set
            {
                style.normal.background = value;
            }
        }
        public Color textColor
        {
            get => style.normal.textColor;
            set
            {
                style.normal.textColor = value;
            }
        }
        public int textSize = 12;
        public FontStyle textStyle
        {
            get => style.fontStyle;
            set
            {
                style.fontStyle = value;
            }
        }
    }

    public class Button : GUIElement
    {
        public static Button Create(Vector2 position, Vector2 size, string title, Action<Button> onClick, Texture2D texture = null, Color? color = null, ScaleMode scaleMode = ScaleMode.StretchToFill, object customObject = null, int textSize = 12, Color? textColor = null, FontStyle textStyle = FontStyle.Normal, TextAnchor textAlignment = TextAnchor.MiddleCenter, bool enabled = true, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            Button b = new Button();
            b.OnClick = onClick;
            b.position = position;
            b.size = size;
            b.texture = texture == null ? Texture2D.whiteTexture : texture;
            b.stickToElement = stickToElement;
            b.color = color ?? new Color(0.2f, 0.2f, 0.3f);
            b.scaleMode = scaleMode;
            b.title = title;
            b.customObject = customObject;
            b.textStyle = textStyle;
            b.textColor = textColor.HasValue ? textColor.Value : Color.white;
            b.textSize = textSize;
            b.textAlignment = textAlignment;
            b.layer = layer;
            b.enabled = enabled;
            b.canvas = canvas;
            return b;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            Rect rect = new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY);
            float colorMultiplier = isBeingHeld ? 0.6f : (mouseOver ? 0.8f : 1f);
            Color col = GUI.color;
            Color colToSet = color * colorMultiplier;
            colToSet.a = color.a;
            GUI.color = colToSet;
            GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill);
            GUI.color = col;
            style.fontSize = (int)(textSize * canvas.fontSizeMultiplier);
            GUI.Label(rect, title, style);
            if (!isBeingHeld && mouseOver && Input.GetMouseButtonDown(0))
            {
                isBeingHeld = true;
            }
            if (isBeingHeld && Input.GetMouseButtonUp(0))
            {
                isBeingHeld = false;
                if (mouseOver && OnClick != null) OnClick.Invoke(this);
            }
        }

        public bool isBeingHeld = false;
        public ScaleMode scaleMode;
        public Texture2D texture;
        public Color color;
        public Action<Button> OnClick;
        public string title;
        private GUIStyle style = new GUIStyle();
        public TextAnchor textAlignment
        {
            get => style.alignment;
            set
            {
                style.alignment = value;
            }
        }
        public Color textColor
        {
            get => style.normal.textColor;
            set
            {
                style.normal.textColor = value;
            }
        }
        public int textSize = 12;
        public FontStyle textStyle
        {
            get => style.fontStyle;
            set
            {
                style.fontStyle = value;
            }
        }
    }

    public class TextureBox : GUIElement
    {
        public static TextureBox Create(Vector2 position, Vector2 size, Texture2D texture = null, Color? color = null, ScaleMode scaleMode = ScaleMode.StretchToFill, Action onMouseDown = null, Action onMouseUp = null, Action onMouse = null, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            TextureBox b = new TextureBox();
            b.position = position;
            b.size = size;
            b.texture = texture == null ? Texture2D.whiteTexture : texture;
            b.stickToElement = stickToElement;
            b.OnMouseDown = onMouseDown;
            b.OnMouseUp = onMouseUp;
            b.OnMouse = onMouse;
            b.color = color ?? Color.white;
            b.scaleMode = scaleMode;
            b.layer = layer;
            b.canvas = canvas;
            return b;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            Rect rect = new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY);
            Color col = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, texture, ScaleMode.StretchToFill);
            GUI.color = col;
            if (OnMouseDown != null && Input.GetMouseButtonDown(0) && rect.Contains(Event.current.mousePosition)) OnMouseDown.Invoke();
            if (OnMouseUp != null && Input.GetMouseButtonUp(0) && rect.Contains(Event.current.mousePosition)) OnMouseUp.Invoke();
            if (OnMouse != null && Input.GetMouseButton(0) && rect.Contains(Event.current.mousePosition)) OnMouse.Invoke();
        }

        public ScaleMode scaleMode;
        public Texture2D texture;
        public Color color;
        public Action OnMouseUp;
        public Action OnMouseDown;
        public Action OnMouse;
    }

    public class DragBox : GUIElement
    {
        public static DragBox Create(Vector2 position, Vector2 size, Texture2D texture = null, Color? color = null, ScaleMode scaleMode = ScaleMode.StretchToFill, Action onMouseDown = null, Action onMouseUp = null, Action onMouse = null, string text = "", int textSize = 12, Color? textColor = null, FontStyle textStyle = FontStyle.Normal, TextAnchor textAlignment = TextAnchor.MiddleCenter, bool enabled = true, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            DragBox b = new DragBox();
            b.position = position;
            b.size = size;
            b.texture = texture == null ? Texture2D.whiteTexture : texture;
            b.stickToElement = stickToElement;
            b.OnMouseDown = onMouseDown;
            b.OnMouseUp = onMouseUp;
            b.OnMouse = onMouse;
            b.color = color.HasValue ? color.Value : Color.white;
            b.scaleMode = scaleMode;
            b.text = text;
            b.enabled = enabled;
            b.textStyle = textStyle;
            b.textColor = textColor ?? Color.white;
            b.textSize = textSize;
            b.textAlignment = textAlignment;
            b.layer = layer;
            b.canvas = canvas;
            return b;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            Rect rect = new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY);
            Color col = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(rect, texture, scaleMode);
            GUI.color = col;
            if (text != string.Empty)
            {
                style.fontSize = (int)(textSize * canvas.fontSizeMultiplier);
                GUI.Label(rect, text, style);
            }
            if (Input.GetMouseButtonDown(0) && mouseOver)
            {
                if (OnMouseDown != null) OnMouseDown.Invoke();
                mouseOffset = canvas.mousePosition - realPosition;
                isBeingHeld = true;
            }
            if (OnMouseUp != null && Input.GetMouseButtonUp(0) && mouseOver) OnMouseUp.Invoke();
            if (OnMouse != null && Input.GetMouseButton(0) && mouseOver) OnMouse.Invoke();
            if (isBeingHeld)
            {
                Vector2 result = canvas.mousePosition - mouseOffset;
                result.x = Mathf.Clamp(result.x, 0, canvas.size.x - size.x);
                result.y = Mathf.Clamp(result.y, 0, canvas.size.y - size.y);
                realPosition = result;
                if (Input.GetMouseButtonUp(0)) isBeingHeld = false;
            }
        }

        private Vector2 mouseOffset;
        private GUIStyle style = new GUIStyle();
        public string text = string.Empty;
        public ScaleMode scaleMode;
        public bool isBeingHeld = false;
        public Texture2D texture;
        public Color color;
        public Action OnMouseUp;
        public Action OnMouseDown;
        public Action OnMouse;
        public TextAnchor textAlignment
        {
            get => style.alignment;
            set
            {
                style.alignment = value;
            }
        }
        public Color textColor
        {
            get => style.normal.textColor;
            set
            {
                style.normal.textColor = value;
            }
        }
        public int textSize = 12;
        public FontStyle textStyle
        {
            get => style.fontStyle;
            set
            {
                style.fontStyle = value;
            }
        }
    }

    public class Label : GUIElement
    {
        public static Label Create(Vector2 position, Vector2 size, string text, int fontSize = 12, Color? color = null, FontStyle fontStyle = FontStyle.Normal, TextAnchor alignment = TextAnchor.UpperLeft, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            Label l = new Label();
            l.receiveRays = false;
            l.position = position;
            l.size = size;
            l.fontSize = fontSize;
            l.text = text;
            l.color = color.HasValue ? color.Value : Color.white;
            l.fontStyle = fontStyle;
            l.alignment = alignment;
            l.stickToElement = stickToElement;
            l.layer = layer;
            l.canvas = canvas;
            return l;
        }

        public override void Draw()
        {
            if (stickToElement != null && stickToElement.canvas == null)
            {
                Destroy();
                return;
            }
            style.fontSize = (int)(fontSize * canvas.fontSizeMultiplier);
            GUI.Label(new Rect(realPosition.x * canvas.positionMultiplierX, realPosition.y * canvas.positionMultiplierY, size.x * canvas.positionMultiplierX, size.y * canvas.positionMultiplierY), text, style);
        }

        private GUIStyle style = new GUIStyle();
        public string text = string.Empty;
        public FontStyle fontStyle
        {
            get => style.fontStyle;
            set
            {
                style.fontStyle = value;
            }
        }
        public Color color
        {
            get => style.normal.textColor;
            set
            {
                style.normal.textColor = value;
            }
        }
        public int fontSize = 12;
        public TextAnchor alignment
        {
            get => style.alignment;
            set
            {
                style.alignment = value;
            }
        }
    }
}

namespace Piki.UnityGUI.BasesAndInterfaces
{
    public abstract class GUIElement
    {
        public abstract void Draw();

        public void Destroy()
        {
            canvas = null;
        }

        public bool DoRaycasting(Vector2 point)
        {
            if (!enabled || !receiveRays) return false;
            return new Rect(realPosition, size).Contains(point);
        }

        public Vector2 position = Vector2.zero;
        public Vector2 realPosition
        {
            get => position + (stickToElement == null ? Vector2.zero : stickToElement.realPosition);
            set
            {
                position = value - (stickToElement == null ? Vector2.zero : stickToElement.realPosition);
            }
        }
        public Vector2 size = Vector2.zero;
        public bool enabled
        {
            get => (stickToElement == null ? localEnabled : stickToElement.enabled && localEnabled);
            set
            {
                localEnabled = value;
            }
        }
        public bool receiveRays = true;
        private bool localEnabled = true;
        public GUIElement stickToElement;
        public object customObject;
        public int layer = 0;

        public bool mouseOver = false;

        public GUICanvas canvas
        {
            get => _canvas;
            set
            {
                if (value == _canvas) return;
                if (_canvas != null) _canvas.elements.Remove(this);
                _canvas = value;
                if (value != null) value.elements.Add(this);
            }
        }
        private GUICanvas _canvas;
    }
}

namespace Piki.UnityGUI.Elements.Prefabs
{
    public class GUIWindow
    {
        public static GUIWindow Create(Vector2 position, Vector2 size, string title, bool hasCloseButton, bool destroyOnClose = false, Color? tabColor = null, Color? windowColor = null, Texture2D windowTexture = null, int textSize = 12, Color? textColor = null, FontStyle textStyle = FontStyle.Normal, TextAnchor textAlignment = TextAnchor.MiddleCenter, bool enabled = true, int layer = 0, GUICanvas canvas = null, GUIElement stickToElement = null)
        {
            GUIWindow b = new GUIWindow();
            Color col = tabColor ?? new Color(0.05f, 0.05f, 0.075f);
            b.tab = DragBox.Create(position, new Vector2(size.x, 30), Texture2D.whiteTexture, col, ScaleMode.ScaleAndCrop, null, null, null, title, textSize, textColor, textStyle, textAlignment, enabled, stickToElement: stickToElement);
            b.window = TextureBox.Create(Vector2.zero, size, windowTexture, color: windowColor ?? new Color(0.1f, 0.1f, 0.15f, 0.7f), stickToElement: b.tab);
            b.closeButton = Button.Create(new Vector2(size.x - 30, 0), new Vector2(30, 30), "X", (Button ins) => { b.Close(); }, color: col, enabled: hasCloseButton, stickToElement: b.tab);
            b.destoryOnClose = destroyOnClose;
            b.canvas = canvas;
            b.layer = layer;

            return b;
        }

        public void Destroy()
        {
            tab.Destroy();
        }

        public void Close()
        {
            if (destoryOnClose) tab.Destroy();
            else enabled = false;
        }

        public string title
        {
            get => tab.text;
            set
            {
                tab.text = value;
            }
        }
        public int layer
        {
            get => tab.layer;
            set
            {
                window.layer = value;
                tab.layer = value;
                closeButton.layer = value;
            }
        }
        public bool enabled
        {
            get => tab.enabled;
            set
            {
                tab.enabled = value;
            }
        }
        public Vector2 position
        {
            get => tab.position;
            set
            {
                tab.position = value;
            }
        }
        public Vector2 size
        {
            get => window.size;
            set
            {
                tab.size = new Vector2(value.x, 30);
                window.size = value;
            }
        }
        public Color tabColor
        {
            get => tab.color;
            set
            {
                tab.color = value;
                closeButton.color = value;
            }
        }
        public Color windowColor
        {
            get => window.color;
            set
            {
                window.color = value;
            }
        }
        public Texture2D windowTexture
        {
            get => window.texture;
            set
            {
                window.texture = value;
            }
        }
        public GUIElement stickToElement
        {
            get => tab.stickToElement;
            set
            {
                tab.stickToElement = value;
            }
        }
        public bool isMoving => tab.isBeingHeld;
        public GUICanvas canvas
        {
            get => _canvas;
            set
            {
                _canvas = value;
                window.canvas = value;
                tab.canvas = value;
                closeButton.canvas = value;
            }
        }
        private GUICanvas _canvas;

        public GUIElement stickableElement => tab;
        public bool destoryOnClose;
        private DragBox tab;
        private TextureBox window;
        private Button closeButton;
    }
}

namespace Piki.UnityGUI.Canvas
{
    public class GUICanvas
    {
        public GUICanvas(bool _enabled = true)
        {
            enabled = _enabled;
        }

        public void Draw()
        {
            if (!enabled) return;
            float a = Screen.height / size.y;
            positionMultiplierX = Screen.width / size.x;
            positionMultiplierY = a;
            fontSizeMultiplier = a;
            var ems = elements.ToArray().Reverse().OrderByDescending(x => x.layer).Reverse();
            foreach (GUIElement e in ems)
            {
                if (e.enabled) e.Draw();
            }
        }

        public void Update()
        {
            if (!enabled) return;
            var pos = mousePosition;
            var ems = elements.ToArray().Reverse().OrderByDescending(x => x.layer);
            GUIElement hit = null;
            foreach (GUIElement e in ems)
            {
                if (hit != null)
                {
                    e.mouseOver = false;
                    continue;
                }
                bool a = e.DoRaycasting(pos);
                e.mouseOver = a;
                if (a) hit = e;
            }
        }

        public Vector2 mousePosition
        {
            get
            {
                Vector2 mouse = Input.mousePosition;
                mouse.y = Screen.height - mouse.y;
                mouse.x /= positionMultiplierX;
                mouse.y /= positionMultiplierY;
                return mouse;
            }
        }

        public float fontSizeMultiplier { get; private set; }
        public float positionMultiplierX { get; private set; }
        public float positionMultiplierY { get; private set; }
        public bool enabled = true;
        public Vector2 size = new Vector2(1920f, 1080f);
        public List<GUIElement> elements = new List<GUIElement>();
    }
}
