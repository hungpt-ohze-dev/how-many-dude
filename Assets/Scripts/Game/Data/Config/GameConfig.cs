using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game", order = 1)]
public class GameConfig : ScriptableObject
{
    [FoldoutGroup("Level")]
    [VerticalGroup("Level/Element")] public int MapMax = 2;
    [VerticalGroup("Level/Element")] public int LevelMax = 20;
    [VerticalGroup("Level/Element")] public int LevelNextToHome = 4;
    [VerticalGroup("Level/Element")] public int LevelCoinReward = 20;
    [VerticalGroup("Level/Element")] public List<int> ListLevelHard = new();

    [FoldoutGroup("GamePlay")]
    [VerticalGroup("GamePlay/Element")] public int GamePlaySecondLeftWarning = 15;
    [VerticalGroup("GamePlay/Element")] public int GamePlayIceCountDown = 3;
    [VerticalGroup("GamePlay/Element")] public bool GamePlayShowHand = false;
    [VerticalGroup("GamePlay/Element")] public int GamePlayCamSize = 30;
    [VerticalGroup("GamePlay/Element")] public float GamePlayCamWidth = 10f;
    [VerticalGroup("GamePlay/Element")] public int GamePlayCreative = 1;

    [FoldoutGroup("Item")]
    [VerticalGroup("Item/Element")] public List<int> ListItemNonMatch = new();

    [FoldoutGroup("Revive")]
    [VerticalGroup("Revive/Element")] public int ReviveAddTime = 60;
    [VerticalGroup("Revive/Element")] public int ReviveCost = 300;

    [FoldoutGroup("Booster")]
    [VerticalGroup("Booster/Element")] public List<BoosterData> Boosters = new();
    [Space(10)]
    [VerticalGroup("Booster/Element")] public float BoosterDelay = 2f;
    [VerticalGroup("Booster/Element")] public int BoosterTimeAdd = 20;
    [VerticalGroup("Booster/Element")] public int BoosterTutAdd = 2;


    [FoldoutGroup("Helper")]
    [VerticalGroup("Helper/Element")] public float HelperTimeShowSuggestion = 10f;

    [FoldoutGroup("Ads")]
    [VerticalGroup("Ads/Element")] public int AdsAddTimeUnlockContainer = 20;

    [FoldoutGroup("Shipper")]
    [VerticalGroup("Shipper/Element")] public int ShipperSecondShowAds = 10;
    [VerticalGroup("Shipper/Element")] public int ShipperSecondLeftWarning = 10;
    [VerticalGroup("Shipper/Element")] public int ShipperReviveCost = 300;
    [VerticalGroup("Shipper/Element")] public int ShipperReviveTimeAdd = 30;

    [FoldoutGroup("Resource")]
    [VerticalGroup("Resource/Element")] public int ResourceMaxHeart = 5;
    [VerticalGroup("Resource/Element")] public int ResourceSecondAddHeart = 60;

    [FoldoutGroup("Heart")]
    [VerticalGroup("Heart/Element")] public int HeartCoinCost = 10;
    [VerticalGroup("Heart/Element")] public int HeartCoinAdd = 1;
    [VerticalGroup("Heart/Element")] public int HeartAdsAdd = 3;
    [VerticalGroup("Heart/Element")] public int HeartMax = 13;

    [FoldoutGroup("Shop")]
    [VerticalGroup("Shop/Element")] public int ShopOffer_FreeCoinHourCountDown = 3;

    [FoldoutGroup("PiggyBank")]
    [VerticalGroup("PiggyBank/Element")] public int PiggyBankMin = 360;
    [VerticalGroup("PiggyBank/Element")] public int PiggyBankMax = 720;

	[FoldoutGroup("GoldRush")]
    [VerticalGroup("GoldRush/Element")] public int GoldRushSecondCooldown = 7200;
    [VerticalGroup("GoldRush/Element")] public int GoldRushCappingTime = 1800;

    [FoldoutGroup("DailyReward")]
    [VerticalGroup("DailyReward/Element")] public int DailyRewardMaxDay = 7;

    [FoldoutGroup("StarterPack")]
    [VerticalGroup("StarterPack/Element")] public int StarterPackCappingTimeShow = 180;

    [FoldoutGroup("SpecialPack")]
    [VerticalGroup("SpecialPack/Element")] public int SpecialPackSecondCooldown = 3600;
    [VerticalGroup("SpecialPack/Element")] public int SpecialPackCappingTimeShow = 3600;
}

