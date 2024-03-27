using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Checkers
{
    public class PageManager : MonoBehaviour
    {
        public List<PageItemView> Items;
        public ToggleGroup PagesGroup;
        public PageIndicator[] PageToggles;

        public GameObject[] PageButtons;

        private int _currentPage;
        private int _maximumPageIndex => Items.Count - 1;

        /// <summary>
        /// Activate next and prev page buttons
        /// </summary>
        public void SetupPageButtons()
        {
            bool fewPages = Items != null && Items.Count > 1;
            for (int i = 0; i < PageButtons.Length; i++)
            {
                PageButtons[i].SetActive(fewPages);
            }
        }

        /// <summary>
        /// Activate page by index
        /// </summary>
        /// <param name="index"></param>
        public void SetActivePage(int index)
        {
            if (index > Items.Count)
            {
                return;
            }
            
            _currentPage = index;

            for (int i = 0; i < Items.Count; i++)
            {
                bool enabled = Items != null && Items[i].PageIndex == index;

                Items[i].gameObject.SetActive(enabled);
            }

            SetActivePageToggle();
        }

        /// <summary>
        /// Activate previous page
        /// </summary>
        public void SetPrevPage()
        {
            var prevIndex = _currentPage == 0 ? _maximumPageIndex : _currentPage - 1;
            SetActivePage(prevIndex);
        }

        /// <summary>
        /// Activate next page
        /// </summary>
        public void SetNextPage()
        {
            var nextIndex = _currentPage >= _maximumPageIndex ? 0 : _currentPage + 1;
            SetActivePage(nextIndex);
        }

        /// <summary>
        /// Register items in array
        /// </summary>
        /// <param name="views"></param>
        public void InitItems(List<PageItemView> views)
        {
            Items = new List<PageItemView>(views);

            SetupPageButtons();
        }

        /// <summary>
        /// Set active page toggle 
        /// </summary>
		private void SetActivePageToggle()
		{
            if (_currentPage > PageToggles.Length - 1)
            {
                return;
            }
            var activeIndicator = PageToggles[_currentPage];

            if (activeIndicator == null || activeIndicator.ToggleRef == null)
            {
                return;
            }

            activeIndicator.ToggleRef.isOn = true;
            if (activeIndicator.ToggleRef.group == PagesGroup)
            {
                PagesGroup.NotifyToggleOn(activeIndicator.ToggleRef);
            }
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indicators"></param>
        public void InitPageIndicators(PageIndicator[] indicators)
        {
            PageToggles = indicators;

            foreach (var indicator in PageToggles)
            {
                indicator.ToggleRef.group = PagesGroup;
            }
        }
    }
}