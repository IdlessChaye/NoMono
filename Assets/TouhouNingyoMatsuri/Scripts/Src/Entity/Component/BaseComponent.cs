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
		protected BaseMonoPlugin _plugin { get; private set; }
		protected GameObject _gameObject { get; private set; }

		public virtual void Init(BaseEntity entity) // 自己的数据设定
		{
			this._entity = entity;
			this._plugin = entity.GetPlugin();
			this._gameObject = entity.GetGameObject();
			SetEnable(true);
		}

		public virtual void Setup() { } // 啥都能干，可以处理同级的comp

		public virtual void Tick0(float deltaTime) { }
		public virtual void Tick1(float deltaTime) { }
		public virtual void Tick2(float deltaTime) { }
		public virtual void Tick3(float deltaTime) { }
		public virtual void Tick4(float deltaTime) { }

		public virtual void Clear()
		{
			SetEnable(false);
			_entity = null;
		}
		public virtual void Destroy() { }

		public void SetEnable(bool isEnable)
		{
			this.isEnable = isEnable;
		}

		public void SetNeedTick(bool needTick)
		{
			this.needTick = needTick;
		}

		public BaseEntity GetEntity()
		{
			return _entity;
		}

		public GameObject GetGameObject()
		{
			return _gameObject;
		}

		public BaseMonoPlugin GetPlugin()
		{
			return _plugin;
		}

	}
}