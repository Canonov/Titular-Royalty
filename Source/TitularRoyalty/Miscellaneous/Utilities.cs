namespace TitularRoyalty
{
    public static class Utilities
    {
        //Helpers
        public static void SetupPlayerForTR(this Faction faction)
        {
            faction.allowGoodwillRewards = false;
            faction.allowRoyalFavorRewards = false;

            foreach (PlayerTitlePermitDef permit in DefDatabase<PlayerTitlePermitDef>.AllDefsListForReading)
            {
                permit.faction = faction.def;
            }
        }

        public static readonly List<TitleTiers> TitleTiers = new List<TitleTiers>() { 
            TitularRoyalty.TitleTiers.Lowborn,
            TitularRoyalty.TitleTiers.Gentry,
            TitularRoyalty.TitleTiers.LowNoble,
            TitularRoyalty.TitleTiers.HighNoble,
            TitularRoyalty.TitleTiers.Royalty,
            TitularRoyalty.TitleTiers.Sovereign
        };
    }
}
