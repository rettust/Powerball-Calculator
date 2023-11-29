using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

List<int> whiteLottoNums = new List<int>(); 
List<int> drawnWhiteLotto = new List<int>();
List<int> playerDrawnWhiteLotto = new List<int>();
Random rnd = new Random();
bool powerballMatch = false;
bool jackpotWon = false;
int jackpot = 0;
int drawnPball = 0;
int playerDrawnPball = 0;
int money = 0;
int games = 0;
int smallWins = 0;
int medWins = 0;
int bigWins = 0;

// initializes it all
StartHere();

// just taking some inputs & validating them
void StartHere()
{
    Console.WriteLine("how much is the powerball worth today? (you can enter in the cash value from powerball.com): ");
    string jackpotString = Console.ReadLine();
    bool parseCheck = int.TryParse(jackpotString, out jackpot);

    if (parseCheck)
    {
        Console.WriteLine("alright, let's see how much money we're gonna lose!");
        Console.WriteLine("press enter to rock 'n roll");
        Console.ReadLine();
        MainLoop();
    }
    else
    {
        Console.WriteLine("that wasn't a number man... what're you trying to pull here?");
        Console.WriteLine("lets try this again...");
        Console.WriteLine("(no commas or letters or anything too scary for this simple little program)");
        Console.WriteLine("(press enter to try again)");
        Console.ReadLine();
        StartHere();
    }
}


// runs through the loop until you win the jackpot
void MainLoop()
{
    while (jackpotWon == false)
    {
        // costs 2 bucks to play the powerball, dude
        money -= 2;
        // purely for fun purposes to see how many games it'll take you to win
        games++;
        // resets all of our booleans & lists back to their initial state
        powerballMatch = false;
        drawnWhiteLotto.Clear();
        playerDrawnWhiteLotto.Clear();
        whiteLottoNums = Enumerable.Range(1, 69).ToList();
        // prints a ticket, draws the lotto numbers
        PowerballGen();
        // checks to see how many matching numbers there are
        var checker = drawnWhiteLotto.Intersect(playerDrawnWhiteLotto);
        // runs function to divvy up rewards (or, more likely, losses)
        LuckOfTheDraw(checker.Count(), powerballMatch);
    }
        // if you've won the jackpot, exit the while loop & show you your stats
        Console.WriteLine("you won the jackpot!");
        PrintInfo();
}

void PowerballGen()
{
    // generates a list of 5 random numbers (the lotto numbers)
    for (int i = 0; i < 5; i++)
    {
        // since we are removing an index from the list of possible lotto numbers, this keeps us from falling out of range
        var upperBound = 68 - i;
        // just randomly picks a number (lazy man's way, could be done fischer-yates style but why bother here)
        var randDraw = rnd.Next(0, upperBound);
        // add the number from the index at the randomly drawn selection
        drawnWhiteLotto.Add(whiteLottoNums[randDraw]);
        // remove the same index from the possible lotto numbers
        whiteLottoNums.RemoveAt(randDraw);
    }

    // reset the list back to it's original state (probably a better way to do this)
    whiteLottoNums = Enumerable.Range(1, 69).ToList();

    // works the exact same way as the previous for loop, but for your ticket numbers instead
    for (int i = 0; i < 5; i++)
    {
        var upperBound = 68 - i;
        var randDraw = rnd.Next(0, upperBound);
        playerDrawnWhiteLotto.Add(whiteLottoNums[randDraw]);
        whiteLottoNums.RemoveAt(randDraw);
    }

    // draw the powerball number
    drawnPball = rnd.Next(1, 26);
    // write the powerball number on your ticket
    playerDrawnPball = rnd.Next(1, 26);

    // check if your powerball number is the drawn number & adjust the boolean appropriately
    if(drawnPball == playerDrawnPball)
    {
        powerballMatch = true;
    } else if (drawnPball != playerDrawnPball)
    {
        powerballMatch = false;
    }
}

void PrintInfo()
{
    Console.WriteLine("your draws were: " + playerDrawnWhiteLotto[0] + ", " + playerDrawnWhiteLotto[1] + ", " + playerDrawnWhiteLotto[2] + ", " + playerDrawnWhiteLotto[3] + ", " + playerDrawnWhiteLotto[4] + ", and powerball number: " + playerDrawnPball);
    Console.WriteLine("drawn numbers were: " + drawnWhiteLotto[0] + ", " + drawnWhiteLotto[1] + ", " + drawnWhiteLotto[2] + ", " + drawnWhiteLotto[3] + ", " + drawnWhiteLotto[4] + ", and powerball number: " + drawnPball);
    Console.WriteLine("you have: " + money + " dolars");
    Console.WriteLine("you\'ve played: " + games + " powerball games");
    Console.WriteLine("you\'ve won: " + smallWins + " small wins ($4-$7)");
    Console.WriteLine("you\'ve won: " + medWins + " medium wins ($100)");
    Console.WriteLine("you\'ve won: " + bigWins + " BIG wins (>$50,000)");
}

// takes checker int & powerball match t/f to see if your tickets a winner
void LuckOfTheDraw(int i, bool pball)
{
    switch (i, pball)
    {
        case (0, false):
            break;
        case (0, true):
            money += 4;
            smallWins++;
            break;
        case (1, true):
            money += 4;
            smallWins++;
            break;
        case (1, false):
            break;
        case (2, false):
            break;
        case (2, true):
            money += 7;
            smallWins++;
            break;
        case (3, false):
            money += 7;
            smallWins++;
            break;
        case (3, true):
            money += 100;
            medWins++;
            break;
        case (4, true):
            money += 50000;
            bigWins++;
            break;
        case(4, false):
            money += 100;
            medWins++;
            break;
        case (5, false):
            money += 1000000;
            bigWins++;
            break;
        case (5, true):
            money += jackpot;
            bigWins++;
            jackpotWon = true;
            break;
    }
    // prints out all your current stats, making this whole thing run much slower than it needs to
    PrintInfo();
}

