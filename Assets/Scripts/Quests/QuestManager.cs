using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Quests
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Quest> onSelectedChanged;
        [SerializeField] private UnityEvent<Quest> onQuestAdded;
        [SerializeField] private UnityEvent<Quest> onQuestCompleted;
        [SerializeField] private UnityEvent<Quest> onQuestAborted;
        
        
        private readonly List<Quest> _quests = new();
        private readonly HashSet<Quest> _completedQuests = new();
        
        public Quest this[int index] => _quests[index];
        public int Count => _quests.Count;
        public Quest SelectedQuest { get; private set; }

        public IEnumerable<(string, bool)> Quests => 
            _quests.Select(quest => (quest.ID, false)).AppendWith(_completedQuests.Select(quest => (quest.ID, true)));
        
        public void Add(Quest quest)
        {
            _quests.Add(quest);
            onQuestAdded.Invoke(quest);
            if (SelectedQuest != null) return;
            
            SelectedQuest = quest;
            onSelectedChanged.Invoke(SelectedQuest);
        }

        public void Add(Quest quest, out int index)
        {
            Add(quest);
            index = _quests.Count - 1;
        }
        
        public void Select(int index)
        {
            if (index < 0 || index >= _quests.Count) SelectedQuest = null;
            else SelectedQuest = _quests[index];
            onSelectedChanged.Invoke(SelectedQuest);
        }
        public void Abort(int index)
        {
            var quest = _quests[index];
            if (SelectedQuest == quest)
            {
                SelectedQuest = null;
                onSelectedChanged.Invoke(SelectedQuest);
            }
            _quests.RemoveAt(index);
            onQuestAborted.Invoke(quest);
        }
        public void Complete(int index)
        {
            var quest = _quests[index];
            if (SelectedQuest == quest)
            {
                SelectedQuest = null;
                onSelectedChanged.Invoke(SelectedQuest);
            }
            _completedQuests.Add(quest);
            _quests.RemoveAt(index);
            onQuestCompleted.Invoke(quest);
        }
        public void Complete(Quest quest)
        {
            if (Find(quest) == -1){
                Debug.LogError("NO SUCH QUEST!");
                return;
            }
            Complete(Find(quest));
        }
        public void Abort(Quest quest)
        {
            Abort(Find(quest));
        }
        public int Find(Quest quest)
        {
            return _quests.IndexOf(quest);
        }
        public List<Quest> Find(Predicate<Quest> condition)
        {
            return _quests.FindAll(condition);
        }
        public bool IsCompleted(Quest quest)
        {
            return _completedQuests.Contains(quest);
        }

        public void RemoveCompleted(Quest quest)
        {
            _completedQuests.Remove(quest);
        }
        public void RemoveCompleted(Predicate<Quest> condition)
        {
            _completedQuests.RemoveWhere(condition);
        }
    }
}