﻿using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Quests
{
    [Serializable]
    [CreateAssetMenu(menuName = "Quests/Quest", fileName = "Quest")]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private new LocalizedString name;
        [SerializeField] private LocalizedString task;
        [SerializeField] private LocalizedString description;

        public string ID => id;
        public LocalizedString Name => name;
        public LocalizedString Task => task;
        public LocalizedString Description => description;
    }
}