using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoBehaviour
{
    public GameObject sword;
    public GameObject cameraArm;
    public Light dir_Light;

    public GameObject ligtningEffect;
    public GameObject sakuraEffect;

    public ParticleSystem sakuraEffectParticle;
    ParticleSystem.ShapeModule sakuraShape;

    private void Start()
    {
        sword.SetActive(false);
        ligtningEffect.SetActive(false);
        sakuraEffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            ligtningEffect.SetActive(true);
            sakuraEffect.SetActive(true);

            StartCoroutine(SakuraShapeRaidusCtrl());
            StartCoroutine(CameraShake());
            StartCoroutine(LightCtrl());
        }
    }

    private void Update()
    {
        if (sword.transform.position.y <= 0.7f)
            ligtningEffect.SetActive(false);

        sakuraShape = sakuraEffectParticle.shape;
    }

    IEnumerator SakuraShapeRaidusCtrl()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            if (sakuraShape.radius <= 0.3f)
                sakuraShape.radius += 0.001f;
            else if (sakuraShape.radius <= 2.2f)
                sakuraShape.radius += 0.005f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator CameraShake()
    {
        for (int i = 0; i < 100; i++)
        {
            float r_x = Random.Range(-0.2f, 0.2f);
            float r_y = Random.Range(-0.4f, 0.4f);
            float r_z = Random.Range(-0.2f, 0.2f);

            cameraArm.transform.rotation = Quaternion.Euler(r_x, r_y, r_z);

            yield return new WaitForSeconds(0.05f);
        }

        cameraArm.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator LightCtrl()
    {
        while (dir_Light.intensity != 0)
        {
            dir_Light.intensity -= 0.008f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void StartSkill()
    {
        StopAllCoroutines();
        ligtningEffect.SetActive(false);
        sakuraEffect.SetActive(false);
        dir_Light.intensity = 1;

        sword.transform.position = new Vector3(0, 10, 0);
        sword.SetActive(true);
    }
}