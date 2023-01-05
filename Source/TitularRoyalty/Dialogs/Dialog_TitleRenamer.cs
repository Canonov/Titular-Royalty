using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using RimWorld;
using UnityEngine;
using Verse;

namespace TitularRoyalty
{
    class Dialog_TitleRenamer : Dialog_GiveName
    {
        private PlayerTitleDef title;

        public Dialog_TitleRenamer(PlayerTitleDef title)
        {
            this.title = title;
            curName = title.label;

            if (title.labelFemale == null)
            {
                curSecondName = "None";
            }
            else
            {
                curSecondName = title.labelFemale;
            }
            

            nameMessageKey = "TR_namemessage";
            invalidNameMessageKey = "TR_invalidmessage";
            secondNameMessageKey = "TR_namemessage2";
            invalidSecondNameMessageKey = "TR_invalidmessage2";
            gainedNameMessageKey = "TR_changedtitle2";

            useSecondName = true;

        }

        protected override bool IsValidName(string s)
        {
            return NamePlayerFactionDialogUtility.IsValidName(s);
        }

        protected override bool IsValidSecondName(string s)
        {
            return NamePlayerSettlementDialogUtility.IsValidName(s);
        }
        
        // Male
        protected override void Named(string s)
        {
            Current.Game.GetComponent<GameComponent_TitularRoyalty>().SaveTitleChange(title, s, Gender.Male);
        }

        // Female
        protected override void NamedSecond(string s)
        {
            Current.Game.GetComponent<GameComponent_TitularRoyalty>().SaveTitleChange(title, s, Gender.Female);
        }

    }
}

//Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement));