using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField]
    Canvas backCanvas;
    [SerializeField]
    GameObject floatingText;

    public GameObject hotKeyGuid;

    public GameObject hotKeyGuidTarget;

    public Text actionText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
                Destroy(gameObject);
        }

        hotKeyGuid.SetActive(false);
    }

    private void Update()
    {
        if (hotKeyGuid.activeSelf)
            hotKeyGuid.transform.position = Camera.main.WorldToScreenPoint(hotKeyGuidTarget.transform.position) + new Vector3(17f, 40f);
    }

    /// <summary>
    /// �������� �޾Ƽ� ũ��Ƽ���̸� ������, �ƴϸ� ���������� ���� �����ؼ� �ؽ�Ʈ�� �������.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="isCrit"></param>
    public void PrintDamageText(float damage, bool isCrit)
    {
        GameObject obj = Instantiate(floatingText, backCanvas.gameObject.transform);

        obj.GetComponent<FloatingTextCtrl>().SetDamageText(damage);

        if (isCrit)
            obj.GetComponent<FloatingTextCtrl>().SetTextColor(Color.red);
        else
            obj.GetComponent<FloatingTextCtrl>().SetTextColor(Color.black);
    }

    /// <summary>
    /// ���ڿ��� �޾Ƽ� ���� �ؽ�Ʈ�� �����ϰ�, �ش� ���ڸ� 1�ʰ� ��Ƽ�� ���״ٰ� ����.
    /// </summary>
    /// <param name="_string"></param>
    /// <returns></returns>
    public IEnumerator PrintActionText(string _string)
    {
        actionText.text = _string;

        if (!actionText.gameObject.activeSelf)
        {
            actionText.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);

            actionText.gameObject.SetActive(false);
            actionText.text = "";
        }
    }
}
