using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Checkers
{
    public class PuzzleEditor : Singleton<PuzzleEditor>
    {
        [Header("Generation data: ")]
        [SerializeField] private PuzzlesContainer _puzlesContainer;
        [SerializeField] private GameObject _whiteSquare;
        [SerializeField] private GameObject _blackSquare;
        [SerializeField] private Transform _board;

        private List<PuzzleCreationSquare> _squares = new List<PuzzleCreationSquare>();

        [Header("Input data: ")]
        public PuzzleInputState CurrentInput = PuzzleInputState.BlackChecker;
        public InputField LoadLevelInput;

        [Header("Output data: ")]
        public Text CurrentPuzzleText;

        private int _currentPuzzleId;

        private readonly Vector2Int _boardSize = new Vector2Int(8, 8);
        private Puzzle _tempPuzzle;

        protected override void Awake()
        {
            base.Awake();

            SetCurrentPuzzleId(-1);
            _tempPuzzle = null;

            GenerateField();
        }

        private void SetCurrentPuzzleId(int id)
        {
            _currentPuzzleId = id;
            CurrentPuzzleText.text = $"Current puzzle: {_currentPuzzleId}";

        }
        public void GenerateField()
        {
            for (int y = 0; y < _boardSize.y; y++)
            {
                for (int x = 0; x < _boardSize.x; x++)
                {
                    var blackSquare = (x + y) % 2 == 0;
                    var prefab = blackSquare ? _blackSquare : _whiteSquare;
                    var square = Instantiate(prefab, _board).GetComponent<PuzzleCreationSquare>();
                    if (square)
                    {
                        square.name = $"{(blackSquare ? "Black" : "White")} Pos({x}:{y})";
                        square.Data = new CreationSquareData() { Pos = new Position { X = x, Y = y } };

                        _squares.Add(square);
                    }
                }
            }
        }

        public void SetBlackCheckerInput(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            ChangeInputState(PuzzleInputState.BlackChecker);
        }

        public void SetWhiteCheckerInput(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            ChangeInputState(PuzzleInputState.WhiteChecker);
        }

        public void SetCrownInput(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            ChangeInputState(PuzzleInputState.Crown);
        }

        public void SetClearInput(bool isOn)
        {
            if (!isOn)
            {
                return;
            }

            ChangeInputState(PuzzleInputState.Clear);
        }

        private void ChangeInputState(PuzzleInputState state)
        {
            CurrentInput = state;
        }

        public void LoadPuzzle()
        {
            var puzzleId = int.Parse(LoadLevelInput.text);
            var puzzle = _puzlesContainer.LoadPuzzle(puzzleId);
            if (puzzle != null)
            {
                _tempPuzzle = puzzle;
                SetCurrentPuzzleId(puzzleId);
                UpdateBoard();
            }
        }

        public void ClearPuzzle()
        {
            _puzlesContainer.ClearPuzzleById(_currentPuzzleId);
            UpdateBoard();
        }

        public void CreateNewPuzzle()
        {
            _tempPuzzle = _puzlesContainer.CreateNewPuzzle();
            SetCurrentPuzzleId(_tempPuzzle.PuzzleId);
            UpdateBoard();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_puzlesContainer);
#endif
        }

        private void UpdateBoard()
        {
            if (_tempPuzzle == null)
            {
                return;
            }
            _squares.ForEach(x => x.ClearInfo());

            var puzzle = _tempPuzzle;

            for (int j = 0; j < puzzle.Checkers.Count; j++)
            {
                var checkeInfo = puzzle.Checkers[j];
                var square = _squares.First(x => x.Data.Pos == checkeInfo.Pos);

                square.Data.Info = new CreationCheckerInfo()
                {
                    Color = checkeInfo.Color,
                    IsCrown = checkeInfo.IsCrown
                };
            }

            for (int i = 0; i < _squares.Count; i++)
            {
                var square = _squares[i];

                square.UpdateView();
            }
        }

        public void OnSquareClicked(CreationSquareData data)
        {
            if (_tempPuzzle == null)
            {
                return;
            }

            var puzzle = _tempPuzzle;
            var resultedInfo = _tempPuzzle.Checkers.FirstOrDefault(x => x.Pos == data.Pos);

            if (resultedInfo == null)
            {
                resultedInfo = new PuzzleSquare() { Pos = data.Pos };
            }

            switch (CurrentInput)
            {
                case PuzzleInputState.BlackChecker:
                    resultedInfo.Color = CheckerColor.Black;
                    break;
                case PuzzleInputState.WhiteChecker:
                    resultedInfo.Color = CheckerColor.White;
                    break;
                case PuzzleInputState.Crown:
                    resultedInfo.IsCrown = !resultedInfo.IsCrown;
                    break;
                case PuzzleInputState.Clear:
                    _tempPuzzle.Checkers.Remove(resultedInfo);
                    resultedInfo = null;
                    break;
            }

            var infoRef = _tempPuzzle.Checkers.FirstOrDefault(x => x.Pos == data.Pos);

            if (resultedInfo == null)
            {
                if (infoRef != null)
                {
                    _tempPuzzle.Checkers.Remove(infoRef);
                }
            }
            else
            {
                if (infoRef == null)
                {
                    _tempPuzzle.Checkers.Add(resultedInfo);
                }
                else
                {
                    infoRef = resultedInfo;
                }
            }

            var square = _squares.First(x => x.Data.Pos == data.Pos);
            square.Data.Info = resultedInfo != null ? new CreationCheckerInfo()
            {
                Color = resultedInfo.Color,
                IsCrown = resultedInfo.IsCrown
            } : null;
            square.UpdateView();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_puzlesContainer);
#endif
        }
    }

    public enum PuzzleInputState
    {
        BlackChecker = 0,
        WhiteChecker,
        Crown,
        Clear
    }
}