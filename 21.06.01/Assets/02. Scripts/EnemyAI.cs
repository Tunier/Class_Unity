using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        PATROL, // ����
        TRACE, // ����
        ATTCK, // ����
        DIE // ���
    }

    public State state = State.PATROL; // �ʱ� ���� ����.

    Transform playerTr; // �÷��̾� ��ġ ���� ����.
    Transform enemyTr; // ��ĳ���� ��ġ ���� ����.

    public float attackDist = 5f; // ���� ��Ÿ�.
    public float traceDist = 10f; // ���� ��Ÿ�.
    public bool isDie = false; // ��� ���� �Ǵ� ����.

    WaitForSeconds ws; // �ð� ���� ����.

    void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");

        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();

        // �ð� ���� ������ 0.3 ������ ����.
        // �ð� ���� ������ �ڷ�ƾ �Լ����� ����.
        ws = new WaitForSeconds(0.3f);
    }

    private void OnEnable()
    {
        // OnEnable�� �ش� ��ũ��Ʈ�� Ȱ��ȭ�� ������ �����.
        // ���� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��.
        StartCoroutine(CheckState());
    }

    // �ڷ�ƾ = �������� �ð����� ����.
    IEnumerator CheckState() // ����üũ �ڷ�ƾ �Լ�.
    {
        while (!isDie) // ���� ����ִµ��� ��� ����ǵ��� while���.
        {
            if (state == State.DIE)
                yield break; // �ڷ�ƾ �Լ� ����.
            // Distance(A,B) - A�� B������ �Ÿ��� ������ִ� �Լ�.
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist) // ���� ��Ÿ� �̳��� �������� ����.
            {
                state = State.ATTCK;
            }
            else if (dist <= traceDist) // ���� ��Ÿ� �̳��� �������� ����.
            {
                state = State.TRACE;
            }
            else // ���ݵ� ������ �ƴϸ� �������·� ����.
            {
                state = State.PATROL;
            }
            yield return ws;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
