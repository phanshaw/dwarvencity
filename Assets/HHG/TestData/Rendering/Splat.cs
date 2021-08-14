using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Rendering
{
    public enum ESplatBlendMode
    {
        Min,
        Max,
        Add,
        Subtract
    }

    [ExecuteInEditMode]
    public class Splat : MonoBehaviour
    {
        private int _heightPropID = Shader.PropertyToID("_HeightMultiplier");
        private int _splatPropID = Shader.PropertyToID("_BaseMap");

        public ESplatBlendMode BlendMode = ESplatBlendMode.Max;
        public float HeightMultiplier = 1;
        public Texture2D SplatMap;
    
        private MaterialPropertyBlock _propBlock;
        private MeshRenderer _meshRenderer;
        
        private MeshFilter _meshFilter;
        private Mesh _mesh;
        private Color _color;

        private Vector3[] _offsets;
        private Matrix4x4[] _matrices;
        private int _instanceCount = 9;
        
        private void OnEnable()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter = GetComponent<MeshFilter>();
            _propBlock = new MaterialPropertyBlock();
            _color = new Color(Random.value, Random.value, Random.value);

            _matrices = new Matrix4x4[_instanceCount];
            _offsets = new Vector3[_instanceCount];
            
            _offsets[0] = Vector3.zero;
            _offsets[1] = Vector3.left;
            _offsets[2] = Vector3.right;
            _offsets[3] = Vector3.forward;
            _offsets[4] = -Vector3.forward;
            _offsets[5] = Vector3.forward + Vector3.left;
            _offsets[6] = Vector3.forward + Vector3.right;
            _offsets[7] = -Vector3.forward + Vector3.left;
            _offsets[8] = -Vector3.forward + Vector3.right;
            
            CreateQuad();
        }

        private void CreateQuad()
        {
            _mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(-1, 0, -1),
                new Vector3(1, 0, -1),
                new Vector3(-1, 0, 1),
                new Vector3(1, 0, 1)
            };
            _mesh.vertices = vertices;

            int[] tris = new int[6]
            {
                // lower left triangle
                0, 2, 1,
                // upper right triangle
                2, 3, 1
            };
            _mesh.triangles = tris;

            Vector3[] normals = new Vector3[4]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };
            _mesh.normals = normals;

            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            _mesh.uv = uv;
            _meshFilter.mesh = _mesh;
        }
        
        public void DrawSplat(Camera renderCamera)
        {
            _propBlock.SetTexture(_splatPropID, SplatMap);
            _propBlock.SetFloat("_HeightMultiplier", HeightMultiplier);
            _meshRenderer.SetPropertyBlock(_propBlock);
            
            for (var i = 0; i < _instanceCount; i++)
            {
                var position = gameObject.transform.position + (_offsets[i] * SplatRenderer.RenderAreaSquareSize);
                var mat = Matrix4x4.TRS(position, gameObject.transform.rotation, gameObject.transform.localScale);
                _matrices[i] = mat;
            }

            Graphics.DrawMeshInstanced(_mesh, 0, _meshRenderer.sharedMaterial, _matrices, 9, _propBlock,
                ShadowCastingMode.Off, false, LayerMask.NameToLayer("Splats"), null); 
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            var halfSize = _meshRenderer.bounds.size.x * 0.5f;

            var o = gameObject;
            Gizmos.DrawWireMesh(_meshFilter.sharedMesh, o.transform.position, o.transform.rotation, o.transform.localScale);
        }
    }
}
