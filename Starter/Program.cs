using System;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

// === Flyt funktioner herop ===
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

void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

void Move()
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            playerX--;
            break;
        case ConsoleKey.RightArrow:
            playerX++;
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
}

void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}
// === slut på funktioner ===

// Main game loop
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

    Move();
}
