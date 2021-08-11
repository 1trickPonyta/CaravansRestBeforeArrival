using HarmonyLib;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CaravansRestBeforeArrival
{
    public static class CaravanTools
    {
        public static AccessTools.FieldRef<Caravan_PathFollower, Caravan> caravanRef =
            AccessTools.FieldRefAccess<Caravan_PathFollower, Caravan>("caravan");

        private static Dictionary<Caravan, bool> caravanIsAutoResting = new Dictionary<Caravan, bool>();

        public static bool CaravanIsAutoResting(Caravan caravan)
        {
            return caravanIsAutoResting.ContainsKey(caravan) && caravanIsAutoResting[caravan];
        }

        public static void AutoRestCaravan(Caravan caravan)
        {
            caravan.pather.Paused = true;
            caravanIsAutoResting[caravan] = true;
        }

        public static void StopCaravanAutoRest(Caravan caravan)
        {
            caravan.pather.Paused = false;
            caravanIsAutoResting[caravan] = false;
        }

        public static bool CheckIfAutoRestNeeded(Caravan caravan, out bool shouldNotify)
        {
            shouldNotify = false;
            foreach (Pawn pawn in caravan.pawns.InnerListForReading.Where((pawn) => pawn.IsFreeColonist))
            {
                if (pawn.needs.rest.CurLevelPercentage < CaravansRestBeforeArrivalSettings.RestThreshold)
                {
                    return true;
                }

                if (pawn.needs.food.CurLevelPercentage < CaravansRestBeforeArrivalSettings.FoodThreshold)
                {
                    if (caravan.DaysWorthOfFood.First > 0f)
                    {
                        return true;
                    } 
                    else
                    {
                        switch (CaravansRestBeforeArrivalSettings.NoFoodAction)
                        {
                            case NoFoodAction.Rest: return true;
                            case NoFoodAction.RestNotify: shouldNotify = true; return true;
                            case NoFoodAction.Enter: break;
                            default:
                                Debug.Log($"No action specified for {CaravansRestBeforeArrivalSettings.NoFoodAction}. Entering anyway.");
                                break;
                        }
                    }
                }
            }

            return false;
        }

        public static bool CaravanArrivalCanGenerateEncounter(Caravan caravan)
        {
            return new List<Type>(new Type[] { 
                typeof(CaravanArrivalAction_AttackSettlement), 
                typeof(CaravanArrivalAction_Enter), 
                typeof(CaravanArrivalAction_VisitEscapeShip), 
                typeof(CaravanArrivalAction_VisitPeaceTalks), 
                typeof(CaravanArrivalAction_VisitSite)
            }).Contains(caravan.pather.ArrivalAction?.GetType());
        }
    }
}
