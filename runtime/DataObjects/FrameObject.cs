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

        private float time = Time.time;

        public FrameObject(Exporter exporter)
        {
            ObjectType = ObjectTypeFrame;
            //-------------------

            
            
            GameObject timelineRoot = null;
            if(exporter._Timeline!=null)
            timelineRoot=exporter._Timeline.GetRootObjectByTime(time);
            
            // Debug.Log(timelineRoot);


            var objs = Object.FindObjectsOfType<GameObject>();
            if (objs == null) return;
            Array.Sort(objs, SortByPosition);


            //===================================================canvas

            var canvases = Object.FindObjectsOfType<FxCanvasObject>();
            Array.Sort(canvases, SortByNodeOrder);
            foreach (var c in canvases)
            {
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

                    DrawObject(cam, obj, exporter);


                }

                commandlist.Add(new EndCanvasCommand(c));
            }
            //===================================================


            //===================================================Draw
            foreach (var obj in objs)
            {
                //----------------skip content of canvas----------------
                bool skip = false;
                foreach (var c in canvases)
                {
                    if (c.root == null)
                    {
                        continue;
                    }

                    if (obj.gameObject.transform.IsChildOf(c.root.transform))
                    {
                        skip = true;
                        break;
                    }

                    if (skip) break;
                }
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

            //===================================================
//            Debug.Log("DrawCommands:"+commandlist.Count);
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
                ;
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
                commandlist.Add(new ParticleSystemCommand(cam,obj));
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