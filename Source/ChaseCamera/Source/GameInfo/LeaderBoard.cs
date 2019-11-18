using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChaseCameraSample
{
    public class LeaderBoard
    {
        private List<int> scores = new List<int>();
        public List<int> Scores
        {
            get {
                return scores;
            }
            set
            {
                scores = value;
            }
        }
    }
}
