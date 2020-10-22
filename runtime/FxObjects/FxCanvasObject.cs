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

        private void OnDrawGizmos()
        {
            var ob = Object.FindObjectOfType<SceneConfig>();
            if (ob == null || !ob.showCanvasUI) return;
            
            if (Application.isPlaying) return;
             UpdateCanvas();
            // if (show_bounds) DrawBounds();
        }
    }
}