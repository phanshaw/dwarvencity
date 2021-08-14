using Game.Rendering;
using UnityEngine;

[ExecuteInEditMode]
public class SplatRenderer : MonoBehaviour
{
    public static int RenderAreaSquareSize = 40;

    [SerializeField] 
    private RenderTexture _targetTexture;
    
    [SerializeField]
    private Camera _splatCamera;

    public Camera SplatCamera => _splatCamera;

    // Update is called once per frame
    void Update()
    {
        _splatCamera.transform.localPosition = Vector3.zero + new Vector3(0, 5, 0);
        _splatCamera.orthographicSize = RenderAreaSquareSize * 0.5f;
        _splatCamera.targetTexture = _targetTexture;
        
        _splatCamera.forceIntoRenderTexture = true;

        var splats = GetComponentsInChildren<Splat>();
        foreach (var splat in splats)
        {
            splat.DrawSplat(_splatCamera);
        }
    }
    
    // Draw a helpful box to show where your splats are relative to the square. 
    private void OnDrawGizmos()
    {
        // Draw bounds
        Gizmos.color = Color.red;

        var pos = gameObject.transform.position;
        var halfSquareSize = RenderAreaSquareSize * 0.5f;
        var p0 = pos + new Vector3(-halfSquareSize, 0, halfSquareSize); // Top left
        var p1 = pos + new Vector3(halfSquareSize, 0, halfSquareSize); // Top right
        var p2 = pos + new Vector3(halfSquareSize, 0, -halfSquareSize); // Bottom right
        var p3 = pos + new Vector3(-halfSquareSize, 0, -halfSquareSize); // Bottom left
        
        Gizmos.DrawLine(p0, p1);
        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p0);
        
        // Draw hex bounds...
        var hexP0 = pos + new Vector3(-halfSquareSize, 0, halfSquareSize); // Top left
        var hexP1 = pos + new Vector3(halfSquareSize, 0, halfSquareSize); // Top right
        var hexP2 = pos + new Vector3(halfSquareSize, 0, -halfSquareSize); // Bottom right
        var hexP3 = pos + new Vector3(-halfSquareSize, 0, -halfSquareSize); // Bottom left
        var hexP4 = pos + new Vector3(-halfSquareSize, 0, halfSquareSize); // Top left
        var hexP5 = pos + new Vector3(halfSquareSize, 0, halfSquareSize); // Top right
    }
}
