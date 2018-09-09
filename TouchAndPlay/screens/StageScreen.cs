using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TouchAndPlay.bubbles;
using Microsoft.Kinect;
using TouchAndPlay.utils;
using Microsoft.Xna.Framework;
using TouchAndPlay.components;
using TouchAndPlay.effects;
using TouchAndPlay.engine;
using TouchAndPlay.engine.bubbles;
using TouchAndPlay.components.gamespecific;
using TouchAndPlay.db;
using TouchAndPlay.db.playerdata;

namespace TouchAndPlay.screens
{
    class StageScreen:BasicScreen
    {
        public enum StageScreenStates
        {
            PREPARING,
            RUNNING,
            PAUSED,
            RESUMING,
            ENDING,
            GAME_OVER,
            FINISHED,
        }

        public enum Quadrants
        {
            Q1, Q2, Q3, Q4
        }

        private KinectManager kinector;

        //arrays
        private List<Bubble> bubblesOnScreen;
        private List<BubbleSet> bubbleSetsOnScreen;
        private List<DragBubble> dragBubblesOnScreen;
        private List<StarBubble> starBubblesOnScreen;
        private List<RedBubble> redBubblesOnScreen;

        private Dictionary<BubbleState, Texture2D> blueHands;

        //textures
        private Texture2D lineTexture;
        private Texture2D directionLineTexture;
        private Texture2D dot;
        private Texture2D progressCircle;
        private Texture2D welcomeBanner;
        private Texture2D progressBarClip;
        private Texture2D starBubbleClip;
        private Texture2D redBubbleClip;

        //iterators
        private short iter1;
        private short iter2;
        private short iter3;

        //intervals and counters
        private int bubbleSetInterval;
        private int bubbleSetCtr;
        private int bubbleInterval;
        private int bubbleCtr;
        private int dragBubbleInterval;
        private int dragBubbleCtr;
        private int msgCtr;
        private int resumeCtr;

        //reference bubble, for referencing purposes only
        private Bubble refBub;

        //states
        private StageScreenStates currentState;
        private StagePreparationStates currentPreparationState;

        //game
        private TAPGame currentGame;

        //effect hanlder
        private EffectHandler effectHandler;


        //UI handler
        private MyUI myUI;
        private SpriteFont basicFont;
        private GoalPanel goalPanel;

       
        //target number of bubbles to pop
        private int soloBubblesToPop;
        private int setBubblesToPop;
        private int dragBubblesToPop;

        //game booleans
        private bool usingMouseAsInput;
        private bool allHighlighted;

        //for randomizing points, these are only references. DO NOT INSTANTIATE
        private List<Vector2> rightReachablePoints;
        private List<Vector2> leftReachablePoints;

        
        private float rightLimbEstSize;
        private int rightLimbNCount;

        private float leftLimbEstSize;
        private int leftLimbNCount;

        private int playerScore;

        private ImageButton expandContractBtn;
        private GraphicsDeviceManager graphics;

        private BasicText scorePane;
        private BasicImage progressBar;

        //game data
        private Dictionary<GameType,List<LevelData>> levels;

        //bool value is true if the referenced quadrant is active
        private Dictionary<Quadrants, bool> quadrants;
        public static JointType originJoint;
        private JointType referenceHand;

        private int currentLevel;
        private int scoreComboCtr;
        private int comboDurationCounter;
        private int comboDuration;
        private int timeLeft;
        private int timeAlloc;
        private int endBufferTime;

        //game counters
        int bubblesPopped;
        int totalBubblesToPop;

        /* ===================================================================
         * CONSTRUCTORS
         * ===================================================================
         */
        public StageScreen(KinectManager kinector, GraphicsDeviceManager graphics)
        {
            this.kinector = kinector;
            this.graphics = graphics;

            base.Initialize();
            this.Init();
        }

        /* ====================================================================
         * INITIALIZER 
         * ====================================================================
         */
        public void Init()
        {
            //initialize bubble lists==================================
            bubblesOnScreen = new List<Bubble>();
            bubbleSetsOnScreen = new List<BubbleSet>();
            dragBubblesOnScreen = new List<DragBubble>();
            starBubblesOnScreen = new List<StarBubble>();
            redBubblesOnScreen = new List<RedBubble>();
            //=========================================================

            //quadrants================================================
            quadrants = new Dictionary<Quadrants, bool>();
            quadrants[Quadrants.Q1] = false;
            quadrants[Quadrants.Q2] = false;
            quadrants[Quadrants.Q3] = false;
            quadrants[Quadrants.Q4] = false;

            //prepare levels===========================================
            levels = new Dictionary<GameType, List<LevelData>>();
            levels[GameType.RANGE_EXERCISE] = new List<LevelData>();
            levels[GameType.COORD_EXERCISE] = new List<LevelData>();
            levels[GameType.PRECISION_EXERCISE] = new List<LevelData>();
            //=========================================================

            //initialize bubbleIntervals===============================
            this.bubbleSetInterval = GameConfig.BUBBLE_SET_INTERVAL;
            this.bubbleInterval = GameConfig.SOLO_BUBBLE_INTERVAL;
            this.dragBubbleInterval = GameConfig.DRAG_BUBBLE_INTERVAL;
            //=========================================================

            //initialize state=========================================
            this.currentState = StageScreenStates.PREPARING;
            //=========================================================

            //create effect handler====================================
            this.effectHandler = new EffectHandler(kinector);
            //=========================================================

            //initialize UI============================================
            this.myUI = new MyUI();
            this.goalPanel = new GoalPanel(effectHandler);
            //=========================================================

            //initialize reference joint===============================
            setReferenceJoint(JointType.HandRight);
            //=========================================================
        }

