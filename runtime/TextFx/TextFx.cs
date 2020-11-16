using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public class TextAnimationNode
    {
        public float start=0.0f;
        public float end=0.0f;
        public GameObject obj = null;
        public Vector3 pos=new Vector3(0,0,0);
    }
    public class TextFx : MonoBehaviour
    {
        [Tooltip("编号范围在0-9999之间")]
        public int soltID = 0;
        public string text = "";
        public float size = 1.0f;
        public TextAlignment align = TextAlignment.Left;
        public Material material = null;

        
        [Header("Time")]
        public float effectDuration = 5;

        public float interval = 0.05f;

        [Tooltip("用于设置连个字符之间时间重叠率(0-2)")]
        public float overlayFactor = 0;
        
        [Header("Animation")]
        public AnimationClip clip = null;
        public TextAnimationType animationType = TextAnimationType.Sequence;
        //"Helvetica"
        private Font font = null;
        
        private string fontName="Helvetica";
        private string lastText = "";
        private float lastSize = 1.0f;
        private Vector3[] transforms = null;

        private List<TextAnimationNode> nodes=new List<TextAnimationNode>();
        
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
            var render = gameObject.GetComponent<MeshRenderer>();
            if (render == null)
            {
                render=gameObject.AddComponent<MeshRenderer>();
            }

            render.material = material;
            
            
            //update count
            var count = text.Length;
            if (gameObject.transform.childCount < count)
            {
                var c = count - gameObject.transform.childCount;
                for (var i = 0; i < c; i++)
                {
                    var obj = new GameObject("T");
                    obj.transform.parent = gameObject.transform;
                    Mesh mesh = new Mesh();
                    
                    Vector3[] points =new Vector3[4];
                    Vector2[] uv =new Vector2[4];

                    int[] triangles = {1, 3, 0, 1, 2, 3};
                    mesh.vertices = points;
                    mesh.uv = uv;
                    mesh.triangles = triangles;
                    
                    var meshfilter = obj.AddComponent<MeshFilter>();
                    meshfilter.mesh = mesh;
                    
                    MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
                    renderer.material = material;
                    
                }
                
            }
            else
            {
                var c =  gameObject.transform.childCount;
                for (var i = 0; i < c; i++)
                {
                    gameObject.transform.GetChild(i).gameObject.SetActive(false);
                }
            }



            int fontSize = 256;
            if (font == null)
            {
                
                font = Font.CreateDynamicFontFromOSFont(fontName, fontSize);
                
                material.mainTexture = font.material.mainTexture;    
            }
            
            font.RequestCharactersInTexture(text);
            float factor = 1.0f / fontSize;

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
                GameObject obj = gameObject.transform.GetChild(i).gameObject;
                
                
                obj.tag = "EditorOnly";
                obj.SetActive(true);
                
                CharacterInfo ch;
                font.GetCharacterInfo(text[i], out ch);

                
                
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


                var meshfilter = obj.GetComponent<MeshFilter>();
                var mesh = meshfilter.sharedMesh;
                
                
                int[] triangles = {1, 3, 0, 1, 2, 3};
                mesh.vertices = points;
                mesh.uv = uv;
                mesh.triangles = triangles;
                meshfilter.mesh = mesh;
                //meshfilter.mesh = mesh;
                
                
                obj.transform.localPosition = new Vector3(pos, 0, 0);
                obj.transform.parent = transform;
                
                pos += ch.advance * size*factor;
            }
            
            //-----------
            lastText = text;
            lastSize = size;
        }



        void ComputeAnimation()
        {
            //-----compute animation-----
            int index = 0;
            var n = text.Length;
            if (n == 0) return;
            float start = 0.0f;
            

            float l = effectDuration / (n - interval * (n - 1));
            float dl = l * (1 - interval);
            foreach (var node in nodes)
            {
                 node.start =start;
                 node.end = start+l;
                
                 start += dl;
            }
            //---------------
            
            float t = Time.time%effectDuration;
            if (t < 0.1)
            {
                foreach (var node in nodes)
                {
                    clip.SampleAnimation(node.obj,0.0f);
                }
            }

            float ad = clip.length;
            foreach (var node in nodes)
            {
                if (node.obj == null) return;
                if (t < node.start || t > node.end)
                {
                    //clip.SampleAnimation(node.obj,clip.length);
                    continue;
                    
                }
                else
                {
                    float dur = node.end - node.start;
                    float dt = t - node.start;
                    float dtt = dt*ad / dur;
                
                    clip.SampleAnimation(node.obj,dtt);    
                }

                
                var pos = node.obj.transform.localPosition;
                pos += node.pos;
                node.obj.transform.localPosition = pos;
                
            }
            return;
        }
        private void Start()
        {
            
            //transforms = null;
            //lastSize = -1;
            UpdateTextNodes();
            nodes.Clear();

            var pos = GlobalUtility.RandomSample(text.Length);
            
            var t = gameObject.transform;
            for (var i = 0; i < text.Length; i++)
            {
                var node = new TextAnimationNode();
                if (animationType == TextAnimationType.Randomize)
                {
                    node.obj = t.GetChild(pos[i]).gameObject;
                }
                else
                {
                    node.obj = t.GetChild(i).gameObject;    
                }
                
                node.pos = node.obj.transform.localPosition;
                nodes.Add(node);
                clip.SampleAnimation(node.obj,0.0f);
            }

        }

        private void Update()
        {
            
            //UpdateTextNodes();
            ComputeAnimation();
            
        }

        private void OnDrawGizmos()
        {
            UpdateTextNodes();
        }
    }
}