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

		private Dictionary<uint, BaseEntity> _id2entityDict = new Dictionary<uint, BaseEntity>(8);
		private LinkedList<BaseEntity> _entityLinList = new LinkedList<BaseEntity>();
		private LinkedList<BaseEntity> _tickEntityLinList = new LinkedList<BaseEntity>();
		private LinkedList<BaseEntity> _toBeAddedEntities = new LinkedList<BaseEntity>();

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
			}
		}


		public override void Init()
		{
			var prefab = Miscs.GetResourceManager().Get<GameObject>(GlobalVars.entityPath);
			if (prefab == null)
				throw new System.Exception("EntityManager Init");

			var _uiCanvasGO = GameObject.Find("EntityRoot");
			_entityRoot = _uiCanvasGO != null ? _uiCanvasGO.transform : null;
			if (_entityRoot == null)
				_entityRoot = GameObject.Instantiate(prefab).transform;

			SetNeedTick(true);
		}

		public void AddEntity(BaseEntity entity)
		{
			if (entity == null)
				return;

			entity.Create(entityId++, _entityRoot);

			_toBeAddedEntities.AddLast(entity);
		}

		public void RemoveEntity(BaseEntity entity)
		{
			if (entity == null)
				return;

			_id2entityDict.Remove(entity.entityId);
			_tickEntityLinList.Remove(entity);
			_entityLinList.Remove(entity);

			entity.Clear();
			entity.Destroy();
		}

		#region Tick Entities

		public override void Tick0(float deltaTime)
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick0(deltaTime);
				}
				node = node.Next;
			}
		}

		public override void Tick1(float deltaTime)
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick1(deltaTime);
				}
				node = node.Next;
			}
		}

		public override void Tick2(float deltaTime)
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while(node != null)
			{
				value = node.Value;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick2(deltaTime);
				}
				node = node.Next;
			}
		}

		public override void Tick3(float deltaTime)
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick3(deltaTime);
				}
				node = node.Next;
			}
		}

		public override void Tick4(float deltaTime)
		{
			var node = _tickEntityLinList.First;
			BaseEntity value = null;
			while (node != null)
			{
				value = node.Value;
				if (value != null && value.needTick && value.isActive)
				{
					value.Tick4(deltaTime);
				}
				node = node.Next;
			}
		}

		public override void TickAddTo(float deltaTime)
		{
			LinkedListNode<BaseEntity> node = null;
			BaseEntity entity = null;
			if (_toBeAddedEntities.Count != 0)
			{
				node = _toBeAddedEntities.First;
				entity = null;
				while (node != null)
				{
					entity = node.Value;
					if (entity != null)
					{
						_entityLinList.AddLast(entity);
						_id2entityDict.Add(entity.entityId, entity);
						if (entity.needTick && entity.isActive)
						{
							_tickEntityLinList.AddLast(entity);
						}
						entity.Setup();
					}
					node = node.Next;
				}

				_toBeAddedEntities.Clear();
			}

			node = _entityLinList.First;
			entity = null;
			while (node != null)
			{
				entity = node.Value;
				if (entity != null && entity.isActive)
				{
					entity.TickAddTo(deltaTime);
				}
				node = node.Next;
			}
		}

		#endregion

		public override void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			AddEntities(scene);
		}

		public override void OnLevelUnLoaded(Scene scene)
		{
			var node = _entityLinList.Last;
			BaseEntity entity = null;
			while (node != null)
			{
				entity = node.Value;
				if (entity != null)
				{
					entity.Clear();
				}
				node = node.Previous;
			}

			node = _entityLinList.Last;
			entity = null;
			while (node != null)
			{
				entity = node.Value;
				if (entity != null)
				{
					entity.Destroy();
				}
				node = node.Previous;
			}

			_tickEntityLinList.Clear();
			_entityLinList.Clear();
			_id2entityDict.Clear();

			SetNeedTick(false);
		}

		public override void Destroy()
		{
			_entityRoot = null;
			entityId = 0;
		}

		public BaseEntity GetEntity(uint entityId)
		{
			BaseEntity entity;
			if (_id2entityDict.TryGetValue(entityId,out entity) == false)
				return null;
			return entity;
		}

		public T GetEntity<T>(uint entityId) where T : BaseEntity
		{
			BaseEntity entity;
			if (_id2entityDict.TryGetValue(entityId, out entity) == false)
				return null;
			return entity as T;
		}

	}
}

