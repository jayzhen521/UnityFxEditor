using System;
using System.IO;
using UnityEngine;

namespace Packages.FxEditor
{
    public class ParticleSystemCommand : CommandObjectBase
    {
        //--------------------------
        public const int ParticleRenderModeNone = 0;
        public const int ParticleRenderModeBillboard = 1;
        public const int ParticleRenderModeStretch = 2;
        public const int ParticleRenderModeHorizontalBillboard = 2;
        public const int ParticleRenderModeVerticalBillboard = 3;

        public const int ParticleRenderModeMesh = 4;

        //--------------------------
        public const int SimulationSpaceLocal = 0;
        public const int SimulationSpaceWorld = 1;


        //---------------------------
        private Matrix4x4 iViewMatrix = new Matrix4x4();
        private Matrix4x4 matirxObjectToWorld = new Matrix4x4();

        private int renderMode = ParticleRenderModeNone;

        private int simulationSpace = SimulationSpaceLocal;
        //---------------------------


        private ParticleSystem.Particle[] _particles = null;

        private int _particleCount = 0;
        private float[] _particlesSize = null;
        private Color[] _particlesColor = null;


        public ParticleSystemCommand(Camera cam, ParticleSystem ps)
        {
            ObjectType = CommandTypeParticleSystem;
            //-------------------------------------------------

            //render mode
            {
                var render = ps.GetComponent<ParticleSystemRenderer>();
                switch (render.renderMode)
                {
                    case ParticleSystemRenderMode.Billboard:
                        renderMode = ParticleRenderModeBillboard;
                        break;
                    case ParticleSystemRenderMode.Stretch:
                        renderMode = ParticleRenderModeStretch;
                        break;
                    case ParticleSystemRenderMode.HorizontalBillboard:
                        renderMode = ParticleRenderModeHorizontalBillboard;
                        break;
                    case ParticleSystemRenderMode.VerticalBillboard:
                        renderMode = ParticleRenderModeVerticalBillboard;
                        break;
                    case ParticleSystemRenderMode.Mesh:
                        renderMode = ParticleRenderModeMesh;
                        break;
                    case ParticleSystemRenderMode.None:
                        renderMode = ParticleRenderModeNone;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            //simulation space
            {
                switch (ps.simulationSpace)
                {
                    case ParticleSystemSimulationSpace.Local:
                        simulationSpace = SimulationSpaceLocal;
                        break;
                    case ParticleSystemSimulationSpace.World:
                        simulationSpace = SimulationSpaceWorld;
                        break;
                }
            }
            //------------------------------------------------
            matirxObjectToWorld = ps.gameObject.transform.localToWorldMatrix;
            iViewMatrix = cam.worldToCameraMatrix;

            int count = ps.particleCount;
            _particleCount = count;


            _particles = new ParticleSystem.Particle[count];
            _particlesColor = new Color[count];
            _particlesSize = new float[count];
            ps.GetParticles(_particles);

            int i = 0;
            foreach (var p in _particles)
            {
                _particlesSize[i] = p.GetCurrentSize(ps);
                _particlesColor[i] = p.GetCurrentColor(ps);

                i++;
            }
        }

        protected override void Write(Stream stream)
        {
            Write(stream, matirxObjectToWorld);
            Write(stream, simulationSpace);
            Write(stream, renderMode);
            Write(stream, _particleCount);
            int i = 0;
            foreach (var p in _particles)
            {
                Write(stream, p.position);

                Write(stream, p.rotation * Mathf.PI / 180.0f);
                Write(stream, _particlesSize[i]);
                Write(stream, _particlesColor[i]);
                i++;
            }
        }
    }
}