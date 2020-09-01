using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace NingyoRi
{
	public class GameManager : FullSingleton<GameManager>
	{
		public static bool isDirtyAdd { get; set; }

		private Dictionary<uint, BaseManager> _managerDict = new Dictionary<uint, BaseManager>(16);

		private LinkedList<BaseGlobalManager> _globalManagerLinList = new LinkedList<BaseGlobalManager>();
		private LinkedList<BaseLocalManager> _localManagerLinList = new LinkedList<BaseLocalManager>();

		private LinkedList<BaseGlobalManager> _tickGlobalManagerLinList = new LinkedList<BaseGlobalManager>();
		private LinkedList<BaseLocalManager> _tickLocalManagerLinList = new LinkedList<BaseLocalManager>();

		private List<BaseGlobalManager> _toBeAddedGlobalList = new List<BaseGlobalManager>(); // 初始化是在Add的那个帧时候完成，但Tick是在下个帧
		private List<BaseLocalManager> _toBeAddedLocalList = new List<BaseLocalManager>();  // _toBeAdded的目的是动态添加，双层分离初始化(Init\Setup)

		private void AddFullManagers()
		{
			FullCoroutineManager.Instance.Init();
			FullMusicManager.Instance.Init();
		}

		private void RemoveFullManagers()
		{
			FullMusicManager.Instance.Destroy();
			FullCoroutineManager.Instance.Destroy();
		}

		private void AddGlobalManagers()
		{
			AddGlobalManager(new ResourceManager());
			AddGlobalManager(new TextMapManager());
			AddGlobalManager(new InputManager());
		}

		private void AddLocalManagers(Scene scene)
		{
			var sceneName = scene.name;
			if (sceneName.Equals("Menu"))
			{
				AddLocalManager(new EntityManager());
				AddLocalManager(new UIManager());
			}
			else if (sceneName.Equals("MainLevel"))
			{
				AddLocalManager(new EntityManager());
				AddLocalManager(new UIManager());
			}
		}



		public override void Awake()
		{
			base.Awake();

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;

			GameManager.Instance.Init();
		}

		public override void Init()
		{
			AddGlobalManagers(); // 下一级的(GlobalManager\LocalManager)放在Init中增删

			AddFullManagers();
		}

		#region On Scene Change

		private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			OnGlobalManagerLoaded(scene, loadSceneMode);
			OnLocalManagerLoaded(scene, loadSceneMode);
		}

		private void OnGlobalManagerLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			var node = _globalManagerLinList.First;
			BaseGlobalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelLoaded(scene, loadSceneMode);
				}
				node = node.Next;
			}
		}

		private void OnLocalManagerLoaded(Scene scene, LoadSceneMode loadSceneMode)
		{
			AddLocalManagers(scene);

			if (_toBeAddedLocalList.Count != 0)
			{
				BaseLocalManager managerLocal = null;
				var eLocal = _toBeAddedLocalList.GetEnumerator();
				while (eLocal.MoveNext())
				{
					managerLocal = eLocal.Current;
					if (managerLocal != null)
					{
						managerLocal.TickAddTo();

						_localManagerLinList.AddLast(managerLocal);
						_managerDict.Add((uint)managerLocal.managerType, managerLocal);
						if (managerLocal.needTick)
							_tickLocalManagerLinList.AddLast(managerLocal);
					}
				}

				eLocal = _toBeAddedLocalList.GetEnumerator();
				while (eLocal.MoveNext())
				{
					managerLocal = eLocal.Current;
					if (managerLocal != null)
						managerLocal.Setup();
				}

				_toBeAddedLocalList.Clear();
			}

			var node = _localManagerLinList.First;
			BaseLocalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelLoaded(scene, loadSceneMode);
				}
				node = node.Next;
			}
		}

		private void OnSceneUnloaded(Scene scene)
		{
			OnGlobalManagerUnloaded(scene);
			OnLocalManagerUnloaded(scene);
		}
		private void OnGlobalManagerUnloaded(Scene scene)
		{
			var node = _globalManagerLinList.Last;
			BaseGlobalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelUnLoaded(scene);
				}
				node = node.Previous;
			}
		}

		private void OnLocalManagerUnloaded(Scene scene)
		{
			var node = _localManagerLinList.Last;
			BaseLocalManager manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.OnLevelUnLoaded(scene);
				}
				node = node.Previous;
			}

			node = _localManagerLinList.Last;
			manager = null;
			while (node != null)
			{
				manager = node.Value;
				if (manager != null)
				{
					manager.Destroy();
					_managerDict.Remove((uint)manager.managerType);
				}
				node = node.Previous;
			}

			_localManagerLinList.Clear();
			_tickLocalManagerLinList.Clear();
			_toBeAddedLocalList.Clear();
		}

		#endregion

		public void AddGlobalManager(BaseGlobalManager manager)
		{
			if (manager == null)
				return;
			if (_managerDict.ContainsKey((uint)manager.managerType))
				return;
			isDirtyAdd = true;
			manager.Init();
			_toBeAddedGlobalList.Add(manager);
		}

		public void AddLocalManager(BaseLocalManager manager)
		{
			if (manager == null)
				return;
			if (_managerDict.ContainsKey((uint)manager.managerType))
				return;
			isDirtyAdd = true;
			manager.Init();
			_toBeAddedLocalList.Add(manager);
		}

		public override void Destroy() // Destroy 模拟 OnSceneUnloaded
		{
			var scene = SceneManager.GetActiveScene();

			OnSceneUnloaded(scene);

			var nodee = _globalManagerLinList.Last;
			BaseGlobalManager managerr = null;
			while (nodee != null)
			{
				managerr = nodee.Value;
				if (managerr != null)
				{
					managerr.Destroy();
					_managerDict.Remove((uint)managerr.managerType);
				}
				nodee = nodee.Previous;
			}

			_globalManagerLinList.Clear();
			_tickGlobalManagerLinList.Clear();
			_toBeAddedGlobalList.Clear();

			_managerDict.Clear();

			UnityEngine.Application.Quit();
		}


		private void Update()
		{
			float deltaTime = Time.deltaTime;

			#region Tick Global Manager

			var nodeGlobal = _tickGlobalManagerLinList.First;
			BaseGlobalManager managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick) // manager 的 isActive 一直都是 true，如果不是，则不是Manager，而是 Entity
				{
					managerGlobal.Tick0(deltaTime);
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick1(deltaTime);
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick2(deltaTime);
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick3(deltaTime);
				}
				nodeGlobal = nodeGlobal.Next;
			}

			nodeGlobal = _tickGlobalManagerLinList.First;
			managerGlobal = null;
			while (nodeGlobal != null)
			{
				managerGlobal = nodeGlobal.Value;
				if (managerGlobal != null && managerGlobal.needTick)
				{
					managerGlobal.Tick4(deltaTime);
				}
				nodeGlobal = nodeGlobal.Next;
			}

			#endregion

			#region Tick Local Manager

			var nodeLocal = _tickLocalManagerLinList.First;
			BaseLocalManager managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick) // manager 的 isActive 一直都是 true，如果不是，则不是Manager，而是 Entity
				{
					managerLocal.Tick0(deltaTime);
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick1(deltaTime);
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick2(deltaTime);
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick3(deltaTime);
				}
				nodeLocal = nodeLocal.Next;
			}

			nodeLocal = _tickLocalManagerLinList.First;
			managerLocal = null;
			while (nodeLocal != null)
			{
				managerLocal = nodeLocal.Value;
				if (managerLocal != null && managerLocal.needTick)
				{
					managerLocal.Tick4(deltaTime);
				}
				nodeLocal = nodeLocal.Next;
			}

			#endregion
		}

		private void LateUpdate()
		{
			float deltaTime = Time.deltaTime;

			if (isDirtyAdd == true)
			{
				isDirtyAdd = false;

				var nodeGlobal = _globalManagerLinList.First;
				BaseGlobalManager managerGlobal = null;
				while (nodeGlobal != null)
				{
					managerGlobal = nodeGlobal.Value;
					if (managerGlobal != null && managerGlobal.isDirtyAdd)
					{
						managerGlobal.TickAddTo();
					}
					nodeGlobal = nodeGlobal.Next;
				}

				var nodeLocal = _localManagerLinList.First;
				BaseLocalManager managerLocal = null;
				while (nodeLocal != null)
				{
					managerLocal = nodeLocal.Value;
					if (managerLocal != null && managerLocal.isDirtyAdd)
					{
						managerLocal.TickAddTo();
					}
					nodeLocal = nodeLocal.Next;
				}

				if (_toBeAddedGlobalList.Count != 0)
				{
					for (int i = 0; i < _toBeAddedGlobalList.Count; i++)
					{
						managerGlobal = _toBeAddedGlobalList[i];
						if (managerGlobal != null)
						{
							managerGlobal.TickAddTo();

							_globalManagerLinList.AddLast(managerGlobal);
							_managerDict.Add((uint)managerGlobal.managerType, managerGlobal);
							if (managerGlobal.needTick)
								_tickGlobalManagerLinList.AddLast(managerGlobal);
						}
					}

					for (int i = 0; i < _toBeAddedGlobalList.Count; i++)
					{
						managerGlobal = _toBeAddedGlobalList[i];
						if (managerGlobal != null)
							managerGlobal.Setup();
					}

					_toBeAddedGlobalList.Clear();
				}

				if (_toBeAddedLocalList.Count != 0)
				{
					for (int i = 0; i < _toBeAddedLocalList.Count; i++)
					{
						managerLocal = _toBeAddedLocalList[i];
						if (managerLocal != null)
						{
							managerLocal.TickAddTo();

							_localManagerLinList.AddLast(managerLocal);
							_managerDict.Add((uint)managerLocal.managerType, managerLocal);
							if (managerLocal.needTick)
								_tickLocalManagerLinList.AddLast(managerLocal);
						}
					}

					for (int i = 0; i < _toBeAddedLocalList.Count; i++)
					{
						managerLocal = _toBeAddedLocalList[i];
						if (managerLocal != null)
							managerLocal.Setup();
					}

					_toBeAddedLocalList.Clear();
				}
			}
		}

		public T GetManager<T>(ManagerType type) where T : class
		{
			BaseManager manager;
			if (_managerDict.TryGetValue((uint)type, out manager) == false)
				return null;
			return manager as T;
		}

		public void QuitGame()
		{
			RemoveFullManagers();

			this.Destroy();
		}


	}
}