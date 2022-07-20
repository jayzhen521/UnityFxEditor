using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Packages.FxEditor
{
    public class FxCanvasObject : MonoBehaviour
    {
        public int width = 512;
        public int height = 512;

        private int oldWidth = 512;
        private int oldHeight = 512;

        public Color backgroundColor = Color.black;

        public bool show_bounds = true;
        public Color bounds_color = Color.yellow;

        public GameObject root = null;


        public int nodeOrder = 0;
        private RenderTexture canvasTexture = null;


        private GameObject titleObject = null;

        public enum Aspect
        {
            _16_9,
            _4_3,
            _1_1,
            _9_16,
            _3_4,
            _free
        };

        public Aspect aspectItem = Aspect._1_1;
        private Aspect oldAspectItem = Aspect._1_1;
        private float aspect = 1.0f;

        private void UpdateAspect()
        {
            switch (aspectItem)
            {
                case Aspect._16_9:
                    {
                        aspect = 16.0f / 9.0f;
                        break;
                    }
                case Aspect._4_3:
                    {
                        aspect = 4.0f / 3.0f;
                        break;
                    }
                case Aspect._1_1:
                    {
                        aspect = 1.0f;
                        break;
                    }
                case Aspect._3_4:
                    {
                        aspect = 3.0f / 4.0f;
                        break;
                    }
                case Aspect._9_16:
                    {
                        aspect = 9.0f / 16.0f;
                        break;
                    }
                case Aspect._free:
                    {
                        aspect = 0.0f;
                        break;
                    }
                default:
                    {
                        aspect = 1.0f;
                        break;
                    }
            }
        }

        private void UpdateCanvas()
        {

            UpdateAspect();


            Camera camera = gameObject.GetComponent<Camera>();
            camera.backgroundColor = backgroundColor;
            camera.clearFlags = CameraClearFlags.Color;

            if (canvasTexture == null)
            {
                canvasTexture = new RenderTexture((int) width, (int) height, 16, RenderTextureFormat.Default);
                canvasTexture.wrapMode = TextureWrapMode.Clamp;
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


        private void OnValidate()
        {
            UpdateAspect();

            if(aspectItem == Aspect._free)
            {
                return;
            }

            if (oldAspectItem != aspectItem || oldWidth != width)
            {
                height = (int)Mathf.Ceil(width / aspect);
                oldHeight = height;
                oldWidth = width;
                oldAspectItem = aspectItem;
            }
            else if(oldHeight != height)
            {
                width = (int)Mathf.Ceil(height * aspect);
                oldHeight = height;
                oldWidth = width;
            }
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
                var obj = new GameObject("title");
                obj.transform.parent = gameObject.transform;

                textMesh = obj.AddComponent<TextMesh>();
                titleObject = obj;
                titleObject.tag = "EditorOnly";

                var offset = new Vector3(0, 6, 0);
                titleObject.transform.position = gameObject.transform.position + offset;
            }


            textMesh.text = name + ":" + nodeOrder;
            textMesh.color = bounds_color;
            textMesh.fontSize = 32;
            textMesh.characterSize = 0.25f;
        }

        private void OnDrawGizmos()
        {
            
            UpdateTitleObject();
            var config = Object.FindObjectOfType<SceneConfig>();
            if (config == null) return;
            if (config.autoRefreshNode)
            {
                UpdateCanvas();
            }
        }
    }
}
