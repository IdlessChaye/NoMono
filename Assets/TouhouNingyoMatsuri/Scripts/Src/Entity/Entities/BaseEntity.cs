using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NingyoRi
{
	public abstract class BaseEntity
	{
		public abstract EntityType entityType { get; }
		public bool needTick { get; private set; }
		public bool isActive { get; private set; }
		public uint entityId { get; private set; }

		protected BaseMonoPlugin _monoPlugin { get; private set; }
		protected string _prefabPath { get; set; }

		private LinkedList<BaseComponent> _compLinList = new LinkedList<BaseComponent>();
		private LinkedList<BaseComponent> _tickCompLinList = new LinkedList<BaseComponent>();
		private Dictionary<uint, BaseComponent> _type2compDict = new Dictionary<uint, BaseComponent>(8);

		private LinkedList<BaseComponent> _destroyCompLinList;

		public void Create(uint entityId)
		{
			SetActive(true);

			Init();

			this.entityId = entityId;

			Setup();
		}

		public void Desert()
		{
			Clear();
			Destroy();
		}

		protected abstract void Init(); // 这里负责设置 _monoPlugin 和 _prefabPath 相关的代码

		protected virtual void Setup()
		{
			// 这里负责new Component 然后对 comp Init
			// 之后才能AddComponent
		}

		public void Tick0()
		{
			if (isActive == false)
				return;
			var node = _tickCompLinList.First;
			BaseComponent value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick)
				{
					value.Tick0();
				}
			}
		}

		public void Tick1()
		{
			if (isActive == false)
				return;
			var node = _tickCompLinList.First;
			BaseComponent value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick)
				{
					value.Tick1();
				}
			}
		}

		public void Tick2()
		{
			if (isActive == false)
				return;
			var node = _tickCompLinList.First;
			BaseComponent value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick)
				{
					value.Tick2();
				}
			}
		}

		public void Tick3()
		{
			if (isActive == false)
				return;
			var node = _tickCompLinList.First;
			BaseComponent value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick)
				{
					value.Tick3();
				}
			}
		}

		public void Tick4()
		{
			if (isActive == false)
				return;
			var node = _tickCompLinList.First;
			BaseComponent value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick)
				{
					value.Tick4();
				}
			}
		}

		public virtual void Clear()
		{
			_destroyCompLinList = new LinkedList<BaseComponent>();
			var node = _compLinList.Last;
			BaseComponent value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Previous;
				if (value != null)
				{
					value = RemoveComponent(value.compType);
					if (value == null)
						Debug.LogError("!");
					_destroyCompLinList.AddLast(value);
				}
			}

			SetActive(false);
		}

		public virtual void Destroy()
		{
			var node = _destroyCompLinList.First;
			BaseComponent value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null)
				{
					value.Destroy();
				}
			}
			_destroyCompLinList.Clear();
			_destroyCompLinList = null;

			if (_monoPlugin != null)
			{
				GameObject.Destroy(_monoPlugin.pluginGO);
			}
		}

		public void SetActive(bool isActive)
		{
			this.isActive = isActive;
		}

		public T GetComponent<T>(ComponentType compType) where T: BaseComponent
		{
			uint typeId = (uint)compType;
			if (_type2compDict.ContainsKey(typeId))
			{
				return _type2compDict[typeId] as T;
			}
			return null;
		}

		public BaseComponent AddComponent(BaseComponent comp)
		{
			if (comp == null)
				return null;
			comp.OnAdd();
			var typeId = (uint)comp.compType;
			if (_type2compDict.ContainsKey(typeId))
			{
				Debug.LogError("还没想好要不要携带多个相同组件,目前不允许");
				return null;
			}
			_compLinList.AddLast(comp);
			if (comp.needTick)
				_tickCompLinList.AddLast(comp);
			_type2compDict.Add(typeId, comp);
			comp.OnAdded();
			return comp;
		}

		public BaseComponent RemoveComponent(ComponentType compType)
		{
			var typeId = (uint)compType;
			if (_type2compDict.ContainsKey(typeId) == false)
			{
				return null;
			}
			var comp = _type2compDict[typeId];
			comp.OnRemove();
			_type2compDict.Remove(typeId);
			if (_tickCompLinList.Contains(comp))
				_tickCompLinList.Remove(comp);
			_compLinList.Remove(comp);
			comp.OnRemoved();
			return comp;
		}

		public BaseMonoPlugin GetPlugin()
		{
			return _monoPlugin;
		}

	}
}