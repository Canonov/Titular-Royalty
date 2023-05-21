//using System;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using JetBrains.Annotations;


namespace TitularRoyalty
{
    public class CompProperties_ManageTitles : CompProperties
    {
	    [NoTranslate, UsedImplicitly] 
	    public string managetitlesIconPath;

        public CompProperties_ManageTitles()
        {
            compClass = typeof(Comp_ManageTitles);
        }
    }
}
