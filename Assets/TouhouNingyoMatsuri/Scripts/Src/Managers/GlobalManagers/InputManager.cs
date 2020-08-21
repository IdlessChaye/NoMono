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

	public class InputManager : BaseGlobalManager
	{
		public override ManagerType managerType
		{
			get
			{
				return ManagerType.Input;
			}
		}
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