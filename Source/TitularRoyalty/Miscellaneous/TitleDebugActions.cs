using System;
using System.Collections.Generic;
using System.Linq;
using TitularRoyalty.Extensions;
using Verse;

namespace TitularRoyalty
{
    public static class TitleDebugActions
    {
        private static GameComponent_PlayerTitlesManager TitleManager => GameComponent_PlayerTitlesManager.Current;
        
        [DebugAction("Titular Royalty", "List Titles", actionType = DebugActionType.Action)]
        private static void ListTitles()
        {
            LogTR.Message("Listing Titles");
            int i = 0;
            TitleManager.Titles.ForEach(x =>
            {
                Log.Message($"{i} - {x.label}");

                var features = string.Join(", ", x.featureDefs.Select(y => y.label ?? y.defName));
                Log.Message("Features: " + features == string.Empty ? "None" : features);

                i++;
            });
        }
        
        [DebugAction("Titular Royalty", "Add Title", actionType = DebugActionType.Action)]
        private static void AddTitle()
        {
            var label = "title_" + Rand.Range(0, 10000);

            var titleData = new PlayerTitleData
            {
                label = label
            };
            
            titleData.featureDefs.Add(TitleFeatureDefOf.TitleFeature_Test);
            
            TitleManager.AddTitle(titleData);
            
        }
        
        [DebugAction("Titular Royalty", "Log Cache", actionType = DebugActionType.Action)]
        private static void LogCache()
        {
            foreach (var pawn in TitleManager.PawnsWithTitles)
            {
                LogTR.Message($"Cache - {pawn.Name} - {(pawn.PlayerRoyalty()?.title?.titleData?.label ?? "unknown")}");
            }
        }
        
        [DebugAction("Titular Royalty", "Remove Title", actionType = DebugActionType.Action)]
        private static void RemoveTitle()
        {
            var debugOptions = new List<DebugMenuOption>();
            
            foreach (var title in GameComponent_PlayerTitlesManager.Current.Titles)
            {
                debugOptions.Add(new DebugMenuOption(title.label, DebugMenuOptionMode.Action, () =>
                {
                    TitleManager.RemoveTitle(title);
                }));
            }
            
            if (debugOptions.Any())
            {
                Find.WindowStack.Add(new Dialog_DebugOptionListLister(debugOptions));
            }

        }
        
        [DebugAction("Titular Royalty", "Grant Title", actionType = DebugActionType.Action)]
        private static void GrantTitle()
        {
            var randomPawnRoyalty = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement().PlayerRoyalty();
            
            LogTR.Message($"Granting Title to {randomPawnRoyalty.Pawn.Name}");
            
            var debugOptions = new List<DebugMenuOption>();
            
            foreach (var title in TitleManager.Titles)
            {
                debugOptions.Add(new DebugMenuOption(title.label, DebugMenuOptionMode.Action, () =>
                {
                    randomPawnRoyalty.SetTitle(title);
                }));
            }
            
            if (debugOptions.Any())
            {
                Find.WindowStack.Add(new Dialog_DebugOptionListLister(debugOptions));
            }
            
        }
    }
}