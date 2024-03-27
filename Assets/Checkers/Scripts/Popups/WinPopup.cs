using UnityEngine;

namespace Checkers
{
    public class WinPopup : GamePopup
    {
        [SerializeField]
        private GameObject _startNextPuzzleButton;

        public void OnEnable()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            UpdateContinueButtonState();
        }

        private void UpdateContinueButtonState()
        {
            var state = GameController.Instance.Mode == GameMode.Puzzle;
            _startNextPuzzleButton.SetActive(state);
        }
    }
}