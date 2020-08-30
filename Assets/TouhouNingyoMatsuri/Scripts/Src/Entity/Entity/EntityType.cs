using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public enum EntityType
	{
		PlayerSpawner,
		Player,
		ThirdPersonCamera,
		AudioListener,
	}

	public partial class PlayerSpawner
	{
		public override EntityType entityType { get { return EntityType.PlayerSpawner; } }
	}

	public partial class PlayerEntity
	{
		public override EntityType entityType { get { return EntityType.Player; } }
	}

	public partial class ThirdPersonCameraEntity
	{
		public override EntityType entityType { get { return EntityType.ThirdPersonCamera; } }
	}

	public partial class AudioListenerEntity
	{
		public override EntityType entityType { get { return EntityType.AudioListener; } }
	}
}