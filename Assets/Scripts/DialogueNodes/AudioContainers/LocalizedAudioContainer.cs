using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogueNodes.AudioContainers
{
    [EditorPath("AudioContainers")]
    public class LocalizedAudioContainer : AudioContainer
    {
        [SerializeField] private LocalizedAudioClip clip;
        public override AudioClip GetClip()
        {
            return clip.LoadAsset();
        }
    }
}