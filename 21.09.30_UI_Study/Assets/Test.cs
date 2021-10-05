using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject go_base;
    public Image Background;

    public bool rotateMark = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v3Original = go_base.transform.localPosition; // ������ ��
        Vector3 v3Distance = new Vector3(0f, 0f, -173);
        Quaternion qRotation = Quaternion.Euler(0f, -360 * Background.fillAmount, 0f); // ����
        Vector3 v3Temp = qRotation * v3Distance;
        Vector3 v3Dest = v3Original + v3Temp; // ������ ������ �Ÿ� 10 ���� 45�� ȸ���� ��ġ�� �̵��� ��. 
        transform.localPosition = v3Dest;

        if (rotateMark)
            transform.eulerAngles = new Vector3(0, 0, -360 * Background.fillAmount);
        else
            transform.eulerAngles = Vector3.zero;
    }
}
