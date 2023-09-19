using System.Collections.Generic;
using TitularRoyalty.Titles;
using Verse;

namespace TitularRoyalty
{
    public class GameComponent_PlayerTitlesManager : GameComponent
    {
        public GameComponent_PlayerTitlesManager(Game game)
        {
            Log.Message("Initializing PlayerTitleManager");
        }
        
        public List<PlayerTitleData> PlayerTitles => playerTitles;
        private List<PlayerTitleData> playerTitles;

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref playerTitles, "playerTitles", LookMode.Deep);
        }
    }
}