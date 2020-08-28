using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public enum KeyType
	{
		Down,
		Hold,
		Up
	}

	public partial class InputManager : BaseGlobalManager
	{

		public override void Init()
		{
			
		}

		public override void Tick2()
		{
			if (Input.GetKey(KeyCode.A))
			{
				//Messenger.Broadcast<KeyType>(GlobalVars.EA, KeyType.Hold);
				//Debug.Log("A");
			}
		}
	}
}