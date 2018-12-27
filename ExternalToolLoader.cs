using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
/*
 * Creator: Ricky Norris
 */
namespace Take_The_Bait
{
    //This class wil simply be created and then run a method that will take a player object and write their data into a  
    //text file. Which you will then be able to load into the game
    class ExternalToolLoader
    {
        public string[] Load()
        {
            const char DEL = '\xE001';
            //Read the stream in
            StreamReader loadState = new StreamReader("save.txt");

            string line;
            string[] readData = new string[3];
            if ((line = loadState.ReadLine()) != null)
            {
                // Add to list.
                readData = line.Split(DEL);
                //Console.WriteLine(line); // Write to console
            }
            loadState.Close();
            return readData;

        }
    }
}
