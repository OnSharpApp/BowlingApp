using ConsoleBowlingApp;

class BowlingProgram 
{
   
    static void Main(string[] args)
    {
        BowlingScoreCard bowlingScoreCard = new BowlingScoreCard();
        string input;
        int userThrow;

        Console.WriteLine("Welcome to the Bowling Scorecard!\n");
        
        while (bowlingScoreCard.frame <= 10)
        {
            int grandTotal = bowlingScoreCard.CalculateScore();
            Console.WriteLine($"Score: {grandTotal}");
            Console.WriteLine($"Frame: {bowlingScoreCard.frame}");            

            bool continueBowling = true;
            bowlingScoreCard.CheckForFinalThrow(ref continueBowling);

            if (continueBowling)
            {
                //Get Users Throw Input
                Console.WriteLine("Please enter the number of pins you knocked down");
                input = Console.ReadLine();
                // Get Remaining Pins
                List<int> currentFrame = bowlingScoreCard.scoreCard.GetFrame(bowlingScoreCard.frame - 1);
                int remainingPins = 10;
                if (bowlingScoreCard.frame < 10 && currentFrame?.Count == 1)
                {
                    remainingPins = 10 - currentFrame[0];
                }
                if (bowlingScoreCard.frame == 10 && currentFrame?.Count == 1)
                {
                    if (currentFrame[0] < 10)
                    {
                        remainingPins = 10 - currentFrame[0];
                    }
                }
                // Verify Valid Input and Add Score
                if (Int32.TryParse(input, out userThrow) && (1 <= userThrow && userThrow <= 10) && userThrow <= remainingPins)
                {
                        userThrow = Int32.Parse(input.Trim());
                        bowlingScoreCard.AddThrow(userThrow);
                        Console.WriteLine("");
                }
                else { Console.WriteLine($"Input not accepted, please enter a number between 0 and {remainingPins}.\n"); }
            }
        }        
    }
}

class BowlingScoreCard
{
    public ScoreCard scoreCard = new ScoreCard();
    public int frame = 1;
    public void AddThrow(int userThrow)
    {
        List<int> frameScores = scoreCard.GetFrame(frame - 1);

        if (frameScores == null) // First Throw
        {
            // Update Current Frame            
            scoreCard.SetFrames(frame - 1, new List<int>() { userThrow });

            if (frame > 2)
            {
                List<int> backTwoFrames = scoreCard.GetFrame(frame - 3);
                List<int> backOneFrame = scoreCard.GetFrame(frame - 2);

                // Check for two strikes before
                if (backTwoFrames != null && backTwoFrames[0] == 10 && backOneFrame != null && backOneFrame[0] == 10)
                {
                    List<int> frameGen = new List<int> { backTwoFrames[0], backTwoFrames[1], userThrow };
                    scoreCard.SetFrames(frame - 3, frameGen);
                }
            }

            if (frame > 1)
            {
                List<int> backOneFrame = scoreCard.GetFrame(frame - 2);
                // Check for strike one before
                if (backOneFrame != null && backOneFrame[0] == 10)
                {
                    List<int> frameGen = new List<int> { backOneFrame[0], userThrow };
                    scoreCard.SetFrames(frame - 2, frameGen);
                }
                // Check for Spare one frame before               
                if (backOneFrame?.Count == 2 && (backOneFrame[0] + backOneFrame[1] == 10) && backOneFrame[0] != 10)
                {
                    List<int> frameGen = new List<int> { backOneFrame[0], backOneFrame[1], userThrow };
                    scoreCard.SetFrames(frame - 2, frameGen);
                }
            }
            if (userThrow == 10)
            {
                if (frame != 10)
                {
                    frame++;
                }
            }

        }
        else if(frameScores.Count == 1) // Second Throw
        {
            
            // Update Current Frame
            List<int> currentFrame = scoreCard.GetFrame(frame - 1);            
            List<int> secondThrowGen = new List<int> { currentFrame[0], userThrow };
            scoreCard.SetFrames(frame - 1, secondThrowGen);

            // Check for Strike
            if (frame > 1)
            {
                List<int> backOneFrame = scoreCard.GetFrame(frame - 2);
                if (backOneFrame[0] == 10)
                {
                    List<int> frameGen = new List<int> { backOneFrame[0], backOneFrame[1], userThrow };
                    scoreCard.SetFrames(frame - 2, frameGen);
                }
            }

            if (frame != 10)
            {
                frame++;
            }
            
        }
        else if (frameScores.Count == 2 && frame == 10) // Third throw. 10th Frame only
        {
            FinalThrow(frameScores, userThrow);
        }
    }

    public void FinalThrow(List<int> frameScores, int userThrow)
    {
        if (frameScores[0] == 10 || frameScores[0] + frameScores[1] == 10) // Third throw. 10th Frame only if Strike or Spare
        {
            List<int> frameGen = new List<int> { frameScores[0], frameScores[1], userThrow };
            scoreCard.SetFrames(9, frameGen);
            int GrandTotal = CalculateScore();
            Console.WriteLine($"\n Final Score: {GrandTotal}");
            frame++;
        }
        else
        {
            int GrandTotal = CalculateScore();
            Console.WriteLine($"\n Final Score: {GrandTotal}");
            frame++;
        }
    }

    public void CheckForFinalThrow(ref bool continueBowling)
    {
        if (frame == 10)
        {
            List<int> frameScores = scoreCard.GetFrame(frame - 1);
            if (frameScores?.Count == 2 && (frameScores[0] != 10 && frameScores[0] + frameScores[1] != 10))
            {
                int GrandTotal = CalculateScore();
                Console.WriteLine($"\n Final Score: {GrandTotal}");
                frame++;
                continueBowling = false;
            }
        }
    }

    public int CalculateScore() {
        List<int>[] scores = scoreCard.GetFrames();
        int total = 0;

        foreach (List<int> frame in scores.Where(frame => frame != null))
        {
            foreach (int score in frame)
            {
                total += score;
            }
        }
        return total;
    }
}