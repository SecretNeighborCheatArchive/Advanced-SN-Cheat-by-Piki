using BackEnd;
using GameModes.GameplayMode;
using GameModes.GameplayMode.Actors;
using GameModes.GameplayMode.Interactables.DoorInteractables;
using GameModes.GameplayMode.Interactables.DoorInteractables.@base;
using GameModes.GameplayMode.Interactables.InventoryItems.Base;
using GameModes.GameplayMode.Levels.Basement;
using GameModes.GameplayMode.Misc;
using GameModes.GameplayMode.Players;
using HoloNetwork.Messaging.Implementations;
using HoloNetwork.NetworkObjects;
using Piki.UnityGUI.Elements;
using Piki.UnityGUI.Elements.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using HoloNetAppModule = ObjectPublicOb287259405257206727Unique;
using HoloNet = ObjectPublicDoBoObBoUnique;
using static UnityEngine.Object;
using GameModes.GameplayMode.Interactables.Lights;
using HoloNetwork.Players;
using GameModes.GameplayMode.Actors.Shared;
using System.Collections;
using MelonLoader;
using GameModes.GameplayMode.Interactables.InventoryItems;
using GameModes.GameplayMode.Players.InventoryManager;
using GameModes.GameplayMode.Players.Configuration;
using GameModes.GameplayMode.Interactables.SceneObjects.Rocket;
using UnityEngine.Rendering.PostProcessing;

//Object1PublicAbstractAcObLiOb1LeObBoAcInUnique = actorskills
public class Game : IGameMode
{
    public Game()
    {
        style.alignment = TextAnchor.UpperCenter;
        style.fontStyle = FontStyle.Bold;
    }

    public void Start()
    {
        Console.WriteLine(RenderSettings.ambientEquatorColor.ToString());
        players = getPlayers.ToList();
        localPlayer = getLocalPlayer;
        inv = localPlayer.GetComponent<InventorySlotManager>();
        SelectPlayer(localPlayer);
        RefreshButtons();
        if (!spawnButtonsSpawned)
        {
            SpawnItemButtons(5, 5);
            spawnButtonsSpawned = true;
        }
        BuffPlayer(localPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.CONTROL_GATES_BUFF);
        BuffPlayer(localPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.INVINCIBLE);
    }

