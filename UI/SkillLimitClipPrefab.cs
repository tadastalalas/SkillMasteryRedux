using System.Xml;
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

namespace SkillMasteryRedux.UI;

// SMR-FEATURE: Skill limit marker on the character screen skill bar | gate: setting:ShowSkillLimit
// Appends the limit segment (clipped fill + indicator) to the vanilla full-learning-rate bar; SkillLimitBar sizes it per frame.
[PrefabExtension("CharacterDeveloper", "descendant::Widget[@Id='FullLearningRateClipParent']/Children/Widget")]
internal sealed class SkillLimitClipPrefab : PrefabExtensionInsertPatch
{
    internal const string ClipId = "SmrSkillLimitClip";

    private static readonly string Xml = $"""
        <Widget Id="{ClipId}" WidthSizePolicy="CoverChildren" HeightSizePolicy="StretchToParent" HorizontalAlignment="Right" SuggestedWidth="0" PositionXOffset="0">
          <Children>
            <Widget WidthSizePolicy="Fixed" HeightSizePolicy="StretchToParent" ClipContents="true" HorizontalAlignment="Right" SuggestedWidth="0" PositionXOffset="0">
              <Children>
                <Widget HeightSizePolicy="StretchToParent" HorizontalAlignment="Center" SuggestedWidth="1000" Sprite="CharacterDeveloper\progress_bar_fill" Color="#FF500050" />
              </Children>
            </Widget>
            <ValueBasedVisibilityWidget WidthSizePolicy="Fixed" HeightSizePolicy="Fixed" HorizontalAlignment="Right" VerticalAlignment="Bottom" IndexToBeVisibleFloat="0.001" WatchType="BiggerThan" IndexToWatchFloat="@LearningRate" SuggestedWidth="70" SuggestedHeight="70" PositionXOffset="0" PositionYOffset="46" DoNotPassEventsToChildren="true">
              <Children>
                <Widget WidthSizePolicy="Fixed" HeightSizePolicy="StretchToParent" SuggestedWidth="40" HorizontalAlignment="Center" Sprite="CharacterDeveloper\indicator" Color="#AD4D49FF" />
              </Children>
            </ValueBasedVisibilityWidget>
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