        public void prepareLevel()
        {
            resetStage();

            LevelMaker.instantiateRangeLevels(levels[GameType.RANGE_EXERCISE]);
            LevelMaker.instantiateCoordLevels(levels[GameType.COORD_EXERCISE]);
            LevelMaker.instantiateDragLevels(levels[GameType.PRECISION_EXERCISE]);

            this.currentLevel = GameConfig.CURRENT_LEVEL;
            this.comboDuration = 40 + GameConfig.SOLOBUBBLE_POP_TIME + 90;
            this.comboDurationCounter = this.comboDuration;
            this.endBufferTime = 1000;

            this.bubblesPopped = 0;

            goalPanel.resetMedals();

            switch (GameConfig.CURRENT_GAME_TYPE)
            {
                case GameType.RANGE_EXERCISE:
                    this.currentGame = TAPGame.SEQUENTIAL_SOLO;
                    this.bubbleCtr = soloBubblesToPop;
                    LevelData levelData1 = levels[GameType.RANGE_EXERCISE].ElementAt(currentLevel-1);
                    this.timeLeft = levelData1.gameDuration;
                    this.timeAlloc = levelData1.gameDuration;
                    this.soloBubblesToPop = levelData1.bubblesToPop;
                    this.totalBubblesToPop = soloBubblesToPop;
                    this.goalPanel.setupPanel(levelData1.goals[LevelData.Goals.SCORE], levelData1.goals[LevelData.Goals.STARS], levelData1.goals[LevelData.Goals.MAX_MISS], levelData1.goals[LevelData.Goals.COMBO], levelData1.goals[LevelData.Goals.BUBBLES], levelData1.goals[LevelData.Goals.MAX_REDHIT]);
                    break;
                case GameType.COORD_EXERCISE:
                    this.currentGame = TAPGame.SEQUENTIAL_SET;
                    LevelData levelData2 = levels[GameType.COORD_EXERCISE].ElementAt(currentLevel-1);
                    this.goalPanel.setupPanel(levelData2.goals[LevelData.Goals.SCORE], levelData2.goals[LevelData.Goals.STARS], levelData2.goals[LevelData.Goals.MAX_MISS], levelData2.goals[LevelData.Goals.COMBO], levelData2.goals[LevelData.Goals.BUBBLES], levelData2.goals[LevelData.Goals.MAX_REDHIT]);
                    this.timeLeft = levelData2.gameDuration;
                    this.timeAlloc = levelData2.gameDuration;
                    this.setBubblesToPop = levelData2.bubblesToPop;
                    this.totalBubblesToPop = setBubblesToPop;
                    break;
                case GameType.PRECISION_EXERCISE:
                    this.currentGame = TAPGame.SEQUENTIAL_DRAG;
                    LevelData levelData3 = levels[GameType.PRECISION_EXERCISE].ElementAt(currentLevel - 1);
                    this.timeLeft = levelData3.gameDuration;
                    this.timeAlloc = levelData3.gameDuration;
                    this.dragBubblesToPop = levelData3.bubblesToPop;
                    this.dragBubbleCtr = dragBubblesToPop;
                    this.totalBubblesToPop = dragBubblesToPop;
                    break;
            }

            progressBar.setX(-progressBar.getWidth() + timeLeft / (float)timeAlloc * progressBar.getWidth());
        }

        private void resetStage()
        {
            bubblesOnScreen.Clear();
            bubbleSetsOnScreen.Clear();
            dragBubblesOnScreen.Clear();
            starBubblesOnScreen.Clear();
            redBubblesOnScreen.Clear();

            effectHandler.Clear();

            msgCtr = 0;
            currentState = StageScreenStates.PREPARING;
            currentPreparationState = StagePreparationStates.CHECK_KINECT_CONNECTION;

            resetScoreToZero();
            comboDurationCounter = 0;
            scoreComboCtr = 0;
        }

        /* ===================================================================
         * CONTENT LOADER
         * ===================================================================
         */
        public override void LoadContent( ContentManager content )
        {
            base.LoadBasicContent(content);
            //bubbles
            blueHands = new Dictionary<BubbleState, Texture2D>();

            blueHands[BubbleState.NORMAL_STATE] = content.Load<Texture2D>("bubbles/bubble_hand_normal");
            blueHands[BubbleState.HIGHLIGHTED_STATE] = content.Load<Texture2D>("bubbles/bubble_hand_highlighted");
            blueHands[BubbleState.STATIC_INACTIVE] = content.Load<Texture2D>("bubbles/bubble_hand_inactive");
            blueHands[BubbleState.LOCKED_IN] = content.Load<Texture2D>("bubbles/bubble_hand_locked");

            starBubbleClip = content.Load<Texture2D>("bubbles/bubble_star2");
            redBubbleClip = content.Load<Texture2D>("bubbles/bubble_red");
            //lines
            lineTexture = content.Load<Texture2D>("lines/line");
            directionLineTexture = content.Load<Texture2D>("lines/directionLine");
            basicFont = content.Load<SpriteFont>("fonts/BasicFont");
            progressCircle = content.Load<Texture2D>("progressCircle");

            dot = content.Load<Texture2D>("effects/basic_particle");
            welcomeBanner = content.Load<Texture2D>("screens/menuscreen/img_welcomebanner");
            effectHandler.LoadContent(content);

            this.progressBarClip = content.Load<Texture2D>("progressBar");

            currentPreparationState = StagePreparationStates.CHECK_KINECT_CONNECTION;

            myUI.LoadContent(content);
            goalPanel.LoadContent(content);
            goalPanel.setupPanel(0,3, 0, 0, 20, 0);
            

            
        }

        public override void createScreen()
        {
            addImage(Gallery.BG_MAIN, 0, 0, 1f, 0.4f);

            expandContractBtn = addImageButton(GameConfig.APP_WIDTH - 60, 10, icon_expand, "Expand", StringAlignment.BOTTOM_CENTERED, true, false, Color.White, Color.Black, FontType.CG_12_REGULAR);

            addImageButton(530, 10, icon_back, "Back", StringAlignment.BOTTOM_CENTERED, true, false, null, null, FontType.CG_12_REGULAR);

            addImage(welcomeBanner, 0, 5);
            scorePane = addText(10, 10, "Score: " + playerScore, Color.White, FontType.CG_14_REGULAR);

            progressBar = addImage(progressBarClip, 0, 7 + welcomeBanner.Height);
            progressBar.alpha = 0.5f;
        }

        /* ===================================================================
         * UPDATE METHODS
         * ===================================================================
         */ 
        public override void Update()
        {
            base.Update();
            updateStageScreenState(); //calls updateGame() if currentState == StageScreenStates.RUNNING
            updateButtons();
        }

