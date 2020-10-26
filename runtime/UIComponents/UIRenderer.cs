using System;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

namespace Packages.FxEditor
{
    public class UIRenderer
    {
        //-------------------------

        private Font font = null;
        //-------------------------
        public void DrawString(Vector3 pos, string text)
        {
            // if(font==null)font = Font.CreateDynamicFontFromOSFont("Helvetica", 16);
            // font.RequestCharactersInTexture(text);
            // font.material.SetVector("_MainTex_ST",new Vector4(0,0,0,0));
            //
            //
            //
            // Gizmos.DrawGUITexture(new Rect(pos.x, pos.y, 100, 100), font.material.mainTexture,font.material);
            // // var shader = font.material.shader;
            // // for (int i = 0; i < shader.GetPropertyCount(); i++)
            // // {
            // //     Debug.Log(shader.GetPropertyName(i));
            // //     
            // // }
        }

        Bounds GetCameraBounds(Camera camera)
        {
            Vector3 c = camera.transform.position;
            float h = camera.orthographicSize * 2;
            float w = h * camera.aspect;
            Vector3 s = new Vector3(w, h, 0.1f);
            return new Bounds(c,s);
        }
        public void DrawCameraBound(Camera camera,Color color)
        {
            if (camera == null) return;


            var bounds = GetCameraBounds(camera);

            Gizmos.color = color;
            //Gizmos.DrawWireCube(c, s);
            Gizmos.DrawWireCube(bounds.center,bounds.size);
            
        }
        private void DrawCanvasUI(FxCanvasObject gameObject)
        {
            Camera camera = gameObject.GetComponent<Camera>();
            DrawCameraBound(camera,gameObject.bounds_color);
        }

        
        
        public void DrawCanvasUIS()
        {
            DrawNodesUI();
            //----------------
            
            var objects = Object.FindObjectsOfType<FxCanvasObject>();
            foreach (var fxCanvasObject in objects)
            {
                DrawCanvasUI(fxCanvasObject);    
            }
            
            DrawCameraBound(Camera.main, Color.white);
        }

        private void DrawNodeLinkLine(FxCanvasSlot slot)
        {
            if (slot.canvas == null) return;

            var pointA = slot.gameObject.transform.position;
            var pointB = slot.canvas.gameObject.transform.position;
            var dir2 = (pointB - pointA).normalized;
            
            
            var pointC = (pointA + pointB) * 0.5f;
            var pointD =pointC-dir2*1.2f;
            //var dir = Vector3.Cross(pointA, pointB);
            pointA.z = 0;
            pointB.z = 0;
            
            var dir = pointB - pointA;
            dir = new Vector3(dir.y, -dir.x, 0);
            dir = dir.normalized;
            
            //var dir2 = (pointB - pointA).normalized;
            
            
            
            Gizmos.DrawLine(pointD,pointC+dir);
            Gizmos.DrawLine(pointD,pointC-dir);
            Gizmos.DrawLine(pointA,pointB);
        }
        public void DrawNodesUI()
        {
            var slots = Object.FindObjectsOfType<FxCanvasSlot>();
            foreach (var slot in slots)
            {
                DrawNodeLinkLine(slot);
            }
            
            //Gizmos.DrawLine(new Vector3(0,0,0),new Vector3(10,1,1) );
        }
    }
}