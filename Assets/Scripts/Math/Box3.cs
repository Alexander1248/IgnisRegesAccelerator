using UnityEngine;

namespace Math
{
    public struct Box3
    {
        public Vector3 Min;
        public Vector3 Max;

        public Vector3 Center => (Min + Max) * 0.5f;
        public Vector3 Size => Max - Min;
        public Vector3 HalfSize => Size * 0.5f;
    
        public Box3(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }
    
        public Box3(
            float minx, float miny, float minz, 
            float maxx, float maxy, float maxz)
        {
            Min = new Vector3(minx, minx, minz);
            Max = new Vector3(maxx, maxy, maxz);
        }
        public Box3(Box3 original) : this(original.Min, original.Max) { }
    
        public static Box3 Union(Box3 a, Box3 b)
        {
            return new Box3(
                Vector3.Min(a.Min, b.Min),
                Vector3.Max(a.Max, b.Max)
            );
        }
        public static Box3 Intersection(Box3 a, Box3 b)
        {
            return new Box3(
                Vector3.Max(a.Min, b.Min),
                Vector3.Min(a.Max, b.Max)
            );
        }
        public static bool SphereIntersection(Box3 box, Vector3 origin, float radius)
        {
            var closest = new Vector3(
                Mathf.Max(box.Min.x, Mathf.Min(origin.x, box.Max.x)),
                Mathf.Max(box.Min.y, Mathf.Min(origin.y, box.Max.y)),
                Mathf.Max(box.Min.z, Mathf.Min(origin.z, box.Max.z))
            );
            return Vector3.Distance(origin, closest) < radius;
        } 
        public static float Distance(Box3 box, Vector3 point)
        {
            var dx = Mathf.Max(Mathf.Abs(point.x - box.Center.x) - box.HalfSize.x, 0);
            var dy = Mathf.Max(Mathf.Abs(point.y - box.Center.y) - box.HalfSize.y, 0);
            var dz = Mathf.Max(Mathf.Abs(point.z - box.Center.z) - box.HalfSize.z, 0);
            return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        public static float Distance(Box3 a, Box3 b)
        {
            var u = Vector3.Max(Vector3.zero, a.Min - b.Max);
            var v = Vector3.Max(Vector3.zero, b.Min - a.Max);
            return Mathf.Sqrt(u.sqrMagnitude + v.sqrMagnitude);
        }

        public bool ContainsInclusive(Vector3 point)
        {
            return point.x >= Min.x && point.x <= Max.x
                                    && point.y >= Min.y && point.y <= Max.y 
                                    && point.z >= Min.z && point.z <= Max.z;
        }

        public bool ContainsExclusive(Vector3 point)
        {
            return point.x > Min.x && point.x < Max.x 
                                   && point.y > Min.y && point.y < Max.y 
                                   && point.z > Min.z && point.z < Max.z;
        }
        public override string ToString()
        {
            return $"[{Min}, {Max}]";
        }
    }
}