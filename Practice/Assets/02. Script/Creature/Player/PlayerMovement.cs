using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    GameObject cameraArm;
    Player_SkillIndicator pSkillIndicator;
    ClickEffect clickEffect;

    float walkMoveSpeed = 5f;
    float runMoveSpeed;
    float backMoveSpeed;
    float jumpForce = 9f;
    KeyCode jumpKeyCode = KeyCode.Space;
    float gravity = -9.81f;

    float speed = 0f;

    float x;
    float z;

    Vector3 moveDirection;

    public Transform[] Rot;

    PlayerInfo playerInfo;
    PlayerActionCtrl playerActionCtrl;
    CharacterController cController;
    NavMeshAgent nav;
    Animator ani;

    KeyCode runKeyCode = KeyCode.LeftShift;

    bool isMove = false;
    bool isRun = false;
    public bool wantMove = false;

    float pushTime = 0;

    Ray ray;
    public RaycastHit hit;

    float effectOverTime = 0.2f;

    void Awake()
    {
        pSkillIndicator = FindObjectOfType<Player_SkillIndicator>();
        clickEffect = FindObjectOfType<ClickEffect>();

        playerInfo = GetComponent<PlayerInfo>();
        playerActionCtrl = GetComponent<PlayerActionCtrl>();
        cController = GetComponent<CharacterController>();
        nav = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();

        isMove = false;
        isRun = false;
    }

    private void Start()
    {
        nav.enabled = false;
    }

    void Update()
    {
        runMoveSpeed = walkMoveSpeed * 2f;
        backMoveSpeed = walkMoveSpeed * 0.85f;

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        effectOverTime += Time.deltaTime;

        if (isRun && effectOverTime >= 0.2f && z > 0 && cController.isGrounded && playerInfo.state != STATE.Attacking)
        {
            effectOverTime = 0;
            var obj = ObjPoolingManager.Instance.GetEffectAtPool(ObjPoolingManager.Effect.RunEffect);
            obj.SetActive(true);
            obj.transform.position = transform.position;
        }


        if (playerInfo.state == STATE.Attacking)
        {
            if (nav.enabled)
            {
                wantMove = false;

                nav.isStopped = true;
                nav.ResetPath();

                clickEffect.clickEffectCanvas.enabled = false;
            }
        }

        if (playerInfo.state != STATE.Die && playerInfo.state != STATE.Attacking)
        {
            #region 키보드로 제어하는 움직임 부분

            if (Input.GetKeyDown(runKeyCode))
                isRun = !isRun;

            if (!cController.isGrounded)
                moveDirection.y += gravity * Time.deltaTime;
            else
                playerInfo.state = STATE.Idle;

            if (Input.GetKeyDown(jumpKeyCode) && cController.isGrounded && cController.enabled && !playerActionCtrl.isWhirlwind)
            {
                moveDirection.y = jumpForce;
                playerInfo.state = STATE.Jump;
            }

            if (!pSkillIndicator.straightIndicator.activeSelf && !playerActionCtrl.isWhirlwind)
            {
                if (x != 0 || z != 0)
                {
                    Vector3 camArmRot = new Vector3(0, cameraArm.transform.eulerAngles.y, 0);
                    transform.rotation = Quaternion.Euler(camArmRot);
                }

                if (x * z == 1)
                    transform.eulerAngles += new Vector3(0, 45, 0);
                else if (x * z == -1)
                    transform.eulerAngles += new Vector3(0, -45, 0);
            }

            if (cController.enabled)
            {
                setMoveDir(x, z);
                cController.Move(moveDirection * Time.deltaTime);
            }
            #endregion

            #region 캐릭터 제어권을 가지는 컴포넌트 변경 부분
            if (x != 0 || z != 0)
            {
                if (nav.enabled)
                {
                    wantMove = false;
                    playerInfo.state = STATE.Idle;

                    nav.isStopped = true;
                    nav.ResetPath();

                    nav.enabled = false;
                    cController.enabled = true;

                    clickEffect.clickEffectCanvas.enabled = false;
                }
            }
            #endregion

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                #region 마우스 우클릭을 했을때 누르는 시간 체크
                if (Input.GetMouseButton(1))
                {
                    pushTime += Time.deltaTime;

                    if (pushTime >= 0.15f)
                        wantMove = false;
                    else
                        wantMove = true;
                }
                #endregion

                #region 네브메쉬 에이전트 제어구문

                if (isRun)
                    nav.speed = runMoveSpeed;
                else
                    nav.speed = walkMoveSpeed;

                if (Input.GetMouseButtonUp(1))
                {
                    pushTime = 0;
                    if (wantMove)
                    {
                        nav.enabled = true;
                        cController.enabled = false;

                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        Physics.Raycast(ray, out hit, Mathf.Infinity);

                        Vector3 mousePos = hit.point;

                        nav.SetDestination(mousePos);

                        StopAllCoroutines();
                        StartCoroutine(clickEffect.ClickEffectCtrl(new Vector3(hit.point.x, hit.point.y + 1.1f, hit.point.z)));

                        isMove = true;
                        playerInfo.state = STATE.Walk;
                    }
                }

                if (nav.enabled)
                {
                    if (Vector3.Distance(nav.destination, transform.position) <= 0.2f)
                    {
                        nav.SetDestination(transform.position);
                        nav.ResetPath();
                        playerInfo.state = STATE.Idle;

                        cController.enabled = true;
                        nav.enabled = false;

                        wantMove = false;

                        isMove = false;
                    }

                    if (isMove)
                    {
                        if (isRun)
                        {
                            playerInfo.state = STATE.Run;
                        }
                        else
                        {
                            playerInfo.state = STATE.Walk;
                        }
                    }
                }
                #endregion
            }
        }
    }

    /// <summary>
    /// GetAxisRaw로 움직임을 받아서 움직이는 방향과 속도를 제어해줌.
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_z"></param>
    void setMoveDir(float _x, float _z)
    {
        if (playerActionCtrl.isWhirlwind)
        {
            speed = walkMoveSpeed;
        }
        else if (_z == 1f)
        {
            if (!isRun)
            {
                speed = walkMoveSpeed;
                if (playerInfo.state != STATE.Jump && playerInfo.state != STATE.Attacking)
                    playerInfo.state = STATE.Walk;
            }
            else
            {
                speed = runMoveSpeed;
                if (playerInfo.state != STATE.Jump && playerInfo.state != STATE.Attacking)
                    playerInfo.state = STATE.Run;
            }
        }
        else if (_z == -1)
        {
            speed = backMoveSpeed;
            if (playerInfo.state != STATE.Jump && playerInfo.state != STATE.Attacking)
                playerInfo.state = STATE.Backoff;
        }
        else if (_x == 1)
        {
            speed = walkMoveSpeed;
            if (playerInfo.state != STATE.Jump && playerInfo.state != STATE.Attacking)
                playerInfo.state = STATE.Walk;
        }
        else if (_x == -1f)
        {
            speed = walkMoveSpeed;
            if (playerInfo.state != STATE.Jump && playerInfo.state != STATE.Attacking)
                playerInfo.state = STATE.Walk;
        }
        else if (_x == 0 && _z == 0)
        {
            speed = 0;
            if (playerInfo.state != STATE.Jump && playerInfo.state != STATE.Attacking)
                playerInfo.state = STATE.Idle;
        }

        if (_z == 0 && _x == 0)
            moveDirection = new Vector3(0, moveDirection.y, 0);
        else if (_z == 1 && _x == -1)
            moveDirection = new Vector3((Rot[0].position.x - transform.position.x) * speed, moveDirection.y, (Rot[0].position.z - transform.position.z) * speed);
        else if (_z == 1 && _x == 0)
            moveDirection = new Vector3((Rot[1].position.x - transform.position.x) * speed, moveDirection.y, (Rot[1].position.z - transform.position.z) * speed);
        else if (_z == 1 && _x == 1)
            moveDirection = new Vector3((Rot[2].position.x - transform.position.x) * speed, moveDirection.y, (Rot[2].position.z - transform.position.z) * speed);
        else if (_z == 0 && _x == -1)
            moveDirection = new Vector3((Rot[3].position.x - transform.position.x) * speed, moveDirection.y, (Rot[3].position.z - transform.position.z) * speed);
        else if (_z == 0 && _x == 1)
            moveDirection = new Vector3((Rot[4].position.x - transform.position.x) * speed, moveDirection.y, (Rot[4].position.z - transform.position.z) * speed);
        else if (_z == -1 && _x == -1)
            moveDirection = new Vector3((Rot[5].position.x - transform.position.x) * speed, moveDirection.y, (Rot[5].position.z - transform.position.z) * speed);
        else if (_z == -1 && _x == 0)
            moveDirection = new Vector3((Rot[6].position.x - transform.position.x) * speed, moveDirection.y, (Rot[6].position.z - transform.position.z) * speed);
        else if (_z == -1 && _x == 1)
            moveDirection = new Vector3((Rot[7].position.x - transform.position.x) * speed, moveDirection.y, (Rot[7].position.z - transform.position.z) * speed);
    }
}