using UnityEngine;
using Verse;
using HarmonyLib;

namespace CaravansRestBeforeArrival
{
    public class CaravansRestBeforeArrivalMod : Mod
    {
        public const string PACKAGE_ID = "caravansrestbeforearrival.1trickPonyta";
        public const string PACKAGE_NAME = "Caravans Rest Before Arrival";

        public static CaravansRestBeforeArrivalSettings Settings;

        public CaravansRestBeforeArrivalMod(ModContentPack content) : base(content)
        {
            Settings = GetSettings<CaravansRestBeforeArrivalSettings>();

            var harmony = new Harmony(PACKAGE_ID);
            harmony.PatchAll();

            Log.Message($"[{PACKAGE_NAME}] Loaded.");
        }

        public override string SettingsCategory() => PACKAGE_NAME;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            CaravansRestBeforeArrivalSettings.DoSettingsWindowContents(inRect);
        }
    }
}
