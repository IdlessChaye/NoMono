using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public class CoroutineManager : FullSingleton<CoroutineManager>
	{
		private List<IEnumerator> enumeratorList = new List<IEnumerator>();

		public void AddCoroutine(float delay, System.Action action)
		{
			var ie = DelayCoroutine(delay, action);
			enumeratorList.Add(ie);
			StartCoroutine(ie);
		}

		private IEnumerator DelayCoroutine(float delay, System.Action action)
		{
			yield return new WaitForSeconds(delay);
			if (action != null)
				action();
		}
	}
}