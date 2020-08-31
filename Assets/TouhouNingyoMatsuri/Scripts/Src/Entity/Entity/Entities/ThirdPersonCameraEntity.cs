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

		public override void Setup()
		{
			AddComponent(new VCThirdPersonCamera());
		}
	}
}