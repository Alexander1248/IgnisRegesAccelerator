using System;
using DialogueNodes.AudioContainers;
using Plugins.DialogueSystem.Scripts.DialogueGraph;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes.PropertyNodes;
using UnityEngine;

namespace DialogueNodes
{
    [EditorPath("Property")]
    public class PlayAudio : Property
    { 
        [SerializeField] private Stage stage;
        [InputPort("AudioContainer")] 
        [HideInInspector] 
        public AudioContainer container;
        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.stage = stage;
            return node;
        }
        public override void OnDrawStart(Dialogue dialogue, Storyline node)
        { 
            if (stage != Stage.OnDrawStart) return;
            if (dialogue.Data["source"] is not AudioSource source)
                throw new ArgumentException("Type of field \"source\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDrawEnd) return;
            if (dialogue.buffer["source"] is not AudioSource source)
                throw new ArgumentException("Type of field \"source\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayStart) return;
            if (dialogue.buffer["source"] is not AudioSource source)
                throw new ArgumentException("Type of field \"source\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            if (stage != Stage.OnDelayEnd) return;
            if (dialogue.buffer["source"] is not AudioSource source)
                throw new ArgumentException("Type of field \"source\" is not AudioSource");
            source.clip = container.GetClip();
            source.Play();
        }
    }
}