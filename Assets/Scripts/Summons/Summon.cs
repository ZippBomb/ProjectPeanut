using UnityEngine;

public abstract class Summon : MonoBehaviour {

    [SerializeField] private Mesh previewMesh;

    [SerializeField] private Vector3 previewOffset;
    [SerializeField] private Vector3 previewSize;
    [SerializeField] private Vector3 previewRotation;

    public virtual void OnSummoned() {

        //

    }

    public Mesh GetPreviewMesh() { return previewMesh; }

    public Vector3 GetPreviewOffset() { return previewOffset; }
    public Vector3 GetPreviewSize() { return previewSize; }
    public Vector3 GetPreviewRotation() { return previewRotation; }
    
}