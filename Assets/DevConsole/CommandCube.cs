using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Console
{
    public class CommandCube : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public CommandCube()
        {
            Name = "Cube";
            Command = "cube";
            Description = "Instantiates a Cube.";
            Help = "Options: \n" +
                    "-x=<value> where <value is the integer value of the x coordinate>\n" +
                    "-y=<value> where <value is the integer value of the y coordinate>\n" +
                    "-z=<value> where <value is the integer value of the z coordinate>\n" +
                    "-xr=<value> where <value is the integer value of the x rotation>\n" +
                    "-yr=<value> where <value is the integer value of the y rotation>\n" +
                    "-zr=<value> where <value is the integer value of the z rotation>";

            AddCommandToConsole();
        }

        public override void RunCommand(string[] args)
        {
            // Variables for Instantiation
            int x = 0, y = 0, z = 0;
            float xr = 0, yr = 0, zr = 0;

            for (int i = 0; i < args.Length; i++)
            {
                string argument = args[i];

                string[] argSplit = Regex.Split(argument, @"\=");

                // Find variable and change
                switch(argSplit[0])
                {
                    case "-x":
                        x = int.Parse(argSplit[1]);
                        break;
                    case "-y":
                        y = int.Parse(argSplit[1]);
                        break;
                    case "-z":
                        z = int.Parse(argSplit[1]);
                        break;
                    case "-xr":
                        xr = int.Parse(argSplit[1]);
                        break;
                    case "-yr":
                        yr = int.Parse(argSplit[1]);
                        break;
                    case "-zr":
                        zr = int.Parse(argSplit[1]);
                        break;
                }
            }

            // Load Prefab
            GameObject prefab = (GameObject)Resources.Load("Cube");

            // Instantiate
            GameObject cube = MonoBehaviour.Instantiate(prefab, new Vector3(x,y,z), Quaternion.Euler(xr,yr,zr));
        }

        public static CommandCube CreateCommand()
        {
            return new CommandCube();
        }
    }
}