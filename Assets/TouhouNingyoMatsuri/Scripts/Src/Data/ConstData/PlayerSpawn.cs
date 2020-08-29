using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	[CreateAssetMenu(fileName = "PlayerSpawn", menuName = @"Data/ConstData/PlayerSpawn")]
	public class PlayerSpawn : ScriptableObject
	{
		public Vector3[] playerPositions;
		public Vector3[] playerRotations;
	}
}