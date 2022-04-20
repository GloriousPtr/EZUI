using UnityEngine;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	public abstract class EZUIProgressorTarget : MonoBehaviour
	{
		/// <summary>
		/// Called when EZUIProgressor notifies this object with change
		/// </summary>
		/// <param name="progressor">source progressor</param>
		internal abstract void ShouldChange(EZUIProgressor progressor);
	}
}
