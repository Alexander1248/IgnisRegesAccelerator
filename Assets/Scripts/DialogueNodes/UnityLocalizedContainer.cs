using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogueNodes
{
    [EditorPath("TextContainers")]
    public class UnityLocalizedContainer : TextContainer
    {
        [SerializeField] private LocalizedString localizedString;
        public override string GetText()
        {
            localizedString.RefreshString();
            return localizedString.GetLocalizedString();
        }
    }
}