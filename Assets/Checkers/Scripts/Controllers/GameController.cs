using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Checkers
{
    public class GameController : Singleton<GameController>
    {
        public UnityEvent StartNewGameEvent;

        public bool IsGameStart;
        public UserColor CurrentUserColor;

        [Space]
        public Core CoreInstance;

        public IEnumerator _moveCoroutine;
        public GameMode Mode;
        public GameResult Result { get; private set; }
        public bool IsContinue { get; set; }
        public bool IsRestart { get; set; }

        public void Update()
        {
            MovesAction();
        }

        /// <summary>
        /// Bot Move actions.
        /// </summary>
        private void MovesAction()
        {
            if (IsGameStart)
            {
                if (CoreInstance.CurrentMoveColor == CheckerColor.Black)
                {
                    switch (Mode)
                    {
                        case GameMode.PlayerVsAI:
                        case GameMode.Puzzle:
                            if (!CoreInstance.IsAiMove)
                            {
                                if (_moveCoroutine != null)
                                {
                                    StopCoroutine(_moveCoroutine);
                                }
                                _moveCoroutine = CoreInstance.BotMove();
                                StartCoroutine(_moveCoroutine);
                            }
                            break;
                        case GameMode.PlayerVsPlayer:
                            if (!CoreInstance.IsAiMove)
                            {
                                if (_moveCoroutine != null)
                                {
                                    StopCoroutine(_moveCoroutine);
                                }
                                _moveCoroutine = CoreInstance.SecondPlayerMove();
                                StartCoroutine(_moveCoroutine);
                            }
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Start game action.
        /// </summary>
        [ContextMenu("Start")]
        public void StartGame()
        {
            var undoCtrl = UndoPerformer.Instance;
            var puzzleCtrl = PuzzleController.Instance;
            var boardCtrl = BoardController.Instance;
            var uiViewCtrl = UiViewController.Instance;
            var ganeRulesCtrl = GameRulesController.Instance;

            uiViewCtrl.ResetUIView();
            boardCtrl.Reset();
            ChangeGameResult(GameResult.None);

            CoreInstance = new Core
            {
                GameRule = ganeRulesCtrl.GetRule()
            };

            IsGameStart = true;
            boardCtrl.InitCurrentTurnObjects();
            if (IsContinue)
            {
                CoreInstance.FirstTurnPlayer(CheckerColor.White);
                undoCtrl.LoadGame();

                GameMode undoMode = undoCtrl.StatesData.Type;
                ChooseMode(undoMode);
                puzzleCtrl.SetPuzzleState(undoMode == GameMode.Puzzle);

                if (undoMode == GameMode.Puzzle)
                {
                    puzzleCtrl.SetPuzzle(undoCtrl.StatesData.PuzzleRec.PuzzleId);
                    CoreInstance.InitPuzzle(undoCtrl.StatesData.PuzzleRec.PassedMoves);
                    CoreInstance.InitPassedMoveText();
                    uiViewCtrl.SetActiveScoreText(false);
                }

                undoCtrl.Undo();
            }
            else if (Mode == GameMode.Puzzle)
            {
                undoCtrl.DeleteLastGame();
                undoCtrl.ResetUndoStates();

                Puzzle puzzle = puzzleCtrl.CurrentPuzzle;

                CurrentUserColor = UserColor.White;
                CoreInstance.FirstTurnPlayer(CheckerColor.White);
                CoreInstance.PrepareBoard(puzzleCtrl.IsPuzzleGameActive, puzzle);
                uiViewCtrl.SetActiveScoreText(false);
            }
            else
            {
                undoCtrl.DeleteLastGame();
                undoCtrl.ResetUndoStates();

                CoreInstance.FirstTurnPlayer((CurrentUserColor == UserColor.White) ? CheckerColor.White : CheckerColor.Black);
                CoreInstance.PrepareBoard();
                uiViewCtrl.SetActiveScoreText(true);
            }
            StartNewGameEvent?.Invoke();
            IsRestart = false;
        }

        /// <summary>
        /// Restart game action.
        /// </summary>
        public void Restart()
        {
            IsGameStart = false;
            IsRestart = true;
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            BoardController.Instance.Reset();
            UndoPerformer.Instance.ResetUndoStates();
            StartGame();
        }

        /// <summary>
        /// Change game mode.
        /// </summary>
        public void ChooseMode(GameMode mode)
        {
            Mode = mode;
        }

        /// <summary>
        /// Update result state.
        /// </summary>
        public void ChangeGameResult(GameResult result)
        {
            Result = result;
        }

        /// <summary>
        /// Save state when application Pause.
        /// </summary>
        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (CoreInstance != null && CoreInstance.IsUserMadeFirstMove && !CoreInstance.GameEnd)
                {
                    CoreInstance.WriteUndoStates();
                    UndoPerformer.Instance.SaveGame();
                }
            }
            else
            {
                if (CoreInstance != null && CoreInstance.IsUserMadeFirstMove && !CoreInstance.GameEnd)
                {
                    UndoPerformer.Instance.Undo();
                }
            }
        }

        /// <summary>
        /// Save state when application focus.
        /// </summary>
        public void OnApplicationFocus(bool focus)
        {
            if (!focus)
            {
                if (CoreInstance != null && CoreInstance.IsUserMadeFirstMove && !CoreInstance.GameEnd)
                {
                    CoreInstance.WriteUndoStates();
                    UndoPerformer.Instance.SaveGame();
                }
            }
            else
            {
                if (CoreInstance != null && CoreInstance.IsUserMadeFirstMove && !CoreInstance.GameEnd)
                {
                    UndoPerformer.Instance.Undo();
                }
            }
        }

        /// <summary>
        /// Set state of continue game to TRUE.
        /// </summary>
        public void Continue()
        {
            IsContinue = true;
            StartGame();
        }
    }
}