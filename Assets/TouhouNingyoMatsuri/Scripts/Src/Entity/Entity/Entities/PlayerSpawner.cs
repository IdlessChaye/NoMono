using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class PlayerSpawner : BaseEntity
	{ 
		protected override string _prefabPath { get { return ""; } }

		private static PlayerSpawn playerSpawnData;
		private const string playerSpawnPath = @"Data/ConstData/PlayerSpawn";

		ThirdPersonCameraEntity camEntity;
		PlayerEntity player;

		protected override void Init()
		{
			playerSpawnData = Miscs.GetResourceManager().Get<PlayerSpawn>(playerSpawnPath);

			player = new PlayerEntity();
			Miscs.GetEntityManager().AddEntity(player);

			Miscs.GetEntityManager().AddEntity(new AudioListenerEntity(player.GetGameObject().transform));

			camEntity = new ThirdPersonCameraEntity();
			Miscs.GetEntityManager().AddEntity(camEntity);
			
		}
		public override void Setup()
		{
			var vcCamera = camEntity.GetComponent<VCThirdPersonCamera>(ComponentType.VCThirdPersonCamera);
			vcCamera.targetTF = player.GetGameObject().transform;
		}

		
	}
}