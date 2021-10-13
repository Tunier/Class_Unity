using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemText_ScrollView_Ctrl : MonoSingletone<SystemText_ScrollView_Ctrl>
{
    public GameObject go_Text;
    public GameObject go_contents;

    public List<GameObject> TextList = new List<GameObject>();

    public void PrintText(string _text)
    {
        var obj = Instantiate(go_Text);
        obj.transform.SetParent(go_contents.transform);
        obj.transform.SetAsFirstSibling();
        TextList.Insert(0, obj);

        var text = obj.GetComponent<Text>();
        text.text = _text;

        if (TextList.Count > 20)
        {
            Destroy(TextList[19].gameObject);
            TextList.RemoveAt(19);
        }
    }
}