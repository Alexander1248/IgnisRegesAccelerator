using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogueNodes
{
    [EditorPath("TextContainers")]
    public class UnityLocalizedContainer : TextContainer
    {
        [SerializeField] private LocalizedString localizedString;
        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.localizedString = new LocalizedString(localizedString.TableReference, localizedString.TableEntryReference);
            return node;
        }
        public override string GetText()
        {
            localizedString.RefreshString();
            return localizedString.GetLocalizedString();
        }
    }
}