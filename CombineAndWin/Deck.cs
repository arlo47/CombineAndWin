using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombineAndWin {
    class Deck {
        private Card[] cards;
        private int cardIndex;
        private const int DECK_SIZE = 52;
        private const int SUIT_SIZE = 13;

        public Deck() {
            InitializeDeck();
            ShuffleDeck();
            cardIndex = 0;
        }

        public int GetCardIndex() {
            return cardIndex;
        }

        //returns a card based on cardIndex, increments cardIndex
        public Card DrawCard() {
            if (cardIndex < cards.Length)
                return cards[++cardIndex];
            else
                throw new IndexOutOfRangeException("No more cards to draw");
        }

        //Fills the deck with 52 cards of the appropriate number and suit
        public void InitializeDeck() {
            cards = new Card[DECK_SIZE];
            string[] suits = { "hearts", "diamonds", "clubs", "spades" };
            int suitIndex = 0;          //used to track the suits still to be added to the deck
            int numberIndex = 1;        //used to track the number of the card

            for (int i = 0; i < DECK_SIZE; i++) {

                //when the card number count is higher than should be allowed, change to next suit and reset number to 1
                if (numberIndex > SUIT_SIZE) {
                    suitIndex++;
                    numberIndex = 1;
                }
                cards[i] = new Card(numberIndex, suits[suitIndex]);
                numberIndex++;
            }
        }

        //reorders the deck in a random fashion
        public void ShuffleDeck() {
            Random random = new Random();

            for (int i = 0; i < cards.Length; i++) {
                int randomIndex = random.Next(52);

                Card temp = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = temp;
            }

        }

        //Prints deck to console, for testing
        public void PrintDeck() {

            foreach (Card card in cards) {
                Console.WriteLine(card.ToString());
            }

        }

        public override string ToString() {
            return $"Deck has {cards.Length} cards, {cards.Length - cardIndex} undrawn cards and cardIndex is at: {cardIndex}";
        }
    }
}
