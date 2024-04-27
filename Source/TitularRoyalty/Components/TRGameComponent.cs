namespace TitularRoyalty;

[UsedImplicitly]
public class TRGameComponent : GameComponent
{
    public static TRGameComponent Current { get; private set; }
    public TRGameComponent(Game game) { Current = this; } // Needs this or else Rimworld throws a fit and errors.
    public TitleDatabase titleDatabase = new TitleDatabase();

    /// <summary>
    /// Code to be run on both loading or starting a new game
    /// </summary>
    private void OnGameStart(bool newGame)
    {
        Utils.SetupPlayerFaction();
    }

    public override void LoadedGame() => OnGameStart(false);
    public override void StartedNewGame() => OnGameStart(true);
    
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Deep.Look(ref titleDatabase, "titleDatabase");
    }

}