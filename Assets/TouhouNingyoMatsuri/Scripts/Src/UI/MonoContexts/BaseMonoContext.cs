using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	[RequireComponent(typeof(Animator))]
	public class BaseMonoContext : MonoBehaviour
	{
		[SerializeField]
		protected Animator animator;

		[HideInInspector]
		public GameObject contextGO;

		private void Awake()
		{
			if (animator == null)
				animator = GetComponent<Animator>();
			contextGO = this.gameObject;
		}

		public virtual void Show()
		{
			animator.SetTrigger("Show");
		}

		public virtual void Hide()
		{
			animator.SetTrigger("Hide");
		}
	}
}