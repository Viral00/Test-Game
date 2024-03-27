using System.Collections.Generic;

namespace Checkers
{
    [System.Serializable]
    public class Puzzle
    {
        public int PuzzleId;
        public int OptimalMoves;
        public List<PuzzleSquare> Checkers = new List<PuzzleSquare>();
    }

    [System.Serializable]
    public class PuzzleSquare
    {
        public Position Pos;
        public CheckerColor Color;
        public bool IsCrown;
    }
}