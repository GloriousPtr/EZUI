using UnityEngine;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo(EZUI.StaticData.EZUI_EditorAssemblyName)]
namespace EZUI
{
	internal static class StaticData
	{
		internal const string EZUI_EditorAssemblyName = "EZUI.Editor";
		
		internal static readonly Vector3 ZeroVector = Vector3.zero;
		internal static readonly Vector3 UnitVector = Vector3.one;
		internal static readonly Vector3 UpVector = Vector3.up;
		internal static readonly Vector3 RightVector = Vector3.right;
		internal static readonly Vector3 ForwardVector = Vector3.forward;
	}
}