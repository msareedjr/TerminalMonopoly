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
            id = idNum;
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
        
    }
}
