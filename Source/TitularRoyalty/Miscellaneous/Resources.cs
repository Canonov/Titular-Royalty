using UnityEngine;

namespace TitularRoyalty;

[StaticConstructorOnStartup, UsedImplicitly]
public class Resources
{
	public static readonly Texture2D CrownIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/givetitleicon");
	public static readonly Texture2D TRWidget = ContentFinder<Texture2D>.Get("UI/TRwidget");
	public static readonly Texture2D TRCrownWidget = ContentFinder<Texture2D>.Get("UI/TRcrownwidget");

	private static Dictionary<string, Texture2D> customIcons;
	public static Dictionary<string, Texture2D> CustomIcons
	{
		get
		{
			if (customIcons.NullOrEmpty())
			{
				customIcons = ContentFinder<Texture2D>.GetAllInFolder("TRIcons").ToDictionary(x => x.name, x => x);

				if (ModLister.HasActiveModWithName("Vanilla Factions Expanded - Empire"))
				{
					foreach (var icon in ContentFinder<Texture2D>.GetAllInFolder("UI/NobleRanks"))
					{
						customIcons.Add(icon.name, icon);
					}
				}
			}

			return customIcons;
		}
	}

	public static Texture2D GetTitleIcon(TitleTiers titleTiers, RealmTypeDef realmTypeDef)
	{
		if (!realmTypeDef.tierIconOverrides.NullOrEmpty())
		{
			if (realmTypeDef.TierIconOverridesTex[(int)titleTiers] != null)
			{
				return realmTypeDef.TierIconOverridesTex[(int)titleTiers];
			}
		}

		return CustomIcons.TryGetValue($"RankIcon{(int)titleTiers}", BaseContent.BadTex);
	}

	public static Texture2D GetTitleIcon(PlayerTitleDef playerTitleDef)
	{
		var trComponent = GameComponent_TitularRoyalty.Current;
		var titlesWithOverride = trComponent.realmTypeDef.TitlesWithOverrides;
		
		if (titlesWithOverride.TryGetValue(playerTitleDef, out var titleOverride) && titleOverride.RTIconOverride != null)
		{
			return titleOverride.RTIconOverride;
		}
		if (playerTitleDef.iconName.NullOrEmpty())
		{
			return GetTitleIcon(playerTitleDef.titleTier, trComponent.realmTypeDef);
		}
		if (CustomIcons.TryGetValue(playerTitleDef.iconName, out var resultTex))
		{
			return resultTex;
		}

		Log.Warning($"{playerTitleDef.label} failed to get icon from name {playerTitleDef.iconName}, it may be missing, reassign it in edit titles");
		playerTitleDef.iconName = null;
		
		return BaseContent.BadTex;
	}


	static Resources()
	{
	}
}