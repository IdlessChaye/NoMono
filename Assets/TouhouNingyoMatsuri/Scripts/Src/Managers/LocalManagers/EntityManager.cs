using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public partial class EntityManager : BaseLocalManager
	{
		private static uint entityId = 0;
		private LinkedList<BaseEntity> _entityLinList = new LinkedList<BaseEntity>();
		private LinkedList<BaseEntity> _tickEntityLinList = new LinkedList<BaseEntity>();
		private Dictionary<uint, BaseEntity> _id2entityDict = new Dictionary<uint, BaseEntity>(8);

		public override void Init()
		{
			entityId = 0;
		}

		public void AddEntity(BaseEntity entity)
		{
			if (entity == null)
				return;
			entity.Create(entityId++);
			_entityLinList.AddLast(entity);
			if (entity.needTick)
				_tickEntityLinList.AddLast(entity);
			_id2entityDict.Add(entity.entityId, entity);
		}

		public void RemoveEntity(BaseEntity entity)
		{
			if (entity == null)
				return;
			entity.Desert();
			_id2entityDict.Remove(entity.entityId);
			if (_tickEntityLinList.Contains(entity))
				_tickEntityLinList.Remove(entity);
			_entityLinList.Remove(entity);
		}

		#region Tick Entities

		public override void Tick2()
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while(node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick2();
				}
			}
		}

		#endregion

		public override void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			if (scene.name.Equals(""))
			{
				
			}
		}

		public override void OnLevelUnLoaded(Scene scene)
		{
			var node = _entityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null)
				{
					value.Clear();
				}
			}
		}

		public override void Destroy()
		{
			_tickEntityLinList.Clear();
			var node = _entityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null)
				{
					value.Destroy();
				}
			}
			_entityLinList.Clear();
			_id2entityDict.Clear();
		}

		public BaseEntity GetEntity(uint entityId)
		{
			BaseEntity entity;
			if (_id2entityDict.TryGetValue(entityId,out entity) == false)
				return null;
			return entity;
		}

	}
}

