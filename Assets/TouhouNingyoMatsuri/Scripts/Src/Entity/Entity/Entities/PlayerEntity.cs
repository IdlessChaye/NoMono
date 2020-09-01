using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class PlayerEntity : CharacterEntity
	{
		protected override string _prefabPath { get { return @"Entity/Player/Player"; } }
		protected override void Init()
		{
			SetNeedTick(true);
		}

		public override void Setup()
		{
			AddComponent(new LCRawPlayerController());
		}
	}
}