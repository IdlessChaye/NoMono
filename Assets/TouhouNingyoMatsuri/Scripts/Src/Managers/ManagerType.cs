using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public enum ManagerType
	{
		TextMap,
		Input,
		Resource,
		UI,
		Entity,
		Music,
		None
	}

	public partial class TextMapManager
	{
		public override ManagerType managerType { get { return ManagerType.TextMap; } }
	}

	public partial class ResourceManager
	{
		public override ManagerType managerType { get { return ManagerType.Resource; } }
	}

	public partial class InputManager
	{
		public override ManagerType managerType { get { return ManagerType.Input; } }
	}

	public partial class UIManager
	{
		public override ManagerType managerType { get { return ManagerType.UI; } }
	}

	public partial class EntityManager
	{
		public override ManagerType managerType { get { return ManagerType.Entity; } }
	}


}