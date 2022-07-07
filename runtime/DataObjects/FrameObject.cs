using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Packages.FxEditor
{
    public class FrameObject : DataObjectBase
    {
        private readonly List<CommandObjectBase> commandlist = new List<CommandObjectBase>();

        private float time = GlobalUtility.time;

        public FrameObject(Exporter exporter)
        {
            ObjectType = ObjectTypeFrame;
            //-------------------


            GameObject timelineRoot = null;
            if (exporter._Timeline != null) timelineRoot = exporter._Timeline.GetRootObjectByTime(time);

            // Debug.Log(timelineRoot);


            var objs = Object.FindObjectsOfType<GameObject>();
            if (objs == null) return;
            Array.Sort(objs, SortByPosition);


            //===================================================canvas
            var canvases = Object.FindObjectsOfType<FxCanvasObject>();
            Array.Sort(canvases, SortByNodeOrder);

            List<GameObject> canvasObjects = new List<GameObject>();

 
            //===================================================

            //1.get bounding box of camera
            Camera curCam = SceneConfig.currentCamera;
            Vector3 camPos = curCam.transform.position;
            float size = curCam.orthographicSize;
            float near = curCam.nearClipPlane;
            float far = curCam.farClipPlane;
            //float aspect = curCam.aspect;
            float fxCamAspect = 1.0f;

            if (curCam != null && curCam.GetComponent<FxCameraProperty>() != null)
            {
                fxCamAspect = (int)curCam.GetComponent<FxCameraProperty>().aspect / 10000.0f;
            }

            Vector3 center = new Vector3(camPos.x, camPos.y, (camPos.z + far - near) / 2.0f);
            Vector3 boundSize = new Vector3(fxCamAspect * size * 2.0f, size * 2.0f, far - near);

            Bounds curCamBounds = new Bounds(center, boundSize);

            //2.collecting meshRenderer
            List<GameObject> validObjs = new List<GameObject>();

            for (int idx = 0; idx < objs.Length; idx++)
            {
                var aMeshRenderer = objs[idx].GetComponent<MeshRenderer>();
                if (aMeshRenderer != null && aMeshRenderer.bounds.Intersects(curCamBounds))
                {
                    //this obj needs to be rendered
                    validObjs.Add(objs[idx]);
                }
            }


            Stack<FxCanvasObject> canvasObjStack = new Stack<FxCanvasObject>();
            Dictionary<FxCanvasObject, bool> canvasObjStatus = new Dictionary<FxCanvasObject, bool>();

            foreach (var c in canvases)
            {

                foreach(var obj in validObjs)
                {
                    FxCanvasSlot needCanvasSlot = obj.GetComponent<FxCanvasSlot>();
                    if (needCanvasSlot && needCanvasSlot.canvas == c)
                    {
                        canvasObjStack.Push(c);
                    }
                }
            }

            while(canvasObjStack.Count != 0)
            {
                FxCanvasObject canvasObj = canvasObjStack.Pop();
                canvasObjStatus[canvasObj] = true;
                if(canvasObj.root != null)
                {
                    Transform transform = canvasObj.root.GetComponent<Transform>();
                    if(transform != null)
                    {
                        int childCount = transform.GetChildCount();

                        for(int i = 0; i < childCount; i++)
                        {
                            Transform childTransform = transform.GetChild(i);
                            FxCanvasSlot slot = childTransform.gameObject.GetComponent<FxCanvasSlot>();
                            if (slot != null && slot.canvas != null)
                            {
                                canvasObjStack.Push(slot.canvas);
                            }
                        }
                    }
                }

                FxCanvasSlot needCanvasSlot = canvasObj.gameObject.GetComponent<FxCanvasSlot>();
                if (needCanvasSlot != null && needCanvasSlot.canvas != null)
                {
                    canvasObjStack.Push(needCanvasSlot.canvas);
                }
            }



                //3.
            if (true)
            {
                foreach (var c in canvases)
                {
                    if (!canvasObjStatus.ContainsKey(c) || canvasObjStatus[c] != true) continue;
                    
                    if (c.root == null) continue;

                    if (timelineRoot != null)
                    {
                        if (!c.root.transform.IsChildOf(timelineRoot.transform)) continue;
                    }

                    Camera cam = c.gameObject.GetComponent<Camera>();
                    commandlist.Add(new BeginCanvasCommand(c, exporter));


                    //Draw
                    foreach (var obj in objs)
                    {
                        //---------for timeline-------------------
                        if (exporter._Timeline != null)
                        {
                            if (timelineRoot == null) continue;
                            if (!obj.transform.IsChildOf(timelineRoot.transform)) continue;
                        }
                        //-----------------------------------------------------

                        if (!obj.transform.IsChildOf(c.root.transform)) continue;

                        canvasObjects.Add(obj);
                        DrawObject(cam, obj, exporter);
                    }

                    commandlist.Add(new EndCanvasCommand(c));
                }
            }
















            //===================================================Draw
            if (true){
                foreach (var obj in validObjs)
                {
                    //----------------skip content of canvas----------------
                    bool skip = (canvasObjects.IndexOf(obj) != -1);
                    // foreach (var c in canvases)
                    // {
                    //     if (c.root == null)
                    //     {
                    //         continue;
                    //     }
                    //
                    //     if (obj.gameObject.transform.IsChildOf(c.root.transform))
                    //     {
                    //         skip = true;
                    //         break;
                    //     }
                    //
                    //     if (skip) break;
                    // }
                    //--------------------------------


                    //---------for timeline-------------------
                    if (exporter._Timeline != null)
                    {
                        if (timelineRoot == null) continue;
                        if (!obj.transform.IsChildOf(timelineRoot.transform))
                        {
                            continue;
                        }
                    }
                    //-----------------------------------------------------

                    if (skip) continue;

                    //DrawObject(Camera.main,obj, exporter);
                    DrawObject(SceneConfig.currentCamera, obj, exporter);
                }
            }

            //===================================================
            //            Debug.Log("DrawCommands:"+commandlist.Count);

            //jay-----------------------------------------------------------
            Rect rect = SceneConfig.currentCamera.rect;

            //1.get camera bounding box
            Camera currentCamera = SceneConfig.currentCamera;
            Bounds cameraBounds = new Bounds();
            //2.get objects in or cross bounding box


            //3.get objects depending canvas


        }


        //-----------------------------------------
        void DrawObject(Camera cam, GameObject obj, Exporter exporter)
        {
            //
            if (obj.tag == "EditorOnly") return;

            //mesh
            {
                var meshrenderer = obj.GetComponent<MeshRenderer>();
                if (meshrenderer != null)
                    DrawMesh(cam, meshrenderer, exporter);
            }

            //particle system
            {
                var ps = obj.GetComponent<ParticleSystem>();
                if (ps != null)
                    DrawParticleSystem(cam, ps, exporter);
            }

            //textFx
            {
                var textFx = obj.GetComponent<TextFx>();
                if (textFx != null)
                {
                    DrawTextFx(cam, obj, exporter);
                }
            }
        }

        void DrawTextFx(Camera cam, GameObject obj, Exporter exporter)
        {
            //-------------------change shader command
            {
                commandlist.Add(new ChangeShaderCommand(cam, obj.gameObject, exporter));
            }
            commandlist.Add(new TextFxSlotCommand(cam, obj, exporter));
        }

        void DrawParticleSystem(Camera cam, ParticleSystem obj, Exporter exporter)
        {
            //-------------------change shader command
            {
                commandlist.Add(new ChangeShaderCommand(cam, obj.gameObject, exporter));
            }

            {
                commandlist.Add(new ParticleSystemCommand(cam, obj));
            }
        }

        void DrawMesh(Camera cam, MeshRenderer obj, Exporter exporter)
        {
            if (obj.gameObject.tag == "EditorOnly") return;
            if (obj.gameObject.GetComponent<TextFx>() != null) return;


            //-------------------change shader command
            {
                commandlist.Add(new ChangeShaderCommand(cam, obj.gameObject, exporter));
            }

            //-------------------set image slot command
            {
                var imageslots = obj.GetComponents<FxImageSlot>();
                foreach (var slot in imageslots)
                {
                    commandlist.Add(new ImageSlotCommand(slot));
                }
            }

            //-------------------set canvas slot command
            {
                //var canvasSlot = obj.GetComponent<FxCanvasSlot>();
                var canvasSlots = obj.GetComponents<FxCanvasSlot>();
                foreach (var slot in canvasSlots)
                {
                    if (slot.canvas == null)
                    {
                        Debug.LogError("有CanvasSlot使用了空的canvas：" + obj.name);
                        continue;
                    }

                    commandlist.Add(new CanvasSlotCommand(slot, exporter));
                }
            }

            //-------------------draw mesh command
            {
                //commandlist.Add(new DrawMeshCommand(Camera.main, obj.gameObject, exporter));
                commandlist.Add(new DrawMeshCommand(SceneConfig.currentCamera, obj.gameObject, exporter));
            }
        }


        static int SortByPosition(GameObject a, GameObject b)
        {
            var ta = a.transform.position;

            var tb = b.transform.position;

            if (ta.z > tb.z)
            {
                return -1;
            }
            else
            {
                return 1;
            }

            return 0;
        }

        static int SortByMaterial(GameObject a, GameObject b)
        {
            var ta = a.GetComponent<Renderer>().material.renderQueue;

            var tb = b.GetComponent<Renderer>().material.renderQueue;

            if (ta > tb)
            {
                return -1;
            }
            else
            {
                return 1;
            }

            return 0;
        }

        static int SortByNodeOrder(FxCanvasObject a, FxCanvasObject b)
        {
            var ta = a.nodeOrder;
            var tb = b.nodeOrder;
            if (ta > tb)
            {
                return -1;
            }
            else
            {
                return 1;
            }

            return 0;
        }

        public override void Write(Stream stream)
        {
            var count = commandlist.Count;
            Write(stream, time);
            Write(stream, count);
            for (var i = 0; i < count; i++)
            {
                var cmd = commandlist[i];
                cmd.WriteToStream(stream);
            }
        }
    }
}