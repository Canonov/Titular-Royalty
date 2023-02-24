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
		public override Vector2 InitialSize => new Vector2(550, 410);

		private int MaxNameLength => 28;

		private string curName;
		private string curNameFemale;
		private string newName;
		private string newNameFemale;

		private bool isInheritable;
		private bool allowDignifiedMeditation;

		private ExpectationDef minExpectation;
		private TitleTiers titleTier;

		private GameComponent_TitularRoyalty TRComponent;
		private PlayerTitleDef titleDef;
		private RoyalTitleOverride originalOverrides;
		private Dialog_ManageTitles manageTitles;

		public Dialog_RoyalTitleEditor(GameComponent_TitularRoyalty trComponent, PlayerTitleDef titleDef, Dialog_ManageTitles manageTitles)
		{
			doCloseX = true;
			forcePause = true;
			draggable = true;
			closeOnClickedOutside = true;
			TRComponent = trComponent;

			this.titleDef = titleDef;

			if (manageTitles != null ) 
			{
				this.manageTitles = manageTitles;
				this.manageTitles.titleEditorOpen = true;
			}

			originalOverrides = TRComponent.GetCustomTitleOverrideFor(this.titleDef);

			SetDefaultVariables();
		}

		public override void PreClose()
		{
			base.PreClose();
			if (manageTitles != null)
			{
				manageTitles.titleEditorOpen = false;
			}
		}

		public void SetDefaultVariables()
		{

			if (originalOverrides.label == "None" || originalOverrides.label == null)
			{
				curName = titleDef.label;
				curNameFemale = titleDef.labelFemale;
			}
			else
			{
				curName = originalOverrides.label;

				if (originalOverrides.labelFemale == "None")
				{
					curNameFemale = null;
				}
				else
				{
					curNameFemale = originalOverrides.labelFemale ?? null;
				}
			}

			isInheritable = originalOverrides.TRInheritable ?? titleDef.TRInheritable;
			allowDignifiedMeditation = originalOverrides.allowDignifiedMeditationFocus ?? titleDef.allowDignifiedMeditationFocus;
			minExpectation = originalOverrides.minExpectation ?? titleDef.minExpectation ?? ExpectationDefOf.ExtremelyLow;
			titleTier = originalOverrides.titleTier ?? titleDef.titleTier;
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
			Rect contentRect = new Rect(8, titleRect.yMax + GenUI.GapSmall / 2, inRect.width - 16, inRect.height - (titleRect.yMax + GenUI.GapSmall) - 35);
			Rect contentRectVisual = new Rect(contentRect);
			contentRectVisual.height -= 2;
			contentRect = contentRect.ContractedBy(2);
			contentRect.x += 2;

			Widgets.DrawTitleBG(contentRectVisual);
			Widgets.DrawBox(contentRectVisual, -1, BaseContent.WhiteTex);

			Listing_Standard listingStandard = new Listing_Standard();
			listingStandard.maxOneColumn = true;

			DrawListing(contentRect, listingStandard);

			/* FINALIZE BUTTONS */
			float iconWidth = 35;
			var bottomRect = new Rect(contentRectVisual.xMin, contentRectVisual.yMax + 1, contentRectVisual.width, iconWidth);
			bottomRect.SplitVertically(iconWidth, out Rect iconRect, out Rect bottomRowRect);

			Widgets.DrawTitleBG(bottomRect);
			Widgets.DrawBox(bottomRect, -1, BaseContent.WhiteTex);
			Widgets.DrawBox(iconRect, -1, BaseContent.WhiteTex);

			//var buttonsRect = bottomRowRect.RightPart(0.4f);
			var leftButtonRect = bottomRowRect.LeftHalf();
			var rightButtonRect = bottomRowRect.RightHalf();

			// Icon
			if (Widgets.ButtonImage(iconRect, titleDef.tierIcon))
			{
				Log.Message("buttonimage");
			}

			// Reset and Submit Buttons
			if (Widgets.ButtonText(leftButtonRect, "TR_titleeditor_reset".Translate(), false, overrideTextAnchor: TextAnchor.MiddleCenter))
			{
				ResetTitleOverride();
				Messages.Message("Resetting Title", MessageTypeDefOf.NeutralEvent, false);
			}
			if (Widgets.ButtonText(rightButtonRect, "TR_titleeditor_submit".Translate(), false, overrideTextAnchor: TextAnchor.MiddleCenter))
			{
				TrySubmitTitleChanges();
				Close(true);
			}
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
			DoDoubleCheckboxRow(checkboxRect, "TR_titleeditor_checkbox_inheritable".Translate(), ref isInheritable, 
									          "TR_titleeditor_checkbox_allowmeditationfocus".Translate(), ref allowDignifiedMeditation);

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

			TooltipHandler.TipRegion(titleTiersRect, new TipSignal("TR_titleeditor_titletier_tooltip".Translate()));
			TooltipHandler.TipRegion(minExpectationsRect, new TipSignal("TR_titleeditor_expectations_tooltip".Translate()));

			if (Widgets.ButtonText(titleTiersRect, "TR_titleeditor_titletier".Translate(this.titleTier.ToString() ?? "None")))
			{
				List<FloatMenuOption> options = new List<FloatMenuOption>();
				foreach(var tier in Utilities.TitleTiers)
				{
					options.Add(new FloatMenuOption(tier.ToString(), delegate
					{
						titleTier = tier;
					}));
				}
				Find.WindowStack.Add(new FloatMenu(options));
			}
			if (Widgets.ButtonText(minExpectationsRect, "TR_titleeditor_expectations".Translate(this.minExpectation.ToStringSafe())))
			{
				List<FloatMenuOption> options = new List<FloatMenuOption>();
				foreach (var expectation in DefDatabase<ExpectationDef>.AllDefsListForReading)
				{
					options.Add(new FloatMenuOption(expectation.LabelCap, delegate
					{
						minExpectation = expectation;
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


		/* APPLY METHODS */
		private void ResetTitleOverride()
		{
			originalOverrides = new RoyalTitleOverride();
			TRComponent.SaveTitleChange(titleDef, originalOverrides);
			SetDefaultVariables();
		}

		private bool TrySubmitTitleChanges()
		{
			/* LABELS */
			if (!NameIsValid(newName))
			{
				Messages.Message("Titular Royalty: Male Title is Invalid, is it nothing?, is it over 64 characters or contains special characters?", MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (!NameIsValid(newNameFemale, true))
			{
				Messages.Message("Titular Royalty: Female Title is Invalid, is it over 64 characters or contains special characters?", MessageTypeDefOf.RejectInput, false);
				return false;
			}

			originalOverrides.label = newName;

			if (newNameFemale == string.Empty)
			{
				originalOverrides.labelFemale = "None";
			}
			else
			{
				originalOverrides.labelFemale = newNameFemale;
			}
			

			/* OTHER */
			originalOverrides.TRInheritable = isInheritable;
			originalOverrides.titleTier = titleTier;
			originalOverrides.allowDignifiedMeditationFocus = allowDignifiedMeditation;
			originalOverrides.minExpectation = minExpectation ?? ExpectationDefOf.Low;

			TRComponent.SetupTitle(titleDef);
			Messages.Message("Titular Royalty: Change Title Success", MessageTypeDefOf.NeutralEvent, false);

			return true;
		}

	}
}
