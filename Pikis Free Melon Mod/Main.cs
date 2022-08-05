using AppControllers;
using MelonLoader;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Gamemode = EnumPublicSealedvaNOGALOMEPRGAMASHCRUnique;

public class Main : MelonMod
{
    public Main()
    {
        Keys.currentKeys = GetKeys();
        ins = this;
    }

    public void SaveKeys(Keys keys)
    {
        if (File.Exists(keysSaveFile)) File.Delete(keysSaveFile);
        if (keys == default) return;
        try
        {
            Stream stream = new FileStream(keysSaveFile, FileMode.Create, FileAccess.Write, FileShare.None);
            new BinaryFormatter().Serialize(stream, keys);
            stream.Close();
        }
        catch { }
    }

    public Keys GetKeys()
    {
        if (!File.Exists(keysSaveFile)) return new Keys();
        try
        {
            Stream stream = new FileStream(keysSaveFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (Keys)new BinaryFormatter().Deserialize(stream);
        }
        catch
        {
            File.Delete(keysSaveFile);
            return new Keys();
        }
    }

    public override void OnGUI()
    {
        if (gamemodeBeforeUpdate == Gamemode.GAMEPLAY) game.OnGUI();
        GuiStuff.gameCanvas.Draw();
        GuiStuff.lobbyCanvas.Draw();
        GuiStuff.menuCanvas.Draw();
        GuiStuff.mainCanvas.Draw();
    }

    public override void OnUpdate()
    {
        Gamemode gm = currentGamemode;
        if (gm != gamemodeBeforeUpdate)
        {
            GuiStuff.gameCanvas.enabled = false;
            GuiStuff.lobbyCanvas.enabled = false;
            GuiStuff.menuCanvas.enabled = false;
            switch (gm)
            {
                case Gamemode.GAMEPLAY: game.Start(); GuiStuff.gameCanvas.enabled = true; break;
                case Gamemode.LOBBY: lobby.Start(); GuiStuff.lobbyCanvas.enabled = true; break;
            }
        }
        gamemodeBeforeUpdate = gm;
        switch (gm)
        {
            case Gamemode.GAMEPLAY: game.Update(); break;
            case Gamemode.LOBBY: lobby.Update(); break;
        }
        GuiStuff.gameCanvas.Update();
        GuiStuff.lobbyCanvas.Update();
        GuiStuff.menuCanvas.Update();
        GuiStuff.mainCanvas.Update();
    }

    //public override void OnFixedUpdate()
    //{
    //    Gamemode gm = currentGamemode;
    //    switch (gm)
    //    {
    //        case Gamemode.GAMEPLAY: game.FixedUpdate(); break;
    //    }
    //}

    public const string version = "0.2";
    public static Main ins;
    public Game game = new Game();
    public Lobby lobby = new Lobby();
    public static string keysSaveFile = Path.Combine(Directory.GetCurrentDirectory(), "Cheat keys");

    private Gamemode currentGamemode => AppController.prop_AppController_0.modes.prop_EnumPublicSealedvaNOGALOMEPRGAMASHCRUnique_0;
    public static Gamemode gamemodeBeforeUpdate;
}
