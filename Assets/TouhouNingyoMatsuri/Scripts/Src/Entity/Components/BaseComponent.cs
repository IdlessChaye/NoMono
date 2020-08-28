using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public abstract class BaseComponent
	{
		public abstract ComponentType compType { get; }
		public bool needTick { get; private set; }
		public bool isEnable { get; private set; }

		protected BaseEntity _entity { get; private set; }

		public virtual void Init(BaseEntity entity)
		{
			this._entity = entity;
			SetEnable(true);
		}

		public virtual void OnAdd() { }
		public virtual void OnAdded() { }

		public virtual void Tick0() { }
		public virtual void Tick1() { }
		public virtual void Tick2() { }
		public virtual void Tick3() { }
		public virtual void Tick4() { }

		public virtual void OnRemove() { }
		public virtual void OnRemoved() { }
		public virtual void Destroy()
		{
			SetEnable(false);
			_entity = null;
		}

		public void SetEnable(bool isEnable)
		{
			this.isEnable = isEnable;
		}

		public BaseEntity GetEntity()
		{
			return _entity;
		}

	}
}