using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class BaseContext
	{
		public string PrefabPath { get; set; }

		public virtual void Init() { }

		public virtual void Setup() { }

		public virtual void BindCallback() { }

		public virtual void BindEvent() { }

		public virtual void Tick() { }

		public virtual void Clear() { }

		public virtual void Destroy() { }
	}
}