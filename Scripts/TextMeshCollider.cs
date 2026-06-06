using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(MeshCollider))]
public class TextMeshCollider : MonoBehaviour
{
    void Start()
    {
        UpdateCollider();
    }

    public void UpdateCollider()
    {
        TMP_Text text =
            GetComponent<TMP_Text>();

        MeshCollider meshCollider =
            GetComponent<MeshCollider>();

        text.ForceMeshUpdate();

        meshCollider.sharedMesh = null;

        meshCollider.sharedMesh =
            text.mesh;
    }
}
