using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using System.Diagnostics;
using TouchAndPlay.utils;


namespace TouchAndPlay
{
    public class KinectManager
    {
        private int APP_WIDTH;
        private int APP_HEIGHT;

        GraphicsDeviceManager graphics;

        KinectSensor kinect;

        Texture2D colorVideo;
        Texture2D depthVideo;
        Texture2D jointTexture;

        DepthImageFormat depthImageFormat;

        Skeleton[] skeletonData;

        Skeleton skeleton;

        Dictionary<JointType, Vector2> previousPoints;

        Painter painter;

        ColorImagePoint colorPoint;

        public KinectManager(int app_width, int app_height, GraphicsDeviceManager graphics)
        {
            this.painter = new Painter();
            this.APP_WIDTH = app_width;
            this.APP_HEIGHT = app_height;
            this.graphics = graphics;

            previousPoints = new Dictionary<JointType, Vector2>();
        }

        public void Initialize()
        {
            try
            {
                kinect = KinectSensor.KinectSensors[0];

                //the next lines adjust the smoothness of the gestures
                TransformSmoothParameters smoothingParam = createSmoothingParams();

                depthImageFormat = DepthImageFormat.Resolution320x240Fps30;

                kinect.SkeletonStream.Enable(smoothingParam);
                //kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                kinect.DepthStream.Enable(depthImageFormat);

                /*uncomment line below to enable depth handler*/
                //kinect.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(kinect_DepthFrameReady);

                //kinect.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinect_ColorFrameReady);
                kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinect_AllFramesReady);

                kinect.Start();

                
                //colorVideo = new Texture2D(graphics.GraphicsDevice, kinect.ColorStream.FrameWidth, kinect.ColorStream.FrameHeight);
                depthVideo = new Texture2D(graphics.GraphicsDevice, kinect.DepthStream.FrameWidth, kinect.DepthStream.FrameHeight);

                kinect.ElevationAngle = 0;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print("Kinect error: " + e.Message);
            }
        }

        public void LoadContent(ContentManager content)
        {
            //load resources
            jointTexture = content.Load<Texture2D>("jointDefaultTexture");
        }

        public TransformSmoothParameters createSmoothingParams()
        {
            TransformSmoothParameters smoothingParam = new TransformSmoothParameters();

            smoothingParam.Smoothing = 0.7f;
            smoothingParam.Correction = 0.3f;
            smoothingParam.Prediction = 1f;
            smoothingParam.JitterRadius = 1f;
            smoothingParam.MaxDeviationRadius = 1f;

            return smoothingParam;
        }

