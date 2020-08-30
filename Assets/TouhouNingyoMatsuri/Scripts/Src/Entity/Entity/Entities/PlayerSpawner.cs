using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class PlayerSpawner : BaseEntity
	{
		public uint playerId { get; private set; }

		private PlayerSpawn playerSpawnData;
		private const string playerSpawnPath = @"Data/ConstData/PlayerSpawn";
		protected override void Init()
		{
			playerSpawnData = Miscs.GetResourceManager().Get<PlayerSpawn>(playerSpawnPath);
		}

		protected override void Setup()
		{
			var player = new PlayerEntity();
			Miscs.GetEntityManager().AddEntity(player);
			playerId = player.entityId;

			var camEntity = new ThirdPersonCameraEntity();
			Miscs.GetEntityManager().AddEntity(camEntity);
			var vcCamera = camEntity.GetComponent<VCThirdPersonCamera>(ComponentType.VCThirdPersonCamera);
			vcCamera.targetTF = player.GetGameObject().transform;
		}
	}
}