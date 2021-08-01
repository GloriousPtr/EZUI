using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace EZUI.Animation
{
	public enum MoveMode { Anchored, Local }
	
	[System.Serializable]
	public struct MoveData
	{
		public State state;
		public Mode mode;
		public MoveMode moveMode;
		
		public float startDelay;
		public float duration;
		public MoveDirection moveDirection;
		public Vector3 from;
		public Vector3 to;
		public Vector3 by;
		public float elasticity;
		public int vibrato;
		public Ease ease;
		
		public MoveData(State state, Mode mode)
		{
			this.state = state;
			this.mode = mode;

			moveMode = MoveMode.Anchored;
			startDelay = 0.0f;
			duration = 1.0f;
			moveDirection = MoveDirection.Left;
			from = StaticData.ZeroVector;
			to = StaticData.ZeroVector;
			by = StaticData.RightVector * 100;
			elasticity = 1.0f;
			vibrato = 12;
			ease = Ease.Linear;
		}
	}

	[System.Serializable]
	public struct RotationData
	{
		public State state;
		public Mode mode;
		
		public float startDelay;
		public float duration;
		public Vector3 from;
		public Vector3 to;
		public Vector3 by;
		public float elasticity;
		public int vibrato;
		public Ease ease;
		public RotateMode rotateMode;

		public RotationData(State state, Mode mode)
		{
			this.state = state;
			this.mode = mode;

			startDelay = 0.0f;
			duration = 1.0f;
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
	public struct ScaleData
	{
		public State state;
		public Mode mode;

		public float startDelay;
		public float duration;
		public Vector3 from;
		public Vector3 to;
		public Vector3 by;
		public float elasticity;
		public int vibrato;
		public Ease ease;

		public ScaleData(State state, Mode mode)
		{
			this.state = state;
			this.mode = mode;

			startDelay = 0.0f;
			duration = 1.0f;
			from = state == State.None ? StaticData.UnitVector : StaticData.ZeroVector;
			to = StaticData.ZeroVector;
			by = StaticData.UnitVector * 0.1f;
			elasticity = 1.0f;
			vibrato = 12;
			ease = Ease.Linear;
		}
	}
	
	[System.Serializable]
	public struct FloatData
	{
		public State state;
		
		public float startDelay;
		public float duration;
		public float from;
		public float to;
		public Ease ease;
		
		public FloatData(State state)
		{
			this.state = state;

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
		public State defaultState;
		
		public bool move;
		public bool rotation;
		public bool scale;
		public bool fade;
		public MoveData moveData;
		public RotationData rotationData;
		public ScaleData scaleData;
		public FloatData fadeData;
		
		public UnityEvent onStarted;
		public UnityEvent onCompleted;

		public bool isAnimating;

#if UNITY_EDITOR
		public bool expanded;
		public bool isAnyAnimationActive;
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
