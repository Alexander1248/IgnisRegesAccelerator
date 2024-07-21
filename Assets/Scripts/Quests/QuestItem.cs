using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Quests
{
    public class QuestItem : MonoBehaviour
    {
        [HideInInspector] public Quest quest;
        [HideInInspector] public UnityEvent<Quest> selected;

        [SerializeField] private TMP_Text questName;
        [SerializeField] private Image point;
        [SerializeField] private Image background;

        public void Start()
        {
            questName.text = quest.Name.GetLocalizedString();
        }
        
        public void Select()
        {
            selected.Invoke(quest);
            background.enabled = true;
        }
        public void Deselect()
        {
            background.enabled = false;
        }
        public void Disable()
        {
            point.color = new Color(
                point.color.r,
                point.color.g,
                point.color.b,
                questName.color.a * 0.5f
            );
            
            questName.color = new Color(
                questName.color.r,
                questName.color.g,
                questName.color.b,
                questName.color.a * 0.5f
            );
        }
    }
}