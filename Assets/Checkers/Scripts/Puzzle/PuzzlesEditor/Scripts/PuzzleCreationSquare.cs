using UnityEngine;
using UnityEngine.UI;

namespace Checkers
{
    public class PuzzleCreationSquare : MonoBehaviour
    {
        public CreationSquareData Data;
        [SerializeField] private Image _checkerImage;
        [SerializeField] private Image _checkerCrown;
        [Space]
        [SerializeField] private Sprite _checkerWhiteSprite;
        [SerializeField] private Sprite _checkerBlackSprite;

        [Space]
        [SerializeField] private Sprite _checkerWhiteCrownSprite;
        [SerializeField] private Sprite _checkerBlackCrownSprite;

        public void ClearInfo()
        {
            Data.Info = null;
        }

        public void UpdateView()
        {
            if (Data != null)
            {
                if (Data.Info == null)
                {
                    _checkerImage.enabled = false;
                    _checkerCrown.enabled = false;
                }
                else
                {
                    var info = Data.Info;

                    _checkerImage.enabled = true;
                    _checkerImage.sprite = info.Color == CheckerColor.White ? _checkerWhiteSprite : _checkerBlackSprite;
                    _checkerCrown.enabled = info.IsCrown;
                    _checkerCrown.sprite = info.Color == CheckerColor.White ? _checkerWhiteCrownSprite : _checkerBlackCrownSprite;

                }
            }
        }

        public void OnSquareClicked()
        {
            PuzzleEditor.Instance.OnSquareClicked(Data);
        }
    }

    public class CreationSquareData
    {
        public Position Pos;
        public CreationCheckerInfo Info;
       
    }
    public class CreationCheckerInfo
    {
        public CheckerColor Color;
        public bool IsCrown;
    }
}
