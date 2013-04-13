using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using TouchAndPlay.utils;
using TouchAndPlay.screens;

namespace TouchAndPlay.effects
{
    public class EffectHandler
    {
        List<BasicEffect> effectsOnScreen;
        List<BasicScoreText> scoreTextsOnScreen;
        List<BasicEffect> handTrail;
        List<BasicEffect> handTrail2;

        Texture2D basicParticleTexture;
        SpriteFont scoreFont;
        KinectManager kinector;

        int updateCount, drawCount;

        Painter painter;
        Vector2 planeOrigin;
        Vector2 rightHand;
        Vector2 leftHand;

        JointType referenceJoint;

        private readonly static Color DEFAULT_EFFECT_COLOR =  Color.LightBlue;

        public EffectHandler(KinectManager kinector)
        {
            this.kinector = kinector;

            leftHand = kinector.getJointPosition(JointType.HandLeft);
            rightHand = kinector.getJointPosition(JointType.HandRight);

            Initialize();
        }

        public void Initialize()
        {
            painter = new Painter();

            effectsOnScreen = new List<BasicEffect>();
            scoreTextsOnScreen = new List<BasicScoreText>();
            handTrail = new List<BasicEffect>();
            handTrail2 = new List<BasicEffect>();

            referenceJoint = JointType.HandRight;

            SetupCartesiaPlaneOrigin();
        }

        private void SetupCartesiaPlaneOrigin()
        {
            //default Cartesian plane origin
            if (referenceJoint == JointType.HandRight)
            {
                planeOrigin = new Vector2(GameConfig.APP_WIDTH / 2 + 30, GameConfig.APP_HEIGHT / 2 - 20);
            }
            else
            {
                planeOrigin = new Vector2(GameConfig.APP_WIDTH / 2 - 30, GameConfig.APP_HEIGHT / 2 - 20);
            }
        }

        public void LoadContent(ContentManager content)
        {
            basicParticleTexture = content.Load<Texture2D>("effects/basic_particle");
            scoreFont = content.Load<SpriteFont>("fonts/ScoreFont");
        }

        public void addParticleEffect(int xPos, int yPos, int particleCount, Color? color= null, float? xVel = null, float? yVel = null, bool allowGravity = true, int maxVel = 5, int minVel = -5)
        {

            effectsOnScreen.Add(new BasicEffect(xPos, yPos, particleCount, basicParticleTexture, color==null?DEFAULT_EFFECT_COLOR:color.Value, xVel, yVel, allowGravity, maxVel, minVel));
            
        }

        public void addScoreEffect(int score, int xPos, int yPos, Color? color = null)
        {
            scoreTextsOnScreen.Add(new BasicScoreText(scoreFont, xPos, yPos, score, color.HasValue? color.Value:Color.White));
        }

        public void addText(string text, int xPos, int yPos, Color? color = null)
        {
            scoreTextsOnScreen.Add(new BasicScoreText(scoreFont, xPos, yPos, text, color.HasValue ? color.Value : Color.White));
        }

        public void Update()
        {
            updateHandPos();
            addHandTrail();
            updateParticleEffects();
            updateTextEffects();
            updateTrail();
        }

        private void updateHandPos()
        {
          
            planeOrigin = kinector.getJointPosition(StageScreen.originJoint);
            leftHand = kinector.getJointPosition(JointType.HandLeft);
            rightHand = kinector.getJointPosition(JointType.HandRight);

        }

        private void updateTrail()
        {
            for (updateCount = 0; updateCount < handTrail.Count; updateCount++)
            {
                handTrail[updateCount].Update();

                if (handTrail[updateCount].isReadForRemoval())
                {
                    handTrail.RemoveAt(updateCount--);
                }
                else
                {
                    handTrail[updateCount].Update();
                }
            }

            for (updateCount = 0; updateCount < handTrail2.Count; updateCount++)
            {
                handTrail2[updateCount].Update();

                if (handTrail2[updateCount].isReadForRemoval())
                {
                    handTrail2.RemoveAt(updateCount--);
                }
                else
                {
                    handTrail2[updateCount].Update();
                }
            }
        }

