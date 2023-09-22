using System;

namespace TitularRoyalty
{
    public static class TitleFeatureMaker
    {
        public static TitleFeature MakeFeature(PlayerTitle title, TitleFeatureDef def)
        {
            var feature = (TitleFeature)Activator.CreateInstance(def.featureClass, title, def);
            feature.PostMake();
            
            return feature;
        }
    }
}