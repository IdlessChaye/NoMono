using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class ThirdPersonCameraEntity : BaseEntity
	{
		protected override string _prefabPath { get { return @"Entity/ThirdCamera"; } }
		protected override void Init()
		{
			SetNeedTick(true);
			AddComponent(new VCThirdPersonCamera());
		}

		public override void Setup()
		{
		}
	}
}