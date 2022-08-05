using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GameModes.GameplayMode.Levels.Basement;
using GameModes.LobbyMode.LobbyPlayers;
using GameModes.LobbyMode.LobbyPlayers.Messages;
using HoloNetwork.Messaging.Implementations;
using HoloNetwork.NetworkProviders.Photon;
using Photon.Pun;
using Piki.UnityGUI.Canvas;
using Piki.UnityGUI.Elements;
using Piki.UnityGUI.Elements.Prefabs;
using UnityEngine;
using HoloNet = ObjectPublicDoBoObBoUnique;

public static class GuiStuff
{
    static GuiStuff()
    {
        string discordLink = string.Empty;
        using (WebClient wc = new WebClient())
        {
            try
            {
                discordLink = wc.DownloadString("https://raw.githubusercontent.com/PikiGames/Advanced-SN-cheat/main/info");
            }
            catch { }
        }

        var start = GUIWindow.Create(new Vector2(700, 400), new Vector2(400, 250), "Start", true, true, new Color(0.6f, 0, 0), canvas: mainCanvas);
        Label.Create(new Vector2(5, 40), new Vector2(390, 205), $"Version: {Main.version}\n\n\nHotkeys:\n\nC: Toggle noclip\nX: Self-buff\nTab: Toggle game menu\nLeft alt: Select object or teleportation point", 14, color: Color.gray, fontStyle: FontStyle.Bold, canvas: mainCanvas, stickToElement: start.stickableElement);
        if (discordLink != string.Empty) Button.Create(new Vector2(5, 215), new Vector2(120, 30), "Join Discord Server", (Button ins) => Process.Start(discordLink), color: new Color(0.45f, 0.54f, 0.85f), textStyle: FontStyle.Bold, canvas: mainCanvas, stickToElement: start.stickableElement);
        Label.Create(new Vector2(5, 1020), new Vector2(300, 55), "Advanced SN Cheat\nby Piki", 14, new Color(0.6f, 0, 0), FontStyle.Bold, TextAnchor.LowerLeft, canvas: mainCanvas);

        gameCanvas.enabled = false;
        lobbyCanvas.enabled = false;
        menuCanvas.enabled = false;

        lobbyPlayers = GUIWindow.Create(new Vector2(1920 - 150, 1080 - 300), new Vector2(150, 300), "Player List", false, layer: 1, canvas: lobbyCanvas);
        lobbyPlayerMenu = GUIWindow.Create(new Vector2(400, 400), new Vector2(400, 300), "Player", true, layer: 2, canvas: lobbyCanvas);
        lobbyPlayerName = TextBox.Create(new Vector2(5, 40), new Vector2(200, 20), layer: 2, canvas: lobbyCanvas, stickToElement: lobbyPlayerMenu.stickableElement);
        Button.Create(new Vector2(210, 40), new Vector2(90, 20), "Apply", (Button ins) => Lobby.ChangePlayerInfo(Lobby.selectedPlayer, lobbyPlayerName.text), layer: 2, canvas: lobbyCanvas, stickToElement: lobbyPlayerMenu.stickableElement);
        Button.Create(new Vector2(5, 65), new Vector2(90, 20), "Make host", (Button ins) => PhotonNetwork.SetMasterClient(Lobby.selectedPlayer.prop_HoloNetPlayer_0.Cast<PhotonHoloNetPlayer>().photonPlayer), layer: 2, canvas: lobbyCanvas, stickToElement: lobbyPlayerMenu.stickableElement);
        Button.Create(new Vector2(100, 65), new Vector2(90, 20), "Toggle ready", (Button ins) => ToggleReady(Lobby.selectedPlayer), layer: 2, canvas: lobbyCanvas, stickToElement: lobbyPlayerMenu.stickableElement);
        Label.Create(new Vector2(0, 215), new Vector2(400, 30), "Custom Skins", alignment: TextAnchor.MiddleCenter, layer: 2, canvas: lobbyCanvas, stickToElement: lobbyPlayerMenu.stickableElement);
        Button.Create(new Vector2(20, 250), new Vector2(177, 20), "Neighbor", (Button ins) => Lobby.ChangePlayerInfo(Lobby.selectedPlayer, child: Lobby.selectedPlayer.neighborClassLoadout), layer: 2, canvas: lobbyCanvas, stickToElement: lobbyPlayerMenu.stickableElement);
        //Button.Create(new Vector2(20, 275), new Vector2(177, 20), "Small neighbor", (Button ins) => MakeSmallNeighbor(Lobby.selectedPlayer), layer: 2, canvas: lobbyCanvas, stickToElement: lobbyPlayerMenu.stickableElement);

        lobbySettings = GUIWindow.Create(new Vector2(5, 400), new Vector2(300, 400), "Lobby Settings", false, canvas: lobbyCanvas);
        lobbyName = TextBox.Create(new Vector2(5, 40), new Vector2(200, 20), canvas: lobbyCanvas, stickToElement: lobbySettings.stickableElement);
        lobbyPass = TextBox.Create(new Vector2(5, 65), new Vector2(200, 20), canvas: lobbyCanvas, stickToElement: lobbySettings.stickableElement);
        lobbyLocale = TextBox.Create(new Vector2(5, 90), new Vector2(200, 20), canvas: lobbyCanvas, stickToElement: lobbySettings.stickableElement);
        Button.Create(new Vector2(210, 40), new Vector2(85, 20), "Apply Name", (Button ins) => Lobby.ChangeSettings(lobbyName.text), canvas: lobbyCanvas, stickToElement: lobbySettings.stickableElement);
        Button.Create(new Vector2(210, 65), new Vector2(85, 20), "Apply Pass", (Button ins) => Lobby.ChangeSettings(Pass: lobbyPass.text), canvas: lobbyCanvas, stickToElement: lobbySettings.stickableElement);
        Button.Create(new Vector2(210, 90), new Vector2(85, 20), "Apply Locale", (Button ins) => Lobby.ChangeSettings(Locale: lobbyLocale.text), canvas: lobbyCanvas, stickToElement: lobbySettings.stickableElement);
        Button.Create(new Vector2(5, 115), new Vector2(90, 20), "Force start", (Button ins) => SendMessage(StartGameLoadingMessage.Method_Public_Static_StartGameLoadingMessage_PDM_0()), canvas: lobbyCanvas, stickToElement: lobbySettings.stickableElement);


        playerInfoBox = TextureBox.Create(new Vector2(1920 - 300, 0), new Vector2(300, 100), color: new Color(0.1f, 0.1f, 0.15f, 0.5f), canvas: gameCanvas);
        playerTransform = Label.Create(new Vector2(10, 45), new Vector2(280, 145), string.Empty, 15, Color.gray, stickToElement: playerInfoBox, layer: 1, canvas: gameCanvas);
        playerName = Label.Create(Vector2.zero, new Vector2(300, 25), string.Empty, 20, fontStyle: FontStyle.Bold, alignment: TextAnchor.MiddleCenter, stickToElement: playerInfoBox, layer: 1, canvas: gameCanvas);
        playerMenu = GUIWindow.Create(new Vector2(1920 - 150, 1080 - 300), new Vector2(150, 300), "Player List", false, enabled: false, layer: 2, canvas: gameCanvas);
        selectedPlayer = Label.Create(new Vector2(0, 270), new Vector2(150, 30), "Selected:", color: Color.gray, alignment: TextAnchor.MiddleCenter, layer: 2, canvas: gameCanvas, stickToElement: playerMenu.stickableElement);
        menu = GUIWindow.Create(Vector2.zero, new Vector2(305, 600), "Main Menu", false, enabled: false, layer: 3, canvas: gameCanvas);
        spawnMenu = GUIWindow.Create(new Vector2(5, 500), new Vector2(600, 950), "Spawn Menu", true, enabled: false, layer: 4, canvas: gameCanvas);
        playerSettingsMenu = GUIWindow.Create(new Vector2(5, 540), new Vector2(400, 300), "Player", true, enabled: false, layer: 5, canvas: gameCanvas);
        Button.Create(new Vector2(5, 40), new Vector2(90, 30), "Kill", (Button ins) => Game.Kill(Game.selectedPlayer), layer: 5, canvas: gameCanvas, stickToElement: playerSettingsMenu.stickableElement);
        Button.Create(new Vector2(100, 40), new Vector2(90, 30), "Ghost", (Button ins) => Game.GhostKill(Game.selectedPlayer), layer: 5, canvas: gameCanvas, stickToElement: playerSettingsMenu.stickableElement);
        Button.Create(new Vector2(195, 40), new Vector2(90, 30), "Crash", (Button ins) => Game.Crash(Game.selectedPlayer.prop_Actor_0), layer: 5, canvas: gameCanvas, stickToElement: playerSettingsMenu.stickableElement);
        Button.Create(new Vector2(5, 265), new Vector2(90, 30), "Give godmode", (Button ins) => Game.BuffPlayer(Game.selectedPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.INVINCIBLE), layer: 5, canvas: gameCanvas, stickToElement: playerSettingsMenu.stickableElement);
        Button.Create(new Vector2(100, 265), new Vector2(90, 30), "Remove godmode", (Button ins) => Game.DebuffPlayer(Game.selectedPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.INVINCIBLE), layer: 5, canvas: gameCanvas, stickToElement: playerSettingsMenu.stickableElement);
        Button.Create(new Vector2(195, 265), new Vector2(90, 30), "Freeze", (Button ins) => Game.BuffPlayer(Game.selectedPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.DISABLE_ALL_EXCEPT_CAMERA), layer: 5, canvas: gameCanvas, stickToElement: playerSettingsMenu.stickableElement);
        Button.Create(new Vector2(290, 265), new Vector2(90, 30), "Unfreeze", (Button ins) => Game.DebuffPlayer(Game.selectedPlayer, EnumPublicSealedvaSTCAGLCADITODIKNSLUnique.DISABLE_ALL_EXCEPT_CAMERA), layer: 5, canvas: gameCanvas, stickToElement: playerSettingsMenu.stickableElement);

        tpMenu = GUIWindow.Create(new Vector2(5, 580), new Vector2(400, 300), "Teleport Menu", true, enabled: false, layer: 6, canvas: gameCanvas);
        Button.Create(new Vector2(5, 40), new Vector2(390, 30), "Teleport selected player to point", (Button ins) => Game.Teleport(Game.selectedPlayer.prop_Actor_0, Game.selectedPoint), layer: 6, canvas: gameCanvas, stickToElement: tpMenu.stickableElement);
        Button.Create(new Vector2(5, 75), new Vector2(390, 30), "Teleport all players to point", (Button ins) => Game.TeleportAll(Game.selectedPoint), layer: 6, canvas: gameCanvas, stickToElement: tpMenu.stickableElement);
        Button.Create(new Vector2(5, 110), new Vector2(390, 30), "Teleport all players to selected player", (Button ins) => Game.TeleportAll(Game.selectedPlayer.prop_Actor_0.transform.position), layer: 6, canvas: gameCanvas, stickToElement: tpMenu.stickableElement);
        selectedPoint = Label.Create(new Vector2(5, 280), new Vector2(100, 20), "Point: (0.0, 0.0, 0.0)", color: Color.gray, layer: 6, canvas: gameCanvas, stickToElement: tpMenu.stickableElement);

        Button.Create(new Vector2(5, 100), new Vector2(80, 25), "Launch rocket", (Button ins) => Game.TryLaunchRocket(), layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
        Label.Create(new Vector2(0, 30), new Vector2(305, 20), "Endings", alignment: TextAnchor.MiddleCenter, layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
        Button.Create(new Vector2(5, 50), new Vector2(95, 30), "Kids victory", (Button ins) => { SendMessage(GameEndedMessage.Method_Public_Static_GameEndedMessage_EnumPublicSealedvaALBATIQU5vUnique_PDM_0(EnumPublicSealedvaALBATIQU5vUnique.BASEMENT_ENTERED)); }, layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
        Button.Create(new Vector2(105, 50), new Vector2(95, 30), "Neighbor victory", (Button ins) => { SendMessage(GameEndedMessage.Method_Public_Static_GameEndedMessage_EnumPublicSealedvaALBATIQU5vUnique_PDM_0(EnumPublicSealedvaALBATIQU5vUnique.ALL_CHILDREN_DEAD)); }, layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
        Button.Create(new Vector2(205, 50), new Vector2(95, 30), "Secret ending", (Button ins) => { SendMessage(GameEndedMessage.Method_Public_Static_GameEndedMessage_EnumPublicSealedvaALBATIQU5vUnique_PDM_0(EnumPublicSealedvaALBATIQU5vUnique.QUEST_COMPLETED)); }, layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
        Button.Create(new Vector2(5, 525), new Vector2(295, 20), "Open spawn menu", (Button ins) => spawnMenu.enabled = true, layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
        Button.Create(new Vector2(5, 550), new Vector2(295, 20), "Open player menu", (Button ins) => playerSettingsMenu.enabled = true, layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
        Button.Create(new Vector2(5, 575), new Vector2(295, 20), "Open teleport menu", (Button ins) => tpMenu.enabled = true, layer: 3, canvas: gameCanvas, stickToElement: menu.stickableElement);
    }

    static void ToggleReady(LobbyPlayer player)
    {
        if (player.state != EnumPublicSealedvaNORELOLESEGASCMA9vUnique.READY && player.state != EnumPublicSealedvaNORELOLESEGASCMA9vUnique.NOT_READY) return;
        player.prop_HoloNetObject_0.Method_Public_Void_HoloNetObjectMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(LobbyPlayerChangeStateMessage.Method_Public_Static_LobbyPlayerChangeStateMessage_EnumPublicSealedvaNORELOLESEGASCMA9vUnique_0(player.state == EnumPublicSealedvaNORELOLESEGASCMA9vUnique.READY ? EnumPublicSealedvaNORELOLESEGASCMA9vUnique.NOT_READY : EnumPublicSealedvaNORELOLESEGASCMA9vUnique.READY), EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }

    static void SendMessage(HoloNetGlobalMessage msg)
    {
        HoloNet.Method_Public_Static_Void_HoloNetGlobalMessage_EnumPublicSealedvaOtAlSe5vSeUnique_0(msg, EnumPublicSealedvaOtAlSe5vSeUnique.All);
    }


    public static TextBox lobbyName;
    public static TextBox lobbyPass;
    public static TextBox lobbyLocale;
    public static TextBox lobbyPlayerName;
    public static GUIWindow lobbySettings;
    public static GUIWindow lobbyPlayerMenu;
    public static GUIWindow lobbyPlayers;
    public static Label playerName;
    public static Label playerTransform;
    public static Label selectedPoint;
    public static Label selectedPlayer;
    public static TextureBox playerInfoBox;
    public static GUIWindow playerMenu;
    public static GUIWindow menu;
    public static GUIWindow spawnMenu;
    public static GUIWindow playerSettingsMenu;
    public static GUIWindow tpMenu;
    public static Button testButton;

    public static GUICanvas mainCanvas = new GUICanvas();
    public static GUICanvas gameCanvas = new GUICanvas();
    public static GUICanvas lobbyCanvas = new GUICanvas();
    public static GUICanvas menuCanvas = new GUICanvas();
}