﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Checkers
{
    public class PuzzleController : Singleton<PuzzleController>
    {
        public bool IsPuzzleGameActive;

        private readonly string _progressKey = "PuzzleProgress";
        private PuzzleProgress _progress;

        public PuzzleData Data;
        public Puzzle CurrentPuzzle;
        public PuzzlesContainer Container;

        public bool OpenAllPuzzles;

        public int OpenedPuzzlesAtStartAmount;

        private readonly string _levelProggressNotFoundDebug = "Can not find result of Puzzle with this id {0}.";

        /// <summary>
        /// Get puzzled from json.
        /// </summary>
        public void GetPuzzlles()
        {
            Data = Container.Data;
        }

        /// <summary>
        /// Initialize progresses data.
        /// </summary>
        private void InitProgresses()
        {
            _progress = new PuzzleProgress(OpenedPuzzlesAtStartAmount);

            for (int i = 0; i < Data.Puzzles.Count; i++)
            {
                _progress.Results.Add(new PuzzleResult(Data.Puzzles[i].PuzzleId, i < OpenedPuzzlesAtStartAmount || OpenAllPuzzles ? PuzzleStatus.Available : PuzzleStatus.NonAvailable));
            }
        }

        private void ValidateProgresses(List<Puzzle> newPuzzles)
        {
            if (_progress == null)
            {
                return;
            }

            int removedAmount = _progress.Results.RemoveAll(result => Data.Puzzles.FirstOrDefault(puzzle => puzzle.PuzzleId == result.Id) == null);
            if (removedAmount > 0)
            {
                Debug.LogWarning($"Have been removed {removedAmount} old puzzles.");
            }

            if (newPuzzles == null || newPuzzles.Count == 0)
            {
                SaveProgress();
                return;
            }

            for (int i = 0; i < newPuzzles.Count; i++)
            {
                var puzzleData = newPuzzles[i];
                _progress.Results.Add(new PuzzleResult(puzzleData.PuzzleId, PuzzleStatus.NonAvailable));
            }

            SaveProgress();
        }

        private void UpdateAvailablePuzzles()
        {
            var availablePuzzlesAmount = _progress.Results.Count(x => x.Status == PuzzleStatus.Available);
            if (availablePuzzlesAmount >= OpenedPuzzlesAtStartAmount)
            {
                return;
            }

            bool hasUnsavedProgress = false;

            for (int i = 0; i < _progress.Results.Count; i++)
            {
                var result = _progress.Results[i];
                if (result.Status == PuzzleStatus.NonAvailable)
                {
                    var shouldOpen = OpenedPuzzlesAtStartAmount - availablePuzzlesAmount++ > 0;
                    result.Status = shouldOpen || OpenAllPuzzles ? PuzzleStatus.Available : PuzzleStatus.NonAvailable;

                    hasUnsavedProgress = true;
                }
            }
            int lastPuzzlIndex = _progress.LastNonAvailablePuzzle;

            PuzzleResult nextNotOpenedPuzzle = _progress.Results.FirstOrDefault(x => x.Status == PuzzleStatus.NonAvailable);
            if (nextNotOpenedPuzzle != null)
            {
                var nextNotOpenedPuzzleId = _progress.Results.IndexOf(nextNotOpenedPuzzle);

                _progress.LastNonAvailablePuzzle = nextNotOpenedPuzzleId + 1;
            }

            if (lastPuzzlIndex != _progress.LastNonAvailablePuzzle)
            {
                hasUnsavedProgress = true;
            }

            if (hasUnsavedProgress)
            {
                SaveProgress();
            }
        }

        /// <summary>
        /// Save data to prefs.
        /// </summary>
        public void SaveProgress()
        {
            string progress = JsonUtility.ToJson(_progress);

            PlayerPrefs.SetString(_progressKey, progress);
        }

        /// <summary>
        /// Get data from prefs and convert to Progress instance.
        /// </summary>
        public PuzzleProgress GetProgress()
        {
            string progress = PlayerPrefs.GetString(_progressKey);

            _progress = JsonUtility.FromJson<PuzzleProgress>(progress);

            if (_progress == null)
            {
                InitProgresses();
                SaveProgress();
                return _progress;
            }
            var tempProgress = new List<PuzzleResult>(_progress.Results);
            var newPuzzles = new List<Puzzle>();
            for (int i = 0; i < Data.Puzzles.Count; i++)
            {
                var puzzleData = Data.Puzzles[i];
                var result = tempProgress.FirstOrDefault(puzzle => puzzle.Id == puzzleData.PuzzleId);

                if (result != null)
                {
                    tempProgress.Remove(result);
                }
                else
                {
                    newPuzzles.Add(puzzleData);
                }
            }

            ValidateProgresses(newPuzzles);
            UpdateAvailablePuzzles();

            return _progress;
        }

        /// <summary>
        /// Get puzzle result by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PuzzleResult GetPuzzleResultById(int id)
        {
            return _progress.Results.Find(puzzle => puzzle.Id == id);
        }

        /// <summary>
        /// Get puzzle format by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Puzzle GetPuzzleById(int id)
        {
            return Data.Puzzles.Find(puzzle => puzzle.PuzzleId == id);
        }

        /// <summary>
        /// Update progress of level by id.
        /// </summary>
        public PuzzleResult UpdatePuzzleResultById(int id, PuzzleStatus newStatus)
        {
            PuzzleResult puzzleProgress = _progress.Results.Find(puzzle => puzzle.Id == id);

            if (puzzleProgress != null)
            {
                if (puzzleProgress.Status != PuzzleStatus.PassedWell)
                    puzzleProgress.Status = newStatus;

                SaveProgress();
            }
            else
            {
                Debug.LogError(string.Format(_levelProggressNotFoundDebug, id));
            }

            return puzzleProgress;
        }

        /// <summary>
        /// Clear all progress data prefs.
        /// </summary>
        public void ClearProgress()
        {
            PlayerPrefs.DeleteKey(_progressKey);
        }

        /// <summary>
        /// Open next puzzle in data.
        /// </summary>
        public void OpenNextPuzzle()
        {
            int nextPuzzleid = _progress.LastNonAvailablePuzzle + 1;
            int currentPuzzleId = CurrentPuzzle.PuzzleId;
            PuzzleResult nextNotOpenedPuzzle = _progress.Results.FirstOrDefault(x => x.Status == PuzzleStatus.NonAvailable);

            PuzzleResult currenPuzzle = _progress.Results.Find(x => x.Id == currentPuzzleId);

            if (currenPuzzle != null && (currenPuzzle.Status == PuzzleStatus.Passed || currenPuzzle.Status == PuzzleStatus.PassedWell))
            {
                return;
            }
            if (nextPuzzleid > _progress.Results.Count)
            {
                return;
            }
            int nextNotOpenedPuzzleId = -1;
            int currentOpenedPuzzleId = -1;
            if (nextNotOpenedPuzzle != null)
            {
                nextNotOpenedPuzzleId = _progress.Results.IndexOf(nextNotOpenedPuzzle);
                currentOpenedPuzzleId = _progress.Results.IndexOf(currenPuzzle);
                //Uncomment if we want to open next puzzle only when passed previous of first not opened puzzle.
                if (nextNotOpenedPuzzleId != nextPuzzleid - 1 /*|| currentOpenedPuzzleId != nextPuzzleid - 2*/)
                {
                    return;
                }
            }

            PuzzleResult nextPuzzle = _progress.Results[nextPuzzleid - 1];
            if (nextPuzzle != null)
            {
                nextPuzzle.Status = PuzzleStatus.Available;
                _progress.LastNonAvailablePuzzle = nextNotOpenedPuzzleId + 1;
                SaveProgress();
                PuzzlesMenuCreator.Instance.UpdatePuzzleItemStatus(nextPuzzle.Id, nextPuzzle);
            }
        }

        /// <summary>
        /// Start next puzzle action.
        /// </summary>
        public void StartNextPuzzle()
        {
            PuzzleResult currentPuzzle = _progress.Results.Find(x => x.Id == CurrentPuzzle.PuzzleId);

            if (currentPuzzle != null)
            {
                int curPuzzleIdInList = _progress.Results.IndexOf(currentPuzzle);

                int nextPuzzleid = curPuzzleIdInList == _progress.Results.Count - 1 ? 0 : curPuzzleIdInList + 1;

                StartPuzzle(Data.Puzzles[nextPuzzleid]);
            }

            /* ------ Open next puzzle from Last puzzle id ------*/
            //int nextPuzzleid = _progress.LastPuzzle;

            //if (nextPuzzleid <= Data.Puzzles.Count)
            //{
            //    StartPuzzle(Data.Puzzles[nextPuzzleid - 1]);
            //}
            /* ------------------------------------------------- */
        }

        /// <summary>
        /// Is has puzzled which are not opening yet.
        /// </summary>
        /// <returns></returns>
        public bool IsHasNotOpenedPuzzles()
        {
            //Logic: TRUE when game exist just one NonAvailable puzzle.
            int countOfNotOpenedPuzzles = _progress != null ? _progress.Results.Where(x => x.Status == PuzzleStatus.NonAvailable).Count() : 0;
            bool result = countOfNotOpenedPuzzles > 0 && _progress != null && _progress.LastNonAvailablePuzzle < _progress.Results.Count;
            return result;
        }

        /// <summary>
        /// Start specific puzzle.
        /// </summary>
        /// <param name="puzzle"></param>
        public void StartPuzzle(Puzzle puzzle)
        {
            SetPuzzle(puzzle.PuzzleId);
            EnablePuzzleState();
            GameController.Instance.StartGame();
        }

        /// <summary>
        /// Set puzzle data for game.
        /// </summary>
        public void SetPuzzle(int puzzleId)
        {
            CurrentPuzzle = GetPuzzleById(puzzleId);
        }

        /// <summary>
        /// Set puzzle data for game.
        /// </summary>
        public void SetPuzzle(PuzzleFormat puzzleFormat)
        {
            CurrentPuzzle = PuzzleConverter.ConvertPuzzle(puzzleFormat);
        }

        /// <summary>
        /// Change puzzle status.
        /// </summary>
        /// <param name="passedMoves"></param>
        /// <param name="optimalMoves"></param>
        public void UpdatePuzzleStatus(int passedMoves, int optimalMoves)
        {
            int id = CurrentPuzzle.PuzzleId;
            PuzzleResult result = UpdatePuzzleResultById(id, (passedMoves <= optimalMoves) ? PuzzleStatus.PassedWell : PuzzleStatus.Passed);
            PuzzlesMenuCreator.Instance.UpdatePuzzleItemStatus(id, result);
        }

        /// <summary>
        /// Set puzzle mode activity to TRUE.
        /// </summary>
        public void EnablePuzzleState()
        {
            SetPuzzleState(true);
        }

        /// <summary>
        /// Set puzzle mode activity to FALSE.
        /// </summary>
        public void DisablePuzzleState()
        {
            SetPuzzleState(false);
        }

        /// <summary>
        /// Change state of puzzle mode activity.
        /// </summary>
        /// <param name="state"></param>
        public void SetPuzzleState(bool state)
        {
            IsPuzzleGameActive = state;
        }
    }
}