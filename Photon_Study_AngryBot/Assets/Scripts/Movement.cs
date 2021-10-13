using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System;

public class Movement : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterController controller;
    Transform transform;
    Animator animator;
    Camera camera;

    Plane plane;
    Ray ray;
    Vector3 hitPoint;

    public float moveSpeed = 10f;

    PhotonView pv;
    CinemachineVirtualCamera virtualCamera;

    // 수신된 데이터.
    Vector3 receivePos;
    Quaternion receiveRot;
    // 수신된 좌표로 이동 및 회전할 속도.
    public float damping = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        // 가상의 바닥을 플레이어의 위치를 기준으로 생성.
        plane = new Plane(transform.up, transform.position);

        if (pv.IsMine) // 내것인지 남것인지 판단.
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
    }

    // 람다식은 표현을 간결하게 > 생산성을 증대시키는 구문
    // 요즘에 많이 사용됨.
    float h => Input.GetAxis("Horizontal");
    float v => Input.GetAxis("Vertical");

    void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        Vector3 moveDir = cameraForward * v + cameraRight * h;
        moveDir.Set(moveDir.x, 0f, moveDir.z);

        controller.SimpleMove(moveDir * moveSpeed);

        float forward = Vector3.Dot(moveDir, transform.forward);
        float strafe = Vector3.Dot(moveDir, transform.right);

        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    void Turn()
    {
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0f;
        plane.Raycast(ray, out enter);
        hitPoint = ray.GetPoint(enter);

        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0;
        transform.localRotation = Quaternion.LookRotation(lookDir);
    }

    void Update()
    {
        if (pv.IsMine)
        {
            Move();
            Turn();
        }
        else // 남의 것의 경우 수신된 데이터로 조작해줘야됨.
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    // 데이터 송/수신에 쓰이는 콜백 함수.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //try
        //{
        //    int x = 5;
        //    int y = 0;
        //    print(x / y);
        //}
        //catch (ArithmeticException e)
        //{
        //    // Exception 클래스는 가장 대장.
        //    // 모든 문제점을 포함하고 있는 카테고리.
        //    // 하위에 세세한 문제들이 개별 명시되어있음.
        //}
        //finally
        //{
        //    // 예외가 발생하더라도 실행함.
        //}

        if (stream.IsWriting) // 로컬 캐릭터(본인)인경우 자신의 데이터를 다른 유저에게 송신.
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else // 수신
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
