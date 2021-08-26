using BepInEx;
using HarmonyLib;
using UnityEngine;
using System;
using System.Reflection;
using HarmonyLib.Tools;
using BepInEx.Harmony;

namespace randomization
{
    [BepInProcess("Stick It To The (Stick) Man.exe")]
    [BepInPlugin("uniquename.sittsm.enable-cheats", "enable-cheats", "0.0.0.0")]
    public class cheats_enable : BaseUnityPlugin
    {

        public void Awake()
        {
            var harmony = new Harmony("uniquename.sittsm.enable-cheats");
            harmony.PatchAll();
        }

        public const string modID = "uniquename.sittsm.enable-cheats";
        public const string modName = "enable-cheats";
    }

	[HarmonyPatch(typeof(PauseMenuContoller), "ToggleMenu")]
	public class patch
    {
        public static bool Prefix(bool menuIsOpen, PauseMenuContoller __instance, ref GameObject ____upgradeScreen, ref PassiveManager ____passiveManager, ref MutateManager ____mutateManager, ref UpgradeManager ____upgradeManager)
		{ 
			if (menuIsOpen)
			{
				__instance.numberOfClicks = 0;
				if (SingletonBehaviour<SpecialAbilities>.HasInstance)
				{
					SingletonBehaviour<SpecialAbilities>.Instance.CancelAbilities();
				}
				SingletonBehaviour<TimeManager>.Instance.LerpTimeScaleTo(0.02f);
				__instance.isMenuOpen = true;
				int penis = UnityEngine.Random.Range(1, 3);
				if (penis == 1)
				{
					____upgradeScreen.SetActive(true);
					____passiveManager.Choose3PassCardUpgrades();
				}
				else if (penis == 2)
				{
					SingletonBehaviour<MoveUIManager>.Instance.RemoveX();
					bool flag = false;
					SingletonBehaviour<MutateManager>.Instance.ResetNumMutatingPlayers();
					for (int i = 0; i < SingletonBehaviour<PlayerManager>.Instance.totalPlayers; i++)
					{
						SingletonBehaviour<MutateManager>.Instance.ToggleUI(false, i);
					}
					for (int j = 0; j < SingletonBehaviour<PlayerManager>.Instance.players.Count; j++)
					{
						int playerNumber = SingletonBehaviour<PlayerManager>.Instance.players[j].PlayerInput._playerNumber;
						if (____mutateManager.CanMutate(playerNumber))
						{
							SingletonBehaviour<MoveUIManager>.Instance.MoveUp(360);
							____mutateManager.SetMutateScreen(playerNumber);
							SingletonBehaviour<MoveUIManager>.Instance.RemoveX();
							flag = true;
						}
					}
					if (!flag)
					{
						____upgradeScreen.SetActive(true);
						____upgradeManager.Choose3UpgradeCards(false);
						for (int k = 0; k < SingletonBehaviour<PlayerManager>.Instance.totalPlayers; k++)
						{
							SingletonBehaviour<MoveUIManager>.Instance.MoveDown(k);
						}
					}
				}
				else
				{
					SingletonBehaviour<MoveUIManager>.Instance.RemoveX();
					____upgradeScreen.SetActive(true);
					____upgradeManager.Choose3UpgradeCards(false);
					for (int l = 0; l < SingletonBehaviour<PlayerManager>.Instance.totalPlayers; l++)
					{
						SingletonBehaviour<MoveUIManager>.Instance.MoveDown(l);
					}
				}
				SingletonBehaviour<MoveUIManager>.Instance.IsMoveSelectable();
				return false;
			}
			SingletonBehaviour<TimeManager>.Instance.LerpTimeScaleTo(1f);
			SingletonBehaviour<UpgradeManager>.Instance.upgradeScreenStates = UpgradeScreenStates.Normal;
			____upgradeManager.RemoveUpgradeCards();
			____passiveManager.RemovePassiveCards();
			____mutateManager.isMutation = false;
			SingletonBehaviour<MutateManager>.Instance.ResetNumMutatingPlayers();
			____upgradeScreen.SetActive(false);
			for (int m = 0; m < SingletonBehaviour<PlayerManager>.Instance.totalPlayers; m++)
			{
				____mutateManager.SetMutateScreen(m);
				SingletonBehaviour<MoveUIManager>.Instance.MoveDown(m);
			}
			SingletonBehaviour<MoveUIManager>.Instance.IsMoveSelectable();
			__instance.isMenuOpen = false;
	
			return false;
        }
    }
}
