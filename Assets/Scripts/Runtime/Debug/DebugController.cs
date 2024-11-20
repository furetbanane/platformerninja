using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    public PlayerController playerController;
    
    private bool showConsole;
    private bool showHelp;

    private string input;

    public static DebugCommand HELP;
    public static DebugCommand SPAWN;

    private Vector3 playerPositionAtStart;
    
    public static DebugCommand<int> SET_PROJECTILE;
    public static DebugCommand<int> ADD_PROJECTILE;

    public List<object> commandList;

    private void Start()
    {
        playerPositionAtStart = playerController.gameObject.transform.position;
    }

    private void Awake()
    {
        HELP = new DebugCommand("help", "Show the list of commands", "help", () =>
        {
            showHelp = true;
        });
        
        SPAWN = new DebugCommand("spawn", "teleports the player to the position they were at the start of the scene", "spawn", () =>
        {
            playerController.gameObject.transform.position = playerPositionAtStart;
        });
        
        SET_PROJECTILE = new DebugCommand<int>("set_projectile", "Set a given amount of projectile to the player", "set_projectile <projectile_amount>", (x) =>
        {
            playerController.ProjectileCount = x;
        });
        
        ADD_PROJECTILE = new DebugCommand<int>("add_projectile", "Add a given amount of projectile to the player", "add_projectile <projectile_amount>", (x) =>
        {
            playerController.ProjectileCount += x;
        });

        commandList = new List<object>()
        {
            HELP,
            SPAWN,
            SET_PROJECTILE,
            ADD_PROJECTILE
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            showConsole = !showConsole;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (showConsole)
            {
                HandleInput();
                input = "";
            }
        }
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");
            y += 100;
        }
        
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    private void HandleInput()
    {
        string[] properties = input.Split(' ');
        
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
                else if (commandList[i] as DebugCommand<int> != null)
                {
                    (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                }
            }
        }
    }
}
