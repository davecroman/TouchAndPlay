using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchAndPlay.engine;

namespace TouchAndPlay.db.playerdata
{
    public class PlayerProfile
    {
        public string username;

        public List<List<List<GameData>>> gamesPlayed;

        public List<GameBest> bestRecords;

        public static int RANGE_EXERCISE_INDEX = 0;
        public static int COORD_EXERCISE_INDEX = 1;
        public static int PRECISION_EXERCISE_INDEX = 2;

        public int Q1_MISS;
        public int Q1_HIT;
        public int Q2_MISS;
        public int Q2_HIT;
        public int Q3_MISS;
        public int Q3_HIT;
        public int Q4_MISS;
        public int Q4_HIT;

        public PlayerProfile(string username)
        {
            this.username = username;

            this.gamesPlayed = new List<List<List<GameData>>>();

            //add list of game types
            this.gamesPlayed.Add(new List<List<GameData>>());
            this.gamesPlayed.Add(new List<List<GameData>>());
            this.gamesPlayed.Add(new List<List<GameData>>());

            
            gamesPlayed.ElementAt(RANGE_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(RANGE_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(RANGE_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(RANGE_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(RANGE_EXERCISE_INDEX).Add(new List<GameData>());
            
            gamesPlayed.ElementAt(COORD_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(COORD_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(COORD_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(COORD_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(COORD_EXERCISE_INDEX).Add(new List<GameData>());

            gamesPlayed.ElementAt(PRECISION_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(PRECISION_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(PRECISION_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(PRECISION_EXERCISE_INDEX).Add(new List<GameData>());
            gamesPlayed.ElementAt(PRECISION_EXERCISE_INDEX).Add(new List<GameData>());

            this.bestRecords = new List<GameBest>();
            this.bestRecords.Add(new GameBest(GameType.RANGE_EXERCISE));
            this.bestRecords.Add(new GameBest(GameType.COORD_EXERCISE));
            this.bestRecords.Add(new GameBest(GameType.PRECISION_EXERCISE));
            
        }

        public PlayerProfile()
        {
            //parameterless constructor
        }

        public string getName(){
            return username;
        }

        internal void recordGameData(GameType gameType, int level, int playerScore, int medalsEarned, int bubblesPopped, int totalBubbles, bool q1, bool q2, bool q3, bool q4, Microsoft.Kinect.JointType jointType)
        {
            switch (gameType)
            {
                case GameType.RANGE_EXERCISE:
                    bestRecords[RANGE_EXERCISE_INDEX].compare(level,playerScore,medalsEarned);
                    gamesPlayed[RANGE_EXERCISE_INDEX].ElementAt(level-1).Add(new GameData(playerScore, gamesPlayed[RANGE_EXERCISE_INDEX].ElementAt(level-1).Count + 1, bubblesPopped, totalBubbles, q1, q2, q3, q4, jointType));
                    break;
                case GameType.COORD_EXERCISE:
                    bestRecords[COORD_EXERCISE_INDEX].compare(level, playerScore, medalsEarned);
                    gamesPlayed[COORD_EXERCISE_INDEX].ElementAt(level - 1).Add(new GameData(playerScore, gamesPlayed[COORD_EXERCISE_INDEX].ElementAt(level - 1).Count + 1, bubblesPopped, totalBubbles, q1, q2, q3, q4, jointType));
                    break;
                case GameType.PRECISION_EXERCISE:
                    bestRecords[PRECISION_EXERCISE_INDEX].compare(level, playerScore, medalsEarned);
                    gamesPlayed[PRECISION_EXERCISE_INDEX].ElementAt(level - 1).Add(new GameData(playerScore, gamesPlayed[PRECISION_EXERCISE_INDEX][level - 1].Count + 1, bubblesPopped, totalBubbles, q1, q2, q3, q4, jointType));
                    break;
            }
        }

        internal int getRecordMedals(GameType gameType, int level)
        {
            switch (gameType)
            {
                case GameType.RANGE_EXERCISE:
                    return bestRecords[RANGE_EXERCISE_INDEX].getMedalsEarned(level);
                case GameType.COORD_EXERCISE:
                    return bestRecords[COORD_EXERCISE_INDEX].getMedalsEarned(level);
                case GameType.PRECISION_EXERCISE:
                    return bestRecords[PRECISION_EXERCISE_INDEX].getMedalsEarned(level);
            }

            return 0;
        }

        internal int getRecordScore(GameType gameType, int level)
        {
            switch (gameType)
            {
                case GameType.RANGE_EXERCISE:
                    return bestRecords[RANGE_EXERCISE_INDEX].getScoreEarned(level);
                case GameType.COORD_EXERCISE:
                    return bestRecords[COORD_EXERCISE_INDEX].getScoreEarned(level);
                case GameType.PRECISION_EXERCISE:
                    return bestRecords[PRECISION_EXERCISE_INDEX].getScoreEarned(level);
            }

            return 0;
        }

        public override string ToString()
        {
            return username;
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
            {
                return obj.ToString().Equals(username);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
