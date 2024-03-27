using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Checkers
{
    public class PuzzlesMenuCreator : Singleton<PuzzlesMenuCreator>
    {
        public PageManager PageManager;
        public ToggleGroup PagesContainer;
        public Transform MenuHolder;
        public PuzzlesPopup Popup;

        [Header("Menu elements: ")]
        public GameObject PageItemView;
        public GameObject PageItem;
        public GameObject PageIndicatorItem;
        public GameObject RowItem;
        public GameObject PuzzleItem;

        [Header("Menu options: ")]
        public int PuzzlesInRow = 5;
        public int PuzzlesInColumn = 5;

        [Space]
        public Sprite DefaultSprite;
        public Sprite PassedSprite;

        public Color32 AvailableTextColor;
        public Color32 NotAvailableTextColor;

        public List<PuzzleItem> Items = new List<PuzzleItem>();

        /// <summary>
        /// Create menu with puzzle items.
        /// </summary>
        public void Create()
        {
            foreach (Transform item in MenuHolder)
            {
                Destroy(item.gameObject);
            }

            PuzzleController ctrl = PuzzleController.Instance;
            List<Puzzle> puzzles = ctrl.Data.Puzzles;
            List<PageItemView> items = new List<PageItemView>();
            List<PageIndicator> indicators = new List<PageIndicator>();
            GameObject scrollItem;
            GameObject curPage = null;
            GameObject curPageIndic;
            GameObject curRow = null;

            int pagesCount = Mathf.CeilToInt(puzzles.Count / PuzzlesInRow * PuzzlesInColumn);
            for (int i = 0; i < puzzles.Count; i++)
            {
                //Create page
                if ((i) % (PuzzlesInRow * PuzzlesInColumn) == 0)
                {
                    scrollItem = Instantiate(PageItemView, MenuHolder);
                    curPage = Instantiate(PageItem, scrollItem.transform);
                    var pageView = scrollItem.GetComponent<PageItemView>();
                    pageView.PageIndex = items.Count;
                    items.Add(pageView);
                }
                //Create page indicator
                if ((i) % (PuzzlesInRow * PuzzlesInColumn) == 0)
                {
                    curPageIndic = Instantiate(PageIndicatorItem, PagesContainer.transform);
                    indicators.Add(curPageIndic.GetComponent<PageIndicator>());
                }
                //Create row
                if ((i) % PuzzlesInRow == 0)
                {
                    curRow = Instantiate(RowItem, curPage.transform);
                }

                PuzzleItem puzzleItem = Instantiate(PuzzleItem, curRow.transform).GetComponent<PuzzleItem>();
                Puzzle puzzle = puzzles[i];
                PuzzleResult result = ctrl.GetPuzzleResultById(puzzle.PuzzleId);

                puzzleItem.ElementId  = puzzle.PuzzleId;
                puzzleItem.name = $"PuzzleItem: {i + 1}";
                puzzleItem.IdText.text = (i + 1).ToString();
                puzzleItem.PuzzleActionButton.onClick.AddListener(() =>  OnPuzzleClicked(puzzle));
                Items.Add(puzzleItem);

                UpdatePuzzleItemStatus(puzzle.PuzzleId, result);
            }

            PageManager.InitPageIndicators(indicators.ToArray());
            PageManager.InitItems(items);
        }

        public void OnPuzzleClicked(Puzzle puzzle)
        {
            PuzzleController.Instance.StartPuzzle(puzzle);
            Popup.Close();
        }

        /// <summary>
        /// Get puzzle menu item sprite depending on puzzle status.
        /// </summary>
        public Sprite GetStatusSprite(PuzzleStatus status)
        {
            switch (status)
            {
                case PuzzleStatus.Passed:
                    return PassedSprite;
                case PuzzleStatus.Available:
                default:
                    return DefaultSprite;
            }
        }

        /// <summary>
        /// Change puzzle item status in menu.
        /// </summary>
        public void UpdatePuzzleItemStatus(int itemId, PuzzleResult result)
        {
            PuzzleItem puzzleItem = Items.Find(x => x.ElementId == itemId);
            bool isAvailable = result != null && result.Status != PuzzleStatus.NonAvailable;

            puzzleItem.IdText.color = isAvailable ? AvailableTextColor : NotAvailableTextColor;
            puzzleItem.StatusImage.sprite = GetStatusSprite(result != null ? result.Status : PuzzleStatus.NonAvailable);
            puzzleItem.PuzzleActionButton.interactable = isAvailable;
        }
    }
}