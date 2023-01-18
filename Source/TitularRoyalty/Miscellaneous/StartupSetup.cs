using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace TitularRoyalty
{

    [StaticConstructorOnStartup]
    public class StartupSetup
    {
        static StartupSetup()
        {
            foreach (PlayerTitleDef title in DefDatabase<PlayerTitleDef>.AllDefsListForReading)
            {
                title.originalLabels = new TitleLabelPair(title.label, title.labelFemale);
            }
        }
    }
}
