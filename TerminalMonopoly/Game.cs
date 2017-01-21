using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace TerminalMonopoly
{
    class Game
    {
        Dictionary<string, Space> spaces = new Dictionary<string, Space>();
        Queue<Cards> chanceCards = new Queue<Cards>(16);
        Queue<Cards> communityCards = new Queue<Cards>(16);
        string[] board = new string[40];
        Player[] players;
        Random rnd = new Random();

        public void StartGame()
        {
            Console.WriteLine("Welcome to terminal monopoly.");
            const string fileName = @"C:\Users\Michael-Admin\Google Drive\TerminalMonopoly\TerminalMonopoly\Monopoly.xml";
            if (!File.Exists(fileName) && !File.Exists("./monopoly.xml"))
            {
                Console.WriteLine("Monopoly.xml file missing! Exiting...");
                return;
            }
            Stream xmlFileStream;
            if (File.Exists(fileName))
                xmlFileStream = File.Open(fileName, FileMode.Open);
            else
                xmlFileStream = File.Open("./monopoly.xml", FileMode.Open);
            XmlDocument monopolyData = new XmlDocument();
            
            board = new string[40];
            int index = 0;
            int index1 = 0;
            int index2 = 0;
            int i;
            int numOfPlayers;
            int piece;
            monopolyData.Load(xmlFileStream);
            Cards[] tmp1 = new Cards[16];
            Cards[] tmp2 = new Cards[16];
            string[] pieces = { "Battleship", "Race Car", "Dog", "Cat", "Wheelbarrow", "Top Hat", "Thimble", "Shoe" };
            
            int[] selectedPieces = new int[6];
            foreach (XmlNode currentNode in monopolyData.DocumentElement)
            {
                switch (currentNode.Name)
                {
                    case "properties":
                        Property nextProperty = new Property();
                        if (nextProperty.LoadFromXml(currentNode))
                            spaces.Add(nextProperty.ID, nextProperty);
                        else
                            Console.WriteLine("Unable to load property from XML!");
                        break;
                    case "utilities":
                        PaidSpace nextUtility = new PaidSpace();
                        if (nextUtility.LoadFromXml(currentNode))
                            spaces.Add(nextUtility.ID, nextUtility);
                        else
                            Console.WriteLine("Unable to load utility from XML!");
                        break;
                    case "railroads":
                        PaidSpace nextRailroad = new PaidSpace();
                        if (nextRailroad.LoadFromXml(currentNode))
                            spaces.Add(nextRailroad.ID, nextRailroad);
                        else
                            Console.WriteLine("Unable to load railroad from XML!");
                        break;
                    case "specials":
                        Space nextSpecial = new Space();
                        if (nextSpecial.LoadFromXml(currentNode))
                            spaces.Add(nextSpecial.ID, nextSpecial);
                        else
                            Console.WriteLine("Unable to load special from XML!");
                        break;
                    case "tiles":
                        board[index] = currentNode.FirstChild.InnerText;
                        index++;
                        break;
                    case "chance":
                        Cards nextChance = new Cards();
                        if (nextChance.LoadFromXml(currentNode))
                        {
                            tmp1[index1] = nextChance;
                            index1++;
                        }
                        else
                            Console.WriteLine("Unable to load chance from XML!");
                        break;
                    case "communitychest":
                        Cards nextCC = new Cards();

                        if (nextCC.LoadFromXml(currentNode))
                        {
                            tmp2[index2] = nextCC;
                            index2++;
                        }
                        else
                            Console.WriteLine("Unable to load community chest from XML!");
                        break;
                }

            }
            Cards[] shuffled = tmp1.OrderBy(x => rnd.Next()).ToArray();
            foreach (Cards c in shuffled)
            {
                chanceCards.Enqueue(c);
            }
            shuffled = tmp2.OrderBy(x => rnd.Next()).ToArray();
            foreach (Cards c in shuffled)
            {
                communityCards.Enqueue(c);
            }
            Console.Write("How many players? ");
            while (!int.TryParse(Console.ReadLine(), out numOfPlayers) || numOfPlayers < 2 || numOfPlayers > 6)
            {
                Console.WriteLine("Please enter a number from 2 - 6!");
            }

            players = new Player[numOfPlayers];
            for (i = 0; i < numOfPlayers; i++)
            {
                for (int j = 0; j < pieces.Length; j++)
                {
                    if (!selectedPieces.Contains(j + 1))
                        Console.Write((j + 1) + ". " + pieces[j] + ", ");
                }
                Console.WriteLine("\b\b ");
                Console.Write("Player " + (i + 1) + " select a piece [1-8]: ");
                if (!int.TryParse(Console.ReadLine(), out piece) || piece < 1 || piece > 8)
                {
                    Console.WriteLine("Please enter a number from 1 - 8!");
                    i--;
                }
                else if (selectedPieces.Contains(piece))
                {
                    Console.WriteLine("That piece has already been selected by another player!");
                }
                else
                {
                    piece--;
                    players[i] = new Player(i, pieces[piece]);
                    selectedPieces[i] = piece;
                }
            }
            playGame(numOfPlayers);
        }

        private void playGame(int numOfPlayers)
        {
            int currentPlayerNum = rnd.Next(numOfPlayers);
            Player currentPlayer = players[currentPlayerNum];
            bool gameWon = false;
            int die1, die2;
            Console.WriteLine(currentPlayer.Piece + " goes first!");
            while (!gameWon)
            {
                Console.ReadLine();
                die1 = rnd.Next(6)+1;
                die2 = rnd.Next(6)+1;
                Console.WriteLine(currentPlayer.Piece + " rolled " + die1 +" & "+ die2 + " = " + (die1 + die2));
                if (die1 == die2)
                {
                    Console.WriteLine("Doubles!");
                    currentPlayerNum--;
                }
                move(currentPlayer, die1 + die2);
                Console.WriteLine(currentPlayer.Piece + " landed on " + nameOfPosition(currentPlayer));
                doAction(currentPlayer, die1 + die2);
                if (die1 == die2 && currentPlayer.Jailed)
                {
                    currentPlayerNum++;
                }
                currentPlayerNum++;
                currentPlayerNum = currentPlayerNum % numOfPlayers;
                currentPlayer = players[currentPlayerNum];
            }
        }
        private void move(Player player, int amount)
        {
            if(player.Position + amount > 39)
            {
                player.addMoney(200);
            }
            player.Position += amount;
        }
        private void moveTo(Player player, string spaceID)
        {
            int location = Array.IndexOf(board, spaceID);
            int spacesToMove;

            if (location > player.Position)
            {
                spacesToMove = location + player.Position;
            }
            else
            {
                spacesToMove = 40 - player.Position + location;
                player.addMoney(200);
            }
            move(player, spacesToMove);
        }
        private void doAction(Player player, int diceAmount)
        {
            Space currentSpace = spaces[board[player.Position]];
            string action = currentSpace.Action;
            switch(action)
            {
                case "rent":
                    Property rentedSpace = (Property)currentSpace;
                    if (rentedSpace.OwnedBy != Player.None && rentedSpace.OwnedBy != player)
                    {
                        player.takeMoney(rentedSpace.Rent);
                        rentedSpace.OwnedBy.addMoney(rentedSpace.Rent);
                        Console.WriteLine(player.Piece + " paid $" + rentedSpace.Rent + " to " + rentedSpace.OwnedBy.Piece + " for rent.");                   
                    }
                    if(rentedSpace.OwnedBy == Player.None)
                    {
                        purchase(rentedSpace, player);
                    }
                    break;
                case "urent":
                    PaidSpace rentedUtility = (PaidSpace)currentSpace;
                    if (rentedUtility.OwnedBy != Player.None && rentedUtility.OwnedBy != player)
                    {
                        int urent;
                        if (ownsGroup(rentedUtility.OwnedBy, currentSpace, "utilities"))
                            urent = 10 * diceAmount;
                        else
                            urent = 4 * diceAmount;
                        player.takeMoney(urent);

                        rentedUtility.OwnedBy.addMoney(rentedUtility.Price);
                    }
                    if (rentedUtility.OwnedBy == Player.None)
                    {
                        purchase(rentedUtility, player);
                    }
                    break;

            }
        }
        private bool ownsGroup(Player player, Space currentSpace, string group)
        {
            Queue<PaidSpace> inGroup = new Queue<PaidSpace>();

            foreach (Space space in spaces.Values)
            {
                switch(group)
                {
                    case "property":
                        Property prop = (Property)space;
                        Property curProp = (Property)currentSpace;
                        if (prop.Color.Equals(curProp.Color))
                        {
                            inGroup.Enqueue(prop);
                        }
                        break;
                    case "utilities":
                        if (space.Action == "urent")
                            inGroup.Enqueue((PaidSpace)space);
                        break;
                    case "railroad":
                        if (space.Action == "rrrent")
                            inGroup.Enqueue((PaidSpace)space);
                        break;
                }
            }
            Player owner = inGroup.Dequeue().OwnedBy;
            if (owner == Player.None)
                return false;
            foreach(PaidSpace s in inGroup)
            {
                if (owner != s.OwnedBy)
                    return false;
            }
            return true;
        }
        private void purchase(PaidSpace space, Player player)
        {
            string ans;
            do
            {
                Console.WriteLine(space.Name + " costs $" + space.Price + ". Do you wish to purchase this property? [Y/n]");
                ans = Console.ReadLine().ToLower();
            }
            while (ans != String.Empty && ans[0] != 'y' && ans[0] != 'n');
            if (ans == String.Empty || ans[0] == 'y')
            {
                player.takeMoney(space.Price);
                space.OwnedBy = player;
                Console.WriteLine(player.Piece + " purchased " + space.Name);
            }
            //else do auction
        }
        private string nameOfPosition(Player player)
        {
            return spaces[board[player.Position]].Name;
        }
    }
}
