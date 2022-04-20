using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
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
		
		public float minValue = 0.0f;
		public float maxValue = 1.0f;
		[SerializeField] private float currentValue = 0.5f;
		[SerializeField] private float currentPercent = 0.5f;
		public float duration = 0.5f;
		public Ease ease = Ease.OutBack;
		public bool useWholeNumbers = true;
		
		[SerializeField] private EZUIProgressorTarget[] progressorTargets;
		
		public UnityEvent<float> OnPercentChanged;
		public UnityEvent<float> OnValueChanged;

		private Tweener _tween;
		private Sequence _sequence;
		private bool _animating = true;
		private int _instanceId;
		
		/// <summary>
		/// Percent: [0.0f, 1.0f]
		/// </summary>
		public float GetCurrentPercent() => currentPercent;
		
		/// <summary>
		/// Value: [minValue, maxValue]
		/// </summary>
		public float GetCurrentValue() => Mathf.Clamp((float) Math.Round(currentValue, useWholeNumbers ? 0 : 2), minValue, maxValue);
		
		/// <summary>
		/// Percent: [0, 100]
		/// </summary>
		public int GetCurrentProgress() => (int) Math.Round(100 * currentPercent);
		
		/// <summary>
		/// Updates the Progressor Targets accordingly
		/// </summary>
		/// <param name="percent">Percent [0.0f, 1.0f]</param>
		public void SetPercent(float percent)
		{
			percent = Mathf.Clamp01(percent);
			EaseValue(Mathf.Lerp(minValue, maxValue, percent));
		}
		
		/// <summary>
		/// Updates the Progressor Targets accordingly without animating
		/// </summary>
		/// <param name="percent">Percent [0.0f, 1.0f]</param>
		public void SetForcePercent(float percent)
		{
			percent = Mathf.Clamp01(percent);
			OnPercentChanged?.Invoke(percent);
			UpdateTargets(Mathf.Lerp(minValue, maxValue, percent));
		}
		
		/// <summary>
		/// Updates the Progressor Targets accordingly
		/// </summary>
		/// <param name="value">Value [minValue, maxValue]</param>
		public void SetValue(float value)
		{
			value = Mathf.Clamp(value, minValue, maxValue);
			EaseValue(value);
		}
		
		/// <summary>
		/// Updates the Progressor Targets accordingly without animating
		/// </summary>
		/// <param name="value">Value [minValue, maxValue]</param>
		public void SetForceValue(float value)
		{
			value = Mathf.Clamp(value, minValue, maxValue);
			OnValueChanged?.Invoke(value);
			UpdateTargets(value);
		}
		
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

		/// <summary>
		/// Animates the value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		internal Sequence EaseValue(float value)
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

		/// <summary>
		/// Updates all the targets
		/// </summary>
		/// <param name="value"></param>
		private void UpdateTargets(float value)
		{
			currentValue = value;
			currentPercent = Mathf.InverseLerp(minValue, maxValue, value);
			
			OnValueChanged?.Invoke(currentValue);
			OnPercentChanged?.Invoke(currentPercent);
			
			foreach (EZUIProgressorTarget target in progressorTargets)
			{
				target.ShouldChange(this);
#if UNITY_EDITOR
				EditorUtility.SetDirty(target);
#endif
			}
		}

		/// <summary>
		/// Kills the DOTween sequence
		/// </summary>
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
