using UnityEngine;

namespace EZUI
{
	public abstract class EZUIProgressorTarget : MonoBehaviour
	{
		public abstract void SetValue(EZUIProgressor progressor);

		public abstract void SetPercent(float percent);
	}
}