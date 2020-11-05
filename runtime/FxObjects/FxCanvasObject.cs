using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Packages.FxEditor
{
    
    public class FxCanvasObject : MonoBehaviour
    {
        public int width = 128;
        public int height = 128;
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
                canvasTexture.wrapMode = TextureWrapMode.Repeat;
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
            string name = gameObject.name;
            
            if (titleObject == null)
            {
                var obj=new GameObject("title");
                obj.transform.parent = gameObject.transform;
                
                 obj.AddComponent<TextMesh>();
                 titleObject = obj;
                 titleObject.tag = "EditorOnly";
            }
            var textMesh = titleObject.GetComponentInChildren<TextMesh>();
            
            
            textMesh.text = name;
            textMesh.color = bounds_color;
            textMesh.fontSize = 32;
            textMesh.characterSize = 0.25f;

            
            
        }
        private void OnDrawGizmos()
        {
            
            UpdateTitleObject();
            var ob = Object.FindObjectOfType<SceneConfig>();
            if (ob == null || !ob.showCanvasUI) return;
            
            if (Application.isPlaying) return;
             UpdateCanvas();
            
        }
    }
}