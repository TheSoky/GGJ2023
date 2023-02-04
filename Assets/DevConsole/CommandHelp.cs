using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandHelp : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public CommandHelp()
        {
            Name = "Help";
            Command = "help";
            Description = "Used to display list of avalable commands.";
            Help = "You can also specify specific command using 'XXXXX -help' for aditional help.";
            
            AddCommandToConsole();
        }


        public override void RunCommand(string[] args)
        {
            DeveloperConsole.Instance.AddMessageToConsole("help   --> Displays this list of commands\n" +
                                                          "clear  --> Clears the Terminal\n" +
                                                          "cube   --> Spawns a cube\n" + 
                                                          "speed  --> Changes the speed of an player" +
                                                          "quit   --> Leaves the game / Quits the game");
        }

        public static CommandHelp CreateCommand()
        {
            return new CommandHelp();
        }
    }
}