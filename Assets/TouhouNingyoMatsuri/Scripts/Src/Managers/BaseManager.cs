
using UnityEngine.SceneManagement;

namespace NingyoRi
{
	public enum ManagerType
	{
		TextMap,
		Input,
		Resource,
		UI,
		Music,
		Entity,
		None
	}

	public class BaseManager
	{
		public virtual ManagerType managerType { get { return ManagerType.None; } }

		public bool needTick { get; set; }

		public virtual void Init() { }


		public virtual void Tick0() { }

		public virtual void Tick1() { }

		public virtual void Tick2() { }

		public virtual void Tick3() { }

		public virtual void Tick4() { }


		public virtual void Destroy() { }

		public virtual void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode) { }

		public virtual void OnLevelUnLoaded(Scene scene) { }

	}

	// LocalManager 在Scene切换时会被Destroy
	public class BaseLocalManager : BaseManager
	{
		
	}

	public class BaseGlobalManager : BaseManager
	{

	}
}