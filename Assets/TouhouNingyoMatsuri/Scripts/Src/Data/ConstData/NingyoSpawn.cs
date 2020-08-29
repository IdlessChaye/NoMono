
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	[CreateAssetMenu(fileName = "NingyoSpawn", menuName = @"Data/ConstData/NingyoSpawn")]
	public class NingyoSpawn : ScriptableObject
	{
		public Vector3[] spawnPositions;
		public Vector3[] spawnRotations;
	}
}