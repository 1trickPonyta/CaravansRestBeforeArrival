using RimWorld.Planet;
using HarmonyLib;
using Verse;
using RimWorld;

namespace CaravansRestBeforeArrival
{
    // Removes the vanilla threat points cap and replaces it with the mod's threat points cap

    [HarmonyPatch(typeof(Caravan_PathFollower))]
    [HarmonyPatch("TryEnterNextPathTile")]
    public static class Patch_Caravan_PathFollower_TryEnterNextPathTile
    {
        public static bool Prefix(Caravan_PathFollower __instance)
        {
            Caravan caravan = CaravanTools.caravanRef(__instance);

            if (!__instance.Paused && CaravanTools.CaravanArrivalCanGenerateEncounter(caravan) && __instance.nextTile == __instance.Destination)
            {
                // The caravan is not resting, so if we have it tracked as auto resting, that 
                // means the player turned off rest manually and we should proceed
                if (CaravanTools.CaravanIsAutoResting(caravan))
                {
                    CaravanTools.StopCaravanAutoRest(caravan);
                    return true;
                }

                // We are about to enter the destination map, so we check our needs first
                if (CaravanTools.CheckIfAutoRestNeeded(caravan, out bool shouldNotify))
                {
                    CaravanTools.AutoRestCaravan(caravan);
                    if (shouldNotify)
                    {
                        Find.LetterStack.ReceiveLetter("CaravansRestBeforeArrival_NoFoodLetterLabel".Translate(), 
                                "CaravansRestBeforeArrival_NoFoodLetterText".Translate(caravan.Label, CaravansRestBeforeArrivalSettings.FoodThreshold.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Absolute)), 
                                LetterDefOf.NeutralEvent, caravan);
                    }
                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Caravan_PathFollower))]
    [HarmonyPatch("PatherTick")]
    public static class Patch_Caravan_PathFollower_PatherTick
    {
        public static void Postfix(Caravan_PathFollower __instance)
        {
            Caravan caravan = CaravanTools.caravanRef(__instance);
            if (CaravanTools.CaravanIsAutoResting(caravan) && !CaravanTools.CheckIfAutoRestNeeded(caravan, out bool shouldNotify))
            {
                // Auto rest is no longer needed, so stop resting
                // This will usually cause the caravan to enter the map on the next tick
                CaravanTools.StopCaravanAutoRest(caravan);
            }
        }
    }
}
