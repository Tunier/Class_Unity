using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEffect : MonoBehaviour
{
    public Canvas clickEffectCanvas;
    [SerializeField]
    GameObject cameraArm;

    private void Start()
    {
        clickEffectCanvas.enabled = false;
    }

    void Update()
    {
        if (clickEffectCanvas.enabled)
        {
            clickEffectCanvas.transform.forward = cameraArm.transform.forward;
            transform.Translate(new Vector3(0, Mathf.Sin(Time.time * 10)) * Time.deltaTime);
        }
    }

    public IEnumerator ClickEffectCtrl(Vector3 _pos)
    {
        clickEffectCanvas.transform.position = _pos;

        clickEffectCanvas.enabled = true;

        yield return new WaitForSeconds(1f);

        clickEffectCanvas.enabled = false;
    }
}
