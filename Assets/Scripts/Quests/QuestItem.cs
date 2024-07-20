using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Quests
{
    public class QuestItem : MonoBehaviour
    {
        [HideInInspector] public int index = -1;
        [HideInInspector] public Quest quest;
        [HideInInspector] public UnityEvent<int> selected;

        [SerializeField] private TMP_Text questName;
        [SerializeField] private Image point;

        public void Start()
        {
            questName.text = quest.Name.GetLocalizedString();
        }
        
        public void Select()
        {
            selected.Invoke(index);
        }
    }
}