        private void updateStageScreenState()
        {
            switch (currentState)
            {
                case StageScreenStates.PREPARING:
                    //see in Draw function the calling of the function prepareStageUI
                    if (currentPreparationState == StagePreparationStates.FINISHED)
                    {
                        currentState = StageScreenStates.RUNNING;
                    }
                    break;
                case StageScreenStates.RUNNING:
                    updateGame();
                    effectHandler.Update();
                    updateComboCounter();
                    if (gameIsOver())
                    {
                        currentState = StageScreenStates.GAME_OVER;
                    }
                    else if (gameIsFinished())
                    {
                        TAPDatabase.recordGame(GameConfig.CURRENT_PROFILE, GameConfig.CURRENT_GAME_TYPE, currentLevel, playerScore, goalPanel.medalsEarned, bubblesPopped, totalBubblesToPop, quadrants[Quadrants.Q1], quadrants[Quadrants.Q2], quadrants[Quadrants.Q3], quadrants[Quadrants.Q4], originJoint);
                        targetScreen = ScreenState.SELECT_GAME_SCREEN;
                        transitionState = TransitionState.TRANSITION_OUT;
                        currentState = StageScreenStates.FINISHED;
                    }
                    break;
                case StageScreenStates.PAUSED:
                    if (kinector.isTrackingSkeleton())
                    {
                        resumeCtr = GameConfig.RESUME_COUNT;
                        currentState = StageScreenStates.RESUMING;
                    }
                    break;
                case StageScreenStates.RESUMING:
                    resumeCtr--;
                    if (resumeCtr <= 0)
                    {
                        currentState = StageScreenStates.RUNNING;
                    }
                    break;
                case StageScreenStates.FINISHED:
                    break;
            }
        }

