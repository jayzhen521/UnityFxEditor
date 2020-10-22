using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class MeshObject : DataObjectBase
    {
        private Mesh _mesh;

        
        public const int Vertex = 0;
        public const int Normal = 1;
        public const int  Tangent= 2;
        public const int Color= 3;
        public const int UV0 = 4;
        public const int UV1 = 5;
        public const int UV2 = 6;
        public const int UV3 = 7;
        public const int UV4 = 8;
        public const int UV5 = 9;
        public const int UV6 = 10;
        public const int UV7 = 11;
        public const int Triangle = 12;
        public const int BindPose= 13;
        public const int BoneWeight= 14;
        
        
        int[] numberOfVertexAttributes=new int[32];
        
        public MeshObject(Mesh mesh)
        {
            
            ObjectType = ObjectTypeMesh;
            //-------------------
            
            _mesh = mesh;
            
            numberOfVertexAttributes[Vertex] = mesh.vertexCount;             //points v3;
            numberOfVertexAttributes[Normal] = mesh.normals.Length;          //normal v3
            numberOfVertexAttributes[Tangent] = mesh.tangents.Length;         //tangents
            numberOfVertexAttributes[Color] = mesh.colors.Length;           //tangents
            numberOfVertexAttributes[UV0] = mesh.uv.Length;               //uv0
            // numberOfVertexAttributes[UV1] =  mesh.uv2.Length;              //uv1
            // numberOfVertexAttributes[UV2] =  mesh.uv3.Length;              //uv2
            // numberOfVertexAttributes[UV3] =  mesh.uv4.Length;              //uv3
            // numberOfVertexAttributes[UV4] =  mesh.uv5.Length;              //uv4
            // numberOfVertexAttributes[UV5] =  mesh.uv6.Length;              //uv5
            // numberOfVertexAttributes[UV6] =  mesh.uv7.Length;              //uv6
            // numberOfVertexAttributes[UV7] =  mesh.uv8.Length;              //uv7
            numberOfVertexAttributes[Triangle] = mesh.triangles.Length;        //triangles
            numberOfVertexAttributes[BindPose] = mesh.bindposes.Length;        //bones
            numberOfVertexAttributes[BoneWeight] = mesh.boneWeights.Length;      //skin weights
        }

        public override void Write(Stream stream)
        {
            Write(stream,numberOfVertexAttributes);
            
            for (int i = 0; i < _mesh.vertexCount; i++)
            {
                Write(stream, _mesh.vertices[i]);
                if(_mesh.vertexCount==_mesh.normals.Length) Write(stream, _mesh.normals[i]);
                if(_mesh.vertexCount==_mesh.tangents.Length) Write(stream, _mesh.tangents[i]);
                if(_mesh.vertexCount==_mesh.colors.Length) Write(stream, _mesh.colors[i]);
                if(_mesh.vertexCount==_mesh.uv.Length) Write(stream, _mesh.uv[i]);
                // if(_mesh.vertexCount==_mesh.uv2.Length) Write(stream, _mesh.uv2[i]);
                // if(_mesh.vertexCount==_mesh.uv3.Length) Write(stream, _mesh.uv2[i]);
                // if(_mesh.vertexCount==_mesh.uv4.Length) Write(stream, _mesh.uv3[i]);
                // if(_mesh.vertexCount==_mesh.uv5.Length) Write(stream, _mesh.uv4[i]);
                // if(_mesh.vertexCount==_mesh.uv6.Length) Write(stream, _mesh.uv5[i]);
                // if(_mesh.vertexCount==_mesh.uv7.Length) Write(stream, _mesh.uv6[i]);
                // if(_mesh.vertexCount==_mesh.uv8.Length) Write(stream, _mesh.uv7[i]);
                // if(_mesh.vertexCount==_mesh.uv2.Length) Write(stream, _mesh.uv8[i]);
            }

            // Write(stream,_mesh.vertices);
            // Write(stream,_mesh.normals);
            // Write(stream,_mesh.tangents);
            // Write(stream,_mesh.colors);
            // Write(stream,_mesh.uv);
            // Write(stream,_mesh.uv2);
            // Write(stream,_mesh.uv3);
            // Write(stream,_mesh.uv4);
            // Write(stream,_mesh.uv5);
            // Write(stream,_mesh.uv6);
            // Write(stream,_mesh.uv7);
            // Write(stream,_mesh.uv8);
            Write(stream,_mesh.triangles);
            
            //poses
            Write(stream,_mesh.bindposes);
            for (int i = 0; i < _mesh.boneWeights.Length; i++)
            {
                var w = _mesh.boneWeights[i];
                Write(stream,w.boneIndex0);
                Write(stream,w.boneIndex1);
                Write(stream,w.boneIndex2);
                Write(stream,w.boneIndex3);
                Write(stream,w.weight0);
                Write(stream,w.weight1);
                Write(stream,w.weight2);
                Write(stream,w.weight3);
            }
            //-------------------
        }
    }
}