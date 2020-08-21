using UnityEngine;
using System.Collections;

namespace UnityChan
{
	public class SpringCollider : MonoBehaviour
	{
        private void Awake() {
            SpringBone.colliders[SpringBone.indexColliders++] =this;
        }
        //半径
        public float radius = 0.5f;

		private void OnDrawGizmosSelected ()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere (transform.position, radius);
		}
	}
}