using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI.Animation
{
	internal enum MoveMode { Anchored, Local }
	
	[System.Serializable]
	internal struct MoveData
	{
		[SerializeField] internal State state;
		[SerializeField] internal Mode mode;
		[SerializeField] internal MoveMode moveMode;
		
		[SerializeField] internal float startDelay;
		[SerializeField] internal float duration;
		[SerializeField] internal MoveDirection moveDirection;
		[SerializeField] internal Vector3 initialValue;
		[SerializeField] internal Vector3 from;
		[SerializeField] internal Vector3 to;
		[SerializeField] internal Vector3 by;
		[SerializeField] internal float elasticity;
		[SerializeField] internal int vibrato;
		[SerializeField] internal Ease ease;
		
		internal MoveData(State state, Mode mode)
		{
			this.state = state;
			this.mode = mode;

			moveMode = MoveMode.Anchored;
			startDelay = 0.0f;
			duration = 1.0f;
			moveDirection = MoveDirection.Left;
			initialValue = StaticData.ZeroVector;
			from = StaticData.ZeroVector;
			to = StaticData.ZeroVector;
			by = StaticData.RightVector * 100;
			elasticity = 1.0f;
			vibrato = 12;
			ease = Ease.Linear;
		}
	}

	[System.Serializable]
	internal struct RotationData
	{
		[SerializeField] internal State state;
		[SerializeField] internal Mode mode;
		
		[SerializeField] internal float startDelay;
		[SerializeField] internal float duration;
		[SerializeField] internal Vector3 initialValue;
		[SerializeField] internal Vector3 from;
		[SerializeField] internal Vector3 to;
		[SerializeField] internal Vector3 by;
		[SerializeField] internal float elasticity;
		[SerializeField] internal int vibrato;
		[SerializeField] internal Ease ease;
		[SerializeField] internal RotateMode rotateMode;

		internal RotationData(State state, Mode mode)
		{
			this.state = state;
			this.mode = mode;

			startDelay = 0.0f;
			duration = 1.0f;
			initialValue = StaticData.ZeroVector;
			from = StaticData.ZeroVector;
			to = StaticData.ZeroVector;
			by = StaticData.ForwardVector * 45.0f;
			elasticity = 1.0f;
			vibrato = 12;
			ease = Ease.Linear;
			rotateMode = RotateMode.FastBeyond360;
		}
	}
	
	[System.Serializable]
	internal struct ScaleData
	{
		[SerializeField] internal State state;
		[SerializeField] internal Mode mode;

		[SerializeField] internal float startDelay;
		[SerializeField] internal float duration;
		[SerializeField] internal Vector3 initialValue;
		[SerializeField] internal Vector3 from;
		[SerializeField] internal Vector3 to;
		[SerializeField] internal Vector3 by;
		[SerializeField] internal float elasticity;
		[SerializeField] internal int vibrato;
		[SerializeField] internal Ease ease;

		internal ScaleData(State state, Mode mode)
		{
			this.state = state;
			this.mode = mode;

			startDelay = 0.0f;
			duration = 1.0f;
			initialValue = StaticData.UnitVector;
			from = state == State.None ? StaticData.UnitVector : StaticData.ZeroVector;
			to = StaticData.ZeroVector;
			by = StaticData.UnitVector * 0.1f;
			elasticity = 1.0f;
			vibrato = 12;
			ease = Ease.Linear;
		}
	}
	
	[System.Serializable]
	internal struct FloatData
	{
		[SerializeField] internal State state;
		
		[SerializeField] internal float startDelay;
		[SerializeField] internal float duration;
		[SerializeField] internal float initialValue;
		[SerializeField] internal float from;
		[SerializeField] internal float to;
		[SerializeField] internal Ease ease;
		
		internal FloatData(State state)
		{
			this.state = state;

			initialValue = 1.0f;
			startDelay = 0.0f;
			duration = 1.0f;
			from = 0.0f;
			to = 0.0f;
			ease = Ease.Linear;
		}
	}
	
	public enum State { None, In, Out }
	public enum Mode { None, Punch, By }
	
	[System.Serializable]
	public class AnimationData
	{
		[SerializeField] internal State defaultState;
		
		[SerializeField] internal bool move;
		[SerializeField] internal bool rotation;
		[SerializeField] internal bool scale;
		[SerializeField] internal bool fade;
		[SerializeField] internal MoveData moveData;
		[SerializeField] internal RotationData rotationData;
		[SerializeField] internal ScaleData scaleData;
		[SerializeField] internal FloatData fadeData;
		
		[SerializeField] internal UnityEvent onStarted;
		[SerializeField] internal UnityEvent onCompleted;

		[SerializeField] internal bool isAnimating;

#if UNITY_EDITOR
		[SerializeField] internal bool expanded;
		[SerializeField] internal bool isAnyAnimationActive;
#endif
		
		public AnimationData(State state, Mode mode)
		{
			defaultState = state;

			move = false;
			rotation = false;
			scale = false;
			fade = false;
			
			moveData = new MoveData(state, mode);
			rotationData = new RotationData(state, mode);
			scaleData = new ScaleData(state, mode);
			fadeData = new FloatData(state);

			onStarted = new UnityEvent();
			onCompleted = new UnityEvent();

			isAnimating = false;
			
#if UNITY_EDITOR
			expanded = false;
			isAnyAnimationActive = false;
#endif
		}
	}
}
