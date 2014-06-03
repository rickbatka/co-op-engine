using co_op_engine.Components.Particles.Decorators;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles.Emitters
{
    class HealBeam : Emitter
    {
        private GameObject source;
        private GameObject target;

        public HealBeam(GameObject source, GameObject target)
        {
            this.source = source;
            this.target = target;
            frequency = 50;
            duration = TimeSpan.FromMilliseconds(300);
        }

        protected override void EmitParticle()
        {
            var beam = new Decorators.FadeDecorator(50, 70,0f,
                new LineParticle() 
            {
                Lifetime = TimeSpan.FromMilliseconds(70),
                Width = 40,
                Texture = AssetRepository.Instance.FuzzyLazer,
                end = target.Position,
                start = source.Position + new Vector2(0,1),
            });

            var startCircle = new Decorators.FadeDecorator(100,300,0f, 
                new Decorators.InflationDecorator(20, 300, 40,
                    new Particle()
                    {
                        Lifetime = TimeSpan.FromMilliseconds(500),
                        Height = 20,
                        Width = 20,
                        Texture = AssetRepository.Instance.HealCircle,
                        Position = source.Position + new Vector2(0,2),
                    }));

            var endCircle = new Decorators.FadeDecorator(200, 500, 0f,
                new Decorators.InflationDecorator(20, 500, 30,
                    new Particle()
                    {
                        Lifetime = TimeSpan.FromMilliseconds(500),
                        Height = 20,
                        Width = 20,
                        Texture = AssetRepository.Instance.HealCircle,
                        Position = target.Position + new Vector2(0, 2),
                    }));

            ParticleEngine.Instance.Add(startCircle);
            ParticleEngine.Instance.Add(endCircle);
            ParticleEngine.Instance.Add(beam);
        }


        //ParticleEngine.Instance.AddEmitter(
        //         new BloodHitEmitter(Receiver, RotationAtTimeOfHit)
        //    );

        //ParticleEngine.Instance.Add(
        //new LineParticle()
        //{
        //    DrawColor = Color.White,
        //    Lifetime = TimeSpan.FromMilliseconds(200),
        //    Texture = AssetRepository.Instance.FuzzyLazer,
        //    width = 40,
        //    end = Owner.Position,
        //    start = collider.Position
        //});

    }
}
