using System.Collections.Generic;

namespace Checkers
{
    [System.Serializable]
    public class PuzzleProgress
    {
        public int LastNonAvailablePuzzle;

        public List<PuzzleResult> Results = new List<PuzzleResult>();

        public PuzzleProgress(int lastPuzzle)
        {
            LastNonAvailablePuzzle = lastPuzzle;
        }
    }

    [System.Serializable]
    public class PuzzleResult
    {
        public int Id;
        public PuzzleStatus Status;

        public PuzzleResult(int id, PuzzleStatus status)
        {
            Id = id;
            Status = status;
        }
    }
}