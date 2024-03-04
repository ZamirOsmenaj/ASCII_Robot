using System;
using System.Timers;

namespace ASCII_Robot
{
    public class Robot
    {
        // Fields for the coordinates of the center of the robot
        private int xCoord;
        private int yCoord;

        // An event that triggers when battery level is below 10% using the build-in delegate EventHandler
        public event EventHandler BatteryLow;

        // Properties for the robot's name and battery level
        public string Name { get; set; }

        public int BatteryLevel { get; set; }

        // A timer that decreases the battery level every 10 seconds
        private System.Timers.Timer timer;

        /* Alternative way of using delegates */
        // A list of RobotAction delegates
        private List<RobotAction> actions;

        // Robot's Constructor based on user's input
        public Robot(string name, int x, int y)
        {
            this.Name = name;
            this.xCoord = x;
            this.yCoord = y;

            this.BatteryLevel = 100; // Battery level starts at 100%
            this.timer = new System.Timers.Timer(10000); // A timer with 10 seconds interval. Ticks every 10 seconds
            this.timer.Elapsed += OnTimerElapsed; // Attach the OnElapsedTimer method to Elapsed event
            this.timer.Enabled = true; // Start the timer

            /* Alternative way of using delegates */
            // this.actions = new List<RobotAction>();
        }

        // Method that raised the OnBatteryLow event
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (this.BatteryLevel > 0)
            {
                this.BatteryLevel--; // Decrease battery level by 1%
            }

            if (this.BatteryLevel < 10)
            {
                // Raise BatteryLow event
                OnBatteryLow(EventArgs.Empty);
            }
        }

        // Method that raises the BatteryLow event
        protected virtual void OnBatteryLow(EventArgs e)
        {
            BatteryLow?.Invoke(this, e); // this is refered to the specific object
        }

        // Indexer
        public string this[string bodyPart] => FindBodyPart(bodyPart);

        private string FindBodyPart(string bodyPart)
        {
            string message = "The coordinates of where the " + bodyPart + " is, are: ";
            int tmpCoordX = 0;
            int tmpCoordY = 0;

            switch (bodyPart)
            {
                case "head":
                    tmpCoordX = this.xCoord - 2;
                    tmpCoordY = this.yCoord - 2;
                    message += "(" + tmpCoordX.ToString() + "," + tmpCoordY.ToString() + ")\n";
                    break;
                case "body":
                    tmpCoordX = this.xCoord - 1;
                    tmpCoordY = this.yCoord + 1;
                    message += "(" + tmpCoordX.ToString() + "," + tmpCoordY.ToString() + ")\n";
                    break;
                case "arms":
                    tmpCoordX = this.xCoord - 3;
                    tmpCoordY = this.yCoord + 1;
                    message += "(" + tmpCoordX.ToString() + "," + tmpCoordY.ToString() + ")\n";
                    break;
                case "legs":
                    tmpCoordX = this.xCoord - 1;
                    tmpCoordY = this.yCoord + 3;
                    message += "(" + tmpCoordX.ToString() + "," + tmpCoordY.ToString() + ")\n";
                    break;
                default:
                    message = "Invalid option!\n";
                    break;
            }

            return message;
        }

        // Method that recharges the battery
        public void Recharge()
        {
            Console.WriteLine($"Current battery level: {this.BatteryLevel}%");
            this.BatteryLevel = 100;
            Console.WriteLine("Recharging...");
            Thread.Sleep(4000);
            Console.WriteLine($"Robot: ({this.Name}) is recharged. Battery level is now: {this.BatteryLevel}%");
            Thread.Sleep(3000);
        }

        // Method that draws robot's head
        public void DrawHead()
        {
            Console.SetCursorPosition(xCoord - 2, yCoord - 2); // Move the cursor to the top left corner of the head
            Console.Write(" ___ ");
            Console.SetCursorPosition(xCoord - 2, yCoord - 1);
            Console.Write("|o_o|"); // Draw the eyes
            Console.SetCursorPosition(xCoord - 2, yCoord);
            Console.Write("|___|"); // Draw the mouth
        }

        // Method that draws the robot's body
        public void DrawBody()
        {
            Console.SetCursorPosition(xCoord - 1, yCoord + 1);
            Console.Write("| |"); // Draw the torso
            Console.SetCursorPosition(xCoord - 1, yCoord + 2);
            Console.Write("|_|"); // Draw the waist
        }

        // Method that draws the robot's arms
        public void DrawArms()
        {
            Console.SetCursorPosition(xCoord - 3, yCoord + 1);
            Console.Write("/"); // Draw the left arm
            Console.SetCursorPosition(xCoord + 3, yCoord + 1);
            Console.Write("\\"); // Draw the right arm
        }

