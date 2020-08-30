using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class ThirdPersonCameraEntity : BaseEntity
	{

		protected override void Init()
		{
			_prefabPath = @"Entity/ThirdCamera";
			SetNeedTick(true);
		}

		protected override void Setup()
		{
			var vcCamera = new VCThirdPersonCamera();
			vcCamera.Init(this);
			AddComponent(vcCamera);
		}
	}
}