        private void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs colorImageFrame)
        {
            //Get raw image
            using (ColorImageFrame colorVideoFrame = colorImageFrame.OpenColorImageFrame())
            {

                if (colorVideoFrame != null)
                {
                    //Create array for pixel data and copy it from the image frame
                    Byte[] pixelData = new Byte[colorVideoFrame.PixelDataLength];
                    colorVideoFrame.CopyPixelDataTo(pixelData);

                    //Convert RGBA to BGRA
                    Byte[] bgraPixelData = new Byte[colorVideoFrame.PixelDataLength];

                    
                    for (int i = 0; i < pixelData.Length; i += 4)
                    {
                        bgraPixelData[i] = pixelData[i + 2];
                        //bgraPixelData[i + 1] = pixelData[i + 1];
                       // bgraPixelData[i + 2] = pixelData[i];
                       // bgraPixelData[i + 3] = (Byte)255; //The video comes with 0 alpha so it is transparent
                    }

                    // Create a texture and assign the realigned pixels
                    colorVideo = new Texture2D(graphics.GraphicsDevice, colorVideoFrame.Width, colorVideoFrame.Height);

                    colorVideo.SetData(bgraPixelData);
                }
            }
        }

        
        private void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs imageFrames)
        {
            using (SkeletonFrame skeletonFrame = imageFrames.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    if ((skeletonData == null) || (this.skeletonData.Length != skeletonFrame.SkeletonArrayLength))
                    {
                        this.skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    //Copy the skeleton data to our array
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData);
                }
            }

            if (skeletonData != null)
            {
                float farthest = 5f;
                //there can be more than one skeleton, but we'll only consider the case where there's just one skeleton
                foreach (Skeleton skel in skeletonData)
                {
                    if (skel.Position.Z < farthest)
                    {
                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            skeleton = skel;
                            farthest = skel.Position.Z;
                        }
                    }
                }
            }
        }



        public void DrawVideo(SpriteBatch spritebatch)
        {
            if ( colorVideo != null )
            {
                spritebatch.Draw(colorVideo, Vector2.Zero, Color.White);
            }
        }

        public void DrawSkeleton(SpriteBatch spriteBatch, JointType? jointType = null)
        {
            if (skeleton != null)
            {
                if (jointType == null)
                {
                    DrawAllJoints(spriteBatch);
                }
                else
                {
                    DrawSpecificJoint(spriteBatch, jointType.Value);
                }
            }
        }

        private void DrawAllJoints(SpriteBatch spriteBatch)
        {
            /*
            foreach (Joint joint in skeleton.Joints)
            {
                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    Vector2 position = getCalibratedPosition(joint);

                    spriteBatch.Draw(jointTexture, new Vector2(position.X, position.Y), null, Color.White, 0.0f, new Vector2(20, 20), 0.25f, SpriteEffects.None, 0.0f);
                }
            }*/

            spriteBatch.Draw(jointTexture, getCalibratedPosition(skeleton.Joints[JointType.Head]), null, Color.White, 0.0f, new Vector2(20, 20), 1.2f, SpriteEffects.None, 0.0f);

            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.ElbowRight]), getCalibratedPosition(skeleton.Joints[JointType.ShoulderRight]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.ElbowRight]), getCalibratedPosition(skeleton.Joints[JointType.WristRight]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.WristRight]), getCalibratedPosition(skeleton.Joints[JointType.HandRight]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.ShoulderRight]), getCalibratedPosition(skeleton.Joints[JointType.ShoulderCenter]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.ShoulderLeft]), getCalibratedPosition(skeleton.Joints[JointType.ShoulderCenter]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.ShoulderLeft]), getCalibratedPosition(skeleton.Joints[JointType.ElbowLeft]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.ElbowLeft]), getCalibratedPosition(skeleton.Joints[JointType.WristLeft]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.HandLeft]), getCalibratedPosition(skeleton.Joints[JointType.WristLeft]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.ShoulderCenter]), getCalibratedPosition(skeleton.Joints[JointType.Spine]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.Spine]), getCalibratedPosition(skeleton.Joints[JointType.HipCenter]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.HipCenter]), getCalibratedPosition(skeleton.Joints[JointType.HipRight]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.HipCenter]), getCalibratedPosition(skeleton.Joints[JointType.HipLeft]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.KneeRight]), getCalibratedPosition(skeleton.Joints[JointType.HipRight]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.KneeLeft]), getCalibratedPosition(skeleton.Joints[JointType.HipLeft]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.KneeLeft]), getCalibratedPosition(skeleton.Joints[JointType.AnkleLeft]), 5, Color.Green);
            painter.DrawLine(spriteBatch, getCalibratedPosition(skeleton.Joints[JointType.KneeRight]), getCalibratedPosition(skeleton.Joints[JointType.AnkleRight]), 5, Color.Green);
        }


        public void DrawSpecificJoint(SpriteBatch spriteBatch, JointType jointType)
        {
            if (skeleton != null)
            {
                Joint joint = skeleton.Joints[jointType];

                Vector2 position = getCalibratedPosition(joint);

                spriteBatch.Draw(jointTexture, position, null, Color.White * 0.75f, 0, new Vector2(jointTexture.Width / 2.0f, jointTexture.Height / 2.0f), 0.5f, SpriteEffects.None, 0f);
            }
        }

        /*-----------------------------PUBLIC METHODS----------------------------------------*/

        public void Stop()
        {
            kinect.Stop();
        }

        public int getElevationAngle()
        {
            return kinect.ElevationAngle;
        }

        

        private Vector2 getCalibratedPosition(Joint joint)
        {   
            colorPoint = kinect.MapSkeletonPointToColor(joint.Position, kinect.ColorStream.Format);
            return new Vector2(colorPoint.X, colorPoint.Y);
        }

        public float getDepth(JointType jointType)
        {
            return skeleton.Joints[jointType].Position.Z;
        }

        
        private Vector2 getCalibratedPosition(JointType jointType)
        {
            if (skeleton != null)
            {
                return getCalibratedPosition(skeleton.Joints[jointType]);
            }
            else
            {
                return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

        }
        public Vector2 getJointPosition(JointType jointType)
        {
            if (skeleton != null && skeleton.Joints[jointType].TrackingState == JointTrackingState.Tracked)
            {
                previousPoints[jointType] = getCalibratedPosition(skeleton.Joints[jointType]);
                return previousPoints[jointType];
            }
            else
            {
                if (previousPoints.ContainsKey(jointType))
                {
                    return previousPoints[jointType];
                }
                else
                {
                    return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }
        }

        public Vector2 getHandPosition(JointType hand = JointType.HandRight)
        {
            if (skeleton != null && skeleton.Joints[hand].TrackingState == JointTrackingState.Tracked)
            {
                return getCalibratedPosition(skeleton.Joints[hand]);
            }
            else
            {
                return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        public Vector2 getShoulderCenterPosition()
        {
            if (skeleton != null && skeleton.Joints[JointType.ShoulderCenter].TrackingState == JointTrackingState.Tracked)
            {
                return getCalibratedPosition(skeleton.Joints[JointType.ShoulderCenter]);
            }
            else
            {
                return new Vector2(GameConfig.APP_WIDTH/2, GameConfig.APP_HEIGHT/2);
            }
        }

        public Vector2 getRightElbowPosition()
        {
            if (skeleton != null && skeleton.Joints[JointType.ElbowRight].TrackingState == JointTrackingState.Tracked)
            {
                return getCalibratedPosition(skeleton.Joints[JointType.ElbowRight]);
            }
            else
            {
                return new Vector2(GameConfig.APP_WIDTH/2 + 50, GameConfig.APP_HEIGHT/2);
            }
        }

        public Vector2 getHeadPosition()
        {
            if (skeleton != null && skeleton.Joints[JointType.Head].TrackingState == JointTrackingState.Tracked )
            {
                return getCalibratedPosition(skeleton.Joints[JointType.Head]);
            }
            else
            {
                return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        public Vector2 getCentralHipPosition()
        {
            if (skeleton != null && skeleton.Joints[JointType.HipCenter].TrackingState == JointTrackingState.Tracked)
            {
                return getCalibratedPosition(skeleton.Joints[JointType.HipCenter]);
            }
            else
            {
                return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        public Vector2 getRightShoulderPosition()
        {
            if (skeleton != null && skeleton.Joints[JointType.ShoulderRight].TrackingState == JointTrackingState.Tracked)
            {
                return getCalibratedPosition(skeleton.Joints[JointType.ShoulderRight]);
            }
            else
            {
                return new Vector2(GameConfig.APP_WIDTH/2, GameConfig.APP_HEIGHT/2);
            }
        }

        public Vector2 getLeftShoulderPosition()
        {
            if (skeleton != null && skeleton.Joints[JointType.ShoulderLeft].TrackingState == JointTrackingState.Tracked)
            {
                return getCalibratedPosition(skeleton.Joints[JointType.ShoulderLeft]);
            }
            else
            {
                return new Vector2(GameConfig.APP_WIDTH/2 - 50, GameConfig.APP_HEIGHT/2);
            }
        }

        /*-----------------------------PUBLIC GETTERS----------------------------------------*/

        public Texture2D getColorVideo()
        {
            return colorVideo;
        }

        public bool isColliding(Rectangle collisionBox, JointType jointType)
        {
            Vector2 jointPos = getCalibratedPosition(jointType);

            if (collisionBox.Intersects(new Rectangle((int)jointPos.X - 25,(int) jointPos.Y - 25, 50, 50)))
            {
                return true;
            }
            return false;
        }

        public KinectStatus getConnectionStatus()
        {
            if (kinect != null)
            {
                return kinect.Status;
            }
            else
            {
                return KinectStatus.Undefined;
            }
        }

        public bool hasFoundSkeleton()
        {
            return skeleton != null;
        }

        public bool isTrackingSkeleton()
        {
            if (skeleton != null)
            {
                return skeleton.TrackingState == SkeletonTrackingState.Tracked;
            }
            else
            {
                return false;
            }
        }

        /*-----------------------------PUBLIC SETTERS-----------------------------------------*/

        public void setTrackingMode(SkeletonTrackingMode trackingMode)
        {
            if (kinect != null)
            {
                kinect.SkeletonStream.TrackingMode = trackingMode;
            }
            else
            {
                MyConsole.print("No Kinect device ready");
            }

        }

        public void setElevationAngle(int angle)
        {
            if (angle <= 15 && angle >= -15)
            {
                kinect.ElevationAngle = angle;
            }
        }

        
    }
}
