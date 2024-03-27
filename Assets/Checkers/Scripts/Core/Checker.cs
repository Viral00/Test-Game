using UnityEngine;

namespace Checkers
{
	[System.Serializable]
    public class Checker
    {
		/// <summary>
		/// Checker id.
		/// </summary>
		public int Id;

		/// <summary>
		/// Reference to square.
		/// </summary>
		public Square Square;

		/// <summary>
		/// Color of checker.
		/// </summary>
		public CheckerColor Color;

		/// <summary>
		/// True when checker is super.
		/// </summary>
		public bool IsCrown;

		/// <summary>
		/// True when checker was beat.
		/// </summary>
		public bool IsBeat;

		public Checker()
		{

		}

		/// <summary>
		/// Create new checker instance.
		/// </summary>
		public Checker(int id, Square square, CheckerColor color)
        {
            Id = id;
            Square = square;
            Color = color;
            IsCrown = false;
            IsBeat = false;
        }

		public Checker(int id, Square square, CheckerColor color, bool isCrown)
		{
			Id = id;
			Square = square;
			Color = color;
			IsCrown = isCrown;
			IsBeat = false;
		}
		public Checker(Checker ch)
		{
			Id = ch.Id;
			Square = ch.Square;
			Color = ch.Color;
			IsCrown = ch.IsCrown;
			IsBeat = ch.IsBeat;
		}


		/// <summary>
		/// Change square ref of checker
		/// </summary>
		/// <param name="square"></param>
		public void MoveTo(Square square)
        {
            Square = square;
        }

		/// <summary>
		/// Set super checker state.
		/// </summary>
        public void BecomeCrownChecker()
        {
			if (!IsCrown)
			{
               BoardController.Instance._checkersViews[Id].ParticlePrefab.GetComponent<ParticleSystem>().Play();
            }
            IsCrown = true;
        }

		/// <summary>
		/// Set beat state.
		/// </summary>
        public void WasBeat()
        {
            IsBeat = true;
        }
    }
}
