using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NingyoRi
{
	public partial class LCRawPlayerController : BaseLogicComponent
	{
		public float animSpeed = 1.5f;              // アニメーション再生速度設定
		public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
													// このスイッチが入っていないとカーブは使われない
		public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

		// 以下キャラクターコントローラ用パラメタ
		// 前進速度
		public float forwardSpeed = 7.0f;
		// 後退速度
		public float backwardSpeed = 2.0f;
		// 旋回速度
		public float rotateSpeed = 2.0f;
		// ジャンプ威力
		public float jumpPower = 3.0f;
		// キャラクターコントローラ（カプセルコライダ）の参照
		private CapsuleCollider col;
		private Rigidbody rb;
		// キャラクターコントローラ（カプセルコライダ）の移動量
		private Vector3 velocity;
		// CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
		private float orgColHight;
		private Vector3 orgVectColCenter;

		private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
		private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照

		// アニメーター各ステートへの参照
		static int idleState = Animator.StringToHash("Base Layer.Idle");
		static int locoState = Animator.StringToHash("Base Layer.Locomotion");
		static int jumpState = Animator.StringToHash("Base Layer.Jump");
		static int restState = Animator.StringToHash("Base Layer.Rest");

		private new PlayerPlugin _plugin;
		private Transform _transform;

		public override void Init(BaseEntity entity)
		{
			base.Init(entity);
			SetNeedTick(true);
			_plugin = base._plugin as PlayerPlugin;

			InitCharacter(0);
		}

		public override void Setup()
		{
			Messenger.Broadcast<Transform>((uint)EventType.ChangeAvatar, _transform);
		}

		public void InitCharacter(int index)
		{
			GameObject go = null;
			if (index == 0)
			{
				go = _plugin.alice;
				Utils.TrySetActive(_plugin.alice, true);
				Utils.TrySetActive(_plugin.marisa, false);
				Utils.TrySetActive(_plugin.reimu, false);
			}
			else if (index == 1)
			{
				go = _plugin.marisa;
				Utils.TrySetActive(_plugin.alice, false);
				Utils.TrySetActive(_plugin.marisa, true);
				Utils.TrySetActive(_plugin.reimu, false);
			}
			else if (index == 2)
			{
				go = _plugin.reimu;
				Utils.TrySetActive(_plugin.alice, false);
				Utils.TrySetActive(_plugin.marisa, false);
				Utils.TrySetActive(_plugin.reimu, true);
			}
			anim = go.GetComponent<Animator>();
			col = go.GetComponent<CapsuleCollider>();
			rb = go.GetComponent<Rigidbody>();
			_transform = go.transform;

			// CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
			orgColHight = col.height;
			orgVectColCenter = col.center;

			Messenger.Broadcast<Transform>((uint)EventType.ChangeAvatar, _transform);
		}

		public override void Tick1(float deltaTime)
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
				InitCharacter(0);
			else if (Input.GetKeyDown(KeyCode.Alpha2))
				InitCharacter(1);
			else if (Input.GetKeyDown(KeyCode.Alpha3))
				InitCharacter(2);
		}

		// 以下、メイン処理.リジッドボディと絡めるので、FixedUpdate内で処理を行う.
		public override void Tick2(float deltaTime)
		{
			float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
			float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義
			anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
			anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
			anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
			currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
			rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする



			// 以下、キャラクターの移動処理
			velocity = new Vector3(0, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
													// キャラクターのローカル空間での方向に変換
			velocity = _transform.TransformDirection(velocity);
			//以下のvの閾値は、Mecanim側のトランジションと一緒に調整する
			if (v > 0.1)
			{
				velocity *= forwardSpeed;       // 移動速度を掛ける
			}
			else if (v < -0.1)
			{
				velocity *= backwardSpeed;  // 移動速度を掛ける
			}

			if (Input.GetButtonDown("Jump"))
			{   // スペースキーを入力したら

				//アニメーションのステートがLocomotionの最中のみジャンプできる
				if (currentBaseState.fullPathHash == locoState)
				{
					//ステート遷移中でなかったらジャンプできる
					if (!anim.IsInTransition(0))
					{
						rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
						anim.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
					}
				}
			}


			// 上下のキー入力でキャラクターを移動させる
			_transform.localPosition += velocity * Time.fixedDeltaTime;

			// 左右のキー入力でキャラクタをY軸で旋回させる
			_transform.Rotate(0, h * rotateSpeed, 0);


			// 以下、Animatorの各ステート中での処理
			// Locomotion中
			// 現在のベースレイヤーがlocoStateの時
			if (currentBaseState.fullPathHash == locoState)
			{
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves)
				{
					resetCollider();
				}
			}
			// JUMP中の処理
			// 現在のベースレイヤーがjumpStateの時
			else if (currentBaseState.fullPathHash == jumpState)
			{
				// ステートがトランジション中でない場合
				if (!anim.IsInTransition(0))
				{

					// 以下、カーブ調整をする場合の処理
					if (useCurves)
					{
						// 以下JUMP00アニメーションについているカーブJumpHeightとGravityControl
						// JumpHeight:JUMP00でのジャンプの高さ（0〜1）
						// GravityControl:1⇒ジャンプ中（重力無効）、0⇒重力有効
						float jumpHeight = anim.GetFloat("JumpHeight");
						float gravityControl = anim.GetFloat("GravityControl");
						if (gravityControl > 0)
							rb.useGravity = false;  //ジャンプ中の重力の影響を切る

						// レイキャストをキャラクターのセンターから落とす
						Ray ray = new Ray(_transform.position + Vector3.up, -Vector3.up);
						RaycastHit hitInfo = new RaycastHit();
						// 高さが useCurvesHeight 以上ある時のみ、コライダーの高さと中心をJUMP00アニメーションについているカーブで調整する
						if (Physics.Raycast(ray, out hitInfo))
						{
							if (hitInfo.distance > useCurvesHeight)
							{
								col.height = orgColHight - jumpHeight;          // 調整されたコライダーの高さ
								float adjCenterY = orgVectColCenter.y + jumpHeight;
								col.center = new Vector3(0, adjCenterY, 0); // 調整されたコライダーのセンター
							}
							else
							{
								// 閾値よりも低い時には初期値に戻す（念のため）					
								resetCollider();
							}
						}
					}
					// Jump bool値をリセットする（ループしないようにする）				
					anim.SetBool("Jump", false);
				}
			}
			// IDLE中の処理
			// 現在のベースレイヤーがidleStateの時
			else if (currentBaseState.fullPathHash == idleState)
			{
				//カーブでコライダ調整をしている時は、念のためにリセットする
				if (useCurves)
				{
					resetCollider();
				}
				// スペースキーを入力したらRest状態になる
				if (Input.GetButtonDown("Jump"))
				{
					anim.SetBool("Rest", true);
				}
			}
			// REST中の処理
			// 現在のベースレイヤーがrestStateの時
			else if (currentBaseState.fullPathHash == restState)
			{
				//cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
				// ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
				if (!anim.IsInTransition(0))
				{
					anim.SetBool("Rest", false);
				}
			}
		}


		// キャラクターのコライダーサイズのリセット関数
		void resetCollider()
		{
			// コンポーネントのHeight、Centerの初期値を戻す
			col.height = orgColHight;
			col.center = orgVectColCenter;
		}
	}
}