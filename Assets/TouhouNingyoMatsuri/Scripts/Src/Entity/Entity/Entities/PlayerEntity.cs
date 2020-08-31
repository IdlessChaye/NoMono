using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class PlayerEntity : CharacterEntity
	{
		protected override void Init()
		{
			_prefabPath = @"Entity/Player/Player";
			SetNeedTick(true);
		}

		public override void Setup()
		{
			AddComponent(new LCRawPlayerController());
		}
	}
}