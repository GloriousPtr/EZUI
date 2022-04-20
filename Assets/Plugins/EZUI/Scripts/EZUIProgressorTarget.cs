using UnityEngine;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	public abstract class EZUIProgressorTarget : MonoBehaviour
	{
		internal abstract void SetValue(EZUIProgressor progressor);

		public abstract void SetPercent(float percent);
	}
}