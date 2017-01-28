using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalMonopoly
{
    class Player
    {
        private int money;
        private int id;
        private int position;
        

        public Player(int idNum, String piece)
        {
            Piece = piece;
            money = 1500;
            id = idNum;
            ownedProperties = new List<Property>();
        }
        public static Player None = new Player(-1, "none");

        public String Piece
        {
            get; set;
        }
        public int ID
        {
            get { return id; }
        }
        public int Money
        {
            get { return money; }
        }
        public int Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value % 40;
            }
        }
        public bool Jailed { get; set; }
        public List<Property> ownedProperties
        {
            get;
        }
        public bool takeMoney(int amount)
        {
            if (amount < money)
                money -= amount;
            else
                return false;

            return true;
        }
        public void addMoney(int amount)
        {
            money += amount;
        }
        public override string ToString()
        {
            string statusString = String.Empty;
            statusString += Piece;
            if (Jailed)
            {
                statusString += " In Jail,";
            }
            statusString += " Position: ";
            statusString += Game.spaces[Game.Board[position]].Name;
            statusString += ", Money: $";
            statusString += money;
            
            return statusString;
        }
    }
}
