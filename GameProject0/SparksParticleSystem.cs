using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;

namespace GameProject0
{
    public class SparksParticleSystem : ParticleSystem
    {
        public SparksParticleSystem(Game game) : base(game, 2000)
        {

        }

        protected override void InitializeConstants()
        {
            textureFilename = "spark";
            minNumParticles = 3;
            maxNumParticles = 8;

            blendState = BlendState.AlphaBlend;
            DrawOrder = AdditiveBlendDrawOrder;
        }

        protected override void InitializeParticle(ref Particle p, Vector2 where)
        {
            p.Initialize(where + new Vector2(RandomHelper.NextFloat(0f, 15f), RandomHelper.NextFloat(0f, 15f)),
                RandomHelper.RandomPostiveVector(20, 40),
                RandomHelper.RandomPostiveVector(20, 40),
                scale: RandomHelper.NextFloat(0.01f, .1f),
                lifetime: RandomHelper.NextFloat(.1f, 1f),
                rotation: RandomHelper.NextFloat(0,MathHelper.TwoPi),
                angularAcceleration: RandomHelper.NextFloat(0,10),
                angularVelocity: RandomHelper.NextFloat(0,10));
        }

        protected override void UpdateParticle(ref Particle particle, float dt)
        {
            base.UpdateParticle(ref particle, dt);

            float normalizedLiftime = particle.TimeSinceStart / particle.Lifetime;

            float alpha = 4 * normalizedLiftime * (1 - normalizedLiftime);
            particle.Color = Color.White * alpha;

            particle.Scale = .75f + .25f * normalizedLiftime;
        }

        public void PlaceSparks(Vector2 where) => AddParticles(where);
    }
}
