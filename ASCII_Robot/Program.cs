using System;
using System.Text.RegularExpressions;

namespace ASCII_Robot
{
    // A delegate that is used for an action that the robot can perform
    public delegate void RobotAction();

    internal class Program
    {
        // Method that handles the BatteryLow event
        static void OnBatteryLow(object sender, EventArgs e)
        {
            // Cast the sender to a ASCII Robot object
            Robot r = sender as Robot;
            // Inform the user that the robot's battery low (below 10%)
            Console.WriteLine($"{r.Name}'s battery is less than 10%.");
        }

        static void Main(string[] args)
        {
            const int NUMBER_OF_ROBOTS = 2;

            int xAxis, yAxis;
            string textFormatCoord, robotName;
            string[] splitedTextFormatCoord;

            Robot[] robot = new Robot[NUMBER_OF_ROBOTS];

            for (int i = 0; i < NUMBER_OF_ROBOTS; i++)
            {
                do
                {
                    Console.Write("Enter the coordinates of the center of the robot in format (x,y): ");
                    textFormatCoord = Console.ReadLine();

                } while (!Regex.IsMatch(textFormatCoord, "^\\d+[,]\\d+$"));

                /* 
                    ^       :   the beginning of the string
                    \d+     :   digits (0-9) (one or more occurencies). This is shorthand for \d{1,}
                    [,]     :   followed by character: ',' (comma). It is the same with using \\, (\\ = \) 
                    \d+     :   digits (0-9) (one or more occurencies). This is shorthand for \d{1,}
                    $       :   before an optional \n, and the end of the string
                */

                splitedTextFormatCoord = textFormatCoord.Split(',');

                xAxis = Convert.ToInt32(splitedTextFormatCoord[0]);
                yAxis = Convert.ToInt32(splitedTextFormatCoord[1]);

                Console.Write("Enter name of Robot: ");
                robotName = Console.ReadLine();

                robot[i] = new Robot(robotName, xAxis, yAxis);
                robot[i].BatteryLow += OnBatteryLow; // register with an event

                /* Alternative way of using delegates 
                 
                We create and use a list of delegates inside the Robot class
                So we add every method to this list of delegates

                // Register actions to currect robot 
                robot[i].AddAction(robot[i].Wave);
                robot[i].AddAction(robot[i].BlinkEyes);

                */

                Console.WriteLine();
            }

            // Call method to draw the created robots
            DrawRobots(robot);

            // Call method to move cursor to the largest position between the two objects
            robot[0].changeCursor(robot[1]);

            // Variables used for implementing the logic of program 
            int robotIndex;

            string name, action, selectedBodyPart;
            string[] actions = ["info", "recharge", "merge", "quit", "wave and blink eyes",];
            // // The below string array is used when we want to make actions seperately
            // // and not together. User selects if wants the robot to only wave or blink his eyes. 
            // string[] actions = ["info", "recharge", "merge", "quit", "wave", "blink eyes"];


            // Main use of delegates:
            // We create a delegate and we assign the delegate to the methods created to Robot class.
            // We are using multicast and the robot makes both actions (wave & blink eyes) 
            RobotAction robotActionDelegate = new RobotAction(robot[0].Wave);
            robotActionDelegate += new RobotAction(robot[0].BlinkEyes);

            // Promt menu and make actions
            while (true)
            {
                Console.Write("\nWhat is your name? ");
                name = Console.ReadLine();
                Console.WriteLine($"Hello {name}");

                Console.Write("What do you want to do? (info, wave and blink eyes, recharge, merge, quit): ");
                // Use when separate actions are selected (distinct wave and blink eyes)
                // Console.Write("What do you want to do? (info, recharge, merge, quit, wave, blink eyes,): ");
                action = Console.ReadLine();

                if (action.Equals(actions[0])) // info. User can have access to a specific part of the robot's body using [indexer]
                {
                    Console.Write("Which part of robot's body you want to see its coordinates? (head, body, arms, legs): ");
                    selectedBodyPart = Console.ReadLine();

                    Console.Write(robot[0][selectedBodyPart]);

                    Thread.Sleep(3000);
                    // Modify terminal
                    AdjustTerminal(robot);
                }
                else if (action.Equals(actions[1])) // Recharge robot
                {
                    robotIndex = RobotSelected(actions[1]);
                    robot[robotIndex].Recharge();

                    AdjustTerminal(robot);

                }
                else if (action.Equals(actions[2])) // Merge robots
                {
                    Robot robot3 = robot[0] + robot[1];
                    Console.Clear();
                    robot3.Draw();

                    Thread.Sleep(3000);
                    AdjustTerminal(robot);
                }
                else if (action.Equals(actions[3])) // Terminate program
                {
                    Console.WriteLine($"Goodbye {name}!");
                    break;
                }
                else if (action.Equals(actions[4])) // Wave and blink eyes action or just wave if we choose executing specific action
                {
                    robotIndex = RobotSelected(actions[4]);
                    // Robot executes actions serially based on the order the delegate was assigned to them
                    robotActionDelegate();

                    // // Execute a specific action based on the user's choice through delegate
                    // RobotAction robotActionDelegate = robot[robotIndex].Wave;
                    // robotActionDelegate();

                    // Alternative way of using delegate via the list of delegates
                    // as the delegate was assigned to this method
                    // robot[robotIndex].Wave();

                    AdjustTerminal(robot);
                }
                /* else if (action.Equals(actions[5])) // Blink eyes action. If we use the logic of executing a specific action
                {                                      // we introduced above
                    robotIndex = RobotSelected(actions[5]);

                    // Execute a specific action based on the user's choice through delegate
                    // RobotAction robotActionDelegate = robot[robotIndex].Wave;
                    // robotActionDelegate();

                    // Modify terminal
                    AdjustTerminal(robot);
                }*/
                else // Invalid input
                {
                    Console.WriteLine("Invalid option! Please try again.");
                }
            }
        }

        // Robot selection method to apply action
        public static int RobotSelected(string action)
        {
            int choice;

            do
            {
                Console.Write($"Which robot do you want to {action}? (1 or 2): ");
                choice = Convert.ToInt32(Console.ReadLine());

            } while (choice < 1 && choice > 2);

            return choice - 1;
        }

        // Method to print all robots (in our case: two robots)
        public static void DrawRobots(Robot[] robots)
        {
            for (int i = 0; i < robots.Length; i++)
            {
                robots[i].Draw();
            }
        }

        // Modify terminal, cause curson moves here and there
        public static void AdjustTerminal(Robot[] robots)
        {
            Console.Clear();
            DrawRobots(robots);
            robots[0].changeCursor(robots[1]);
        }
    }
}


