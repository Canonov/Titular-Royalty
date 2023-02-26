using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{
	public class ManageTitlesWidget
	{
		public static void AddWidget(WidgetRow row, bool worldView)
		{
			if (!worldView && row.ButtonIcon(Resources.TRWidget, "Open Titular Royalty Manager"))
			{
				Find.WindowStack.Add(new Dialog_ManageTitles());
			}
		}
	}
}
