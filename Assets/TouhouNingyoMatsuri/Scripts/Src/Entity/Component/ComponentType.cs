using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public enum ComponentType
	{
		VCThirdPersonCamera,
		LCRawPlayerController
	}

	public partial class VCThirdPersonCamera
	{
		public override ComponentType compType { get { return ComponentType.VCThirdPersonCamera; } }
	}

	public partial class LCRawPlayerController
	{
		public override ComponentType compType { get { return ComponentType.LCRawPlayerController; } }
	}
}