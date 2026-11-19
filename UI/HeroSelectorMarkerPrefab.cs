using System.Xml;
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

namespace SkillMasteryRedux.UI;

// SMR-FEATURE: Hero dropdown marks unspent attribute/focus points and unopened perks | gate: setting:ShowHeroListMarkers
// Appends an orange circle (left) and a perk-count shield (right) to each row of the character screen hero dropdown; bound to HeroSelectorItemVM.
[PrefabExtension("CharacterDeveloper", "descendant::ButtonWidget[@Id='DropdownItemButton']/Children/ScrollingTextWidget")]
internal sealed class HeroSelectorMarkerPrefab : PrefabExtensionInsertPatch
{
    private const string Xml = """
        <Widget WidthSizePolicy="StretchToParent" HeightSizePolicy="StretchToParent" DoNotAcceptEvents="true">
          <Children>
            <Widget WidthSizePolicy="Fixed" HeightSizePolicy="Fixed" SuggestedWidth="12" SuggestedHeight="12" HorizontalAlignment="Left" VerticalAlignment="Center" MarginLeft="11" Sprite="BlankWhiteCircle" Color="#F2A03DFF" IsVisible="@SmrHasUnspentPoints" />
            <MapBarUnreadBrushWidget WidthSizePolicy="Fixed" HeightSizePolicy="Fixed" SuggestedWidth="24" SuggestedHeight="24" HorizontalAlignment="Right" VerticalAlignment="Center" MarginRight="4" Brush="CharacterDeveloper.Companion.PerkSelection" UnreadTextWidget="SmrHeroUnreadText" IsVisible="@SmrHasUnopenedPerks">
              <Children>
                <TextWidget Id="SmrHeroUnreadText" WidthSizePolicy="StretchToParent" HeightSizePolicy="StretchToParent" Brush.FontSize="14" IntText="@SmrUnopenedPerks" />
              </Children>
            </MapBarUnreadBrushWidget>
          </Children>
        </Widget>
        """;

    private readonly XmlDocument _document = Load();

    public override InsertType Type => InsertType.Append;

    [PrefabExtensionXmlDocument(false)]
    public XmlDocument GetPrefabExtension() => _document;

    private static XmlDocument Load()
    {
        XmlDocument document = new();
        document.LoadXml(Xml);
        return document;
    }
}
