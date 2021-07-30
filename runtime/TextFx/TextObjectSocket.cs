using System;
using UnityEditor;
using UnityEngine;

namespace Packages.FxEditor
{
    public enum TextSocketPostions
    {
        Top,
        Center,
        Bottom,
        Left,
        Right
    }

    public class TextObjectSocket : ScriptBase
    {
        public TextFx textObject = null;
        public TextSocketPostions socketPostion = TextSocketPostions.Center;
        public Vector2 sizeScale = Vector2.one;
        public float offset = 0.0f;

        public void Start()
        {
        }

        public void Update()
        {
        }

        public override void BeginExport()
        {
        }

        public override void UpdateAnimation()
        {
        }

        public override void EndExport()
        {
        }


        void UpdateSocket()
        {
            var ts = gameObject.transform;
            // var rot = ts.rotation;
            // ts.position = Vector3.zero;
            // ts.rotation=Quaternion.identity;
            //
            // ts.localScale = Vector3.one;
            //ts.rotation=Quaternion.identity;

            var textbounds = textObject.getBounds();


            switch (socketPostion)
            {
                case TextSocketPostions.Top:
                {
                    var pos = ts.localPosition;
                    var textPos = textbounds.center;
                    var size = textbounds.size;
                    textPos.y += size.y * 0.5f;

                    size.x *= sizeScale.x;
                    size.y *= sizeScale.y;


                    textPos.y += size.y * .5f;
                    textPos.y += offset;

                    ts.localPosition = new Vector3(textPos.x, textPos.y, pos.z);
                    ts.localScale = new Vector3(size.x, size.y, 1);
                }
                    break;
                case TextSocketPostions.Bottom:
                {
                    var pos = ts.localPosition;
                    var textPos = textbounds.center;
                    var size = textbounds.size;
                    textPos.y -= size.y * 0.5f;

                    size.x *= sizeScale.x;
                    size.y *= sizeScale.y;


                    textPos.y -= size.y * .5f;
                    textPos.y -= offset;


                    ts.localPosition = new Vector3(textPos.x, textPos.y, pos.z);
                    ts.localScale = new Vector3(size.x, size.y, 1);
                }
                    break;
                case TextSocketPostions.Center:
                {
                    var pos = ts.localPosition;
                    var textPos = textbounds.center;
                    var size = textbounds.size;
                    size.x *= sizeScale.x;
                    size.y *= sizeScale.y;

                    ts.localPosition = new Vector3(textPos.x, textPos.y, pos.z);
                    ts.localScale = new Vector3(size.x, size.y, 1);
                }
                    break;
                case TextSocketPostions.Left:
                {
                    var pos = ts.localPosition;
                    var textPos = textbounds.center;
                    var size = textbounds.size;

                    textPos.x -= size.x * 0.5f;
                    size.x = 1.0f;

                    size.x *= sizeScale.x;
                    size.y *= sizeScale.y;
                    textPos.x -= size.x * 0.5f;
                    textPos.x -= offset;

                    //textPos.x -= size.x*.5f;

                    ts.localPosition = new Vector3(textPos.x, textPos.y, pos.z);
                    ts.localScale = new Vector3(size.x, size.y, 1);
                }
                    break;
                case TextSocketPostions.Right:
                {
                    var pos = ts.localPosition;
                    var textPos = textbounds.center;
                    var size = textbounds.size;

                    textPos.x += size.x * 0.5f;
                    size.x = 1.0f;

                    size.x *= sizeScale.x;
                    size.y *= sizeScale.y;
                    textPos.x += size.x * 0.5f;
                    textPos.x += offset;


                    //textPos.x -= size.x*.5f;

                    ts.localPosition = new Vector3(textPos.x, textPos.y, pos.z);
                    ts.localScale = new Vector3(size.x, size.y, 1);
                }
                    break;
            }
        }

        [MenuItem("FxEditor/创建物体/文字装饰")]
        public static void OnCreateSocketObject()
        {
            if (Selection.activeObject == null)
            {
                EditorUtility.DisplayDialog("错误", "需要选择Text对象", "确定");
                return;
            }

            var gameObject = Selection.activeObject as GameObject;
            var textObject = gameObject.GetComponent<TextFx>();
            if (textObject == null)
            {
                EditorUtility.DisplayDialog("错误", "需要选择Text对象", "确定");
                return;
            }

            var obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            obj.name = "TextBorder";
            var material = new Material(Shader.Find("HLFx/TextureColorMask"));
            obj.GetComponent<Renderer>().material = material;
            var sk = obj.AddComponent<TextObjectSocket>();
            sk.textObject = textObject;
            obj.transform.parent = gameObject.transform;

        }

        private void OnDrawGizmos()
        {
            if (textObject == null) return;
            UpdateSocket();
        }
    }
}