using System;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace TitularRoyalty
{
	public enum TitleTiers
	{
		Lowborn = 0,
		Gentry = 1,
		LowNoble = 2,
		HighNoble = 3,
		Royalty = 4,
		Sovereign = 5,
	}

	[StaticConstructorOnStartup, UsedImplicitly]
	public class Resources
	{
		public static readonly Texture2D CrownIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/givetitleicon");
		public static readonly Texture2D TRWidget = ContentFinder<Texture2D>.Get("UI/TRwidget");
		public static readonly Texture2D TRCrownWidget = ContentFinder<Texture2D>.Get("UI/TRcrownwidget");

		static Resources()
		{
		}
	}
}