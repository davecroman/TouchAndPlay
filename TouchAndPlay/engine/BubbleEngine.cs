using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.utils;
using Microsoft.Xna.Framework;
using TouchAndPlay.bubbles;
using Microsoft.Xna.Framework.Graphics;

namespace TouchAndPlay.engine
{
    class BubbleEngine
    {

        /* ======================================================================
         * BUBBLE CREATORS
         * ======================================================================
         */

        //arrays
        private List<Bubble> bubblesOnScreen;
        private List<BubbleSet> bubbleSetsOnScreen;
        private List<DragBubble> dragBubblesOnScreen;
        private Dictionary<BubbleState, Texture2D> blueHands;

        //textures
        private Texture2D lineTexture;
        private Texture2D directionLineTexture;
        private Texture2D dot;

        public BubbleEngine()
        {

        }

        public LoadContent(ContentManager content){

        }

        //creates a drag bubble
        private void createDragBubble(float minDistance = 100)
        {
            dragBubblesOnScreen.Add(new DragBubble(DragBubbleType.HAND, blueHands, directionLineTexture, effectHandler));
            dragBubblesToPop--;
        }

        private void createDragBubble(Vector2 pos1, Vector2 pos2)
        {
            dragBubblesOnScreen.Add(new DragBubble(DragBubbleType.HAND, blueHands, directionLineTexture, effectHandler, pos1, pos2));
            dragBubblesToPop--;
        }
        //creates a set bubble
        private void createBubbleSet(float minDistance = 100)
        {
            bubbleSetsOnScreen.Add(new BubbleSet(BubbleSetType.HANDHAND, blueHands, lineTexture, effectHandler));
            setBubblesToPop--;
        }

        private void createBubbleSet(Vector2 pos1, Vector2 pos2)
        {
            bubbleSetsOnScreen.Add(new BubbleSet(BubbleSetType.HANDHAND, blueHands, lineTexture, effectHandler, pos1, pos2));
            setBubblesToPop--;
        }
        //creates a random bubble
        private void createRandomBubble()
        {
            int randInt = Randomizer.random(0, rightReachablePoints.Count - 1);
            Vector2 randVector2 = rightReachablePoints.ElementAt(randInt) + kinector.getRightShoulderPosition();
            bubblesOnScreen.Add(new Bubble(randVector2.X, randVector2.Y, blueHands, BubbleType.HAND));

            rightReachablePoints.RemoveAt(randInt);
            soloBubblesToPop--;
        }

        private void createSoloBubble(Vector2 position, Vector2 referencePoint)
        {
            bubblesOnScreen.Add(new Bubble(position.X, position.Y, blueHands, BubbleType.HAND, referencePoint));

            soloBubblesToPop--;
        }
    }
}
