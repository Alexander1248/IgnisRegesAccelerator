using System;
using System.IO;
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
            var size = Mathf.Min(rect.rect.size.x, rect.rect.size.y);
            while (rect.childCount < used)
            {
                var o = Instantiate(usesPrefab, rect);
                var t = o.GetComponent<RectTransform>();
                t.anchorMax = t.anchorMin = new Vector2(1, 0);
                t.sizeDelta = new Vector2(0.1f, 0.1f) * size;
                t.anchoredPosition = new Vector2(-0.1f, 0.1f * rect.childCount) * size;
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

        public override byte[] SaveState()
        {
            using var stream = new MemoryStream();
            stream.Write(BitConverter.GetBytes(amountPerRound));
            stream.Write(BitConverter.GetBytes(rounds));
            stream.Write(BitConverter.GetBytes(used));
            return stream.ToArray();
        }

        public override void LoadState(byte[] data)
        {
            amountPerRound = BitConverter.ToInt32(data, 0);
            rounds = BitConverter.ToInt32(data, sizeof(int));
            used = BitConverter.ToInt32(data, sizeof(int) << 1);
        }
    }
}