using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBowlingApp
{
    public class ScoreCard
    {

        private List<int>[] frames = new List<int>[10]; // Using a jagged array since the frame size is variable based on future frames

        public List<int>[] GetFrames()
        {
            return frames;
        }
        public List<int> GetFrame(int frame)
        {
            return frames[frame];
        }

        public void SetFrames(int frame, List<int> pins)
        {
            frames[frame] = pins;
        }
    }
}
