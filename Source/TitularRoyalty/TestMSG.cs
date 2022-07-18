using RimWorld;
using Verse;

namespace TitularRoyalty.Verbs
{
    [StaticConstructorOnStartup]
    public static class TestMsg
    {
        static TestMsg()
        {
            Log.Message("Hello There");
        }
    }
}
