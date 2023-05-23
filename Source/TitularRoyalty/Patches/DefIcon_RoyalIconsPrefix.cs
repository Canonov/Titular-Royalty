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
				var icon = Resources.GetTitleIcon(titleDef, GameComponent_TitularRoyalty.Current) ?? null;
				Log.Message((icon == null).ToString());
				if (icon != null)
				{
					Widgets.DrawTextureFitted(rect, icon, scale);
					return false;
				}
			}
			return true;
		}
	}
}
