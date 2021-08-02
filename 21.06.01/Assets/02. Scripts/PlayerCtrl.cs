using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����Ŭ����
// Ŭ���� ����ȭ
// Ŭ������ ��� ������ �޸� ����ȭ ����� �ν����Ϳ� ǥ�õ�.
[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;

}

public class PlayerCtrl : MonoBehaviour
{
    // ����������
    // Private�� �ۼ��� ��ũ��Ʈ ���ο����� ����.
    // ����Ƽ �⺻ ���´� Private��
    // �߰��� Protected ��� ���������ڰ� ����.
    float h = 0f;
    float v = 0f;
    float r = 0f; // ���콺 �� �޾Ƽ� ȸ�� ��Ű�� ���� ����.

    Transform tr; // Ʈ������ ������Ʈ ���� ����
    // Public���� ����� ������ Inspector â�� �����.    
    public float moveSpeed = 8f;
    public float rotSpeed = 400f;

    public PlayerAnim playerAnim;
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        anim.clip = playerAnim.idle;
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal"); // ��ǲ.�����޾ƿ�(���ӵ��� ����)(����)
        v = Input.GetAxis("Vertical"); // ��ǲ.�����޾ƿ�(���ӵ��� ����)(����)
        r = Input.GetAxis("Mouse X"); // ��ǲ.�����޾ƿ�(���콺 �¿�� ������ ĳ���Ͱ� ���ư�)
        // print("H�� " + h + " V�� " + v);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        // ���� ����ȭ
        // �밢���̵��� �Ϲ����� �̵����� ũ�Ⱑ Ŀ�� ���� ������.
        // �̸� �ذ��ϰ��� ������ ũ�⸦ �����ϰ� 1�� ����ȭ ��Ŵ.
        moveDir = moveDir.normalized;

        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        // print(Vector3.Magnitude(Vector3.forward + Vector3.right));
        // print(Vector3.Magnitude((Vector3.forward + Vector3.right).normalized));

        // �ִϸ��̼� ���۱���
        if (v >= 0.1f) // ����
            // CrossFade(�ִϸ��̼� �̸�, ��ȯ�ð�)
            anim.CrossFade(playerAnim.runF.name, 0.2f);
        else if (v <= -0.1f) // ����
            anim.CrossFade(playerAnim.runB.name, 0.2f);
        else if (h >= 0.1f) // ������
            anim.CrossFade(playerAnim.runR.name, 0.2f);
        else if (h <= -0.1f) // ����
            anim.CrossFade(playerAnim.runL.name, 0.2f);
        else // ������ idle ���·� ��ȯ.
            anim.CrossFade(playerAnim.idle.name, 0.1f);
    }
}
