using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Checkers
{
    [CreateAssetMenu(fileName = "PuzzlesContainer", menuName = "Puzzles/PuzzlesContainer")]
    public class PuzzlesContainer : ScriptableObject
    {
        public PuzzleData Data;

        public Puzzle CreateNewPuzzle()
        {
            if (Data.Puzzles == null)
            {
                Data.Puzzles = new List<Puzzle>();
            }
            var createdPuzzle = new Puzzle() { PuzzleId = Data.Puzzles.Count + 1 };

            Data.Puzzles.Add(createdPuzzle);

            return createdPuzzle;
        }

        public Puzzle LoadPuzzle(int id)
        {
            return Data.Puzzles.FirstOrDefault(x => x.PuzzleId == id);
        }

        public void ClearPuzzleById(int id)
        {
            var puzzle = LoadPuzzle(id);
            if (puzzle != null)
            {
                puzzle.Checkers = new List<PuzzleSquare>();
            }
        }
    }
}