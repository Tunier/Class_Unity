using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target; // ī�޶� ������ ���
    public float moveDamping = 15f; // �̵��ӵ� ���
    public float rotateDamping = 10f; // ȸ���ӵ� ���
    public float distance = 5f; // ���� ������ �Ÿ�
    public float height = 4f; // ���� ������ ����
    public float targetOffset = 2f; // ���� ��ǥ�� ������

    Transform tr;

    [Header("�� ��ֹ� ����")]
    public float heightAboveWall = 7f;
    public float colliderRadius = 1.8f;
    public float overDamping = 5f;
    public float originHeight = 0f;

    void Start()
    {
        tr = GetComponent<Transform>();
        originHeight = height;
    }

    private void Update()
    {
        if (Physics.CheckSphere(tr.position, colliderRadius))
        {
            height = Mathf.Lerp(height, heightAboveWall, Time.deltaTime * overDamping);
        }
        else
        {
            height = Mathf.Lerp(height, originHeight, Time.deltaTime * overDamping);
        }
    }

    // �ݹ��Լ� - ȣ���� ���� ���� �ʾƵ� �˾Ƽ� �۵��ϴ� �Լ�.
    // �̺�Ʈ Ʈ���ŵ� �������� ������ ����.
    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);

        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping);
        // �����Լ��˻�. (���� ���� ������ �ε巴�� �����)

        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);
        // ����Ƽ���� ����ϴ� ����, ����� �������¸� Ǫ�µ� ���

        tr.LookAt(target.position + (target.up * targetOffset));
        // ī�޶� ��� Ÿ���� ������.(ĳ������ ���� ���� �����ִ°��� �������� ���ϰ� ������)
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // DrawWireSphere(��ġ, ����)
        // ������ �̷���� ������ ����� �׸�(���信�� ǥ�õ�, ����׿�)
        Gizmos.DrawWireSphere(target.position + (target.up * targetOffset), 0.1f);
        // ���� ī�޶�� ���� ���� ���̿� ���� �׸�.
        // ��߰� �������� ���̿� ���� �׸�.
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}
