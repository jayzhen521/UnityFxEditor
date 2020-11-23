using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Packages.FxEditor
{
    public class ObjectLayoutTools
    {
        //------------horizontal--------------
        [MenuItem("FxEditor/排列工具/上对齐")]
        public static void OnAlignTopHor()
        {
            bool first = true;
            var firstBound = GlobalUtility.GetSelectionBounds();

            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                
                {
                    var p = obj.transform.position;
                    var d = firstBound.max.y - bounds.max.y;
                    obj.transform.position = new Vector3(p.x,p.y+d,p.z);
                }
            }
        }
        [MenuItem("FxEditor/排列工具/水平居中")]
        public static void OnAlignCenterHor()
        {
            bool first = true;
            var firstBound = new Bounds(Vector3.zero, Vector3.one);
            
            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                if (first)
                {
                    first = false;
                    firstBound = bounds;
                }
                else
                {
                    var p = obj.transform.position;
                    obj.transform.position = new Vector3(p.x,firstBound.center.y,p.z);
                }
            }
        }
        [MenuItem("FxEditor/排列工具/下对齐")]
        public static void OnAlignBoundHor()
        {
            bool first = true;
            var firstBound = GlobalUtility.GetSelectionBounds();
            
            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                
                
                {
                    var p = obj.transform.position;
                    var d = firstBound.min.y - bounds.min.y;
                    obj.transform.position = new Vector3(p.x,p.y+d,p.z);
                }
            }
        }


   
        [MenuItem("FxEditor/排列工具/水平分布")]
        public static void OnHorizontalDistribute()
        {
            if (Selection.gameObjects.Length < 2) return;
            
            float r = 9999.0f;
            float minValue =r;
            float maxValue =-r;
            
            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                minValue = Mathf.Min(minValue, bounds.center.x);
                maxValue = Mathf.Max(maxValue, bounds.center.x);
            }
            Debug.Log("m:"+minValue+","+maxValue);
            List<Vector3> points=new List<Vector3>();
            float delta = (maxValue - minValue) / (Selection.gameObjects.Length-1);
            
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                var p = new Vector3(minValue + i * delta, 0, 0);
                points.Add(p);
            }

            GameObject[] objects=Selection.gameObjects.ToArray();
            
            Array.Sort(objects, delegate(GameObject A, GameObject B)
            {
                var b1 = A.transform.position;// GlobalUtility.GetGameObjectBounds(A);
                var b2 = B.transform.position;// GlobalUtility.GetGameObjectBounds(B);
                if (b1.x < b2.x)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
                return 0;
            });

            for (int i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                {
                    var p = obj.transform.position;
                    obj.transform.position = new Vector3(points[i].x,p.y,p.z);
                }
            }
        }
        
        
        //------------vertical--------------
        [MenuItem("FxEditor/排列工具/左对齐")]
        public static void OnAlignLeftVert()
        {
            bool first = true;
            var firstBound = GlobalUtility.GetSelectionBounds();

            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                
                {
                    var p = obj.transform.position;
                    var d = firstBound.min.x - bounds.min.x;
                    obj.transform.position = new Vector3(p.x+d,p.y,p.z);
                }
            }
        }
        [MenuItem("FxEditor/排列工具/垂直居中")]
        public static void OnAlignCenterVert()
        {
            bool first = true;
            var firstBound = new Bounds(Vector3.zero, Vector3.one);
            
            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                if (first)
                {
                    first = false;
                    firstBound = bounds;
                }
                else
                {
                    var p = obj.transform.position;
                    obj.transform.position = new Vector3(firstBound.center.x,p.y,p.z);
                }
            }
        }
        [MenuItem("FxEditor/排列工具/右对齐")]
        public static void OnAlignBoundVert()
        {
            bool first = true;
            var firstBound = GlobalUtility.GetSelectionBounds();
            
            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                
                
                {
                    var p = obj.transform.position;
                    var d = firstBound.max.x - bounds.max.x;
                    obj.transform.position = new Vector3(p.x+d,p.y,p.z);
                }
            }
        }
        
        [MenuItem("FxEditor/排列工具/垂直分布")]
        public static void OnVerticalDistribute()
        {
            if (Selection.gameObjects.Length < 2) return;
            
            float r = 9999.0f;
            float minValue =r;
            float maxValue =-r;
            
            foreach (var obj in Selection.gameObjects)
            {
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                minValue = Mathf.Min(minValue, bounds.center.y);
                maxValue = Mathf.Max(maxValue, bounds.center.y);
            }
            
            List<Vector3> points=new List<Vector3>();
            float delta = (maxValue - minValue) / (Selection.gameObjects.Length-1);
            
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                var p = new Vector3(0,minValue + i * delta,  0);
                points.Add(p);
            }

            GameObject[] objects=Selection.gameObjects.ToArray();
            
            Array.Sort(objects, delegate(GameObject A, GameObject B)
            {
                var b1 = A.transform.position;// GlobalUtility.GetGameObjectBounds(A);
                var b2 = B.transform.position;// GlobalUtility.GetGameObjectBounds(B);
                if (b1.y < b2.y)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
                return 0;
            });

            for (int i = 0; i < objects.Length; i++)
            {
                var obj = objects[i];
                var bounds = GlobalUtility.GetGameObjectBounds(obj);
                {
                    var p = obj.transform.position;
                    obj.transform.position = new Vector3(p.x,points[i].y,p.z);
                }
            }
        }
    }
}