using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogueNodes.AudioContainers
{
    [EditorPath("AudioContainers")]
    public class LocalizedAudioContainer : AudioContainer
    {
        [SerializeField] private LocalizedAudioClip clip;
        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.clip = new LocalizedAudioClip();
            node.clip.TableReference = clip.TableReference;
            node.clip.TableEntryReference = clip.TableEntryReference;
            return node;
        }
        public override AudioClip GetClip()
        {
            return clip.LoadAsset();
        }
    }
}