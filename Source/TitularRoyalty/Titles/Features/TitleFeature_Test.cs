using Verse;

namespace TitularRoyalty
{
    public class TitleFeature_Test : TitleFeature
    {
        
        public TitleFeature_Test(PlayerTitle title, TitleFeatureDef def) : base(title, def) { }
        public override void Tick()
        {
            LogTR.Message(LabelCap + " Tick");
        }

        public override void Notify_TitleRemoved()
        {
            LogTR.Message(LabelCap + " Notify_TitleRemoved");
        }

        public override void PostMake()
        {
            LogTR.Message(LabelCap + " PostMake");
        }

        public override void PostAdd()
        {
            LogTR.Message(LabelCap + " PostAdd");
        }

        public override void PostRemove()
        {
            LogTR.Message(LabelCap + " PostRemove");
        }
    }
}