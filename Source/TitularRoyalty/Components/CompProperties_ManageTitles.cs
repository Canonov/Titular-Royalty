//using System;

namespace TitularRoyalty;

public class CompProperties_ManageTitles : CompProperties
{
    [NoTranslate, UsedImplicitly] 
    public string managetitlesIconPath;

    public CompProperties_ManageTitles()
    {
        compClass = typeof(Comp_ManageTitles);
    }
}