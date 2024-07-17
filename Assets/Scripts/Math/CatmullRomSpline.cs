using UnityEngine;

namespace Math
{
    public class CatmullRomSpline : MonoBehaviour
    {
        [SerializeField] private Transform[] controlPoints;
        [SerializeField] [Min(1)] private int segments = 10;

        private void OnDrawGizmos()
        {
            if (controlPoints.Length < 4)
                return;

            for (var i = 0; i < Count; i++)
            {
                var previousPoint = Get(i - 1);
                for (var j = 1; j <= Segments; j++)
                {
                    var t = j / (float)Segments;
                    var point = Compute(i, t);
                    Gizmos.DrawLine(previousPoint, point);
                    previousPoint = point;
                }
            }
        }

        public Vector3 Get(int i) => controlPoints[i + 2].position;
        public Vector3 Compute(int i, float t)
        {
            var pt = CatmullRom(
                controlPoints[i].position, 
                controlPoints[i + 1].position,
                controlPoints[i + 2].position,
                controlPoints[i + 3].position,
                t);
            return pt;
        }

        public int Count => controlPoints.Length - 3;
        public int Segments => segments;

        private static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            var t2 = t * t;
            var t3 = t2 * t;

            return 0.5f * (2 * p1 + (-p0 + p2) * t +
                           (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
                           (-p0 + 3 * p1 - 3 * p2 + p3) * t3);
        }
    }
}