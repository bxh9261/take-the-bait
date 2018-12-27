using System;

namespace Take_The_Bait
{
    /*
     * Creator: Brad Hanel
     * Name: Program
     * Summary: runs the game that's about it
    */
    public static class Program
    {
        //entry point for the game 
        [STAThread]
        static void Main()
        {
            using (var game = new Take_The_Bait())
                game.Run();
        }
    }
}
