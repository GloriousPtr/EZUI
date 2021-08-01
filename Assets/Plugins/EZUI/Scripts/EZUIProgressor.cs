using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EZUI
{
	public class EZUIProgressor : MonoBehaviour
	{
#if UNITY_EDITOR
		[MenuItem("GameObject/EZUI/Linear Progress Bar", false, 1)]
		public static void AddLinearProgressBar(MenuCommand menuCommand)
		{
			GameObject obj = Instantiate(Resources.Load<GameObject>("EZUI/Prefabs/LinearProgressBar"),
				Selection.activeTransform.transform, false);
			Selection.activeGameObject = obj;
		}
		
		[MenuItem("GameObject/EZUI/Radial Progress Bar", false, 1)]
		public static void AddRadialProgressBar(MenuCommand menuCommand)
		{
			GameObject obj = Instantiate(Resources.Load<GameObject>("EZUI/Prefabs/RadialProgressBar"),
				Selection.activeTransform.transform, false);
			Selection.activeGameObject = obj;
		}
#endif

		[SerializeField] private EZUIProgressorTarget[] progressorTargets;
		
		[SerializeField] private float minValue = 0.0f;
		[SerializeField] private float maxValue = 1.0f;
		[SerializeField] private float currentValue = 0.5f;
		[SerializeField] private float currentPercent = 0.5f;

		[SerializeField] private float duration = 0.5f;
		[SerializeField] private Ease ease = Ease.OutBack;
		[SerializeField] private bool useWholeNumbers = true;
		
		public UnityEvent<float> OnPercentChanged;
		public UnityEvent<float> OnValueChanged;

		private Tweener _tween;
		private Sequence _sequence;
		private bool _animating = true;
		private int _instanceId;
		
		public int CurrentProgress => (int) Math.Round(100 * currentPercent);
		public float CurrentValue => Mathf.Clamp((float) Math.Round(currentValue, useWholeNumbers ? 0 : 2), minValue, maxValue);

		public float CurrentPercent => currentPercent;

		private void Awake()
		{
			_instanceId = GetInstanceID();
		}

		private void OnEnable()
		{
			KillSequence();
		}

		private void OnDisable()
		{
			KillSequence();
		}

		public void SetForcePercent(float percent)
		{
			percent = Mathf.Clamp01(percent);
			OnPercentChanged?.Invoke(percent);
			UpdateTargets(Mathf.Lerp(minValue, maxValue, percent));
		}
		
		public void SetForceValue(float value)
		{
			value = Mathf.Clamp(value, minValue, maxValue);
			OnValueChanged?.Invoke(value);
			UpdateTargets(value);
		}

		public void SetPercent(float percent)
		{
			percent = Mathf.Clamp01(percent);
			EaseValue(Mathf.Lerp(minValue, maxValue, percent));
		}
		
		public void SetValue(float value)
		{
			value = Mathf.Clamp(value, minValue, maxValue);
			EaseValue(value);
		}
		
		public Sequence EaseValue(float value)
		{
			if (_animating && _sequence != null)
			{
				_animating = false;
				KillSequence();
			}
			
			if (_sequence == null)
			{
				_animating = true;
				_tween = DOTween.To(() => currentValue, UpdateTargets, value, duration)
					.SetId(_instanceId)
					.SetEase(ease)
					.SetAutoKill(false);
			}
			else
			{
				_tween.ChangeEndValue(value, true);
			}

			if (_sequence == null)
				_sequence = DOTween.Sequence();

			_sequence.Append(_tween).Play();
			return _sequence;
		}

		private void UpdateTargets(float value)
		{
			currentValue = value;
			currentPercent = Mathf.InverseLerp(minValue, maxValue, value);
			
			OnValueChanged?.Invoke(currentValue);
			OnPercentChanged?.Invoke(currentPercent);
			
			foreach (EZUIProgressorTarget target in progressorTargets)
			{
				target.SetValue(this);
#if UNITY_EDITOR
				EditorUtility.SetDirty(target);
#endif
			}
		}

		private void KillSequence()
		{
			DOTween.Kill(_instanceId);
			
			if (_sequence == null)
				return;
			
			_sequence.Kill();
			_sequence = null;
		}
	}
}