        // Method that draws the robot's legs
        public void DrawLegs()
        {
            Console.SetCursorPosition(xCoord - 1, yCoord + 3);
            Console.Write("/ \\"); // Draw the legs
        }

        // Method that draws the robot
        public void Draw()
        {
            DrawHead();
            DrawBody();
            DrawArms();
            DrawLegs();

            Console.WriteLine();
        }

        /* Alternative way of using delegates */
        // Method that adds an action to the list of actions that the robot can perform
        /* public void AddAction(RobotAction action)
        {
            actions.Add(action);
        } */

        // Method that makes the robot wave. We can also say that he cheers
        public void Wave()
        {
            // Save the original positions of the arms
            int leftArmX = xCoord - 3;
            int rightArmX = xCoord + 3;
            int armY = yCoord + 1;

            // Move the arms up and down
            for (int i = 0; i < 5; i++)
            {
                // Erase the arms
                Console.SetCursorPosition(leftArmX, armY);
                Console.Write(" ");
                Console.SetCursorPosition(rightArmX, armY);
                Console.Write(" ");

                // Move the arms up
                armY--;
                Console.SetCursorPosition(leftArmX, armY);
                Console.Write("\\");
                Console.SetCursorPosition(rightArmX, armY);
                Console.Write("/");

                // Wait for a while
                Thread.Sleep(500);

                // Erase the arms
                Console.SetCursorPosition(leftArmX, armY);
                Console.Write(" ");
                Console.SetCursorPosition(rightArmX, armY);
                Console.Write(" ");

                // Move the arms down
                armY++;
                Console.SetCursorPosition(leftArmX, armY);
                Console.Write("/");
                Console.SetCursorPosition(rightArmX, armY);
                Console.Write("\\");

                // Wait for a while
                Thread.Sleep(500);
            }
        }

        // Method that makes the robot blink his eyes
        public void BlinkEyes()
        {
            // Save the original positions of the eyes
            int eyesPosX = xCoord - 2;
            int eyesPosY = yCoord - 1;

            // Blink eyes
            for (int i = 0; i < 5; i++)
            {
                DontBlinkEye(eyesPosX, eyesPosY);

                BlinkLeftEye(eyesPosX, eyesPosY);

                DontBlinkEye(eyesPosX, eyesPosY);

                BlinkRightEye(eyesPosX, eyesPosY);

            }

            // Restore the eyes to the original state
            DontBlinkEye(eyesPosX, eyesPosY);
        }

        // Method that handles the blinking of left eye
        public void BlinkLeftEye(int eyesPosX, int eyesPosY)
        {
            // Blink left eye
            Console.SetCursorPosition(eyesPosX, eyesPosY);
            Console.Write("|-_o|"); // Draw the eyes

            // Wait for a while
            Thread.Sleep(500);
        }

        // Method that handles the blinking of right eye
        public void BlinkRightEye(int eyesPosX, int eyesPosY)
        {
            // Blink right eye
            Console.SetCursorPosition(eyesPosX, eyesPosY);
            Console.Write("|o_-|"); // Draw the eyes

            // Wait for a while
            Thread.Sleep(500);
        }

        // Method that keep both eyes open
        public void DontBlinkEye(int eyesPosX, int eyesPosY)
        {
            // Both eyes opened
            Console.SetCursorPosition(eyesPosX, eyesPosY);
            Console.Write("|o_o|"); // Draw the eyes

            // Wait for a while
            Thread.Sleep(500);
        }

        // Move cursor to the largest position between the two objects/robots based on their coordinates
        public void changeCursor(Robot r)
        {
            int maxX = this.xCoord > r.xCoord ? this.xCoord : r.xCoord;
            int maxY = this.yCoord > r.yCoord ? this.yCoord : r.yCoord;
            Console.SetCursorPosition(maxX - 1, maxY + 3);
            Console.WriteLine("\n");
        }

        // An operator that combines two robots into a new robot
        public static Robot operator +(Robot r1, Robot r2)
        {
            // Merge names of the two robots to create a new name for the new robot
            string mergedRobotName = r1.Name + r2.Name;

            // Average coordinates of the two robots
            int x = (r1.xCoord + r2.xCoord) / 2;
            int y = (r1.yCoord + r2.yCoord) / 2;

            Robot r3 = new Robot(mergedRobotName, x, y);

            // Average the battery levels
            r3.BatteryLevel = (r1.BatteryLevel + r2.BatteryLevel) / 2;


            return r3;
        }
    }
}
