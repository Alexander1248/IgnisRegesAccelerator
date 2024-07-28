﻿using System;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Items.Active
{
    
    [CreateAssetMenu(menuName = "Items/Heal")]
    public class HealItem : Item
    {
        [SerializeField] private int amountPerRound;
        [SerializeField] private int rounds;
        [SerializeField] private AudioClip sound;
        [FormerlySerializedAs("uses")] [SerializeField] private GameObject usesPrefab;
        private int used;

        private void Awake()
        {
            used = rounds;
        }

        public override bool Use(Inventory inventory, int x, int y, GameObject player, AudioSource audioSource)
        {
            Debug.Log(this);
            audioSource.clip = sound;
            audioSource.Play();
            var health = player.GetComponent<Health>();
            health.ChangeHealth(amountPerRound);
            used--;
            return used <= 0;
        }

        public override void Draw(RectTransform rect)
        {
            while (rect.childCount < used)
            {
                var o = Instantiate(usesPrefab, rect);
                var t = o.GetComponent<RectTransform>();
                t.anchorMax = t.anchorMin = new Vector2(1, 0);
                t.sizeDelta = rect.rect.size * new Vector2(0.1f, 0.1f);
                t.anchoredPosition = rect.rect.size * new Vector2(-0.1f, 0.1f * (1 + rect.childCount));
            }

            while (rect.childCount > used)
            {
                var transform = rect.GetChild(rect.childCount - 1);
                transform.parent = null;
                Destroy(transform.gameObject);
            }
        }

        protected bool Equals(HealItem other)
        {
            return base.Equals(other) && amountPerRound == other.amountPerRound && rounds == other.rounds && used == other.used;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HealItem)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), amountPerRound, rounds, usesPrefab, used);
        }
    }
}