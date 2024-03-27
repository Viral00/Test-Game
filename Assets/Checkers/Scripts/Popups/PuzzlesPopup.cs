using UnityEngine;

namespace Checkers
{
    public class PuzzlesPopup : GamePopup
    {
        [SerializeField] private PageManager _pageManager;

        public void OnEnable()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            _pageManager.SetActivePage(0);
        }
    }
}