using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace TouchAndPlay.db
{
    public class GameData
    {
        public int score;
        public int name;
        public int gameNum;
        public int bubblesPopped;
        public int totalBubbles;

        public DateTime day;
        public JointType refJoint;

        public List<bool> quadrants;

        public int GameNumber
        {
            get
            {
                return gameNum;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
        }

        public DateTime Date
        {
            get
            {
                return day;
            }
        }

        public GameData()
        {
            //parameterless
        }

        public int BubblesPopped{
            get
            {
                return bubblesPopped;
            }
        }

        public int TotalBubbles
        {
            get
            {
                return totalBubbles;
            }
        }

        public GameData(int score, int gameNum, int bubblesPopped, int totalBubbles, bool q1Active = true, bool q2Active = true, bool q3Active = true, bool q4Active = true, JointType refJoint = JointType.ShoulderRight)
        {
            this.score = score;
            this.gameNum = gameNum;
            this.day = DateTime.Today;
            this.refJoint = refJoint;
            this.bubblesPopped = bubblesPopped;
            this.totalBubbles = totalBubbles;
            
            this.quadrants = new List<bool>();

            quadrants.Add(q1Active);
            quadrants.Add(q2Active);
            quadrants.Add(q3Active);
            quadrants.Add(q4Active);
        }
    }
}
