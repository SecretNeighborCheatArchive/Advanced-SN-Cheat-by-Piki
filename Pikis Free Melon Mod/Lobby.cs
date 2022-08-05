using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Piki.UnityGUI.Elements;
using UnityEngine;
using GameModes.LobbyMode;
using GameModes.LobbyMode.LobbyPlayers;
using BackEnd;
using HoloNetwork.NetworkProviders.Photon;
using GameModes.Shared.Loadouts.Classes;
using GameModes.LobbyMode.LobbyPlayers.Messages;

public class Lobby : IGameMode
{
    public void Start()
    {
        GuiStuff.lobbyName.text = PhotonNetwork.CurrentRoom.CustomProperties["C2"].ToString();
        GuiStuff.lobbyPass.text = PhotonNetwork.CurrentRoom.CustomProperties["C3"].ToString();
        GuiStuff.lobbyLocale.text = PhotonNetwork.CurrentRoom.CustomProperties["C1"].ToString();
        SelectPlayer(localPlayer);
        RefreshButtons(getPlayers);
    }

    public static void ChangeSettings(string Name = null, string Locale = null, string Pass = null, byte? maxPlayers = null)
    {
        Room room = PhotonNetwork.CurrentRoom;
        Hashtable hashtable = room.CustomProperties;
        if (Name != null) hashtable["C2"] = Name; //name
        if (Pass != null) hashtable["C3"] = Pass; //password
        if (Locale != null) hashtable["C1"] = Locale; //locale
        if (Name != null) hashtable["N"] = Name; //name
        if (Pass != null) hashtable["P"] = Pass; //password
        room.SetCustomProperties(hashtable);
        if (maxPlayers.HasValue)
        {
            room.MaxPlayers = maxPlayers.Value;
        }
    }

    private void ChangePlayerWindow(LobbyPlayer plr)
    {
        GuiStuff.lobbyPlayerMenu.enabled = true;
        PlayerInfo info = plr.playerInfo;
        GuiStuff.lobbyPlayerMenu.title = info.displayName;
        GuiStuff.lobbyPlayerName.text = info.displayName;
    }

    public void RefreshSelectedPlayerWindow(LobbyPlayer plr)
    {
        var btn = buttons.FirstOrDefault(x => (LobbyPlayer)x.customObject == plr);
        if (btn != null) btn.title = plr.playerInfo.displayName;
        if (plr != selectedPlayer) return;
        GuiStuff.lobbyPlayerMenu.title = plr.playerInfo.displayName;
    }

    public static void ChangePlayerInfo(LobbyPlayer plr, string name = null, string id = null, ActorClassLoadout child = null, ActorClassLoadout neighbor = null)
    {
        PlayerInfo i = plr.playerInfo;
        if (name != null) i.displayName = name;
        if (id != null) i.playerID = id;
        ActorClassLoadout c = child == null ? plr.explorerClassLoadout : child;
        ActorClassLoadout n = neighbor == null ? plr.neighborClassLoadout : neighbor;
        plr.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(LobbyPlayerSyncInfoMessage.Method_Public_Static_LobbyPlayerSyncInfoMessage_PlayerInfo_ActorClassLoadout_ActorClassLoadout_PDM_0(i, c, n), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    public void RefreshButtons(Il2CppSystem.Collections.Generic.List<LobbyPlayer> plrs)
    {
        foreach (var b in buttons) b.canvas = null;
        buttons.Clear();
        float a = 35f;
        foreach (LobbyPlayer plr in plrs)
        {
            buttons.Add(Button.Create(new Vector2(10, a), new Vector2(130, 35), plr.playerInfo.displayName, OnBtnClick, color: plr.prop_HoloNetPlayer_0.prop_Boolean_1 ? new Color(0.5f, 0.5f, 0f) : new Color(0.2f, 0.2f, 0.3f), customObject: plr, layer: 1, canvas: GuiStuff.lobbyCanvas, stickToElement: GuiStuff.lobbyPlayers.stickableElement));
            a += 40f;
        }
        lastPlayerCount = plrs.Count;
    }

    public void SelectPlayer(LobbyPlayer plr)
    {
        selectedPlayer = plr;
        ChangePlayerWindow(plr);
    }

    private void OnBtnClick(Button ins)
    {
        LobbyPlayer plr = (LobbyPlayer)ins.customObject;
        SelectPlayer(plr);
    }

    public List<Button> buttons = new List<Button>();
    public static LobbyPlayer selectedPlayer;
    private string lastOwner;
    public LobbyPlayer localPlayer => LobbyController.prop_LobbyController_0.players.prop_LobbyPlayer_0;
    private int lastPlayerCount = 0;
    public Il2CppSystem.Collections.Generic.List<LobbyPlayer> getPlayers => LobbyController.prop_LobbyController_0.players.prop_List_1_LobbyPlayer_0;

    public void Update()
    {
        if (Time.frameCount % 5 != 0 || !PhotonNetwork.InRoom) return;
        var plrs = getPlayers;
        if (plrs.Count != lastPlayerCount) RefreshButtons(plrs);
        string currentOwner = PhotonNetwork.MasterClient.UserId;
        if (lastOwner != currentOwner)
        {
            lastOwner = currentOwner;
            RefreshButtons(plrs);
        }
    }
}
