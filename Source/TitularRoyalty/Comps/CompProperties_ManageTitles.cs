//using System;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;


namespace TitularRoyalty
{
    public class CompProperties_ManageTitles : CompProperties
    {
        [NoTranslate]
        public string managetitlesIconPath;

        public bool dontManageTitles = false;

        public CompProperties_ManageTitles()
        {
            compClass = typeof(Comp_ManageTitles);
        }
    }
}
