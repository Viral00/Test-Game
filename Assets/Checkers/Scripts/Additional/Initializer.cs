using DG.Tweening;
using UnityEngine;

namespace Checkers
{
	public class Initializer : MonoBehaviour
	{
		private void Awake()
		{
			Application.targetFrameRate = 300;
			QualitySettings.antiAliasing = 0;
			UiViewController.Instance.Iniialize();

			GameRulesController.Instance.SetRuleFromProgress();

			InitializeDOTween();
			BoardController.Instance.InitColor();
			PuzzleController.Instance.GetPuzzlles();
			PuzzleController.Instance.GetProgress();
			PuzzlesMenuCreator.Instance.Create();
		}

		/// <summary>
		/// Init dotween for freeze excluding.
		/// </summary>
		private void InitializeDOTween()
		{
			transform.DOKill();
			transform.DOLocalMove(Vector3.zero, 0.001f);
			transform.DORotate(Vector3.zero, 0.001f);
			transform.DOShakePosition(0.001f, Vector3.zero, vibrato: 10);

			float variable = 0f;
			DOTween.To(() => variable, x => variable = x, 0f, 0.001f);
		}
	}
}