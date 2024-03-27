using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

namespace Checkers
{
    public class UiViewController : Singleton<UiViewController>
    {
        [Header("UI Panels:")]
        public GamePopup WinPanel;
        public GamePopup InfoPanel;
        public GamePopup ChooseColorPanel;
        public GamePopup ChooseModePanel;
        public GamePopup ChooseRulePanel;

        [Header("UI objects:")]
        public Canvas MainCanvas;
        public Image ReplayImage;
        public Image ResetCameraImage;
        public Image NoAdsImage;

        [Header("UI components:")]
        public Image MainButttonImage;
        public Text WinMessageText;

        [Header("Text objects:")]
        public GameObject EnemyScoreTextContainer;
        public GameObject PlayerScoreTextContainer;
        [Space]
        public TextMesh EnemyScoreTextMesh;
        public TextMesh PlayerScoreTextMesh;
        [Space]
        public TextMesh EnemyNameTextMesh;
        public TextMesh PlayerNameTextMesh;

        public UnityEvent ReplayRequested;

        public UnityEvent ChangeCamera;

        public UnityEvent ShowNoAdsAction;

        public UnityEvent StartNextPuzzleCicked;

        [Header("Info texts:")]
        public string PlayerOneName = "Player 1";
        public string PlayerTwoName = "Player 2";
        public string YourName = "You";
        public string AiName = "AI";

        [Header("Rule UI:")]
        public Text RuleLabel;
        public Image RuleImage;

        public void Iniialize()
        {
            GameRulesController.Instance.SetRuleEvent += ChangeRule;
            //AdsController.Instance.RewardAction += OnRewardAction;
        }

       

        public void OnGameStarted()
        {
            if (!ReplayImage.enabled)
                ReplayImage.enabled = true;
        }

        /// <summary>
        /// Enable reset camera button.
        /// </summary>
        public void OnActivateResetCameraButton()
        {
            ResetCameraImage.enabled = true;
        }

        /// <summary>
        /// Disable reset camera button.
        /// </summary>
        public void OnDeactivateResetCameraButton()
        {
            ResetCameraImage.enabled = false;
        }

        /// <summary>
        /// Disable no ads button.
        /// </summary>
        public void DeactivateNoAdsButton()
        {
            NoAdsImage.enabled = (false);
        }

        /// <summary>
        /// Show no ads info pop up.
        /// </summary>
        public void ShowNoAdsInfoPopUp()
        {
            AlertPopUpController.ShowAlertPopUp(
                msg: $"After watching rewarded video Ads will be disappear for one game session. Video start from {DataConfig.Instance.NoAdsRewardAppearDelay} seconds.",
                parent: MainCanvas.transform);
            StartCoroutine(NoAdsClickedCoroutine());
        }

        /// <summary>
        /// Show win panel with message of user win.
        /// </summary>
        public void ShowUserWinMessage()
        {
            GameMode mode = GameController.Instance.Mode;

            var message = string.Empty;

            switch (mode)
            {
                case GameMode.PlayerVsAI:
                    message = $"{YourName} Win";
                    break;
                case GameMode.PlayerVsPlayer:
                    message = $"{PlayerOneName} Win";
                    break;
                case GameMode.Puzzle:
                    var puzzle = PuzzleController.Instance.CurrentPuzzle;
                    message = $"Puzzle {puzzle.PuzzleId} complete!";
                    break;
            }

            WinMessageText.text = message;
            WinPanel.Open();
        }

        /// <summary>
        /// Show win panel with message of AI win.
        /// </summary>
        public void ShowAIWinMessage()
        {
            GameMode mode = GameController.Instance.Mode;

            WinMessageText.text = $"{(mode == GameMode.PlayerVsAI ? AiName : PlayerTwoName)}  Win";
            WinPanel.Open();
        }


        /// <summary>
        /// Show info panel after click on Info button.
        /// </summary>
        public void ShowInfoPanel()
        {
            InfoPanel.Open();
        }

        /// <summary>
        /// Called when click on replay button.
        /// </summary>
        public void ReplayClicked()
        {
            ReplayRequested?.Invoke();
        }

        /// <summary>
        /// Called when click on next puzzle button.
        /// </summary>
        public void StartNextPuzzleClicked()
        {
            StartNextPuzzleCicked?.Invoke();
        }

        /// <summary>
        /// Called when click on Main Button.
        /// </summary>
        public void MainButtonActivate()
        {
            if (!MainButttonImage.enabled)
                MainButttonImage.enabled = true;
        }

        /// <summary>
        /// Called when click on ChangeCamera button.
        /// </summary>
        public void ChangeCameraClicked()
        {
            ChangeCamera?.Invoke();
        }

        private IEnumerator NoAdsClickedCoroutine()
        {
            yield return new WaitForSeconds(DataConfig.Instance.NoAdsRewardAppearDelay);
            NoAdsRewardedClicked();
        }

        /// <summary>
        /// Called when click on NoAds button.
        /// </summary>
        public void NoAdsRewardedClicked()
        {
            ShowNoAdsAction?.Invoke();
        }

        /// <summary>
        /// Open url when click on like us button.
        /// </summary>
        public void LikeUs(string url)
        {
            Application.OpenURL(url);
        }

        /// <summary>
        /// Reset ui view visual.
        /// </summary>
        public void ResetUIView()
        {
            WinPanel.Close();
            UpdateGameScore(12, 12);
            UpdateUserNames();
        }

        /// <summary>
        /// Change game score (user vs ai).
        /// </summary>
        public void UpdateGameScore(int playerScoreValue, int enemyScoreValue)
        {
            PlayerScoreTextMesh.text = playerScoreValue.ToString();
            EnemyScoreTextMesh.text = enemyScoreValue.ToString();
        }

        /// <summary>
        /// Update user names fron fields in inspector.
        /// </summary>
        public void UpdateUserNames()
        {
            GameMode mode = BoardController.Instance.GameControllerComponent.Mode;

            PlayerNameTextMesh.text = (mode == GameMode.PlayerVsAI ? YourName : PlayerOneName);
            EnemyNameTextMesh.text = (mode == GameMode.PlayerVsAI ? AiName : PlayerTwoName);
        }

        public void ShowGameResultAction(GameResult result)
        {
            switch (result)
            {
                case GameResult.Won:

                    ShowUserWinMessage();
                    break;
                case GameResult.Lose:
                    ShowAIWinMessage();
                    break;
            }
        }

        public void ChangeRule(CheckersRule rule)
        {
            RuleLabel.text = $"Rule: {rule.Label}";
            RuleImage.sprite = rule.Preview;
        }

        public void SetActiveScoreText(bool state)
        {
            PlayerScoreTextContainer.SetActive(state);
            EnemyScoreTextContainer.SetActive(state);
        }

        private void OnDestroy()
        {
            GameRulesController.Instance.SetRuleEvent -= ChangeRule;
        }
    }
}
