using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class EnemyCoordinationController : MonoBehaviour
    {
        [SerializeField] private GameObject[] targets;
        private BVH<EnemyAI> Enemies = new();

        public GameObject[] Targets => targets;

        public void Add(EnemyAI enemy)
        {
            Enemies.Add(enemy);
        }
        public void Remove(EnemyAI enemy)
        {
            Enemies.Remove(enemy);
        }

        public void FindNearest(Vector3 position, float radius, List<EnemyAI> result)
        {
            Enemies.FindNearest(position, radius, result);
        }
    }
}