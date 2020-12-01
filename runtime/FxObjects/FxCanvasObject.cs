using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Packages.FxEditor
{
    
    public class FxCanvasObject : MonoBehaviour
    {
        public int width = 512;
        public int height = 512;
        public Color backgroundColor=Color.black;

        public bool show_bounds = true;
        public Color bounds_color = Color.yellow;
        
        public GameObject root = null;


        public int nodeOrder = 0;
        private RenderTexture canvasTexture = null;


        private GameObject titleObject = null;


        private void UpdateCanvas()
        {
            Camera camera = gameObject.GetComponent<Camera>();
            camera.backgroundColor = backgroundColor;
            camera.clearFlags = CameraClearFlags.Color;
            
            if (canvasTexture == null)
            {
                canvasTexture = new RenderTexture((int) width, (int) height, 16, RenderTextureFormat.Default);
                canvasTexture.wrapMode = TextureWrapMode.Repeat;
                canvasTexture.depth = 0;
            }

            if (canvasTexture.width != width || canvasTexture.height != height)
            {
                canvasTexture = new RenderTexture((int) width, (int) height, 16, RenderTextureFormat.Default);
                canvasTexture.wrapMode = TextureWrapMode.Clamp;
                canvasTexture.depth = 0;
                Debug.Log("update canvas!");
            }

            // canvasTexture.width = width;
            // canvasTexture.height = height;

            camera.aspect = (float) width / height;

            camera.targetTexture = canvasTexture;
            var old = RenderTexture.active;
            RenderTexture.active = camera.targetTexture;
            camera.Render();
            RenderTexture.active = old;
        }

        public RenderTexture GetRenderTexture()
        {
            return canvasTexture;
        }

        private void Update()
        {
            UpdateCanvas();
        }

        void DrawBounds()
        {
            Camera camera = gameObject.GetComponent<Camera>();

            Vector3 c = camera.transform.position;
            float h = camera.orthographicSize * 2;
            float w = h * camera.aspect;
            Vector3 s = new Vector3(w, h, 0.1f);

            Gizmos.color = bounds_color;
            Gizmos.DrawWireCube(c, s);
            
        }

        Bounds CameraBound(Camera camera)
        {
            Vector3 c = camera.transform.position;
            float h = camera.orthographicSize * 2;
            float w = h * camera.aspect;
            Vector3 s = new Vector3(w, h, 0.1f);
            
            return new Bounds(c, s);
        }
        
        
        void UpdateTitleObject()
        {
            
            
            var textMesh = gameObject.GetComponentInChildren<TextMesh>();
            if (textMesh == null)
            {
                var obj=new GameObject("title");
                obj.transform.parent = gameObject.transform;
                
                 textMesh=obj.AddComponent<TextMesh>();
                 titleObject = obj;
                 titleObject.tag = "EditorOnly";
                 
                 var offset=new Vector3(0,6,0);
                 titleObject.transform.position = gameObject.transform.position+offset;
            }
            
            
            textMesh.text = name+":"+nodeOrder;
            textMesh.color = bounds_color;
            textMesh.fontSize = 32;
            textMesh.characterSize = 0.25f;

            
            
        }
        private void OnDrawGizmos()
        {

            var config = Object.FindObjectOfType<SceneConfig>();
            if (config == null) return;
            
            UpdateTitleObject();
            if (config == null || !config.showCanvasUI) return;
            
            if (Application.isPlaying&&config.autoRefreshNode==false) return;
             UpdateCanvas();
        }
    }
}