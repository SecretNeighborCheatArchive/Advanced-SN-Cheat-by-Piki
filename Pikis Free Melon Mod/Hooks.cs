using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using GameModes.GameplayMode.Interactables.InventoryItems;
using GameModes.LobbyMode;
using GameModes.LobbyMode.LobbyPlayers;
using GameModes.LobbyMode.LobbyPlayers.Messages;
using HarmonyLib;
using System;
using Ui.Screens.CustomGame;

[HarmonyPatch(typeof(CustomGameScreen), MethodType.Normal)]
[HarmonyPatch("Method_Private_Void_List_1_Object1PublicStInStBoUnique_0")]
class AntiPass
{
    static bool Prefix(ref CustomGameScreen __instance)
    {
        LobbyController.prop_LobbyController_0.Method_Public_Void_ObjectPublicStInStBoObStBoInBoStUnique_0(__instance.field_Private_ObjectPublicStInStBoObStBoInBoStUnique_0);
        return false;
    }
}

[HarmonyPatch(typeof(LobbyPlayer), MethodType.Normal)]
[HarmonyPatch("Method_Private_Void_LobbyPlayerSyncInfoMessage_PDM_0")]
class OnPlayerInfoChange
{
    static void Postfix(ref LobbyPlayer __instance)
    {
        if (Main.gamemodeBeforeUpdate == EnumPublicSealedvaNOGALOMEPRGAMASHCRUnique.LOBBY) Main.ins.lobby.RefreshSelectedPlayerWindow(__instance);
    }
}

[HarmonyPatch(typeof(LobbyPlayer), MethodType.Normal)]
[HarmonyPatch("Method_Public_Void_KickPlayerMessage_PDM_0")]
class OnKick
{
    static bool Prefix()
    {
        return false;
    }
}

[HarmonyPatch(typeof(RifleInventoryItem), MethodType.Normal)]
[HarmonyPatch("Method_Protected_Void_ShootMessage_PDM_0")]
class RifleInfAmmo
{
    static void Prefix(ref RifleInventoryItem __instance)
    {
        __instance.cameraShakeAmount = 0;
    }

    static void Postfix(ref RifleInventoryItem __instance)
    {
        __instance.field_Protected_ObscuredBool_0 = true;
    }
}

//[HarmonyPatch(typeof(Object1PublicAbstractAcObLiOb1LeObBoAcInUnique), MethodType.Normal)]
//[HarmonyPatch("Method_Public_get_Boolean_1")]
//class FuckSkillCooldowns
//{
//    static bool Prefix(ref bool __result)
//    {
//        __result = false;
//        return false;
//    }
//}