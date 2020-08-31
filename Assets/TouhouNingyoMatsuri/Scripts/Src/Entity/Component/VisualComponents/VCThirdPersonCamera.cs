using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class VCThirdPersonCamera : BaseVisualComponent
	{
		public Transform targetTF { get; set; }

		private SphericalVector sphericalVector = new SphericalVector(5f, 1f, 0.3f);
		private float viewMoveSpeed;
		private Transform _transform;

		public override void Init(BaseEntity entity)
		{
			base.Init(entity);
			SetNeedTick(true);

			viewMoveSpeed = 0.03f;
			_transform = _gameObject.transform;

			Messenger.AddListener<Transform>((uint)EventType.ChangeAvatar, (t) => targetTF = t);
		}

		public override void Tick2()  // このカメラ切り替えはFixedUpdate()内でないと正常に動かない
		{
			if (targetTF == null)
				return;

			float h = Input.GetAxis("Mouse X");//水平视角移动)
			float v = Input.GetAxis("Mouse Y");//垂直视角移动
			sphericalVector.azimuth += h * viewMoveSpeed;
			sphericalVector.zenith -= v * viewMoveSpeed;
			sphericalVector.zenith = Mathf.Clamp(sphericalVector.zenith, 0f, 1f);
			float s = Input.GetAxis("Mouse ScrollWheel");//滚轮拉近视角
			sphericalVector.length -= s * 10f;
			sphericalVector.length = Mathf.Clamp(sphericalVector.length, 2f, 5f);
			Vector3 destTargetPosition = targetTF.position + Vector3.up * 0.5f;
			Vector3 destCameraPosition = destTargetPosition + sphericalVector.Position;//设定摄像机位置
																					   //transform.position = Vector3.Lerp(transform.position,destCameraPosition,Time.fixedDeltaTime * smooth);
			_transform.position = destCameraPosition;
			_transform.forward = GetLookAtVector3(_transform.position, destTargetPosition); //设定摄像机朝向
		}

		Vector3 GetLookAtVector3(Vector3 oriPosition, Vector3 destPosition)
		{
			Vector3 offsetPosition = destPosition - oriPosition;
			return offsetPosition;
		}
	}
}