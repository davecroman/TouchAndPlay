using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using TouchAndPlay.db.playerdata;
using Microsoft.Kinect;

namespace TouchAndPlay.db
{
    class TAPDatabase
    {
        private static String dirPath;
        
        private const String profilesDir = "\\profiles\\";

        public static List<PlayerProfile> playerProfiles;

        static TAPDatabase()
        {
            playerProfiles = new List<PlayerProfile>();
        }

        public static void setup()
        {
            dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TouchAndPlay") + "\\";
            initialiseProfiles();
            loadProfiles();
            loadGameConfig();
        }

        private static void initialiseProfiles()
        {
            if (!Directory.Exists(dirPath + profilesDir))
            {
                Directory.CreateDirectory(dirPath + profilesDir);
                PlayerProfile guestProfile = new PlayerProfile("Guest");
                saveProfile(guestProfile);
                GameConfig.CURRENT_PROFILE = guestProfile.getName();
            }
        }

        private static void loadGameConfig()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(GameConfigInstance));
            GameConfigInstance g;

            if (Directory.Exists(dirPath))
            {

                if (File.Exists(dirPath + "GameConfig.xml"))
                {

                    using (TextReader textWriter = new StreamReader(@dirPath + "GameConfig.xml"))
                    {
                        g = (GameConfigInstance)deserializer.Deserialize(textWriter);
                        textWriter.Close();
                    }

                    GameConfig.CURRENT_PROFILE = g.CURRENT_PROFILE;
                }
                else
                {
                    MyConsole.print("Game Config file not existing. Using default values");
                }
            }
    
        }

        public static void saveProfile(PlayerProfile profile)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PlayerProfile));

                using (TextWriter textWriter = new StreamWriter(@dirPath + profilesDir + profile.getName() + ".xml"))
                {
                    serializer.Serialize(textWriter, profile);
                    textWriter.Close();
                }
                playerProfiles.Add(profile);
            }
            catch (Exception e)
            {
                MyConsole.print(e.InnerException.ToString());
            }
        }

        public static void saveGameConfig()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameConfigInstance));

            using (TextWriter textWriter = new StreamWriter(@dirPath + "GameConfig.xml"))
            {
                serializer.Serialize(textWriter, new GameConfigInstance());
                textWriter.Close();
            }
        }

        public static void loadProfiles()
        {
            string[] files = Directory.GetFiles(dirPath + profilesDir, "*.xml" );
            if (files.Length > 0)
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(PlayerProfile));

                for (int i = 0; i < files.Length; i++)
                {
                    using (TextReader textReader = new StreamReader(@files[i]))
                    {
                        PlayerProfile profile = (PlayerProfile)deserializer.Deserialize(textReader);
                        playerProfiles.Add(profile);
                    }
                }
            }
            else
            {
                GameConfig.CURRENT_PROFILE = "Guest";
            }
        }

        public static void updateProfile(PlayerProfile profile)
        {
            if (profileExists(profile.username))
            {
                deleteProfile(profile.username);
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PlayerProfile));

                using (TextWriter textWriter = new StreamWriter(@dirPath + profilesDir + profile.getName() + ".xml"))
                {
                    serializer.Serialize(textWriter, profile);
                    textWriter.Close();
                }
            }
            catch (Exception e)
            {
                MyConsole.print(e.InnerException.ToString());
            }
        }

        public static void deleteProfile(string name)
        {
            try
            {
                File.Delete(dirPath + profilesDir + name + ".xml");
            }
            catch (Exception e)
            {
                MyConsole.print(e.InnerException.ToString());
            }
        }


        public static void recordGame(string profileName, engine.GameType gameType, int currentLevel, int playerScore, int medalsEarned, int bubblesPopped, int totalBubbles, bool q1, bool q2, bool q3, bool q4, JointType refJoint )
        {
            PlayerProfile profile = getProfile(profileName);

            if (profile == null)
            {
                profile = createProfile(profileName);
            }

            profile.recordGameData(gameType, currentLevel, playerScore, medalsEarned, bubblesPopped, totalBubbles, q1, q2, q3, q4, refJoint);
            updateProfile(profile);
        }

        private static PlayerProfile createProfile(string name)
        {
            PlayerProfile newProfile = new PlayerProfile(name);

            saveProfile(newProfile);

            return newProfile;
        }

        public static bool profileExists(string profileName)
        {
            if (getProfile(profileName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static PlayerProfile getProfile(string profileName)
        {
            for (int index = 0; index < playerProfiles.Count; index++)
            {
                if (playerProfiles[index].Equals(profileName))
                {
                    return playerProfiles[index];
                }
            }

            return null;
        }

        public static int getRecordMedals(string profileName, engine.GameType gameType, int level)
        {
            PlayerProfile profile = getProfile(profileName);

            if (profile != null)
            {
                return profile.getRecordMedals(gameType, level);
            }
            else
            {
                return 0;
            }
        }

        internal static int getRecordScore(string profileName, engine.GameType gameType, int level)
        {
            PlayerProfile profile = getProfile(profileName);

            if (profile != null)
            {
                return profile.getRecordScore(gameType, level);
            }
            else
            {
                return 0;
            }
        }
    }
}
