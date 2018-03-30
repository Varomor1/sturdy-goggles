using System;
using System.Threading;

namespace Tetris
{
    class Program
    {


        static void Main(string[] args)
        {
            Config.Initialize();

            Game1 game = new Game1();
            game.Run();
        }
    }


    public static class Test
    {

        public static void TestMethod(){

            Console.WriteLine("testing each blocktype ToString method...");
            for(int iii = 0; iii < 7; iii++){
                Console.WriteLine(Config.GetBlockType(iii).ToString());
            }

            Console.WriteLine("creating new field...");
            Field testField = new Field();

            Console.WriteLine("field ToString: \n" + testField.ToString());

            
            Console.WriteLine("rotation block 1 right: \n");
            testField.RotateRight();
            Console.WriteLine("field ToString: \n" + testField.ToString());

            Console.WriteLine("moving right 5 times: \n");
            Console.WriteLine(testField.MoveRight());
            Console.WriteLine(testField.MoveRight());
            Console.WriteLine(testField.MoveRight());
            Console.WriteLine(testField.MoveRight());
            Console.WriteLine(testField.MoveRight());
            
            Console.WriteLine("field ToString: \n" + testField.ToString());

            Console.WriteLine("moving right 1 time: \n");
            Console.WriteLine(testField.MoveRight());


            Console.WriteLine("rotation block 1 right: \n");
            testField.RotateRight();
            Console.WriteLine("field ToString: \n" + testField.ToString());

            Console.WriteLine("moving right 1 time: \n");
            Console.WriteLine(testField.MoveRight());
            Console.WriteLine("field ToString: \n" + testField.ToString());

            Console.WriteLine("moving right 1 time: \n");
            Console.WriteLine(testField.MoveRight());
            Console.WriteLine("field ToString: \n" + testField.ToString());

            for(int iii = 0; iii < 70; iii++){
                Console.WriteLine("moving down... \n");
                Console.WriteLine(testField.MoveDown());
                Console.WriteLine("field ToString: \n" + testField.ToString());
                Thread.Sleep(100);
            }

            Console.WriteLine("177/12 = " + (177/12));

            testField.Restart();
            
            Console.WriteLine("field ToString: \n" + testField.ToString());
        }
            
    }
}
