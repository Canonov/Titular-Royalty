using TitularRoyalty.Titles;
using Verse;

namespace TitularRoyalty.Commands
{
    public static class TitleDebugActions
    {
        [DebugAction("Titular Royalty", "List Titles", actionType = DebugActionType.Action)]
        private static void ListTitles()
        {
            Log.Message("Listing Titles");
            GameComponent_PlayerTitlesManager.Current.Titles.ForEach(x => Log.Message(x.label));
        }
        
        [DebugAction("Titular Royalty", "Add Title", actionType = DebugActionType.Action)]
        private static void AddTitle()
        {
            GameComponent_PlayerTitlesManager.Current.AddTitle(new PlayerTitleData()
            {
                label = "title_" + Rand.Range(0, 10000), 
            });
        }
    }
}