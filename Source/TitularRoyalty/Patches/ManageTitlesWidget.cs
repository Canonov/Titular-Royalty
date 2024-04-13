namespace TitularRoyalty;

public class ManageTitlesWidget
{
	public static void AddWidget(WidgetRow row, bool worldView)
	{
		if (!worldView && row.ButtonIcon(Resources.TRWidget, "Open Titular Royalty Manager"))
		{
			Find.WindowStack.Add(new Dialog_ManageTitles());
		}
	}
}