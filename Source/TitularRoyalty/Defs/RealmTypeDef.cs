//using System; 

using UnityEngine;
using JetBrains.Annotations;

namespace TitularRoyalty;

[UsedImplicitly]
public class RealmTypeDef : Def
{
    
    [UsedImplicitly] 
    public List<RoyalTitleOverride> titleOverrides;
    public Dictionary<PlayerTitleDef, RoyalTitleOverride> TitlesWithOverrides
    {
        get
        {
            if (!titleOverrides.NullOrEmpty())
            {
                return titleOverrides.ToDictionary(x => x.titleDef, x => x);
            }
            return new Dictionary<PlayerTitleDef, RoyalTitleOverride>();
        }
    }

    public string iconPath;
    private Texture2D icon;
    public Texture2D Icon => icon ??= ContentFinder<Texture2D>.Get(iconPath ?? string.Empty, false) ?? Resources.CrownIcon;

    private List<Texture2D> tierIconOverridesTex;
    public List<Texture2D> TierIconOverridesTex
    {
        get
        {
            if (!tierIconOverridesTex.NullOrEmpty()) 
                return tierIconOverridesTex;
            
            tierIconOverridesTex = new List<Texture2D>();
            foreach(string str in tierIconOverrides)
            {
                Resources.CustomIcons.TryGetValue(str, out var texture);
                if (texture == null)
                {
                    Log.Warning($"tierIconOverrides for {this.defName} are invalid: \n{tierIconOverrides.ToStringSafe()}\n{Resources.CustomIcons.ToStringFullContents()}");
                    return null;
                }
                else
                {
                    tierIconOverridesTex.Add(texture);
                }
            }
            return tierIconOverridesTex;
        }
    }
    [UsedImplicitly] 
    public List<string> tierIconOverrides;
    
}