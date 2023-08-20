using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using HarmonyLib;
using UnityEngine;

namespace TitularRoyalty
{
	public static class DefIcon_RoyalIconsPrefix
	{
		public static bool Patch(Rect rect, Def def, float scale = 1f)
		{
			if (def is PlayerTitleDef titleDef)
			{
				var icon = Resources.GetTitleIcon(titleDef, GameComponent_TitularRoyalty.Current);
				
				if (icon != null)
				{
					Widgets.DrawTextureFitted(rect, icon, scale);
					return false;
				}
				
				Log.Message("Titular Royalty: Could not find icon for " + titleDef.defName + " in DefIcon_RoyalIconsPrefix.");
			}
			return true;
		}
	}
}
