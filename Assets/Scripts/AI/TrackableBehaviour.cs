using UnityEngine;

namespace AI
{
    public class TrackableBehaviour<T> : MonoBehaviour where T : TrackableBehaviour<T>
    {
        public BVH<T>.Node Location { get; protected internal set; }
    }
}