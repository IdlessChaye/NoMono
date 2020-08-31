using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public partial class EntityManager : BaseLocalManager
	{
		private Transform _entityRoot;

		private static uint entityId = 0;
		private LinkedList<BaseEntity> _entityLinList = new LinkedList<BaseEntity>();
		private LinkedList<BaseEntity> _tickEntityLinList = new LinkedList<BaseEntity>();
		private Dictionary<uint, BaseEntity> _id2entityDict = new Dictionary<uint, BaseEntity>(8);

		private void AddEntities(Scene scene)
		{
			var sceneName = scene.name;
			if (sceneName.Equals("Menu"))
			{
				AddEntity(new AudioListenerEntity(null));
			}
			else if (sceneName.Equals("MainLevel"))
			{
				var playerSpawner = new PlayerSpawner();
				AddEntity(playerSpawner);
				AddEntity(new AudioListenerEntity(GetEntity(playerSpawner.playerId).GetGameObject().transform));
			}
		}


		public override void Init()
		{
			SetNeedTick(true);

			var prefab = Miscs.GetResourceManager().Get<GameObject>(GlobalVars.entityPath);
			if (prefab == null)
				throw new System.Exception("EntityManager Init");

			var _uiCanvasGO = GameObject.Find("EntityRoot");
			_entityRoot = _uiCanvasGO != null ? _uiCanvasGO.transform : null;
			if (_entityRoot == null)
				_entityRoot = GameObject.Instantiate(prefab).transform;
		}

		public void AddEntity(BaseEntity entity)
		{
			if (entity == null)
				return;
			entity.Create(entityId++, _entityRoot);
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

		public override void Tick0()
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick0();
				}
			}
		}

		public override void Tick1()
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick1();
				}
			}
		}

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

		public override void Tick3()
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick3();
				}
			}
		}

		public override void Tick4()
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				node = node.Next;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick4();
				}
			}
		}

		#endregion

		public override void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			AddEntities(scene);
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
			entityId = 0;
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

