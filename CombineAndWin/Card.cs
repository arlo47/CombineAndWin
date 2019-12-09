using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombineAndWin {
    class Card : IComparable<Card> {
        private string suit;            //hearts, diamonds, spades, club
        private int number;             //can be 1 to 13

        public Card(int number, string suit) {
            this.suit = suit;
            this.number = number;
        }

        public string Suit {
            get { return suit; }
            set { suit = value; }
        }
        public int Number {
            get { return number; }
            set { number = value; }
        }

        public int CompareTo(Card otherCard) {
            return this.Number.CompareTo(otherCard.Number);
        }

        public override string ToString() {
            return $"[{Number} of {Suit}]";
        }
    }
}
