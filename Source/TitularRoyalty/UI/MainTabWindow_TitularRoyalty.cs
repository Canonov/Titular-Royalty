using RimWorld;
using UnityEngine;
using Verse;

namespace TitularRoyalty.UI
{
    public class MainTabWindow_TitularRoyalty : MainTabWindow
    {
        private Vector2 scrollPosition_titleList;
        private float scrollViewHeight_titleList;

        private Vector2 scrollPosition_titleDetails;
        private float scrollViewHeight_titleDetails;
        

        public override Vector2 InitialSize => new Vector2(base.InitialSize.x, Verse.UI.screenHeight - 35);
        
        public override void PreOpen()
        {
            base.PreOpen();
            scrollPosition_titleDetails = Vector2.zero;
        }

        public override void PostClose()
        {
            base.PostClose();
        }

        public override void DoWindowContents(Rect inRect)
        {
            TitleMenuUIUtility.DoTitleWindow(inRect);
        }
    }
}