//
// Unityちゃん用の三人称カメラ
// 
// 2013/06/07 N.Kobyasahi
//
using UnityEngine;
using System.Collections;

// 像这种模式比较多的东西就应该用状态机来做，我凉了
public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject target;
    public float viewMoveSpeed;
    public float smooth = 3f;		// カメラモーションのスムーズ化用変数

    private SphericalVector sphericalVector = new SphericalVector(5f, 1f, 0.3f);

    private Transform standardPos;			// the usual position for the camera, specified by a transform in the game
	private Transform frontPos;			// Front Camera locater
	private Transform jumpPos;          // Jump Camera locater

    private bool isSpecialView;
    private bool isShaking;
    private float shakeRange;

    void FixedUpdate ()	// このカメラ切り替えはFixedUpdate()内でないと正常に動かない
	{
        if(standardPos == null)
            return;

        isSpecialView = false;

        if(Input.GetButton("Fire1"))	// left Ctlr
		{
            isSpecialView = true;
            // Change Front Camera
            setCameraPositionFrontView();
		}else if(Input.GetButton("Fire2"))	//Alt
		{
            isSpecialView = true;
            // Change Jump Camera
            setCameraPositionJumpView();
		}else {
            isSpecialView = false;
            float h = Input.GetAxis("Mouse X");//水平视角移动)
            float v = Input.GetAxis("Mouse Y");//垂直视角移动
            sphericalVector.azimuth += h * viewMoveSpeed;
            sphericalVector.zenith -= v * viewMoveSpeed;
            sphericalVector.zenith = Mathf.Clamp(sphericalVector.zenith, 0f, 1f);
            float s = Input.GetAxis("Mouse ScrollWheel");//滚轮拉近视角
            sphericalVector.length -= s * 10f;
            sphericalVector.length = Mathf.Clamp(sphericalVector.length, 2f, 5f);
            Vector3 destTargetPosition = standardPos.position + Vector3.up * 0.5f;
            Vector3 destCameraPosition = destTargetPosition + sphericalVector.Position;//设定摄像机位置
            //transform.position = Vector3.Lerp(transform.position,destCameraPosition,Time.fixedDeltaTime * smooth);
            transform.position = destCameraPosition;
            transform.forward = GetLookAtVector3(transform.position, destTargetPosition); //设定摄像机朝向
            Vector3 forwardVector2 = -sphericalVector.Position;
            target.SendMessage("PlayerLookAt", new Vector3(forwardVector2.x, 0, forwardVector2.z));
        }

        if(isShaking) {
            float upShakeRange = Random.Range(-shakeRange, shakeRange);
            float rightShakeRange = Random.Range(-shakeRange, shakeRange);
            float forwardShakeRange = Random.Range(-shakeRange, shakeRange);
            transform.position += upShakeRange * Vector3.up;
            transform.position += rightShakeRange * Vector3.right;
            transform.position += forwardShakeRange * Vector3.forward;
            shakeRange = Mathf.Lerp(shakeRange, 0, 5 * Time.deltaTime);
            if(shakeRange < 0.1f) {
                isShaking = false;
                shakeRange = 0f;
            }
            if(!isSpecialView) {
                Vector3 destTargetPosition = standardPos.position + Vector3.up * 0.5f;
                transform.forward = GetLookAtVector3(transform.position, destTargetPosition);
            }
        }
    }


    void setCameraPositionFrontView() {
        // Change Front Camera
        if(isShaking) {
            transform.position = frontPos.position;
            transform.forward = frontPos.forward;
        } else { 
            transform.position = Vector3.Lerp(transform.position, frontPos.position, 5*Time.fixedDeltaTime * smooth);
            transform.forward = Vector3.Lerp(transform.forward, frontPos.forward, 5*Time.fixedDeltaTime * smooth);
        }
    }

    void setCameraPositionJumpView()
	{
        // Change Jump Camera
        if(isShaking) {
            transform.position = jumpPos.position;
            transform.forward = jumpPos.forward;
        } else {
		    transform.position = Vector3.Lerp(transform.position, jumpPos.position, Time.fixedDeltaTime * smooth);	
		    transform.forward = Vector3.Lerp(transform.forward, jumpPos.forward, Time.fixedDeltaTime * smooth);
        }
    }

    void SetStandardPos(GameObject go)
    {
        target = go;
        standardPos = go.transform.GetChild(2).transform;
        frontPos = go.transform.GetChild(3).transform;
        jumpPos = go.transform.GetChild(4).transform;
    }

    void CameraShake(float newShakeRange) {
        isShaking = true;
        shakeRange = Mathf.Max(shakeRange,newShakeRange);
    }

    void OnDestroy() {

    }

    Vector3 GetLookAtVector3(Vector3 oriPosition, Vector3 destPosition) {
        Vector3 offsetPosition = destPosition - oriPosition;
        return offsetPosition;
    }
}
