using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshColliderCtrl : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    MeshCollider m_collider;

    float delay = 0.2f;

    private void Awake()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        m_collider = GetComponent<MeshCollider>();

        StartCoroutine(UpdateCollider());
    }

    IEnumerator UpdateCollider()
    {
        while (true)
        {
            Mesh colliderMesh = new Mesh();
            meshRenderer.BakeMesh(colliderMesh);
            m_collider.sharedMesh = null;
            m_collider.sharedMesh = colliderMesh;

            yield return new WaitForSeconds(delay);
        }
    }
}
