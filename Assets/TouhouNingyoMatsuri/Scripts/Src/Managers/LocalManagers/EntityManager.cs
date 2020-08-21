using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class EntityManager : BaseLocalManager
	{
		public override ManagerType managerType
		{
			get
			{
				return ManagerType.Entity;
			}
		}

	}
}

