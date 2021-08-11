using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace CaravansRestBeforeArrival
{
    public enum NoFoodAction
    {
        Rest,
        RestNotify,
        Enter
    }

    public class CaravansRestBeforeArrivalSettings : ModSettings
    {
        public static float RestThreshold = 0.4f;
        public static float FoodThreshold = 0.4f;
        public static NoFoodAction NoFoodAction = NoFoodAction.RestNotify;

        public static void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            listingStandard.Label("CaravansRestBeforeArrival_DoNotEnterIf".Translate());
            listingStandard.Gap();

            Rect rect0 = listingStandard.GetRect(30f);
            rect0.SplitVertically(100f, out Rect rect1, out Rect rect2);
            rect2.SplitVertically(300f, out Rect rect3, out Rect rect4);
            Widgets.Label(rect1, "CaravansRestBeforeArrival_RestThreshold".Translate());
            int restThresholdPercent = (int) GenMath.RoundTo(RestThreshold * 100, 1f);
            int previousRestThresholdPercent = restThresholdPercent;
            string restThresholdBuffer = restThresholdPercent.ToString();
            Widgets.IntEntry(rect3, ref restThresholdPercent, ref restThresholdBuffer);
            if (restThresholdPercent != previousRestThresholdPercent)
            {
                RestThreshold = Mathf.Clamp(GenMath.RoundTo(restThresholdPercent / 100f, 0.01f), 0f, 1f);
            }
            Widgets.Label(rect4, " %");
            listingStandard.Gap();

            Rect rect5 = listingStandard.GetRect(30f);
            rect5.SplitVertically(100f, out Rect rect6, out Rect rect7);
            rect7.SplitVertically(300f, out Rect rect8, out Rect rect9);
            Widgets.Label(rect6, "CaravansRestBeforeArrival_FoodThreshold".Translate());
            int foodThresholdPercent = (int)GenMath.RoundTo(FoodThreshold * 100, 1f);
            int previousFoodThresholdPercent = foodThresholdPercent;
            string foodThresholdBuffer = foodThresholdPercent.ToString();
            Widgets.IntEntry(rect8, ref foodThresholdPercent, ref foodThresholdBuffer);
            if (foodThresholdPercent != previousFoodThresholdPercent)
            {
                FoodThreshold = Mathf.Clamp(GenMath.RoundTo(foodThresholdPercent / 100f, 0.01f), 0f, 1f);
            }
            Widgets.Label(rect9, " %");
            listingStandard.Gap();

            if (listingStandard.ButtonTextLabeled("CaravansRestBeforeArrival_NoFoodAction".Translate(), labelForNoFoodAction(NoFoodAction)))
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                foreach (NoFoodAction noFoodAction in Enum.GetValues(typeof(NoFoodAction)))
                {
                    list.Add(new FloatMenuOption(labelForNoFoodAction(noFoodAction), delegate ()
                    {
                        NoFoodAction = noFoodAction;
                    }));
                }
                Find.WindowStack.Add(new FloatMenu(list));
            }

            listingStandard.End();
        }

        private static string labelForNoFoodAction(NoFoodAction noFoodAction)
        {
            switch (noFoodAction)
            {
                case NoFoodAction.Rest: return "CaravansRestBeforeArrival_NoFoodActionRest".Translate();
                case NoFoodAction.RestNotify: return "CaravansRestBeforeArrival_NoFoodActionRestNotify".Translate();
                case NoFoodAction.Enter: return "CaravansRestBeforeArrival_NoFoodActionEnter".Translate();
                default: return "ERROR";
            }
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref RestThreshold, "RestThreshold");
            Scribe_Values.Look(ref FoodThreshold, "FoodThreshold");
            Scribe_Values.Look(ref NoFoodAction, "NoFoodAction");
        }
    }
}
