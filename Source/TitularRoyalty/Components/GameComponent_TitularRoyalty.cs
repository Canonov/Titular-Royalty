//using System; 

namespace TitularRoyalty;

[UsedImplicitly]
public class GameComponent_TitularRoyalty : GameComponent
{
    public static GameComponent_TitularRoyalty Current { get; private set; }
    public GameComponent_TitularRoyalty(Game game) { Current = this; } // Needs this or else Rimworld throws a fit and errors.

    /// <summary>
    /// Code to be run on both loading or starting a new game
    /// </summary>
    private void OnGameStart(bool newGame)
    {
        Faction.OfPlayer.allowGoodwillRewards = false;
        Faction.OfPlayer.allowRoyalFavorRewards = false;
        StartupSetup.ApplyModSettings();
    }

    public override void LoadedGame() => OnGameStart(false);
    public override void StartedNewGame() => OnGameStart(true);
    
    public override void ExposeData()
    {
        base.ExposeData();
    }

}