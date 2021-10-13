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

    // ���ŵ� ������.
    Vector3 receivePos;
    Quaternion receiveRot;
    // ���ŵ� ��ǥ�� �̵� �� ȸ���� �ӵ�.
    public float damping = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        // ������ �ٴ��� �÷��̾��� ��ġ�� �������� ����.
        plane = new Plane(transform.up, transform.position);

        if (pv.IsMine) // �������� �������� �Ǵ�.
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
    }

    // ���ٽ��� ǥ���� �����ϰ� > ���꼺�� �����Ű�� ����
    // ���� ���� ����.
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
        else // ���� ���� ��� ���ŵ� �����ͷ� ��������ߵ�.
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * damping);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * damping);
        }
    }

    // ������ ��/���ſ� ���̴� �ݹ� �Լ�.
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
        //    // Exception Ŭ������ ���� ����.
        //    // ��� �������� �����ϰ� �ִ� ī�װ�.
        //    // ������ ������ �������� ���� ��õǾ�����.
        //}
        //finally
        //{
        //    // ���ܰ� �߻��ϴ��� ������.
        //}

        if (stream.IsWriting) // ���� ĳ����(����)�ΰ�� �ڽ��� �����͸� �ٸ� �������� �۽�.
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else // ����
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
