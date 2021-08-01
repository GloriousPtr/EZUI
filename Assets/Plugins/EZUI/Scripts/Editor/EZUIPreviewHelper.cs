using DG.DOTweenEditor;
using DG.Tweening;
using EZUI.Animation;
using UnityEngine;
using EZUI;

namespace EZUI_Editor
{
	public class EZUIPreviewHelper
	{
		private static bool _previewing = false;
		
		public static void Preview(EZUIBase ezuiBase, AnimationData animationData)
		{
			if (_previewing)
				return;
			
			_previewing = true;
			
			ezuiBase.Init();
			Transform transform = ezuiBase.transform;
			
			ezuiBase.gameObject.SetActive(true);
			
			Vector3 localPosition = transform.localPosition;
			Vector3 localRotation = transform.localEulerAngles;
			Vector3 localScale = transform.localScale;
			float alpha = ezuiBase.GetComponent<CanvasGroup>().alpha;
			
			Tween tween = ezuiBase.StartTransition(animationData);
			
			Sequence previewSequence = DOTween.Sequence();
			previewSequence.Join(tween);
			previewSequence.AppendInterval(0.5f);
			previewSequence.AppendCallback(() =>
			{
				transform.localPosition = localPosition;
				transform.eulerAngles = localRotation;
				transform.localScale = localScale;
				ezuiBase.GetComponent<CanvasGroup>().alpha = alpha;
				
				_previewing = false;
			});
			DOTweenEditorPreview.PrepareTweenForPreview(previewSequence, false);
			DOTweenEditorPreview.Start();
		}

		public static void ScrubProgressor(EZUIProgressor progressor, float value)
		{
			Sequence previewSequence = DOTween.Sequence();
			previewSequence.Join(progressor.EaseValue(value));
			DOTweenEditorPreview.PrepareTweenForPreview(previewSequence);
			DOTweenEditorPreview.Start();
		}

		public static void SimulateProgressor(EZUIProgressor progressor, float value)
		{
			if (_previewing)
				return;
			
			_previewing = true;
			
			float originalValue = progressor.CurrentValue;
			progressor.SetForcePercent(0.0f);
			
			Sequence previewSequence = DOTween.Sequence();
			previewSequence.Join(progressor.EaseValue(value));
			previewSequence.AppendInterval(0.5f);
			previewSequence.OnComplete(() =>
			{
				progressor.SetForceValue(originalValue);
				_previewing = false;
			});
			
			DOTweenEditorPreview.PrepareTweenForPreview(previewSequence, false);
			DOTweenEditorPreview.Start();
		}
	}
}