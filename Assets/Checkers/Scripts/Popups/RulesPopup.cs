using UnityEngine;

namespace Checkers
{
    public class RulesPopup : GamePopup
    {
        [SerializeField] private GamePopup _puzzlesPopup;
        [SerializeField] private GamePopup _modePopup;

        public void OnRulePreviewClicked()
        {
            if (GameController.Instance.Mode == GameMode.Puzzle)
            {
                _puzzlesPopup.Open();
            }
            else
            {
                _modePopup.Open();
            }
        }
    }
}