        private bool gameIsFinished()
        {
            
            
            if (starBubblesOnScreen.Count == 0 && redBubblesOnScreen.Count == 0 && effectHandler.isInactive() )
            {
                MyConsole.print("here");
                switch (GameConfig.CURRENT_GAME_TYPE)
                {
                    case GameType.RANGE_EXERCISE:
                        return soloBubblesToPop <= 0 && bubblesOnScreen.Count == 0;
                    case GameType.PRECISION_EXERCISE:
                        return dragBubblesToPop <= 0 && dragBubblesOnScreen.Count == 0;
                    case GameType.COORD_EXERCISE:
                        return setBubblesToPop <= 0 && bubbleSetsOnScreen.Count == 0;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool gameIsOver()
        {
            return timeLeft <= 0;
        }

        private void updateComboCounter()
        {
            if (comboDurationCounter > 0)
            {
                comboDurationCounter--;
                if (comboDurationCounter == 0)
                {
                    scoreComboCtr = 0;
                }
            }
        }

        private void updateTimer()
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                progressBar.setX(-progressBar.getWidth() + timeLeft / (float)timeAlloc * progressBar.getWidth());
            }
        }

        private void updateButtons()
        {
            for (int count = 0; count < buttonsOnScreen.Count; count++)
            {
                if (buttonsOnScreen[count].isClicked())
                {
                    switch (buttonsOnScreen[count].label)
                    {
                        case "Back":
                            targetScreen = ScreenState.SELECT_GAME_SCREEN;
                            transitionState = TransitionState.TRANSITION_OUT;
                            break;
                        case "Contract":
                        case "Expand":
                            graphics.ToggleFullScreen();
                            if (graphics.IsFullScreen)
                            {
                                expandContractBtn.changeImage(icon_contract);
                                expandContractBtn.changeText("Contract");
                            }
                            else
                            {
                                expandContractBtn.changeImage(icon_expand);
                                expandContractBtn.changeText("Expand");
                            }
                            break;
                    }
                }
            }

            if (graphics.IsFullScreen && expandContractBtn.label == "Expand")
            {
                expandContractBtn.changeImage(icon_contract);
                expandContractBtn.changeText("Contract");
            }
            else if (!graphics.IsFullScreen && expandContractBtn.label == "Contract")
            {
                expandContractBtn.changeImage(icon_expand);
                expandContractBtn.changeText("Expand");
            }
        }

        private void updateGame()
        {
            if (kinector.isTrackingSkeleton() || usingMouseAsInput)
            {
                switch( currentGame ){
                    case TAPGame.SEQUENTIAL_SOLO:
                        updateGameSequentialSolo();
                        updateSoloBubbles();
                        break;
                    case TAPGame.SEQUENTIAL_SET:
                        updateGameSequentialSet();
                        updateSetBubbles();
                        break;
                    case TAPGame.SEQUENTIAL_DRAG:
                        updateGameSequentialDrag();
                        updateDragBubbles();
                        break;
                    case TAPGame.SEQUENTIAL_MIXED:
                        updateGameSequentialMixed();
                        updateSoloBubbles();
                        updateDragBubbles();
                        updateSetBubbles();
                        break;
                }

                updateTimer();
                updateStarBubbles();
                updateRedBubbles();
            }
            else
            {
                currentState = StageScreenStates.PAUSED;
            }
            //effectHandler.addParticleEffect((int)handPos.X, (int)handPos.Y, 1, Color.Yellow, null, null, true, 2,-2);
        }

        private void updateDragBubbles()
        {
            for (iter1 = 0; iter1 < dragBubblesOnScreen.Count; iter1++)
            {
                if (dragBubblesOnScreen[iter1].currentState != DragBubbleState.REMOVAL_STATE)
                {
                    //update the bubble's state
                    dragBubblesOnScreen[iter1].Update();

                    refBub = dragBubblesOnScreen[iter1].bubble1;
                    
                    if (refBub.isActive())
                    {
                        switch (refBub.bubbleType)
                        {
                            case BubbleType.HAND:
                                if (dragBubblesOnScreen[iter1].currentState != DragBubbleState.LOCKED_IN)
                                {
                                    if (kinector.isColliding(refBub.collisionBox, referenceHand))
                                    {
                                        refBub.setState(BubbleState.HIGHLIGHTED_STATE);
                                        refBub.jointHovering = JointType.HandRight;
                                        dragBubblesOnScreen[iter1].currentState = DragBubbleState.HOVERED;
                                    }
                                    else
                                    {
                                        refBub.setState(BubbleState.NORMAL_STATE);
                                        dragBubblesOnScreen[iter1].currentState = DragBubbleState.NORMAL;
                                        allHighlighted = false;
                                    }
                                }
                                else
                                {
                                    dragBubblesOnScreen[iter1].follow(refBub.jointHovering==JointType.HandRight?kinector.getHandPosition(JointType.HandRight):kinector.getHandPosition(JointType.HandLeft));
                                }
                                break;
                        }

                        
                    }else if (dragBubblesOnScreen[iter1].isReadyToPop())
                    {
                        incrementScoreAndCombo(dragBubblesOnScreen[iter1].bubble2.getAbsolutePos());
                        bubblesPopped += 1;
                        incrementPieChart(true, getPositionQuadrant(refBub.pos));
                    }

                    dragBubblesOnScreen[iter1].resetPopCounters();
                    
                }
                else
                {
                    if (!dragBubblesOnScreen[iter1].popped)
                    {
                        createMissMessage(dragBubblesOnScreen[iter1].midPoint);
                        incrementPieChart(false, getPositionQuadrant(dragBubblesOnScreen[iter1].bubble2.pos));
                    }
                    dragBubblesOnScreen.RemoveAt(iter1--);
                    continue;
                }

            }
        }

        private void updateSetBubbles()
        {
            for (iter1 = 0; iter1 < bubbleSetsOnScreen.Count; iter1++)
            {
                if (bubbleSetsOnScreen[iter1].currentState != BubbleSetState.REMOVAL_STATE)
                {
                    //update the bubble's state
                    bubbleSetsOnScreen[iter1].Update();

                    //we assume first that all bubbles are touched until we found one that isn't
                    allHighlighted = true;

                    for (iter3 = 0; iter3 < bubbleSetsOnScreen[iter1].bubbles.Count; iter3++)
                    {
                        bubbleSetsOnScreen[iter1].bubbles[0].setReferencePoint(kinector.getJointPosition(originJoint));
                        bubbleSetsOnScreen[iter1].bubbles[1].setReferencePoint(kinector.getJointPosition(originJoint));

                        refBub = bubbleSetsOnScreen[iter1].bubbles[iter3];

                        if (refBub.isActive())
                        {
                            switch (refBub.bubbleType)
                            {
                                case BubbleType.HAND:
                                    if (kinector.isColliding(refBub.collisionBox, JointType.HandRight) || kinector.isColliding(refBub.collisionBox, JointType.HandLeft))
                                    {
                                        refBub.setState(BubbleState.HIGHLIGHTED_STATE);
                                    }
                                    else
                                    {
                                        refBub.setState(BubbleState.NORMAL_STATE);
                                        allHighlighted = false;
                                    }
                                    break;
                            }
                        }
                    }

                    if (refBub.isReadyToPop())
                    {
                        incrementScoreAndCombo(bubbleSetsOnScreen[iter1].midPoint);
                        bubblesPopped += 1;
                        incrementPieChart(true, getPositionQuadrant(bubbleSetsOnScreen[iter1].bubbles[1].pos));

                    }

                    if (!allHighlighted)
                    {
                        //we start counting to popping only when all bubbles are highlighted/touched
                        bubbleSetsOnScreen[iter1].resetPopCounters();
                    }
                }
                else
                {
                    if (!bubbleSetsOnScreen[iter1].popped)
                    {
                        createMissMessage(bubbleSetsOnScreen[iter1].midPoint);
                        incrementPieChart(false, getPositionQuadrant(bubbleSetsOnScreen[iter1].bubbles[1].pos));
                    }

                    bubbleSetsOnScreen.RemoveAt(iter1--);
                    continue;
                }
               
            }
        }

        private void incrementPieChart(bool hit, Quadrants q)
        {
            PlayerProfile profile = TAPDatabase.getProfile(GameConfig.CURRENT_PROFILE);
            if (profile == null)
            {
                return;
            }

            switch (q)
            {
                case Quadrants.Q1:
                    if (hit)
                    {
                        profile.Q1_HIT += 1;
                    }
                    else
                    {
                        profile.Q1_MISS += 1;
                    }
                    break;
                case Quadrants.Q2:
                    if (hit)
                    {
                        profile.Q2_HIT += 1;
                    }
                    else
                    {
                        profile.Q2_MISS += 1;
                    }
                    break;
                case Quadrants.Q3:
                    if (hit)
                    {
                        profile.Q3_HIT += 1;
                    }
                    else
                    {
                        profile.Q3_MISS += 1;
                    }
                    break;
                case Quadrants.Q4:
                    if (hit)
                    {
                        profile.Q4_HIT += 1;
                    }
                    else
                    {
                        profile.Q4_MISS += 1;
                    }
                    break;
            }
        }

        private void updateStarBubbles()
        {
            for (iter1 = 0; iter1 < starBubblesOnScreen.Count; iter1++)
            {
                starBubblesOnScreen[iter1].Update();

                if (kinector.isColliding(starBubblesOnScreen[iter1].collisionBox, JointType.HandRight))
                {
                    effectHandler.addParticleEffect((int)starBubblesOnScreen[iter1].position.X, (int)starBubblesOnScreen[iter1].position.Y, 20, Color.Yellow);
                    effectHandler.addParticleEffect((int)starBubblesOnScreen[iter1].position.X, (int)starBubblesOnScreen[iter1].position.Y, 20, Color.LightYellow);
                    effectHandler.addScoreEffect(10, (int)starBubblesOnScreen[iter1].position.X, (int)starBubblesOnScreen[iter1].position.Y, Color.Yellow);
                    starBubblesOnScreen.RemoveAt(iter1--);

                    addToScore(10);
                    goalPanel.addCollectedStar();
                    
                }else if(starBubblesOnScreen[iter1].hasPassed())
                {
                    starBubblesOnScreen.RemoveAt(iter1);
                    iter1--;
                }
            }
        }

        private void updateSoloBubbles()
        {
            for (iter1 = 0; iter1 < bubblesOnScreen.Count; iter1++)
            {
                refBub = bubblesOnScreen[iter1];

                if (!usingMouseAsInput)
                {
                    refBub.setReferencePoint(kinector.getRightShoulderPosition());
                }

                refBub.Update();

                if (refBub.isReadyForRemoval())
                {
                    bubblesOnScreen.RemoveAt(iter1--);
                    continue;

                }
                else if ( refBub.isActive() )
                {
                    switch (refBub.bubbleType)
                    {
                        case BubbleType.HAND:
                            if (kinector.isColliding(refBub.collisionBox, referenceHand))
                            {
                                refBub.setState(BubbleState.HIGHLIGHTED_STATE);

                                if (refBub.isReadyToPop())
                                {
                                    //create explosion effect=====================
                                    Vector2 absPos = refBub.getAbsolutePos();
                                    effectHandler.addParticleEffect((int)absPos.X, (int)absPos.Y, 5 + scoreComboCtr*5, Color.RoyalBlue);
                                    effectHandler.addParticleEffect((int)absPos.X, (int)absPos.Y, 3 + scoreComboCtr*3, Color.White);
                                    //=============================================

                                    incrementScoreAndCombo(absPos);
                                    bubblesPopped += 1;
                                    incrementPieChart(true, getPositionQuadrant(refBub.pos));
                                }
                            }
                            else
                            {
                                refBub.setState(BubbleState.NORMAL_STATE);
                            }
                            break;
                    }
                }
                else if (refBub.wasMissed())
                {
                    createMissMessage(refBub.getAbsolutePos());
                    incrementPieChart(false, getPositionQuadrant(refBub.pos));
                }
            }
        }

        private void updateRedBubbles()
        {
            for (iter1 = 0; iter1 < redBubblesOnScreen.Count; iter1++)
            {
                if (!redBubblesOnScreen[iter1].isReadyForRemoval())
                {
                    redBubblesOnScreen[iter1].Update();

                    if (redBubblesOnScreen[iter1].isActive())
                    {
                        if (kinector.isColliding(redBubblesOnScreen[iter1].collisionBox, referenceHand))
                        {
                            redBubblesOnScreen[iter1].GoAway();
                            effectHandler.addText("Hit!", (int)redBubblesOnScreen[iter1].getPos().X, (int)redBubblesOnScreen[iter1].getPos().Y - 30, Color.Red);
                            effectHandler.addScoreEffect(-10, (int)redBubblesOnScreen[iter1].getPos().X, (int)redBubblesOnScreen[iter1].getPos().Y, Color.Red);
                            addToScore(-10);
                        }
                    }
                }
                else
                {
                    redBubblesOnScreen.RemoveAt(iter1--);
                }
            }
        }

        private void createMissMessage(Vector2 pos)
        {
            effectHandler.addText("Miss!", (int)pos.X, (int)pos.Y, Color.Red);
        }

        private void incrementScoreAndCombo(Vector2 absPos)
        {
            //add a combo message==========================
            if (scoreComboCtr > 0)
            {
                effectHandler.addText("Combo x" + (scoreComboCtr + 1), (int)absPos.X, (int)absPos.Y - 40, Color.White);
            }
            //============================================

            //update goal panel=================
            goalPanel.addCollectedBubble();
            //==================================

            //update score and combo counter======================
            addToScore(GameConfig.BUBBLE_SCORE + GameConfig.BUBBLE_SCORE * scoreComboCtr);
            effectHandler.addScoreEffect((scoreComboCtr + 1) * GameConfig.BUBBLE_SCORE, (int)absPos.X, (int)absPos.Y, Color.Yellow);
            comboDurationCounter = comboDuration;
            scoreComboCtr += 1;
            //====================================================
        }

        private void addToScore(int value)
        {
            playerScore += value;
            goalPanel.addScore(value);

            if (playerScore < 0)
            {
                playerScore = 0;
            }
            scorePane.setLabel("Score: " + playerScore);
        }

        private void resetScoreToZero()
        {
            playerScore = 0;
            scorePane.setLabel("Score: " + playerScore);
        }

        private void updateGameSequentialSet()
        {
            bubbleSetCtr--;
            if (setBubblesToPop > 0 && timeLeft > 0)
            {
                if (bubbleSetCtr <= 0)
                {
                    createBubbleSet();
                    createRedBubble(1);
                    createStarBubble(0.3f);

                    bubbleSetCtr = bubbleSetInterval;
                }
                else if (bubbleSetCtr > GameConfig.MIN_GAP_BETWEEN_BUBBLES && bubbleSetsOnScreen.Count == 0)
                {
                    bubbleSetCtr = GameConfig.MIN_GAP_BETWEEN_BUBBLES;
                }
            }
            else
            {
                //end game
            }
        }

        private void updateGameSequentialDrag()
        {
            dragBubbleCtr--;

            if (dragBubblesToPop > 0)
            {
                if (dragBubbleCtr <= 0)
                {
                    int rand1, rand2;

                    do
                    {
                        rand1 = Randomizer.random(0, rightReachablePoints.Count - 1);
                    } while (!isInActiveQuadrant(rightReachablePoints[rand1]));

                    do
                    {
                        rand2 = Randomizer.random(0, rightReachablePoints.Count - 1);
                    } while (!isInActiveQuadrant(rightReachablePoints[rand2]));

                    createDragBubble(rightReachablePoints[rand1], rightReachablePoints[rand2]);
                    createRedBubble();
                    createStarBubble(0.3f);
                    dragBubbleCtr = dragBubbleInterval;
                }
                else if (dragBubbleCtr > GameConfig.MIN_GAP_BETWEEN_BUBBLES && dragBubblesOnScreen.Count == 0)
                {
                    dragBubbleCtr = GameConfig.MIN_GAP_BETWEEN_BUBBLES;
                }
            }
            else
            {
                //end game
            }
        }

        private void updateGameSequentialMixed()
        {
            if (bubbleSetsOnScreen.Count + bubblesOnScreen.Count + dragBubblesOnScreen.Count == 0)
            {
                int randIndex, randIndex2, combination;
                Vector2 pos, pos2;
                Vector2 rightShoulderPos = kinector.getRightShoulderPosition();
                Vector2 leftShoulderPos = kinector.getLeftShoulderPosition();
                for (int count = 0; count < 4; count++)
                {
                    //combination = Randomizer.random(1, 5);
                    combination = 1;

                    switch (combination)
                    {
                        case 1:
                            randIndex = Randomizer.random(0, leftReachablePoints.Count - 1);
                            ///pos = leftReachablePoints[randIndex] + leftShoulderPos;
                            createSoloBubble(leftReachablePoints[randIndex], leftShoulderPos);
                            leftReachablePoints.RemoveAt(randIndex);
                            break;
                        case 2:
                            randIndex = Randomizer.random(0, rightReachablePoints.Count - 1);
                            pos = rightReachablePoints[randIndex] + rightShoulderPos;
                            createSoloBubble(pos, Vector2.Zero);
                            rightReachablePoints.RemoveAt(randIndex);
                            break;
                        case 3:
                            randIndex = Randomizer.random(0, leftReachablePoints.Count - 1);
                            randIndex2 = Randomizer.random(0, rightReachablePoints.Count - 1);
                            pos = leftReachablePoints[randIndex] + leftShoulderPos;
                            pos2 = rightReachablePoints[randIndex2] + rightShoulderPos;
                            createBubbleSet(pos, pos2);
                            leftReachablePoints.RemoveAt(randIndex);
                            rightReachablePoints.RemoveAt(randIndex2);
                            count += 1; //we add 1 to count since we created 2 bubbles
                            break;
                        case 4:
                            randIndex = Randomizer.random(0, leftReachablePoints.Count - 1);
                            randIndex2 = Randomizer.random(0, rightReachablePoints.Count - 1);
                            pos = leftReachablePoints[randIndex] + leftShoulderPos;
                            pos2 = rightReachablePoints[randIndex2] + rightShoulderPos;
                            createDragBubble(pos, pos2);
                            leftReachablePoints.RemoveAt(randIndex);
                            rightReachablePoints.RemoveAt(randIndex2);
                            count += 1; //we add 1 to count since we created 2 bubbles
                            break;
                    }
                }
            }
        }

        private void updateGameSequentialSolo()
        {
            bubbleCtr--;

            if (soloBubblesToPop > 0 && timeLeft > 0)
            {
                if (bubbleCtr <= 0)
                {
                    int rand;

                    if (originJoint == JointType.ShoulderRight)
                    {
                        do
                        {
                            rand = Randomizer.random(0, rightReachablePoints.Count - 1);
                        } while (!isInActiveQuadrant(rightReachablePoints[rand]));

                        createSoloBubble(rightReachablePoints[rand], Vector2.Zero);
                    }
                    else
                    {
                        do
                        {
                            rand = Randomizer.random(0, leftReachablePoints.Count - 1);
                        } while (!isInActiveQuadrant(leftReachablePoints[rand]));

                        createSoloBubble(leftReachablePoints[rand], Vector2.Zero);
                    }

                    
                    createRedBubble();
                    createStarBubble(0.3f);
                    bubbleCtr = bubbleInterval;
                }
                else if (bubbleCtr > GameConfig.MIN_GAP_BETWEEN_BUBBLES && bubblesOnScreen.Count == 0)
                {
                    bubbleCtr = GameConfig.MIN_GAP_BETWEEN_BUBBLES;
                }
            }
        }

        /* ======================================================================
         * BUBBLE CREATORS
         * ======================================================================
         */

        //creates a drag bubble
        private void createDragBubble(float minDistance = 100)
        {
            dragBubblesOnScreen.Add( new DragBubble(DragBubbleType.HAND, blueHands, progressCircle, directionLineTexture, effectHandler) );
            dragBubblesToPop--;
        }

        private void createDragBubble(Vector2 pos1, Vector2 pos2)
        {
            dragBubblesOnScreen.Add(new DragBubble(DragBubbleType.HAND, blueHands, progressCircle, directionLineTexture, effectHandler, pos1, pos2, new Vector2(GameConfig.APP_WIDTH/2, GameConfig.APP_HEIGHT/2)));

            dragBubblesToPop--;
        }
        //creates a set bubble
        private void createBubbleSet(float minDistance = 130)
        {
            Vector2 pos1;
            Vector2 pos2;
            int randIndex1, randIndex2;

            do
            {
                randIndex1 = Randomizer.random(0, leftReachablePoints.Count - 1);
                pos1 = leftReachablePoints.ElementAt(randIndex1);
                randIndex2 = Randomizer.random(0, rightReachablePoints.Count - 1);
                pos2 = rightReachablePoints.ElementAt(randIndex2);
            } while (MathUtil.getDistance(pos1, pos2) < minDistance);

            //pos1 += kinector.getLeftShoulderPosition();
            //pos2 += kinector.getRightShoulderPosition();

            bubbleSetsOnScreen.Add(new BubbleSet(BubbleSetType.HANDHAND, blueHands, progressCircle, lineTexture, effectHandler, pos1, pos2));
            setBubblesToPop--;
        }

        private void createBubbleSet(Vector2 pos1, Vector2 pos2)
        {
            bubbleSetsOnScreen.Add(new BubbleSet(BubbleSetType.HANDHAND,blueHands, progressCircle, lineTexture,effectHandler,pos1,pos2));
            setBubblesToPop--;

            
        }
        //creates a random bubble
        private void createRandomBubble()
        {
            int randInt = Randomizer.random(0,rightReachablePoints.Count-1);
            Vector2 randVector2 = rightReachablePoints.ElementAt(randInt) + kinector.getRightShoulderPosition();
            bubblesOnScreen.Add(new Bubble(randVector2.X, randVector2.Y, blueHands, progressCircle, BubbleType.HAND));

            rightReachablePoints.RemoveAt(randInt);
            soloBubblesToPop--;
        }

        private void createSoloBubble(Vector2 position, Vector2 referencePoint)
        {
            if (usingMouseAsInput)
            {
                bubblesOnScreen.Add(new Bubble(position.X, position.Y, blueHands, progressCircle, BubbleType.HAND, new Vector2(GameConfig.APP_WIDTH/2f, GameConfig.APP_HEIGHT/2f)));  
            }
            else
            {
                bubblesOnScreen.Add(new Bubble(position.X, position.Y, blueHands, progressCircle, BubbleType.HAND, referencePoint));
            }

            for (iter1 = 0; iter1 < redBubblesOnScreen.Count; iter1++)
            {
                if (MathUtil.getDistance(redBubblesOnScreen[iter1].getPos(), position + referencePoint) < 150)
                {
                    redBubblesOnScreen[iter1].GoAway();
                }
            }

            createRedBubble(3);

            soloBubblesToPop--;
        }

        private void createStarBubble(float chance = 1f)
        {
            if (Randomizer.random(1, 100) <= chance * 100)
            {
                starBubblesOnScreen.Add(new StarBubble(starBubbleClip, effectHandler));
            }
           
        }

        private void createRedBubble(int count = 1)
        {
            for (int ct = 0; ct < count; ct++)
            {
                bool posFound = false;
                Vector2 position = new Vector2();

                while (!posFound)
                {
                    if( referenceHand == JointType.HandRight ){
                        position = rightReachablePoints.ElementAt(Randomizer.random(0, rightReachablePoints.Count-1)) + kinector.getRightShoulderPosition();
                    }
                    else if (referenceHand == JointType.HandLeft)
                    {
                        position = leftReachablePoints.ElementAt(Randomizer.random(0, leftReachablePoints.Count - 1)) + kinector.getLeftShoulderPosition();
                    }


                    switch (currentGame)
                    {
                        case TAPGame.SEQUENTIAL_SOLO:
                            for (iter1 = 0; iter1 < bubblesOnScreen.Count; iter1++)
                            {
                                if (MathUtil.getDistance(position, bubblesOnScreen[iter1].getAbsolutePos()) < 150)
                                {
                                    break;
                                }
                                else if (iter1 + 1 == bubblesOnScreen.Count)
                                {
                                    posFound = true;
                                }
                            }
                            break;
                        case TAPGame.SEQUENTIAL_SET:
                            for (iter1 = 0; iter1 < bubbleSetsOnScreen.Count; iter1++)
                            {
                                for (int i = 0; i < bubbleSetsOnScreen[iter1].bubbles.Count; i++)
                                {
                                    if (MathUtil.getDistance(position, bubbleSetsOnScreen[iter1].bubbles[i].getAbsolutePos()) < 150)
                                    {
                                        break;
                                    }
                                    else if (i + 1 == bubbleSetsOnScreen[iter1].bubbles.Count)
                                    {
                                        posFound = true;
                                    }
                                }

                                if (posFound == false)
                                {
                                    break;
                                }
                                else if (iter1 + 1 < bubbleSetsOnScreen.Count)
                                {
                                    posFound = false;
                                }
                                else
                                {
                                    posFound = true;
                                }

                            }
                            break;
                        case TAPGame.SEQUENTIAL_DRAG:
                            for (iter1 = 0; iter1 < dragBubblesOnScreen.Count; iter1++)
                            {
                                if (MathUtil.getDistance(dragBubblesOnScreen[iter1].bubble1.getAbsolutePos(), position) < 150)
                                {
                                    break;
                                }

                                if (MathUtil.getDistance(dragBubblesOnScreen[iter1].bubble2.getAbsolutePos(), position) < 150)
                                {
                                    break;
                                }

                                posFound = true;
                            }
                            break;
                    }
                }

                redBubblesOnScreen.Add(new RedBubble(redBubbleClip, position, Vector2.Zero));
            }
            
        }

        public void setState(StageScreenStates state)
        {
            currentState = StageScreenStates.PREPARING;
        }
        /*
         * ==========================================================================
         * USER INTERFACE HANDLERS
         * ==========================================================================
         */
        //draw function.
        public override void Draw(SpriteBatch sprite)
        {
            //kinector.DrawVideo(sprite);
            base.Draw(sprite);
            kinector.DrawSkeleton(sprite);
            goalPanel.Draw(sprite);
            

            switch (currentState)
            {
                case StageScreenStates.PREPARING:
                    PrepareStageUI(sprite);
                    break;
                case StageScreenStates.RUNNING:
                    DrawBubbles(sprite);
                    effectHandler.Draw(sprite);
                    DrawReachablePoints(sprite, originJoint == JointType.ShoulderLeft, originJoint == JointType.ShoulderRight);
                    break;
                case StageScreenStates.PAUSED:
                    myUI.writeNotificationWindow(sprite, "PAUSED", "Unable to track user. No skeleton tracked.", currentState);
                    break;
                case StageScreenStates.RESUMING:
                    myUI.writeNotificationWindow(sprite, "RESUMING", "in " + resumeCtr / 80 + " seconds.", currentState);
                    DrawBubbles(sprite);
                    break;
            }

            base.DrawTransitionBox(sprite);
        }

        private void DrawReachablePoints(SpriteBatch sprite, bool left, bool right)
        {
            if (right)
            {
                for (int i = 0; i < rightReachablePoints.Count; i++)
                {
                    sprite.Draw(dot, rightReachablePoints[i] + kinector.getRightShoulderPosition(), Color.White * 0.25f);
                }
            }

            if (left)
            {
                for (int i = 0; i < leftReachablePoints.Count; i++)
                {
                    sprite.Draw(dot, leftReachablePoints[i] + kinector.getLeftShoulderPosition(), Color.White * 0.25f);
                }
            }
                     
        }

        //draws all bubbles that are on screen
        private void DrawBubbles(SpriteBatch sprite)
        {
            for (iter2 = 0; iter2 < bubblesOnScreen.Count; iter2++)
            {
                bubblesOnScreen[iter2].Draw(sprite);
            }

            for (iter2 = 0; iter2 < bubbleSetsOnScreen.Count; iter2++)
            {
                bubbleSetsOnScreen[iter2].Draw(sprite);
            }

            for (iter2 = 0; iter2 < dragBubblesOnScreen.Count; iter2++)
            {
                dragBubblesOnScreen[iter2].Draw(sprite);
            }

            for (iter2 = 0; iter2 < redBubblesOnScreen.Count; iter2++)
            {
                redBubblesOnScreen[iter2].Draw(sprite);
            }

            for (iter2 = 0; iter2 < starBubblesOnScreen.Count; iter2++)
            {
                starBubblesOnScreen[iter2].Draw(sprite);
            }

            
        }

        //preparationWindow
        private void PrepareStageUI(SpriteBatch sprite)
        {
            switch (currentPreparationState)
            {
                case StagePreparationStates.CHECK_KINECT_CONNECTION:
                    if (kinector.getConnectionStatus() != KinectStatus.Connected)
                    {
                        myUI.writeNotificationWindow(sprite, "NO KINECT SENSOR DETECTED", "Kinect device not properly connected.", Color.OrangeRed, Color.White, Color.LightGray);
                        //myUI.WriteToPreparationWindow(sprite, "Kinect device not properly connected.");
                    }
                    else
                    {
                        myUI.writeNotificationWindow(sprite, "PREPARING SENSOR", "Kinect device ready.", Color.GreenYellow, Color.White, Color.LightGray);
                        //myUI.WriteToPreparationWindow(sprite, "Kinect device ready");
                    }

                    msgCtr += 1;
                    if (msgCtr >= GameConfig.MESSAGE_SWITCH_TIME)
                    {
                        msgCtr = 0;
                        if (kinector.getConnectionStatus() == KinectStatus.Connected)
                        {
                            usingMouseAsInput = false;
                            currentPreparationState = StagePreparationStates.CHECK_SKELETON;
                        }
                        else
                        {
                            msgCtr = 200;
                            usingMouseAsInput = true;
                            currentPreparationState = StagePreparationStates.READY;
                        }
                        
                    }
                    break;
                case StagePreparationStates.CHECK_SKELETON:
                    if (!kinector.hasFoundSkeleton())
                    {
                        myUI.writeNotificationWindow(sprite, "UNABLE TO DETECT USER", "No user tracked yet.", Color.OrangeRed, Color.White, Color.LightGray);
                    }
                    else
                    {
                        myUI.writeNotificationWindow(sprite, "CALIBRATING", "A user is now being tracked", Color.Green, Color.White, Color.LightGray);
                        
                        msgCtr += 1;
                        if (msgCtr >= GameConfig.MESSAGE_SWITCH_TIME)
                        {
                            msgCtr = 0;
                            currentPreparationState = StagePreparationStates.CHECK_DEPTH;
                        }
                    }
                    break;
                case StagePreparationStates.CHECK_DEPTH:
                    if (kinector.hasFoundSkeleton())
                    {
                        if (kinector.getDepth(JointType.HipCenter) < 2f)
                        {
                            myUI.writeNotificationWindow(sprite, "ADJUST USER POSITION", "Please move farther away from the sensor.", Color.OrangeRed, Color.White, Color.LightGray);
                        }
                       
                        else
                        {
                            myUI.writeNotificationWindow(sprite, "IDEAL POSITION ACHIEVED", "1.5 meters away from sensor", Color.OrangeRed, Color.White, Color.LightGray);
                            msgCtr = 0;
                            currentPreparationState = StagePreparationStates.ADJUST_ANGLE;
                        }

                    }
                    else
                    {
                        currentPreparationState = StagePreparationStates.CHECK_SKELETON;
                        msgCtr = 0;
                    }

                    break;
                case StagePreparationStates.ADJUST_ANGLE:
                    if (kinector.hasFoundSkeleton())
                    {
                        if (kinector.getHeadPosition().Y > GameConfig.APP_HEIGHT/3 )
                        {
                            if (msgCtr <= 0)
                            {
                                kinector.setElevationAngle(kinector.getElevationAngle() - 3);
                                msgCtr = GameConfig.ANGLE_ADJUST_WAIT_TIME;
                                
                            }
                            else
                            {
                                myUI.writeNotificationWindow(sprite, "ADJUSTING ANGLE", "Please wait...", Color.OrangeRed, Color.White, Color.LightGray);
                                msgCtr--;
                            }

                        }
                        else
                        {
                            msgCtr = 0;
                            currentPreparationState = StagePreparationStates.MEASURE_SEGMENTS;
                        }

                    }
                    else
                    {
                        currentPreparationState = StagePreparationStates.CHECK_SKELETON;
                        msgCtr = 0;
                    }

                    break;
                case StagePreparationStates.MEASURE_SEGMENTS:
                    myUI.writeNotificationWindow(sprite, "SCALING BODY SEGMENTS", "Please Remain still.", Color.OrangeRed, Color.White, Color.LightGray);
                    msgCtr += 1;
                    
                    //measure right limb size by getting mean average
                    rightLimbEstSize += MathUtil.getDistance(kinector.getRightShoulderPosition(), kinector.getRightElbowPosition()) + MathUtil.getDistance(kinector.getRightElbowPosition(),kinector.getHandPosition());
                    rightLimbNCount += 1;

                    //measure left limb size by getting mean average
                    leftLimbEstSize += MathUtil.getDistance(kinector.getRightShoulderPosition(), kinector.getRightElbowPosition()) + MathUtil.getDistance(kinector.getRightElbowPosition(),kinector.getHandPosition());
                    leftLimbNCount += 1;

                    if (msgCtr >= GameConfig.MESSAGE_SWITCH_TIME + 150)
                    {
                        //right limb mean
                        rightLimbEstSize = rightLimbEstSize / rightLimbNCount + 30;
                        rightReachablePoints = Randomizer.getEvenListOfPoints(rightLimbEstSize, 50, 50);

                        //left limb mean
                        leftLimbEstSize = leftLimbEstSize / leftLimbNCount + 30;
                        leftReachablePoints = Randomizer.getEvenListOfPoints(leftLimbEstSize, 50, 50);

                        //soloBubblesToPop = rightReachablePoints.Count;
                        msgCtr = 300;
                        currentPreparationState = StagePreparationStates.READY;

                        rightLimbNCount = 0;
                        leftLimbNCount = 0;
                        rightLimbEstSize = 0;
                        leftLimbEstSize = 0;
                    }
                    break;
                case StagePreparationStates.READY:
                    myUI.writeNotificationWindow(sprite, "READY", "Starting in " + msgCtr / 80 + " second/s", Color.Yellow, Color.White, Color.White);
                    msgCtr -= 1;
                    if (msgCtr <= 80)
                    {
                        if (rightLimbEstSize == 0 || leftLimbEstSize == 0)
                        {
                            //right limb mean
                            rightLimbEstSize = 200;
                            rightReachablePoints = Randomizer.getEvenListOfPoints(rightLimbEstSize, 40, 40);

                            //left limb mean
                            leftLimbEstSize = 200;
                            leftReachablePoints = Randomizer.getEvenListOfPoints(leftLimbEstSize, 40, 40);
                        }
                        currentPreparationState = StagePreparationStates.FINISHED;
                    }
                    break;
                case StagePreparationStates.FINISHED:
                    break;

            }
        }

        /*
         * ==========================================================================
         * PUBLIC METHODS
         * ==========================================================================
         */
        public void setReferenceJoint(JointType jointType)
        {

            this.referenceHand = jointType;
            if (jointType == JointType.HandRight)
            {
                StageScreen.originJoint = JointType.ShoulderRight;
            }
            else
            {
                StageScreen.originJoint = JointType.ShoulderLeft;
            }

            effectHandler.setReferenceJoint(jointType);
        }

        /* ====================================================================
         * QUADRANT HANDLERS
         * ===================================================
         */ 
        internal void setActiveQuadrants(bool q1, bool q2, bool q3, bool q4)
        {
            if (!q1 && !q2 && !q3 && !q4)
            {
                q1 = q2 = q3 = q4 = true;
            }

            quadrants[Quadrants.Q1] = q1;
            quadrants[Quadrants.Q2] = q2;
            quadrants[Quadrants.Q3] = q3;
            quadrants[Quadrants.Q4] = q4;
        }

        private bool isInActiveQuadrant(Vector2 pos)
        {
            Quadrants positionQuadrant = getPositionQuadrant(pos);

            return quadrants[positionQuadrant];
        }

        private Quadrants getPositionQuadrant(Vector2 pos)
        {
            if (pos.X >= 0 && pos.Y >= 0)
            {
                return Quadrants.Q4;
            }
            else if (pos.X < 0 && pos.Y >= 0)
            {
                return Quadrants.Q3;
            }
            else if (pos.X < 0 && pos.Y < 0)
            {
                return Quadrants.Q2;
            }
            else
            {
                return Quadrants.Q1;
            }
        }

    }
}
