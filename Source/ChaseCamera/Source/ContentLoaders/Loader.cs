using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace ChaseCameraSample
{
    public class Loader
    {
        private WaveInfo waveInfo;
        public WaveInfo WaveInfo
        {
            get { return waveInfo; }
        }

        private EnvironmentInfo environmentInfo;
        public EnvironmentInfo EnvironmentInfo
        {
            get { return environmentInfo; }
        }

        private LeaderBoard leaderBoard;
        public LeaderBoard LeaderBoard
        {
            get { return leaderBoard; }
        }

        // Constructor
        public Loader()
        {
            waveInfo = new WaveInfo();
            environmentInfo = new EnvironmentInfo();
            leaderBoard = new LeaderBoard();
        }

        #region Wave Information
        public bool ReadWaveInfo(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    waveInfo = (WaveInfo)new XmlSerializer(typeof(WaveInfo)).Deserialize(reader.BaseStream);
                }
                return true;
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: XML File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
                return false;
            }
        }
        #endregion

        #region Environment Information
        public EnvironmentInfo ReadEnvironmentInfo(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    return environmentInfo = (EnvironmentInfo)new XmlSerializer(typeof(EnvironmentInfo)).Deserialize(reader.BaseStream);
                }
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: XML File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
                return null;
            }
        }
        #endregion

        #region LeaderBoard
        public void ReadLeaderBoard(string filename)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {

                    leaderBoard = (LeaderBoard)new XmlSerializer(typeof(LeaderBoard)).Deserialize(reader.BaseStream);
                }
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: XML File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
            }
        }

        public void WriteLeaderBoard(List<int> scores, string filename)
        {
            leaderBoard.Scores = scores;
            StreamWriter writer = new StreamWriter(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(LeaderBoard));
            serializer.Serialize(writer, this.leaderBoard);
            writer.Close();
        }
        #endregion

        public WaveInfo ReadXML(string filename, int i =0)
        {
            WaveInfo waveInfo;
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    return waveInfo = (WaveInfo)new XmlSerializer(typeof(WaveInfo)).Deserialize(reader.BaseStream);
                }
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: XML File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
                return null;
            }
        }

        
    }

}
