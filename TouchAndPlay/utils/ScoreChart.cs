using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TouchAndPlay.db;
using TouchAndPlay.db.playerdata;

namespace TouchAndPlay.utils
{
    public partial class ScoreChart : Form
    {
        List<GameData> gamesToShow;

        public ScoreChart()
        {
            InitializeComponent();
            gamesToShow = new List<GameData>();
            updateUi();
        }

        private void updateUi()
        {
            profilesComboBox.DataSource = TAPDatabase.playerProfiles;
            profilesComboBox2.DataSource = TAPDatabase.playerProfiles;
            profileComboBox3.DataSource = TAPDatabase.playerProfiles;
            profilesComboBox.Update();
            profilesComboBox2.Update();
            profileComboBox3.Update();

            profilesComboBox.SelectedItem = GameConfig.CURRENT_PROFILE;
            profilesComboBox2.SelectedItem = GameConfig.CURRENT_PROFILE;
            profileComboBox3.SelectedItem = GameConfig.CURRENT_PROFILE;

            gameTypeSelection.SelectedItem = "Range Exercise";
            gameTypeSelection2.SelectedItem = "Range Exercise";
            levelComboBox.SelectedIndex = 0;
            levelComboBox2.SelectedIndex = 0;

            leftShoulderCheckBox.Checked = true;
            leftShoulderCheckBox2.Checked = true;
            rightShoulderCheckBox.Checked = true;
            rightShoulderCheckBox2.Checked = true;

            bubblesPoppedChart.Series["Right"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            bubblesPoppedChart.Series["Right"].MarkerSize = 10;
            bubblesPoppedChart.Series["Left"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            bubblesPoppedChart.Series["Left"].MarkerSize = 10;

            showAllCheckBox2.Checked = true;
            showAllQCheckBox.Checked = true;

            updateScoreTab();
            updateBubblesPoppedTab();
            updateMotionRangeTab();
        }

        public int getGameIndex(string gameType)
        {
            switch (gameType)
            {
                case "Range Exercise":
                    return 0;
                case "Coordination Exercise":
                    return 1;
                case "Precision Exercise":
                    return 2;
                default:
                    return 0;
            }
        }

        public void UpdateFromDB()
        {
            updateUi();
            switch (tabControl.SelectedTab.Text)
            {
                case "Scores":
                    updateScoreTab();
                    break;
                case "Limb Speed":
                    updateBubblesPoppedTab();
                    break;
                case "Range of Motion":
                    updateMotionRangeTab();
                    break;
            }
        }

        public void updateMotionRangeTab()
        {
            if (profileComboBox3.SelectedItem == null)
            {
                return;
            }
            PlayerProfile profile = TAPDatabase.getProfile(profileComboBox3.SelectedItem.ToString());

            if (profile.Q1_HIT + profile.Q1_MISS > 5)
            {
                this.pieChart_Q1.Series["Pie"].Points.Clear();
                this.pieChart_Q1.Series["Pie"].Points.Add(profile.Q1_HIT);
                this.pieChart_Q1.Series["Pie"].Points.Add(profile.Q1_MISS);
                this.pieChart_Q1.Series["Pie"].Points[0].Label = "Hit";
                this.pieChart_Q1.Series["Pie"].Points[1].Label = "Miss";
                
                
                q1_noDataLabel.Hide();
                pieChart_Q1.Show();
            }
            else
            {
                q1_noDataLabel.Show();
                pieChart_Q1.Hide();
            }

            if (profile.Q2_HIT + profile.Q2_MISS > 10)
            {
                this.pieChart_Q2.Series["Pie"].Points.Clear();
                this.pieChart_Q2.Series["Pie"].Points.Add(profile.Q2_HIT);
                this.pieChart_Q2.Series["Pie"].Points.Add(profile.Q2_MISS);
                this.pieChart_Q2.Series["Pie"].Points[0].Label = "Hit";
                this.pieChart_Q2.Series["Pie"].Points[1].Label = "Miss";

                pieChart_Q2.Show();
                q2_noDataLabel.Hide();
            }
            else
            {
                q2_noDataLabel.Show();
                pieChart_Q2.Hide();
            }

            if (profile.Q3_HIT + profile.Q3_MISS > 10)
            {
                this.pieChart_Q3.Series["Pie"].Points.Clear();
                this.pieChart_Q3.Series["Pie"].Points.Add(profile.Q3_HIT);
                this.pieChart_Q3.Series["Pie"].Points.Add(profile.Q3_MISS);
                this.pieChart_Q3.Series["Pie"].Points[0].Label = "Hit";
                this.pieChart_Q3.Series["Pie"].Points[1].Label = "Miss";

                pieChart_Q3.Show();
                q3_noDataLabel.Hide();
            }
            else
            {
                q3_noDataLabel.Show();
                pieChart_Q3.Hide();
            }

            if (profile.Q4_HIT + profile.Q4_MISS > 10)
            {
                this.pieChart_Q4.Series["Pie"].Points.Clear();
                this.pieChart_Q4.Series["Pie"].Points.Add(profile.Q4_HIT);
                this.pieChart_Q4.Series["Pie"].Points.Add(profile.Q4_MISS);
                this.pieChart_Q4.Series["Pie"].Points[0].Label = "Hit";
                this.pieChart_Q4.Series["Pie"].Points[1].Label = "Miss";

                pieChart_Q4.Show();
                q4_noDataLabel.Hide();
            }
            else
            {
                q4_noDataLabel.Show();
                pieChart_Q4.Hide();
            }

        }

        public void updateBubblesPoppedTab()
        {
            gamesToShow.Clear();

            if (profilesComboBox2.SelectedItem != null)
            {
                if (TAPDatabase.profileExists(profilesComboBox2.SelectedItem.ToString()))
                {
                    try
                    {
                        List<GameData> games = TAPDatabase.getProfile(profilesComboBox2.SelectedItem.ToString()).gamesPlayed.ElementAt(getGameIndex(gameTypeSelection2.SelectedItem.ToString())).ElementAt(levelComboBox2.SelectedIndex);
                        List<GameData> leftGamesToShow = new List<GameData>();
                        List<GameData> rightGamesToShow = new List<GameData>();

                        bubblesPoppedChart.Series["Right"].Points.Clear();
                        bubblesPoppedChart.Series["Left"].Points.Clear();
                        

                        for (int index = 0; index < games.Count; index++)
                        {
                            if (quadrantsMatch2(games[index]) && checkedReferenceJointMatch2(games[index]))
                            {
                                if (games[index].refJoint == Microsoft.Kinect.JointType.ShoulderRight)
                                {
                                    rightGamesToShow.Add(games[index]);
                                    bubblesPoppedChart.Series["Right"].Points.Add(games[index].BubblesPopped);
                                }
                                else
                                {
                                    leftGamesToShow.Add(games[index]);
                                    bubblesPoppedChart.Series["Left"].Points.Add(games[index].BubblesPopped);
                                }
                            }
                        }

                        if (bubblesPoppedChart.Series["Left"].Points.Count > 1 || bubblesPoppedChart.Series["Right"].Points.Count > 1)
                        {
                            noDataLabel2.Hide();
                            limbSpeedDetailsGroup.Show();
                            bubblesPoppedChart.Show();
                            int leftDays = 0, rightDays = 0;

                            if (leftGamesToShow.Count > 1)
                            {
                                leftDays = (int)(leftGamesToShow[leftGamesToShow.Count - 1].Date.Day - leftGamesToShow[0].Date.Day);
                            }

                            if(rightGamesToShow.Count > 1)
                            {

                                rightDays = (int)(rightGamesToShow[rightGamesToShow.Count - 1].Date.Day - rightGamesToShow[0].Date.Day);
                            }

                            ComputeLimbSpeedDetails(bubblesPoppedChart.Series["Left"].Points, bubblesPoppedChart.Series["Right"].Points, leftDays, rightDays);

                            bubblesPoppedChart.ChartAreas[0].AxisX.Maximum = 7;
                            bubblesPoppedChart.ChartAreas[0].AxisY.Maximum = games[0].totalBubbles;
                            //this.bubblesPoppedChart.DataBind();
                        }
                        else
                        {
                            noDataLabel2.Show();
                            limbSpeedDetailsGroup.Hide();
                            bubblesPoppedChart.Hide();
                        }
                    }
                    catch (Exception e)
                    {
                        MyConsole.print(e.InnerException.ToString());
                    }
                }
            }
        }

        private void ComputeLimbSpeedDetails(System.Windows.Forms.DataVisualization.Charting.DataPointCollection leftPoints, System.Windows.Forms.DataVisualization.Charting.DataPointCollection rightPoints, int leftDays, int rightDays)
        {
            int index;
            
            float rightAverageBubbles = 0;
            float rightMeanPerGame = 0;
            float rightMeanPerDay = 0;
            float rightAbsImprovement = 0;

            float leftAverageBubbles = 0;
            float leftMeanPerGame = 0;
            float leftMeanPerDay = 0;
            float leftAbsImprovement = 0;


            if (leftPoints.Count > 1)
            {
                //=============compute left average bubbles============
                for (index = 0; index < leftPoints.Count; index++)
                {
                    leftAverageBubbles += (float)leftPoints[index].YValues.ElementAt(0);
                }

                leftAverageBubbles /= leftPoints.Count;

                //=============compute left mean per game and per day===============
                for (index = 1; index < leftPoints.Count; index++)
                {
                    leftMeanPerGame += (float)leftPoints[index].YValues.ElementAt(0) - (float)leftPoints[index - 1].YValues.ElementAt(0);
                }

                if (leftDays > 0)
                {
                    leftMeanPerDay = leftMeanPerGame / leftDays;
                }

                if (leftPoints.Count > 1)
                {
                    leftMeanPerGame /= (leftPoints.Count - 1);
                }

                //============compute left absolute improvement====================
                leftAbsImprovement = (float)(leftPoints[leftPoints.Count - 1].YValues.ElementAt(0) - leftPoints[0].YValues.ElementAt(0));

            }

            if (rightPoints.Count > 1)
            {
                //============compute right average bubbles================
                for (index = 0; index < rightPoints.Count; index++)
                {
                    rightAverageBubbles += (float)rightPoints[index].YValues.ElementAt(0);
                }

                rightAverageBubbles /= rightPoints.Count;


                //============compute right mean per game and per day======
                for (index = 1; index < rightPoints.Count; index++)
                {
                    rightMeanPerGame += (float)rightPoints[index].YValues.ElementAt(0) - (float)rightPoints[index - 1].YValues.ElementAt(0);
                }

                if (rightDays > 0)
                {
                    rightMeanPerDay = rightMeanPerGame / rightDays;
                }

                if (rightPoints.Count > 1)
                {
                    rightMeanPerGame /= (rightPoints.Count - 1);
                }


                //============compute right absolute improvement
                rightAbsImprovement = (float)(rightPoints[rightPoints.Count - 1].YValues.ElementAt(0) - rightPoints[0].YValues.ElementAt(0));
            }

            left_averageBubblesPopped.Text = leftAverageBubbles.ToString();
            left_meanImprovementPerGame.Text = leftMeanPerGame.ToString();
            left_meanImprovementPerDay.Text = leftMeanPerDay.ToString();
            left_absoluteImprovement.Text = leftAbsImprovement.ToString();
            
            right_averageBubblesPopped.Text = rightAverageBubbles.ToString();
            right_meanImprovementPerGame.Text = rightMeanPerGame.ToString();
            right_meanImprovementPerDay.Text = rightMeanPerDay.ToString();
            right_absoluteImprovement.Text = rightAbsImprovement.ToString();
        }

        private bool checkedReferenceJointMatch(GameData game)
        {
            return leftShoulderCheckBox.Checked && game.refJoint == Microsoft.Kinect.JointType.ShoulderLeft ||
                   rightShoulderCheckBox.Checked && game.refJoint == Microsoft.Kinect.JointType.ShoulderRight;
        }

        private bool checkedReferenceJointMatch2(GameData game)
        {
            return leftShoulderCheckBox2.Checked && game.refJoint == Microsoft.Kinect.JointType.ShoulderLeft ||
                   rightShoulderCheckBox2.Checked && game.refJoint == Microsoft.Kinect.JointType.ShoulderRight;
        }

        public void updateScoreTab()
        {
            gamesToShow.Clear();

            chart1.ResetAutoValues();

            if (profilesComboBox.SelectedItem != null)
            {
                if (TAPDatabase.profileExists(profilesComboBox.SelectedItem.ToString()))
                {
                    try
                    {
                        if (levelComboBox.SelectedItem == null)
                        {
                            levelComboBox.SelectedIndex = 0;
                        }

                        List<GameData> games = TAPDatabase.getProfile(profilesComboBox.SelectedItem.ToString()).gamesPlayed.ElementAt(getGameIndex(gameTypeSelection.SelectedItem.ToString())).ElementAt(levelComboBox.SelectedIndex);

                        for (int index = 0; index < games.Count; index++)
                        {
                            if (quadrantsMatch(games[index]) && checkedReferenceJointMatch(games[index]))
                            {
                                gamesToShow.Add(games[index]);
                            }
                        }

                        if (gamesToShow.Count > 0)
                        {
                            if (gamesToShow.Count > 7)
                            {
                                gamesToShow.RemoveRange(0, gamesToShow.Count - 7);
                            }

                            ComputeScoreDetails(gamesToShow);

                            this.chart1.Show();
                            noDataLabel.Hide();
                            detailsGroupBox.Show();

                            this.chart1.DataSource = gamesToShow;
                          
                            chart1.Series["Scores"].XValueMember = "GameNumber";
                            chart1.Series["Scores"].YValueMembers = "Score";
                            this.chart1.DataBind();
                        }
                        else
                        {
                            noDataLabel.Show();
                            chart1.Hide();
                            detailsGroupBox.Hide();
                        }
                    }
                    catch (Exception e)
                    {
                        MyConsole.print(e.ToString());
                    }
                }
            }
        }

        private void ComputeScoreDetails(List<GameData> gamesToShow)
        {
            float average = 0;
            float meanChangePerGame = 0;
            float meanChangePerDay = 0;
            float absoluteChange = gamesToShow[gamesToShow.Count - 1].Score - gamesToShow[0].Score;
            int percentageImp = (int) ((absoluteChange / gamesToShow[0].Score) * 100);

            //compute average
            for (int index = 0; index < gamesToShow.Count; index++)
            {
                average += gamesToShow[index].Score;
            }

            average /= gamesToShow.Count;

            //compute mean change per game
            for (int index = 1; index < gamesToShow.Count; index++)
            {
                meanChangePerGame += (gamesToShow[index].Score - gamesToShow[index - 1].Score);
            }

            if (gamesToShow.Count > 1)
            {
                meanChangePerGame /= (gamesToShow.Count - 1);
            }

            if (gamesToShow[gamesToShow.Count - 1].Date.Day - gamesToShow[0].Date.Day > 0)
            {
                meanChangePerDay = absoluteChange / (gamesToShow[gamesToShow.Count - 1].Date.Day - gamesToShow[0].Date.Day);
            }

            label_AverageScore.Text = average.ToString();
            label_meanChangePerGame.Text = meanChangePerGame.ToString();
            label_meanChangePerDay.Text = meanChangePerDay.ToString();
            label_absoluteChange.Text = absoluteChange.ToString() + " || " + percentageImp.ToString() + "%";
        }

        private bool quadrantsMatch(GameData gameData)
        {
            return showAllQCheckBox.Checked ||
                   (gameData.quadrants[0] == q1CheckBox.Checked &&
                   gameData.quadrants[1] == q2CheckBox.Checked &&
                   gameData.quadrants[2] == q3CheckBox.Checked &&
                   gameData.quadrants[3] == q4CheckBox.Checked);
        }

        private bool quadrantsMatch2(GameData gameData)
        {
            return showAllCheckBox2.Checked ||
                   (gameData.quadrants[0] == q1CheckBox2.Checked &&
                   gameData.quadrants[1] == q2CheckBox2.Checked &&
                   gameData.quadrants[2] == q3CheckBox2.Checked &&
                   gameData.quadrants[3] == q4CheckBox2.Checked);
        }

        private void ScoreChart_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void gameTypeSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void levelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void profilesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void rightShoulderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (showAllQCheckBox.Checked)
            {
                q1CheckBox.Enabled = false;
                q2CheckBox.Enabled = false;
                q3CheckBox.Enabled = false;
                q4CheckBox.Enabled = false;
            }
            else
            {
                q1CheckBox.Enabled = true;
                q2CheckBox.Enabled = true;
                q3CheckBox.Enabled = true;
                q4CheckBox.Enabled = true;
            }

            UpdateFromDB();
        }

        private void leftShoulderCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void gameTypeSelection2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox5_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {
            if (showAllCheckBox2.Checked)
            {
                q1CheckBox2.Enabled = false;
                q2CheckBox2.Enabled = false;
                q3CheckBox2.Enabled = false;
                q4CheckBox2.Enabled = false;
            }
            else
            {
                q1CheckBox2.Enabled = true;
                q2CheckBox2.Enabled = true;
                q3CheckBox2.Enabled = true;
                q4CheckBox2.Enabled = true;
            }

            UpdateFromDB();
        }

        private void rightShoulderCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void levelComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void noDataLabel2_Click(object sender, EventArgs e)
        {

        }

        private void viewGameNumber_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void viewDay_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFromDB();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