    public static void Crash(Actor a)
    {
        Teleport(a, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
    }

    public static void Teleport(Actor a, Vector3 position)
    {
        a.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(ActorTeleportPositionMessage.Method_Public_Static_ActorTeleportPositionMessage_Vector3_0(position), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    public static void TeleportAll(Vector3 position)
    {
        foreach (Player plr in players)
        {
            Teleport(plr.prop_Actor_0, position);
        }
    }

    public static void TryLaunchRocket()
    {
        var a = FindObjectOfType<RocketLaunchButton>();
        if (a == null) return;
        a.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(HoloNetObjectIdMessage.Method_Public_Static_T_HoloNetObjectId_0<LaunchRocketMessage>(a.prop_HoloNetObject_0.oid), EnumPublicSealedvaOtAlSe5vSeUnique.All);
        a.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(HoloNetObjectIdMessage.Method_Public_Static_T_HoloNetObjectId_0<LaunchRocketMessage>(a.prop_HoloNetObject_0.oid), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    private void SpawnItemButtons(float offset, int maxItemsInRow)
    {
        int a = 0;
        Vector2 pos = new Vector2(offset, 30f + offset);
        Vector2 size = new Vector2(115, 30);
        foreach (var obj in netObjectManager.field_Private_List_1_GameObject_0)
        {
            var comp = obj.GetComponent<InventoryItem>();
            if (!comp) continue;
            spawnButtons.Add(Button.Create(pos, size, comp.name.Replace("Item_", ""), OnSpawnItemButton, customObject: obj, layer: 4, canvas: GuiStuff.gameCanvas, stickToElement: GuiStuff.spawnMenu.stickableElement));
            pos.x += size.x + offset;
            a++;
            if (a >= maxItemsInRow)
            {
                a = 0;
                pos.y += size.y + offset;
                pos.x = offset;
            }
        }
    }

    public void OnSpawnItemButton(Button ins)
    {
        SpawnItem((GameObject)ins.customObject);
    }

    public void SpawnItem(GameObject obj)
    {
        Transform cam = Camera.main.transform;
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit, 3);
        Vector3 spawnPos = isHit ? hit.point : cam.position + cam.forward * 3;
        HoloNet.Method_Public_Static_Void_HoloNetGlobalMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(SpawnNetObjectMessage.Method_Public_Static_SpawnNetObjectMessage_HoloNetObjectId_Int32_Vector3_Quaternion_0(netObjectManager.Method_Private_HoloNetObjectId_0(), netObjectManager.Method_Private_Int32_GameObject_PDM_0(obj), spawnPos, default), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    public void RefreshButtons()
    {
        foreach (var b in buttons) b.canvas = null;
        buttons.Clear();
        float a = 35f;
        foreach (Player plr in players)
        {
            buttons.Add(Button.Create(new Vector2(10, a), new Vector2(130, 35), plr.prop_PlayerInfo_0.displayName, OnBtnClick, color: plr.prop_Boolean_0 ? Color.red / 2 : new Color(0.2f, 0.2f, 0.3f), customObject: plr, layer: 2, canvas: GuiStuff.gameCanvas, stickToElement: GuiStuff.playerMenu.stickableElement));
            a += 40f;
        }
    }

    public bool DoRaycasting(out RaycastHit hit)
    {
        Transform t = Camera.main.transform;
        Ray ray = new Ray(t.position, t.forward);
        RaycastHit _hit;
        bool flag = Physics.Raycast(ray, out _hit, 100f, 1 << 0 | 1 << 8);
        hit = _hit;
        return flag;
    }

    public void TrySelectObject(RaycastHit hit)
    {
        HoloNetObject net = GetAllComponents<HoloNetObject>(hit.transform);
        if (!net) return;
        DoorInteractableBase door = GetAllComponents<DoorInteractableBase>(hit.transform);
        selectedObject = door ? null : net;
        Window win = GetAllComponents<Window>(hit.transform);
        LightInteractable light = GetAllComponents<LightInteractable>(hit.transform);
        var position = currentItemWindow == null ? new Vector2(200, 200) : currentItemWindow.position;
        var window = GUIWindow.Create(position, new Vector2(410, 300), hit.transform.name, true, true, layer: 4, canvas: GuiStuff.gameCanvas);

        if (door)
        {
            var b = Button.Create(new Vector2(5, 35), new Vector2(130, 30), "Toggle door", (Button ins) => 
            {
                if (door.prop_Boolean_2) door.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(SimpleHoloNetObjectMessage.Method_Public_Static_T_0<DoorCloseMessage>(), EnumPublicSealedvaOtAlSe5vSeUnique.All);
                else door.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(SimpleHoloNetObjectMessage.Method_Public_Static_T_0<DoorOpenMessage>(), EnumPublicSealedvaOtAlSe5vSeUnique.All);
            }, color: new Color(0.2f, 0.2f, 0.3f), canvas: GuiStuff.gameCanvas, layer: 4, stickToElement: window.stickableElement);
        }
        else
        {
            var btn = Button.Create(new Vector2(275, 35), new Vector2(130, 30), "Destory", (Button ins) =>
            {
                DestroyObject(net);
                window.Close();
            }, color: new Color(0.5f, 0, 0), canvas: GuiStuff.gameCanvas, layer: 4, stickToElement: window.stickableElement);
        }

        if (win)
        {
            var b = Button.Create(new Vector2(5, 35), new Vector2(130, 30), "Break", (Button ins) =>
            {
                win.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(WindowCrashMessage.Method_Public_Static_WindowCrashMessage_Vector3_Vector3_PDM_0(default, default), EnumPublicSealedvaOtAlSe5vSeUnique.All);
            }, color: new Color(0.2f, 0.2f, 0.3f), canvas: GuiStuff.gameCanvas, layer: 4, stickToElement: window.stickableElement);
        }

        if (light)
        {
            var b = Button.Create(new Vector2(5, 35), new Vector2(130, 30), "Toggle light", (Button ins) =>
            {
                light.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(SimpleHoloNetObjectMessage.Method_Public_Static_T_0<LightInteractableToggleLightMessage>(), EnumPublicSealedvaOtAlSe5vSeUnique.All);
            }, color: new Color(0.2f, 0.2f, 0.3f), canvas: GuiStuff.gameCanvas, layer: 4, stickToElement: window.stickableElement);
        }

        if (currentItemWindow != null)
        {
            currentItemWindow.Close();
        }
        currentItemWindow = window;
    }

    public static T GetAllComponents<T>(Transform transform) where T : Component
    {
        Transform a = transform;
        while (a != null)
        {
            T com = a.GetComponent<T>();
            if (com != null) return com;
            a = a.parent;
        }
        return null;
    }

    public void OnBtnClick(Button ins)
    {
        SelectPlayer((Player)ins.customObject);
    }

    public static void DestroyObject(HoloNetObject obj)
    {
        HoloNet.Method_Public_Static_Void_HoloNetGlobalMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(NetObjectDestroyMessage.Method_Public_Static_NetObjectDestroyMessage_HoloNetObjectId_0(obj.oid), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    public void Update()
    {
        if (Input.GetKeyDown(Keys.currentKeys.menu)) menu = !_menu;
        if (Input.GetKeyDown(Keys.currentKeys.delete) && selectedObject != null && currentItemWindow.canvas != null)
        {
            DestroyObject(selectedObject);
            currentItemWindow.Close();
        }
        if (Input.GetKeyDown(Keys.currentKeys.selfbuff))
        {
            BuffPlayer(localPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.SPEEDUP);
            BuffPlayer(localPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.THROW_FORCE_BOOST);
        }
        if (Input.GetKeyDown(Keys.currentKeys.noclip))
        {
            noClip = !noClip;
            if (!noClip)
            {
                foreach (Actor a in localPlayer.prop_List_1_Actor_0)
                {
                    Collider c = a.GetComponent<Collider>();
                    if (c) c.enabled = true;
                }
            }
        }
        if (noClip) DoNoclip();
        if (Input.GetKeyDown(Keys.currentKeys.select))
        {
            RaycastHit hit;
            bool flag = DoRaycasting(out hit);
            if (flag)
            {
                selectedPoint = hit.point;
                GuiStuff.selectedPoint.text = "Point: " + selectedPoint.ToString();
                TrySelectObject(hit);
            }
        }
        if (players.Count < 1) return;
        if (players.RemoveAll(x => x == null) > 0) RefreshButtons();
        foreach (var b in buttons)
        {
            if (b.customObject == null)
            {
                b.canvas = null;
                continue;
            }
            Player p = (Player)b.customObject;
            if (p.prop_Boolean_2) b.color = Color.yellow / 2;
        }
        buttons.RemoveAll(x => x.customObject == null);
        if (selectedPlayer == null)
        {
            SelectPlayer(players[UnityEngine.Random.Range(0, players.Count - 1)]);
        }
        Transform plr = selectedPlayer.prop_Actor_0.transform;
        GuiStuff.playerTransform.text = $"<b>Position:</b> {plr.position.ToString()}\n<b>Rotation:</b> {plr.eulerAngles.ToString()}";
    }

    public void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.B))
        {
            if (inv != null)
            {
                var a = inv.currentItem;
                if (a != null && a as RifleInventoryItem != null)
                {
                    (a as RifleInventoryItem).Method_Public_Void_Actor_PDM_0(localPlayer.prop_Actor_0);
                }
            }
        }
    }

    private void DoNoclip()
    {
        Actor a = localPlayer.prop_Actor_0;
        Transform b = Camera.main.transform;
        a.GetComponent<Collider>().enabled = false;

        a.transform.position += (b.forward * Input.GetAxis("Vertical") + a.transform.right * Input.GetAxis("Horizontal")) * Time.deltaTime * 10f;
    }
    
    public void OnGUI()
    {
        foreach (Player plr in players)
        {
            if (plr == null || plr.prop_Boolean_2 || plr.prop_HoloNetObject_0.prop_HoloNetPlayer_0.prop_Boolean_0) continue;
            Vector3 pos = plr.prop_Actor_0.transform.position;
            Vector3 point = Camera.main.WorldToScreenPoint(pos);
            if (point.z > 0)
            {
                point.y = Screen.height - point.y;
                style.normal.textColor = plr.prop_Boolean_0 ? Color.red : Color.white;
                GUI.Label(new Rect(point.x - 200, point.y, 400, 100), plr.prop_PlayerInfo_0.displayName, style);
            }
        }

        if (selectedPoint != null)
        {
            Vector3 point = Camera.main.WorldToScreenPoint(selectedPoint);
            if (point.z > 0)
            {
                point.y = Screen.height - point.y;
                Render.DrawBox(point, new Vector2(10, 10), Color.green);
            }
        }
    }

    public static void BuffPlayer(Player plr, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique buff)
    {
        plr.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(ApplyBuffByIdMessage.Method_Public_Static_ApplyBuffByIdMessage_EnumPublicSealedvaSTCAGLCADITODIKNSLUnique_ArrayOf_Object_0(buff, null), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    public static void DebuffPlayer(Player plr, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique buff)
    {
        plr.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(VanishBuffMessage.Method_Public_Static_VanishBuffMessage_EnumPublicSealedvaSTCAGLCADITODIKNSLUnique_0(buff), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    public void SelectPlayer(Player plr)
    {
        PlayerInfo info = plr.prop_PlayerInfo_0;
        GuiStuff.playerName.text = info.displayName;
        GuiStuff.selectedPlayer.text = "Selected:\n" + info.displayName;
        selectedPlayer = plr;
    }

    public static void Kill(Player plr)
    {
        plr.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(DecideDeathMessage.Method_Public_Static_DecideDeathMessage_EnumPublicSealedvaNEKNBARE5vUnique_0(EnumPublicSealedvaNEKNBARE5vUnique.NEIGHBOR_KILL), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    public static void GhostKill(Player plr)
    {
        foreach (HoloNetPlayer p in HoloNetPlayer.prop_List_1_HoloNetPlayer_0) if (p.uniqueId._value != plr.prop_HoloNetObject_0.prop_HoloNetPlayer_0.uniqueId._value) plr.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_HoloNetPlayer_0(DecideDeathMessage.Method_Public_Static_DecideDeathMessage_EnumPublicSealedvaNEKNBARE5vUnique_0(EnumPublicSealedvaNEKNBARE5vUnique.NEIGHBOR_KILL), p);
    }

    private InventorySlotManager inv;
    public static Vector3 selectedPoint;
    private HoloNetObject selectedObject;
    public Player getLocalPlayer => GameController.prop_GameController_0.players.prop_Player_0;
    private static readonly Color ambient = new Color(0.051f, 0.136f, 0.206f);
    public Player localPlayer;
    //private ObjectPublicUILiUI1ObLi1ObUILiUnique netObjectManager => HoloNetAppModule.prop_ObjectPublicOb287259405257206727Unique_0.field_Public_ObjectPublicLi1GaDi2HoUIHoInUIUnique_0;
    public Player[] getPlayers => GameController.prop_GameController_0..prop_List_1_Player_0.ToArray();
    public List<Button> buttons = new List<Button>();
    private List<Button> spawnButtons = new List<Button>();
    public static List<Player> players;
    public static Player selectedPlayer;
    public GUIWindow currentItemWindow;
    public bool noClip = false;
    private bool spawnButtonsSpawned = false;
    private bool _menu = false;
    public bool menu
    {
        get => _menu;
        set
        {
            _menu = value;
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            GuiStuff.menu.enabled = value;
            GuiStuff.playerMenu.enabled = value;
            if (!value)
            {
                GuiStuff.spawnMenu.enabled = false;
                GuiStuff.playerSettingsMenu.enabled = false;
                GuiStuff.tpMenu.enabled = false;
            }
        }
    }

    private GUIStyle style = new GUIStyle();
}
