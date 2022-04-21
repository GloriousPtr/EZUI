using EZUI.Animation;
using UnityEngine;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	public class EZUIPopup : EZUIBase
	{
		[SerializeField] internal string key;
		[SerializeField] private AnimationData showData = new AnimationData(State.In, Mode.None);
		[SerializeField] private AnimationData hideData = new AnimationData(State.Out, Mode.None);
		
		internal void SetVisible(bool active, bool immediate = false)
		{
			if (active)
				ResetState();
			
			if (active == gameObject.activeSelf)
				return;
			
			if (immediate)
			{
				gameObject.SetActive(active);
				return;
			}
			
			if (!gameObject.activeSelf)
				gameObject.SetActive(true);

			StartTransition(active ? showData : hideData, immediate, () =>
			{
				if (!active)
					gameObject.SetActive(false);
			});
		}
		
		protected override bool UseCustomStartPosition(out Vector3 customPosition)
		{
			customPosition = Vector3.zero;
			return true;
		}

		protected override void SetInitialState()
		{
			showData.moveData.initialValue = rectTransform.localPosition;
			showData.rotationData.initialValue = rectTransform.localEulerAngles;
			showData.scaleData.initialValue = rectTransform.localScale;
			showData.fadeData.initialValue = canvasGroup.alpha;
		}

		private void ResetState()
		{
			rectTransform.localPosition = StaticData.ZeroVector;
			rectTransform.localRotation = Quaternion.identity;
			rectTransform.localScale = StaticData.UnitVector;
			canvasGroup.alpha = 1.0f;
		}
	}
}
