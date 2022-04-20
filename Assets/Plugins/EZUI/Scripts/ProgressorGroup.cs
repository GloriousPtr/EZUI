using UnityEngine;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
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

			for (int i = 0; i < progressors.Length; i++)
				sum += progressors[i].GetCurrentPercent();

			sum /= progressors.Length;
			OnProgressChanged?.Invoke(sum);
		}
	}
}
