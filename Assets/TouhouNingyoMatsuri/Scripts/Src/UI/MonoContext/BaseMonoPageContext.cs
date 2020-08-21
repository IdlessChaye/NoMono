using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class BaseMonoPageContext : BaseMonoContext
	{
		public void ShowPage()
		{
			animator.SetTrigger("ShowPage");
		}

		public void HidePage()
		{
			animator.SetTrigger("HidePage");
		}
	}
}