using System.Collections.Generic;
using TitularRoyalty.Extensions;
using TitularRoyalty.Titles;
using Verse;

namespace TitularRoyalty
{
    public static class TitleDebugActions
    {
        [DebugAction("Titular Royalty", "List Titles", actionType = DebugActionType.Action)]
        private static void ListTitles()
        {
            LogTR.Message("Listing Titles");
            GameComponent_PlayerTitlesManager.Current.Titles.ForEach(x => Log.Message(x.label));
        }
        
        [DebugAction("Titular Royalty", "Add Title", actionType = DebugActionType.Action)]
        private static void AddTitle()
        {
            LogTR.Message(GameComponent_PlayerTitlesManager.Current.ToString());
            GameComponent_PlayerTitlesManager.Current.AddTitle(new PlayerTitleData()
            {
                label = "title_" + Rand.Range(0, 10000), 
            });
        }
        
        [DebugAction("Titular Royalty", "Grant Title", actionType = DebugActionType.Action)]
        private static void GrantTitle()
        {
            var randomPawnRoyalty = Find.AnyPlayerHomeMap.mapPawns.FreeColonists.RandomElement().PlayerRoyalty();
            
            LogTR.Message($"Granting Title to {randomPawnRoyalty.Pawn.Name}");
            
            var debugOptions = new List<DebugMenuOption>();
            
            foreach (var title in GameComponent_PlayerTitlesManager.Current.Titles)
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