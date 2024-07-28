﻿using Plugins.DialogueSystem.Scripts.DialogueGraph;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Attributes;
using Plugins.DialogueSystem.Scripts.DialogueGraph.Nodes;
using UnityEngine;

namespace DialogueNodes.TextPlayers
{
    [EditorPath("Drawers")]
    public class CurvedTextPlayer : TextPlayer
    {
        [SerializeField] private string narrator;
        [SerializeField] private AnimationCurve curve;

        private Narrator _narrator;
        private string _currentText;
        private float _timeLimit;
        private float _time;
        private int _index;

        public override AbstractNode Clone()
        {
            var node = Instantiate(this);
            node.narrator = narrator;
            node.curve = curve;
            return node;
        }

        public override void OnDrawStart(Dialogue dialogue, Storyline storyline)
        {
            _narrator = dialogue.GetNarrator(narrator);
            _currentText = textContainer.GetText();
            _time = 0;
            _index = 0;
            ComputeLimit();
        }

        public override void OnDrawEnd(Dialogue dialogue, Storyline storyline)
        {
            
        }

        public override void OnDelayStart(Dialogue dialogue, Storyline storyline)
        {
            
        }

        public override void OnDelayEnd(Dialogue dialogue, Storyline storyline)
        {
            
        }
        private void ComputeLimit()
        {
            if (IsCompleted())
            {
                _timeLimit = 0;
                return;
            }
            _timeLimit = curve.Evaluate((float) _index / _currentText.Length);
        }

        public override void Draw(Dialogue dialogue)
        {
            PlayDraw(dialogue);
            
            _time += Time.deltaTime;
            if (_time < _timeLimit) return;
            while (_time >= _timeLimit)
            {
                _index++;
                _time -= _timeLimit;
            }
            ComputeLimit();
        }

        public override void PauseDraw(Dialogue dialogue)
        {
            _narrator.Clear();
        }

        public override void PlayDraw(Dialogue dialogue)
        {
            if (!IsCompleted())
                _narrator?.SpeakWithSound(_currentText[..(_index + 1)]);
        }

        public override bool IsCompleted()
        {
            return _index >= _currentText.Length;
        }
    }
}