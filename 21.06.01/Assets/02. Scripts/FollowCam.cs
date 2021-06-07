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
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
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
}
