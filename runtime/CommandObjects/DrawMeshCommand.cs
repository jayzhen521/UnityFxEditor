using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Packages.FxEditor
{
    public class DrawMeshCommand : CommandObjectBase
    {
        private MeshObject _meshObject;
        

        public DrawMeshCommand(Camera cam, GameObject gameObject, Exporter exporter)
        {
             ObjectType = CommandTypeDrawMesh;
            //-------------------------------------------------
            
             var meshfilter = gameObject.GetComponent<MeshFilter>();
             var mesh = meshfilter.sharedMesh;
             _meshObject = exporter.GetObject(mesh) as MeshObject;
             // //------------------
            
        }

        protected override void Write(Stream stream)
        {
            Write(stream,_meshObject.ObjectID);
        }
    }
}