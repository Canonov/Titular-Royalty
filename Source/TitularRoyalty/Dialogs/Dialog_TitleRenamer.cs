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
        private RoyalTitleDef title;

        public Dialog_TitleRenamer(RoyalTitleDef title)
        {
            this.title = title;
            curName = title.label;

            if (title.labelFemale == null)
            {
                curSecondName = "remove";
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

        protected override void Named(string s)
        {
            Current.Game.GetComponent<GameComponent_TitularRoyalty>().SaveTitleChange(Gender.Male, title.seniority, s);
        }
        protected override void NamedSecond(string s)
        {
            Current.Game.GetComponent<GameComponent_TitularRoyalty>().SaveTitleChange(Gender.Female, title.seniority, s);
        }

    }
}

//Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement));