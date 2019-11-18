using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class WavePattern
    {
        private int height = 0;
        public int Height
        {
            get { return height; }
        }

        private int width = 0;
        public int Width
        {
            get { return width; }
        }
        
        private string line1;
        public string Line1
        {
            get { return line1; }
            set
            {
                height = 0;
                width = 0;
                patterns.Clear();
                line1 = value;
                height++;
                width = Math.Max(value.Length,width);
                patterns.Add(value);
            }
        }

        private string line2;
        public string Line2
        {
            get { return line2; }
            set
            {
                line2 = value;
                height++;
                width = Math.Max(value.Length, width);
                patterns.Add(value);
            }
        }

        private string line3;
        public string Line3
        {
            get { return line3; }
            set
            {
                line3 = value;
                height++;
                width = Math.Max(value.Length, width);
                patterns.Add(value);
            }
        }

        private string line4;
        public string Line4
        {
            get { return line4; }
            set
            {
                line4 = value;
                height++;
                width = Math.Max(value.Length, width);
                patterns.Add(value);
            }
        }

        private string line5;
        public string Line5
        {
            get { return line5; }
            set
            {
                line5 = value;
                height++;
                width = Math.Max(value.Length, width);
                patterns.Add(value);
            }
        }

        private string line6;
        public string Line6
        {
            get { return line6; }
            set
            {
                line6 = value;
                height++;
                width = Math.Max(value.Length, width);
                patterns.Add(value);
            }
        }

        private string line7;
        public string Line7
        {
            get { return line7; }
            set
            {
                line7 = value;
                height++;
                width = Math.Max(value.Length, width);
                patterns.Add(value);
            }
        }


        private List<string> patterns;
        public List<string> Patterns
        {
            get { return patterns; }
            set
            {
                patterns = value;
            }
        }

        public WavePattern()
        {
            patterns = new List<string>();
        }
    }
}
