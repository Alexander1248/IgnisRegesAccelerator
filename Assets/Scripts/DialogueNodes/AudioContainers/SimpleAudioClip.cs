using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;

namespace DialogueNodes.AudioContainers
{
    [EditorPath("AudioContainers")]
    public class SimpleAudioClip : AudioContainer
    {
        [SerializeField] private AudioClip clip;
        public override AudioClip GetClip()
        {
            return clip;
        }
    }
}