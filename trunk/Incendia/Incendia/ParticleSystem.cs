using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incendia
{
    class ParticleSystem
    {
        private List<Particle> particles = new List<Particle>();

        public List<IParticleManipulator> Manipulators { get; set; }

        public Texture2D Texture { get; set; }

        public Vector2 EmitterLocation { get; set; }
        public float MinDirection { get; set; }
        public float MaxDirection { get; set; }

        public float MinLifetime { get; set; }
        public float MaxLifetime { get; set; }
        public float EmissionRate { get; set; }

        public Curve Speed { get; set; }
        public float SpeedDeviation { get; set; }

        public Curve Rotation { get; set; }
        public float RotationDeviation { get; set; }

        public Curve Scale { get; set; }
        public float ScaleDeviation { get; set; }
        
        public Curve ColorR { get; set; }
        public Curve ColorG { get; set; }
        public Curve ColorB { get; set; }
        public Curve ColorA { get; set; }

        private Random random = new Random();
        private float emissionError;

        public ParticleSystem(Texture2D texture,
            Vector2 emitterLocation, float minDirection, float maxDirection,
            float minLifetime, float maxLifetime, float emissionRate,
            Curve speed, float speedDeviation,
            Curve rotation, float rotationDeviation,
            Curve scale, float scaleDeviation,
            Curve colorR, Curve colorG, Curve colorB, Curve colorA)
        {
            Manipulators = new List<IParticleManipulator>();

            Texture = texture;

            EmitterLocation = emitterLocation;
            MinDirection = minDirection;
            MaxDirection = maxDirection;

            MinLifetime = minLifetime;
            MaxLifetime = maxLifetime;
            EmissionRate = emissionRate;

            Speed = speed;
            SpeedDeviation = speedDeviation;

            Rotation = rotation;
            RotationDeviation = rotationDeviation;

            Scale = scale;
            ScaleDeviation = scaleDeviation;
        
            ColorR = colorR;
            ColorG = colorG;
            ColorB = colorB;
            ColorA = colorA;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Particle particle in particles)
            {
                particle.Update(gameTime);
                float age = particle.Age / particle.Lifetime;

                Vector2 direction = particle.Velocity;
                if (direction != Vector2.Zero)
                    direction.Normalize();
                particle.Velocity = direction * Speed.Evaluate(age) * particle.SpeedMultiplier;
                particle.Rotation = Rotation.Evaluate(age) * particle.RotationMultiplier;
                particle.Scale = Scale.Evaluate(age) * particle.ScaleMultiplier;

                particle.Color = new Color(ColorR.Evaluate(age), ColorG.Evaluate(age), ColorB.Evaluate(age), ColorA.Evaluate(age));
            }

            for (int i = 0; i < particles.Count; i++)
                if (particles[i].Age >= particles[i].Lifetime)
                {
                    particles.RemoveAt(i);
                    i--;
                }

            float emission = (float)gameTime.ElapsedGameTime.TotalSeconds * EmissionRate;
            emissionError += emission - (float)Math.Floor(emission);
            emission -= emission - (float)Math.Floor(emission);
            emission += (float)Math.Floor(emissionError);
            emissionError -= (float)Math.Floor(emissionError);
            for (int i = 0; i < emission; i++)
                particles.Add(GenerateParticle());

            foreach (IParticleManipulator manipulator in Manipulators)
                foreach (Particle particle in particles)
                    manipulator.ManipulateParticle(particle);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
                particle.Draw(spriteBatch);
        }

        private Particle GenerateParticle()
        {
            float speedMultiplier = 1 + random.NextFloat(-SpeedDeviation, SpeedDeviation);
            float rotationMultiplier = 1 + random.NextFloat(-RotationDeviation, RotationDeviation);
            float scaleMultiplier = 1 + random.NextFloat(-ScaleDeviation, ScaleDeviation);

            return new Particle(Texture, EmitterLocation,
                Utils.Vector2FromSpeedAndDirection(Speed.Evaluate(0) * speedMultiplier, random.NextFloat(MinDirection, MaxDirection)),
                speedMultiplier, Rotation.Evaluate(0) * rotationMultiplier, rotationMultiplier,
                new Color(ColorR.Evaluate(0), ColorG.Evaluate(0), ColorB.Evaluate(0), ColorA.Evaluate(0)),
                Scale.Evaluate(0), scaleMultiplier, random.NextFloat(MinLifetime, MaxLifetime));
        }
    }
}
