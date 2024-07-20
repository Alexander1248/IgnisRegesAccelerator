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
    }
}