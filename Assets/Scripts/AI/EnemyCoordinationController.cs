using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class EnemyCoordinationController : MonoBehaviour
    {
        private BVH<EnemyAI> Enemies = new();

        public void Add(EnemyAI enemy)
        {
            Enemies.Add(enemy);
        }

        public void FindNearest(Vector3 position, float radius, List<EnemyAI> result)
        {
            Enemies.FindNearest(position, radius, result);
        }
    }
}