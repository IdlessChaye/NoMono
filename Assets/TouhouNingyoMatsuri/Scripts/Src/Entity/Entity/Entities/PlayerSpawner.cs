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
			playerSpawnData = ResourceManager.instance.Get<PlayerSpawn>(playerSpawnPath);

			player = new PlayerEntity();
			EntityManager.instance.AddEntity(player);

			EntityManager.instance.AddEntity(new AudioListenerEntity(player.GetGameObject().transform));

			camEntity = new ThirdPersonCameraEntity();
			EntityManager.instance.AddEntity(camEntity);
			
		}
		public override void Setup()
		{
			var vcCamera = camEntity.GetComponent<VCThirdPersonCamera>(ComponentType.VCThirdPersonCamera);
			vcCamera.targetTF = player.GetGameObject().transform;
		}

		
	}
}