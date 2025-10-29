using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;


int playerX = 0;
int playerY = 0;


int foodX = 0;
int foodY = 0;


string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };


string player = states[0];


int food = 0;




bool TerminalResized() =>
    height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;


void ShowFood()
{
    food = random.Next(0, foods.Length);
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}


void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}


bool ShouldFreeze()
{
    return player == "(X_X)";
}


bool ShouldSpeedUp()
{
    return player == "(^-^)";
}


void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}


bool PlayerConsumedFood()
{
    return playerX >= foodX &&
           playerX <= foodX + foods[food].Length - 1 &&
           playerY == foodY;
}


void Move(int speed = 1)
{
    int lastX = playerX;
    int lastY = playerY;

    ConsoleKey key = Console.ReadKey(true).Key;


    if (key != ConsoleKey.UpArrow &&
        key != ConsoleKey.DownArrow &&
        key != ConsoleKey.LeftArrow &&
        key != ConsoleKey.RightArrow &&
        key != ConsoleKey.Escape)
    {
        Console.Clear();
        Console.WriteLine("Non-directional input detected. Program exiting.");
        Environment.Exit(0);
    }

    switch (key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            playerX -= speed;
            break;
        case ConsoleKey.RightArrow:
            playerX += speed;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
    }

    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) Console.Write(" ");

    
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

   
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);

    if (PlayerConsumedFood())
    {
        ChangePlayer();
        ShowFood();

        
        if (ShouldFreeze())
        {
            FreezePlayer();
        }
    }
}

void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}

InitializeGame();
Console.WriteLine("Game started. Press Ctrl+C to quit.");

while (!shouldExit)
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
        Environment.Exit(0);
    }


    if (ShouldSpeedUp())
        Move(3);
    else
        Move();
}
