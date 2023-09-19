using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TitularRoyalty.Titles;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{
    [UsedImplicitly]
    public class GameComponent_PlayerTitlesManager : GameComponent
    {
        public static GameComponent_PlayerTitlesManager Current { get; private set; }
        
        public GameComponent_PlayerTitlesManager(Game game)
        {
            Current = this;
            Log.Message("Initializing PlayerTitleManager");
        }
        
        public List<PlayerTitleData> Titles => titles ??= new List<PlayerTitleData>();
        private List<PlayerTitleData> titles = new List<PlayerTitleData>();

        
        /// <summary>
        /// Add a new title to the manager
        /// </summary>
        /// <param name="titleData">Title to add to the list</param>
        /// <param name="createAfter">If not null, the title to insert the new one after</param>
        public void AddTitle(PlayerTitleData titleData, PlayerTitleData createAfter = null)
        {
            if (createAfter != null)
            {
                var nextIdx = Titles.IndexOf(createAfter);
                if (nextIdx == -1)
                {
                    Log.Error($"Couldn't find title {createAfter} in the list of titles");
                }
                
                Titles.Insert(nextIdx, titleData);
                return;
            }
            
            Titles.Add(titleData);
        }

        /// <summary>
        /// Remove Title from the Manager
        /// </summary>
        /// <param name="titleData"></param>
        public void RemoveTitle(PlayerTitleData titleData)
        {
            if (Titles.Contains(titleData))
            {
                Titles.Remove(titleData);
                return;
            }
            
            Log.Error("Tried to remove a title that doesn't exist");
        }

        public override void ExposeData()
        {
            Scribe_Collections.Look(ref titles, "playerTitles", LookMode.Deep);
        }
    }
}