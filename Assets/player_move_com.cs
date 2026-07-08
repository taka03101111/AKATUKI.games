using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class player_move_com : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 3.0f;
    public float rotateSpeed = 120.0f;
    public float gravity = -9.8f;

    [Header("視点設定")]
    public Transform viewCamera;
    public float cameraLookSpeed = 80.0f;
    public float minCameraAngle = -30.0f;
    public float maxCameraAngle = 45.0f;

    [Header("アニメーション時間")]
    public float attackTime = 1.0f;
    public float damageTime = 0.8f;

    private Animator animator;
    private CharacterController controller;

    private int damageCount = 0;
    private bool isAction = false;
    private bool isDead = false;

    private float cameraPitch = 0.0f;
    private float verticalVelocity = 0.0f;

    private string currentAnim = "";

    private const string IDLE = "Combat (1)";
    private const string RUN_FORWARD = "前に走る";
    private const string RUN_BACK = "後ろに走る";
    private const string RUN_LEFT = "左に走る";
    private const string RUN_RIGHT = "右に走る";
    private const string ATTACK_STAB = "剣をさす攻撃";
    private const string ATTACK_FULL = "剣をフル攻撃";
    private const string DAMAGE = "Take Damage (1)";
    private const string DEATH = "Death 01";

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (viewCamera == null && Camera.main != null)
        {
            viewCamera = Camera.main.transform;
        }

        PlayAnim(IDLE);
    }

    void Update()
    {
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage();
            return;
        }

        if (isAction) return;

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(ActionAnimation(ATTACK_STAB, attackTime));
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(ActionAnimation(ATTACK_FULL, attackTime));
            return;
        }

        LookControl();
        MoveControl();
    }

    void MoveControl()
    {
        Vector3 move = Vector3.zero;
        string nextAnim = IDLE;

        if (Input.GetKey(KeyCode.W))
        {
            move += transform.forward;
            nextAnim = RUN_FORWARD;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move -= transform.forward;
            nextAnim = RUN_BACK;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            move -= transform.right;
            nextAnim = RUN_LEFT;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            move += transform.right;
            nextAnim = RUN_RIGHT;
        }

        if (move != Vector3.zero)
        {
            move.Normalize();
        }

        // 重力処理
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1.0f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 velocity = move * moveSpeed;
        velocity.y = verticalVelocity;

        // CharacterControllerで移動するので壁を貫通しにくい
        controller.Move(velocity * Time.deltaTime);

        PlayAnim(nextAnim);
    }

    void LookControl()
    {
        float horizontal = 0.0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1.0f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1.0f;
        }

        transform.Rotate(0, horizontal * rotateSpeed * Time.deltaTime, 0);

        if (viewCamera != null)
        {
            float vertical = 0.0f;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                vertical = -1.0f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                vertical = 1.0f;
            }

            cameraPitch += vertical * cameraLookSpeed * Time.deltaTime;
            cameraPitch = Mathf.Clamp(cameraPitch, minCameraAngle, maxCameraAngle);

            viewCamera.localEulerAngles = new Vector3(cameraPitch, 0, 0);
        }
    }

    void TakeDamage()
    {
        damageCount++;

        if (damageCount >= 3)
        {
            isDead = true;
            isAction = true;
            PlayAnim(DEATH);
        }
        else
        {
            StartCoroutine(ActionAnimation(DAMAGE, damageTime));
        }
    }

    IEnumerator ActionAnimation(string animName, float waitTime)
    {
        isAction = true;

        PlayAnim(animName);

        yield return new WaitForSeconds(waitTime);

        if (isDead) yield break;

        isAction = false;
        PlayAnim(IDLE);
    }

    void PlayAnim(string animName)
    {
        if (animator == null) return;

        if (currentAnim == animName) return;

        currentAnim = animName;
        animator.CrossFade(animName, 0.1f);
    }
}