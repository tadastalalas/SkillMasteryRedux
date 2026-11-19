using System.Xml;
using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;

namespace SkillMasteryRedux.UI;

// SMR-FEATURE: Learning limit tooltip over the whole skill bar | gate: always (the extra row is gated in SkillLimitTooltip)
// Kept from Show Skill Limit: a hint covering the bar so the tooltip also shows over the limit segment.
[PrefabExtension("CharacterDeveloper", "descendant::Widget[@Id='FullLearningRateClipParent']")]
internal sealed class SkillLimitHintPrefab : PrefabExtensionInsertPatch
{
    private const string Xml = """
                               <HintWidget DataSource="{LearningLimitTooltip}" WidthSizePolicy="StretchToParent" HeightSizePolicy="StretchToParent" Command.HoverBegin="ExecuteBeginHint" Command.HoverEnd="ExecuteEndHint" IsEnabled="false" />
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