        private void updateTextEffects()
        {
            for (updateCount = 0; updateCount < scoreTextsOnScreen.Count; updateCount++)
            {
                scoreTextsOnScreen[updateCount].Update();

                if (scoreTextsOnScreen[updateCount].isReadyForRemoval())
                {
                    scoreTextsOnScreen.RemoveAt(updateCount--);
                }
            }
        }

        private void updateParticleEffects()
        {
            for (updateCount = 0; updateCount < effectsOnScreen.Count; updateCount++)
            {
                if (effectsOnScreen[updateCount].isReadForRemoval())
                {
                    effectsOnScreen.RemoveAt(updateCount--);
                }
                else
                {
                    effectsOnScreen[updateCount].Update();
                }
            }
        }

        private void addHandTrail(Color? color = null, float? xVel = 0, float? yVel = 0, int maxVel = 5, int minVel = -5)
        {
            if (referenceJoint == JointType.HandRight)
            {
                handTrail.Add(new BasicEffect((int)rightHand.X, (int)rightHand.Y, 1, basicParticleTexture, color == null ? DEFAULT_EFFECT_COLOR : color.Value, xVel, yVel, false, maxVel, minVel));
                if (GameConfig.CURRENT_GAME_TYPE == engine.GameType.COORD_EXERCISE)
                {
                    handTrail2.Add(new BasicEffect((int)leftHand.X, (int)leftHand.Y, 1, basicParticleTexture, color == null ? DEFAULT_EFFECT_COLOR : color.Value, xVel, yVel, false, maxVel, minVel));
                }
            }
            else
            {
                handTrail.Add(new BasicEffect((int)leftHand.X, (int)leftHand.Y, 1, basicParticleTexture, color == null ? DEFAULT_EFFECT_COLOR : color.Value, xVel, yVel, false, maxVel, minVel));
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            DrawPlane(sprite);
            
            for (drawCount = 0; drawCount < effectsOnScreen.Count; drawCount++)
            {
                effectsOnScreen[drawCount].Draw(sprite);
            }

            for (drawCount = 0; drawCount < scoreTextsOnScreen.Count; drawCount++)
            {
                scoreTextsOnScreen[drawCount].Draw(sprite);
            }

            //draw a lot per dot pair
            for (drawCount = 1; drawCount < handTrail.Count; drawCount++)
            {
                painter.DrawLine(sprite, new Vector2(handTrail[drawCount - 1].xPos, handTrail[drawCount - 1].yPos), new Vector2(handTrail[drawCount].xPos, handTrail[drawCount].yPos), 3, Color.White * handTrail[drawCount - 1].particles[0].alpha);
                
            }

            //draw a lot per dot pair
            for (drawCount = 1; drawCount < handTrail2.Count; drawCount++)
            {
                painter.DrawLine(sprite, new Vector2(handTrail2[drawCount - 1].xPos, handTrail2[drawCount - 1].yPos), new Vector2(handTrail2[drawCount].xPos, handTrail2[drawCount].yPos), 3, Color.White * handTrail2[drawCount - 1].particles[0].alpha);

            }  
        }

        public void DrawPlane(SpriteBatch sprite)
        {

            painter.DrawLine(sprite, new Vector2(planeOrigin.X, 0), new Vector2(planeOrigin.X, GameConfig.APP_HEIGHT), 1, Color.White * 0.2f);
            painter.DrawLine(sprite, new Vector2(0, planeOrigin.Y), new Vector2(GameConfig.APP_WIDTH, planeOrigin.Y), 1, Color.White * 0.2f);
        }

        public void Clear()
        {
            effectsOnScreen.Clear();
            scoreTextsOnScreen.Clear();
            handTrail.Clear();
        }

        public bool isInactive()
        {
            return effectsOnScreen.Count == 0 && scoreTextsOnScreen.Count == 0;
        }

        public void setReferenceJoint(JointType joint)
        {
            this.referenceJoint = joint;

            SetupCartesiaPlaneOrigin();
        }
    }
}
