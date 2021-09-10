using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mySingle : MonoBehaviour
{
    private static mySingle instance = null;
    // Ŭ���� ��ü�� ���������� �����ϰ� null�� �ʱ�ȭ�Ѵ�.
    // �Ϲ������� Ŭ���� ��ü�� ���������� �����Ҷ��� private�� �����Ѵ�.

    // ������ü�� �̿��� �̱����� ����
    // �̱��� : ���α׷��� �����ϴ� ����
    // ���� �ϳ��� �����ؾ��ϴ� ����� ������
    // (���� ��ȯ�ǵ�, �ٸ� Ŭ�������� �ش� ����� ����ؾ� �ϵ�)
    // ��� ��Ȳ���� ����� �ϳ����� ���� ����ϵ���
    // ���ִ� ���α׷� ����(������ ����)

    public int i = 0;
    public int i2 = 1;

    public static mySingle Instance
    {
        // �̱������� ���� Ŭ���� ��ü instance��
        // Ŭ������ ���ԵǾ� �ֱ� ������
        // ����Ϸ��� Ŭ������ �����ؾ� �Ѵ�.
        // �׷��� �̹� ������ Ŭ������ ������
        // �ߺ������� �������� �����ϱ� ������
        // ���Ե� instance�� ����Ҽ� ���ٴ� ����� �����.
        // ������ �̸� �ذ��ϱ� ���ؼ�
        // ��ü�� ������ �ʾƵ� �ش� ������ �����Ҽ� �ֵ���
        // static���� �Լ��� �����.
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // static ������ null�̶�� ����
            // �ش� �Լ��� ���°� ó���̶� ���̱� ������
            // static���� instance�� �ڱ� �ڽ��� �ִ´�.
            // �ٸ� ���������� �Ȱ� Ŭ������ü ���̱� ������
            // ����Ƽ ���ӿ�����Ʈ�� ���� �Ѿ�� �����ȴ�.
            // �׷��� Ŭ������ �Բ� ���ӿ�����Ʈ�� �������� �ʵ���
            // DonDestroyOnLoad�� �ش� Ŭ���� ��ü�� ������Ʈ�� ������
            // ���ӿ�����Ʈ�� �߰��Ͽ� ������ ���´�.
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
            // �̹� instance�� �����Ѵٸ�
            // �ߺ��� ��ü�� �������� �ʵ���
            // ������ ������Ʈ�� �����Ѵ�.
        }
    }

    public void Print()
    {
        print("i : " + i + "\n");
        print("i2 : " + i2 + "\n");
    }
}
