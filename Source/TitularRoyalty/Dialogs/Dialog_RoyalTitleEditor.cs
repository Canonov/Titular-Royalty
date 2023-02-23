using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using RimWorld;
using SettingsHelper;
using UnityEngine;
using Verse;
using Verse.Grammar;
using Verse.Noise;

namespace TitularRoyalty
{
	public class Dialog_RoyalTitleEditor : Window
	{
		public override Vector2 InitialSize => new Vector2(550, 375);

		private int MaxNameLength => 28;

		private string curName;
		private string curNameFemale;
		private string newName;
		private string newNameFemale;

		private bool isInheritable;
		private bool canGiveSpeeches;

		private GameComponent_TitularRoyalty TRComponent;
		private PlayerTitleDef titleDef;

		public Dialog_RoyalTitleEditor(GameComponent_TitularRoyalty trComponent, PlayerTitleDef titleDef)
		{
			doCloseX = true;
			forcePause = true;
			draggable = true;
			TRComponent = trComponent;

			this.titleDef = titleDef;
			curName = titleDef.label;
			curNameFemale = titleDef.labelFemale;
		}
 
		public override void DoWindowContents(Rect inRect)
		{
			/* TITLE */
			Rect titleRect = new Rect(4, 0, inRect.width - 8, 40);

			Text.Font = GameFont.Medium;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(titleRect, "TR_titleeditor_label".Translate());
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;

			/* CONTENT */
			Rect contentRect = new Rect(8, titleRect.yMax + GenUI.GapSmall / 2, inRect.width - 16, inRect.height - (titleRect.yMax + GenUI.GapSmall));
			Rect contentRectVisual = new Rect(contentRect);
			contentRectVisual.height -= 2;
			contentRect = contentRect.ContractedBy(2);
			contentRect.x += 2;

			Widgets.DrawTitleBG(contentRectVisual);
			Widgets.DrawBox(contentRectVisual, -1, BaseContent.WhiteTex);

			Listing_Standard listingStandard = new Listing_Standard();
			listingStandard.maxOneColumn = true;

			DrawListing(contentRect, listingStandard);

		}

		public void DrawListing(Rect contentRect, Listing_Standard listingStandard)
		{
			listingStandard.Begin(contentRect);

			// Title Labels
			var namesRect = listingStandard.GetRect(35 + Text.CalcHeight("TR_titleeditor_title".Translate(), contentRect.width) + 10);
			DoTitleNamesRow(namesRect);
			listingStandard.Gap(GenUI.GapSmall);

			// Dropdowns
			var dropdownRect = listingStandard.GetRect(35);
			DoTiersAndExpectationsDropdowns(dropdownRect);
			listingStandard.Gap(GenUI.GapSmall);

			// Checkbox Settings
			listingStandard.Gap(GenUI.GapSmall / 2);

			Text.Anchor = TextAnchor.MiddleCenter;
			listingStandard.Label("TR_titleeditor_checkboxes_label".Translate());
			Text.Anchor = TextAnchor.UpperLeft;

			listingStandard.Gap(GenUI.GapSmall / 2);

			var checkboxRect = listingStandard.GetRect(28);
			DoDoubleCheckboxRow(checkboxRect, "TR_checkbox_inheritable".Translate(), ref isInheritable, "TR_checkbox_cangivespeeches".Translate(), ref canGiveSpeeches);

			// End the List
			listingStandard.End();
		}

		private void DoTiersAndExpectationsDropdowns(Rect dropdownRect)
		{
			var titleTiersRect = dropdownRect.LeftHalf();
			var minExpectationsRect = dropdownRect.RightHalf();

			titleTiersRect.width -= 8;
			minExpectationsRect.width -= 8;
			minExpectationsRect.x += 4;

			if (Widgets.ButtonText(titleTiersRect, $"".Translate(titleDef.titleTier.ToString())))
			{
				List<FloatMenuOption> options = new List<FloatMenuOption>();
				for (int i = 0; i < Enum.GetNames(typeof(TitleTiers)).Length; i++)
				{
					options.Add(new FloatMenuOption(Enum.GetName(typeof(TitleTiers), i), delegate
					{
						// todo
					}));
				}
				Find.WindowStack.Add(new FloatMenu(options));
			}
			if (Widgets.ButtonText(minExpectationsRect, titleDef.minExpectation.ToString()))
			{
				List<FloatMenuOption> options = new List<FloatMenuOption>();
				foreach (var expectation in DefDatabase<ExpectationDef>.AllDefsListForReading)
				{
					options.Add(new FloatMenuOption(expectation.LabelCap, delegate
					{
						// todo
					}));
				}
				Find.WindowStack.Add(new FloatMenu(options));
			}
		}

		private void DoTitleNamesRow(Rect namesRect)
		{
			// Labels
			var labelsRect = namesRect.TopPartPixels(namesRect.height - 35 - (GenUI.GapSmall / 2));
			labelsRect.y += GenUI.GapSmall / 2;
			var maleLabelRect = labelsRect.LeftHalf();
			var femaleLabelRect = labelsRect.RightHalf();

			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(maleLabelRect, "TR_titleeditor_title".Translate());
			Widgets.Label(femaleLabelRect, "TR_titleeditor_titlefemale".Translate());
			Text.Anchor = TextAnchor.UpperLeft;

			// Input Rects
			var textFieldsRect = namesRect.BottomPartPixels(35);
			var maleNameRect = textFieldsRect.LeftHalf();
			var femaleNameRect = textFieldsRect.RightHalf();

			maleNameRect.width -= 8;
			femaleNameRect.width -= 8;
			femaleNameRect.x += 4;

			// User Input
			curName = Widgets.TextField(maleNameRect, curName, MaxNameLength);
			curNameFemale = Widgets.TextField(femaleNameRect, curNameFemale, MaxNameLength);
			newName = curName?.Trim();
			newNameFemale = curNameFemale?.Trim();
		}

		private void DoDoubleCheckboxRow(Rect checkboxRect, string checkbox1Label, ref bool checkbox1Value, string checkbox2Label, ref bool checkbox2Value)
		{
			var leftRect = checkboxRect.LeftHalf();
			var rightRect = checkboxRect.RightHalf();

			leftRect.width -= 8;
			rightRect.width -= 8;
			rightRect.x += 4;

			Widgets.CheckboxLabeled(leftRect, checkbox1Label, ref checkbox1Value, placeCheckboxNearText: false);
			Widgets.CheckboxLabeled(rightRect, checkbox2Label, ref checkbox2Value, placeCheckboxNearText: false);
		}

		private bool NameIsValid(string name, bool female = false)
		{
			if ((!female && name.Length == 0) || name.Length > MaxNameLength || GrammarResolver.ContainsSpecialChars(name))
			{
				return false;
			}
			return true;
		}

	}
}
