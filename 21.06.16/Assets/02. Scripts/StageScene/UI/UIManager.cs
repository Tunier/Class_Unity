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
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void PrintDamageText(float damage, bool isCrit)
    {
        GameObject obj = Instantiate(floatingText, backCanvas.gameObject.transform);

        obj.GetComponent<FloatingTextCtrl>().SetDamageText(damage);
        
        if (isCrit)
            obj.GetComponent<FloatingTextCtrl>().SetTextColor(Color.red);
        else
            obj.GetComponent<FloatingTextCtrl>().SetTextColor(Color.black);
    }

    public IEnumerator PrintActionText(string _string)
    {
        actionText.text = _string;
        actionText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        actionText.gameObject.SetActive(false);
        actionText.text = "";
    }
}
