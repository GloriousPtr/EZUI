using UnityEngine;
using UnityEngine.Events;

namespace EZUI
{
	public class ProgressorGroup : MonoBehaviour
	{
		[SerializeField] private EZUIProgressor[] progressors;

		public UnityEvent<float> OnProgressChanged;

		private void OnEnable()
		{
			Refresh(0);
			
			foreach (EZUIProgressor progressor in progressors)
				progressor.OnPercentChanged.AddListener(Refresh);
		}
		
		private void OnDisable()
		{
			foreach (EZUIProgressor progressor in progressors)
				progressor.OnPercentChanged.AddListener(Refresh);
		}

		private void Refresh(float percent)
		{
			float sum = 0;
			
			foreach (EZUIProgressor progressor in progressors)
				sum += progressor.CurrentPercent;

			sum /= progressors.Length;
			OnProgressChanged?.Invoke(sum);
		}
	}
}