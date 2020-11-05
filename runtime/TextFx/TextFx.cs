using System;
using UnityEngine;

namespace Packages.FxEditor
{
    public class TextFx : MonoBehaviour
    {
        public int soltID = 0;
        public string text = "";
        public float size = 1.0f;
        public TextAlignment align = TextAlignment.Left;

        //"Helvetica"
        private Font font = null;
        private string lastText = "";
        

        void RebuildMesh()
        {
            Mesh mesh = null;
            // Generate a mesh for the characters we want to print.
            var vertices = new Vector3[text.Length * 4];
            var triangles = new int[text.Length * 6];
            var uv = new Vector2[text.Length * 4];
            Vector3 pos = Vector3.zero;
            for (int i = 0; i < text.Length; i++)
            {
                // Get character rendering information from the font
                CharacterInfo ch;
                font.GetCharacterInfo(text[i], out ch);

                vertices[4 * i + 0] = pos + new Vector3(ch.minX, ch.maxY, 0);
                vertices[4 * i + 1] = pos + new Vector3(ch.maxX, ch.maxY, 0);
                vertices[4 * i + 2] = pos + new Vector3(ch.maxX, ch.minY, 0);
                vertices[4 * i + 3] = pos + new Vector3(ch.minX, ch.minY, 0);

                uv[4 * i + 0] = ch.uvTopLeft;
                uv[4 * i + 1] = ch.uvTopRight;
                uv[4 * i + 2] = ch.uvBottomRight;
                uv[4 * i + 3] = ch.uvBottomLeft;

                triangles[6 * i + 0] = 4 * i + 0;
                triangles[6 * i + 1] = 4 * i + 1;
                triangles[6 * i + 2] = 4 * i + 2;

                triangles[6 * i + 3] = 4 * i + 0;
                triangles[6 * i + 4] = 4 * i + 2;
                triangles[6 * i + 5] = 4 * i + 3;

                // Advance character position
                pos += new Vector3(ch.advance, 0, 0);
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uv;
        }

        void UpdateTextNodes()
        {
            if (lastText == text) return;

            int fontSize = 128;
            float factor = 1.0f / fontSize;
            font = Font.CreateDynamicFontFromOSFont("Helvetica", fontSize);
            font.RequestCharactersInTexture(text);
            lastText = text;

            for (var j = 0; j < 10; j++)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform t = transform.GetChild(i);
                    t.parent = null;
                    UnityEngine.Object.DestroyImmediate(t.gameObject);
                }
            }

            //-------------
            float textWidth = 0.0f;
            for (int i = 0; i < text.Length; i++)
            {
                CharacterInfo ch;
                font.GetCharacterInfo(text[i], out ch);
                textWidth += ch.advance * size*factor;
            }
            //-------------------------------

            float pos = 0.0f;
            switch (align)
            {
                case TextAlignment.Left:
                    pos = 0.0f;
                    break;
                case TextAlignment.Center:
                    pos = -textWidth*0.5f;
                    break;
                case TextAlignment.Right:
                    pos = -textWidth;
                    break;
                
            }
            //float s = 0.01f;
            for (int i = 0; i < text.Length; i++)
            {
                GameObject obj = new GameObject("T");
                obj.tag = "EditorOnly";
                
                CharacterInfo ch;
                font.GetCharacterInfo(text[i], out ch);

                
                Mesh mesh = new Mesh();
                float r = 1.0f;
                Vector3[] points =
                {
                    (new Vector3(ch.minX, ch.maxY, 0)) * size*factor,
                    (new Vector3(ch.maxX, ch.maxY, 0)) * size*factor,
                    (new Vector3(ch.maxX, ch.minY, 0)) * size*factor,
                    (new Vector3(ch.minX, ch.minY, 0)) * size*factor
                };


                Vector2[] uv =
                {
                    ch.uvTopLeft,
                    ch.uvTopRight,
                    ch.uvBottomRight,
                    ch.uvBottomLeft
                };


                // Vector2[] uv =
                // {
                //     ch.uvTopLeft,
                //     ch.uvTopRight,
                //     ch.uvBottomRight,
                //     ch.uvBottomLeft
                // };


                int[] triangles = {1, 3, 0, 1, 2, 3};
                mesh.vertices = points;
                mesh.uv = uv;
                mesh.triangles = triangles;


                var meshfilter = obj.AddComponent<MeshFilter>();
                meshfilter.mesh = mesh;

                MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
                renderer.material = font.material;
                obj.transform.position = new Vector3(pos, 0, 0);
                obj.transform.parent = transform;
                
                pos += ch.advance * size*factor;
            }
        }

        private void Start()
        {
            
        }

        private void Update()
        {
        }

        private void OnDrawGizmos()
        {
            UpdateTextNodes();
        }
    }
}