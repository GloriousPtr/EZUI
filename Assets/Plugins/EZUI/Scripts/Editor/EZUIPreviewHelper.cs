using System.Reflection;
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
			
			System.Type type = ezuiBase.GetType();
			
			// Call ezuiBase.Init()
			MethodInfo initMethod;
			{
				initMethod = type.GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
				if (initMethod == null)
				{
					Debug.LogError("Couldn't find method Init in EZUIBase class");
					_previewing = false;
					return;
				}
			}
			initMethod.Invoke(ezuiBase, null);

			Transform transform = ezuiBase.transform;
			
			ezuiBase.gameObject.SetActive(true);
			
			Vector3 localPosition = transform.localPosition;
			Vector3 localRotation = transform.localEulerAngles;
			Vector3 localScale = transform.localScale;
			float alpha = ezuiBase.GetComponent<CanvasGroup>().alpha;
			
			// Call ezuiBase.StartTransition(animationData, false, null);
			MethodInfo transitionMethod;
			{
				transitionMethod = type.GetMethod("StartTransition",
					BindingFlags.NonPublic | BindingFlags.Instance);
				
				if (transitionMethod == null)
				{
					Debug.LogError("Couldn't find method StartTransition in EZUIBase class");
					_previewing = false;
					return;
				}
			}
			Tween tween = (Tween) transitionMethod.Invoke(ezuiBase, new object[]{ animationData, false, null });
			
			Sequence previewSequence = DOTween.Sequence();
			previewSequence.AppendInterval(0.5f);
			previewSequence.Append(tween);
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