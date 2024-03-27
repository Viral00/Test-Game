using System;
using UnityEngine;

namespace Checkers
{
    [Serializable]
    public class PositionIndex
    {
        public int Index;
        public Vector2 Position;
        public float Distance;

        public PositionIndex(int index, Vector2 position)
        {
            Index = index;
            Position = position;
        }

        /// <summary>
        /// Change distance to center for specific item in scroll.
        /// </summary>
        /// <param name="dist"></param>
        public void UpdateDistance(float dist)
        {
            Distance = dist;
        }
    }
}