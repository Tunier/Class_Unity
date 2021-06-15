using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // ����ũ ȿ���� �༭ ��鸮�Ե� ī�޶��� Ʈ������.
    public Transform shakeCamera;
    public bool shakeRotate = false;
    Vector3 originPos; // ���� ��ġ.
    Quaternion originRot; // ���� ȸ����.

    void Start()
    {
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;
    }

    public IEnumerator ShakeCamera(float duration = 0.05f, float mPos = 0.03f, float mRot = 0.1f)
    {
        // ��� �ð� ����� ����.
        float passTime = 0f;
        // ������ ���� �ð����ȸ� ��鸮���� ����
        while (passTime < duration)
        {
            // �������� 1�� ������ ��ü ��߿��� ��ǥ�� ����
            // (x, y, z)�� �ּ� (-1, -1, -1~) ~ (1 ,1 ,1) ������ ���� ����.
            // ��鸲�Ҷ� ���̾�.
            Vector3 shakePos = Random.insideUnitSphere;
            // ������ ������ ��ġ ���� ���� ī�޶��� ��ġ�� ��������
            shakeCamera.localPosition = shakePos * mPos;

            // ī�޶� ȸ�� ��ų���.
            if (shakeRotate)
            {
                // PerlinNoise : ����Ģ���� ����� ������� �����Ͽ� �ϰ����� �ִ� ���� ����� �߻�.
                // ���� ��Ģ���� �ֵ��� ����. �����̳� �繰�� ��ġ�Ҷ� ���� ���Ǹ� ����
                // ���� �ʵ忡 �ִ� ������ Ǯ ���� ���� �� ���� ����.                
                float noise = Mathf.PerlinNoise(Time.time * mRot, 0f);
                Vector3 shakeRot = new Vector3(0, 0, noise);
                // ������ ���� ȸ������ ī�޷��� ����.
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);

            }
            passTime += Time.deltaTime;
            yield return null;
        }
        // ���� �Ŀ� ī�޶��� ��ġ�� ȸ������ �ʱ갪���� �ٽ� ����.
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }

    void Update()
    {

    }
}
