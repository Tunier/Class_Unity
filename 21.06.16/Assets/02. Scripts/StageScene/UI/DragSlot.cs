using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public Slot dragSlot;
    public SkillSlot dragSkillSlot;
    public ShopSlot shopSlot;

    [SerializeField]
    private Image image;

    void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image _Image)
    {
        image.sprite = _Image.sprite;
        SetColorAlpha(1);
    }

    public void SetColorAlpha(float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
