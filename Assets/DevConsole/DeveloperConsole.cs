using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Console
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }
        public abstract string Help { get; protected set; }

        public void AddCommandToConsole()
        {
            string addMessage = " command has been added to the console.</color>";

            DeveloperConsole.AddCommandsToConsole(Command, this);
            Debug.Log("<color=lime>" + Name + addMessage);
        }

        public abstract void RunCommand(string[] args);
    }

    public class DeveloperConsole : MonoBehaviour
    {
        public static DeveloperConsole Instance { get; private set; }
        public static Dictionary<string, ConsoleCommand> Commands { get; private set; }

        [Header("UI Components")]
        public Canvas consoleCanvas;
        public Text consoleText;
        public Text inputText;
        public InputField consoleInput;

        private void Awake()
        {
            if(Instance != null)
            {
                return;
            }

            Instance = this;
            Commands = new Dictionary<string, ConsoleCommand>();
        }

        private void Start()
        {
            consoleCanvas.gameObject.SetActive(false);
            CreateCommands();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logMessage, string stackTrace, LogType type)
        {
            string _message = "<color=cyan>[" + type.ToString() + "]</color> " + logMessage;
            AddMessageToConsole(_message);
        }

        private void CreateCommands()
        {
            CommandQuit.CreateCommand();
            CommandCube.CreateCommand();
            CommandClear.CreateCommand();
            CommandHelp.CreateCommand();
        }

        public static void AddCommandsToConsole(string _name, ConsoleCommand _command)
        {
            if(!Commands.ContainsKey(_name))
            {
                Commands.Add(_name, _command);
            }
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F1))
            {
                consoleCanvas.gameObject.SetActive(!consoleCanvas.gameObject.activeInHierarchy);
            }

            if(consoleCanvas.gameObject.activeInHierarchy)
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    if(inputText.text != "")
                    {
                        AddMessageToConsole(">"+inputText.text);
                        ParseInput(inputText.text);
                    }
                    consoleInput.text = "";
                }
            }
        }

        public void AddMessageToConsole(string msg)
        {
            consoleText.text += msg + "\n";
        }

        public void ClearConsole()
        {
            consoleText.text = "";
        }

        private void ParseInput(string input)
        {
            string[] _input = input.Split(' ');

            if (_input.Length == 0 || _input == null)
            {
                Debug.LogWarning("<color=red>Unknown command. Type 'help' for a list of commands.</color>");
                return;
            }

            if (!Commands.ContainsKey(_input[0]))
            {
                Debug.LogWarning("<color=red>Unknown command. Type 'help' for a list of commands.</color>");
            }
            else
            {
                // Create a list to leverage Linq
                List<string> args = _input.ToList();

                // Remove index 0 (the command)
                args.RemoveAt(0);

                // Check if '-help' was passed
                if (args.Contains("-help"))
                {
                    AddMessageToConsole("==============================");
                    AddMessageToConsole(Commands[_input[0]].Description);
                    AddMessageToConsole("------------------------------");
                    AddMessageToConsole(Commands[_input[0]].Help);
                    AddMessageToConsole("==============================");

                    return;
                }

                // Run the command & pass args
                Commands[_input[0]].RunCommand(args.ToArray());
            }
        }
    }
}