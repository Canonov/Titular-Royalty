using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{
    public class TitleFeatureDef : Def
    {
        public Type featureClass = typeof(TitleFeature);

        //public TitleFeatureCategoryDef category;

        public List<TitleFeatureDef> mutuallyExclusiveWith;

        [NoTranslate, UsedImplicitly] 
        public string iconPath;
        
        [Unsaved(false)]
        private Texture2D cachedIcon;
        
        public Texture2D Icon
        {
            get
            {
                if (cachedIcon == null)
                {
                    if (iconPath.NullOrEmpty())
                    {
                        cachedIcon = BaseContent.BadTex;
                    }
                    else
                    {
                        cachedIcon = ContentFinder<Texture2D>.Get(iconPath) ?? BaseContent.BadTex;
                    }
                }
                return cachedIcon;
            }
        }

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (var error in base.ConfigErrors())
            {
                yield return error;
            }

            if (featureClass != typeof(TitleFeature) && !featureClass.IsSubclassOf(typeof(TitleFeature)) )
            {
                yield return "featureClass must be a subclass of TitleFeature";
            }

            // if (category == null)
            // {
            //     yield return $"{label} ({defName}) must have a category";
            // }
        }
    }
}