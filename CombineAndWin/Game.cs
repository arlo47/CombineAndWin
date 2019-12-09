using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombineAndWin {
    class Game {

        private Card[] hand;
        private int cardsInHand;    //Index specifying number of cards in hand
        private int points;
        private Deck deck;

        public const int DISCARDING_ROUNDS = 3;

        private const int STARTING_POINTS = 1000;
        private const int MAX_HAND_SIZE = 5;
        private const int HIGH_FIVE_POINTS = 800;
        private const int SEQUENCE_POINTS = 600;
        private const int QUADRUPLETS_POINTS = 400;
        private const int FAMILY_POINTS = 200;
        private const int MIXED_SEQUENCE_POINTS = 100;
        private const int DOUBLE_TWINS_POINTS = 50;

        public Game() {
            hand = new Card[5];
            deck = new Deck();
            points = STARTING_POINTS;
            cardsInHand = 0;
        }

        public Deck GetDeck() {
            return deck;
        }

        public int GetPoints() {
            return points;
        }

        public void AddPoints(int pointsToAdd) {
            points += pointsToAdd;
        }

        public void SubtractPoints(int pointsToSubtract) {

            if (points > pointsToSubtract)
                points -= pointsToSubtract;
            else if (points > 0 && points < pointsToSubtract)
                points = 0;
            
        }
        
        public Card[] GetHand() {
            return hand;
        }

        public int GetNumberOfCardsInHand() {
            return cardsInHand;
        }

        //Adds cards to end of array
        public void AddToHand() {

            Card card = GetDeck().DrawCard();

            try {
                if (cardsInHand < MAX_HAND_SIZE) {
                    hand[cardsInHand++] = card;
                }

            }
            catch (Exception ex) {
                throw new Exception($"Only {MAX_HAND_SIZE} cards allowed in hand.\n{ex.Message}");
            }
        }

        //Adds cards too specified index
        public void AddToHand(int index) {

            Card card = GetDeck().DrawCard();

            try {
                if (cardsInHand < MAX_HAND_SIZE) {
                    hand[index] = card;
                    cardsInHand++;
                }
                    
            }
            catch (Exception ex) {
                throw new Exception($"Only {MAX_HAND_SIZE} cards allowed in hand.\n{ex.Message}");
            }
        }

        //Used to manually add cards for testing
        public void AddToHand(Card card) {

            try {
                if (cardsInHand < MAX_HAND_SIZE) {
                    hand[cardsInHand++] = card;
                }

            }
            catch (Exception ex) {
                throw new Exception($"Only {MAX_HAND_SIZE} cards allowed in hand.\n{ex.Message}");
            }
        }

        public void FillHand() {

            for (int i = 0; i < hand.Length; i++) {
                if (hand[i] == null)
                    hand[i] = deck.DrawCard();
            }
        }

        public bool DiscardCard(int indexToRemove) {

            if (GetHand()[indexToRemove] == null)
                throw new Exception($"Card already discarded.");
            else {
                GetHand()[indexToRemove] = null;
                cardsInHand--;
                return true;
            }         
        }

        #region Winning Combination Tests
        //Checks if hand has 10, jack, queen, king and ace, all of the same suit
        public bool IsHighFive(Card[] playerHand) {

            if (!IsFullHand(playerHand))
                return false;

            Array.Sort(playerHand);
            string suitToMatch = playerHand[0].Suit;
            int[] sequenceToMatch = { 1, 10, 11, 12, 13 };

            for (int i = 0; i < playerHand.Length; i++) {
                if (playerHand[i].Number != sequenceToMatch[i] || !playerHand[i].Suit.Equals(suitToMatch))
                    return false;
            }

            AddPoints(HIGH_FIVE_POINTS);
            return true;
        }

        //Checks if hand has 5 consecutive cards of the same suit
        public bool IsSequence(Card[] playerHand) {

            if (!IsFullHand(playerHand))
                return false;

            Array.Sort(playerHand);
            string suitToMatch = playerHand[0].Suit;

            for (int i = 0; i < playerHand.Length - 1; i++) {
                if ((playerHand[i].Number + 1) != playerHand[i + 1].Number || !playerHand[i].Suit.Equals(suitToMatch))
                    return false;
            }

            AddPoints(SEQUENCE_POINTS);
            return true;
        }

        //Checks if 4 of the cards are the same suit and number. 5th card can be anything.
        public bool IsQuadruplets(Card[] playerHand) {

            if (!IsFullHand(playerHand))
                return false;

            Dictionary<int, int> cardCount = new Dictionary<int, int>();
            bool IsSameSuit = true;
            string suitToCheck = playerHand[0].Suit;

            foreach (Card card in playerHand) {
                if (cardCount.ContainsKey(card.Number))
                    cardCount[card.Number] += 1;
                else
                    cardCount.Add(card.Number, 1);

                if (!card.Suit.Equals(suitToCheck))
                    IsSameSuit = false;
            }

            if (cardCount.Count != 2 || IsSameSuit)
                return false;
            else if (cardCount.ElementAt(0).Value != 1 && cardCount.ElementAt(1).Value != 4 &&
                     cardCount.ElementAt(0).Value != 4 && cardCount.ElementAt(1).Value != 1)
                return false;

            AddPoints(QUADRUPLETS_POINTS);
            return true;

        }

        //Checks if all 5 cards are of the same suit
        public bool IsFamily(Card[] playerHand) {

            if (!IsFullHand(playerHand))
                return false;

            Array.Sort(playerHand);
            string suitToMatch = playerHand[0].Suit;

            for (int i = 0; i < playerHand.Length - 1; i++) {
                if (!playerHand[i].Suit.Equals(suitToMatch))
                    return false;
            }

            AddPoints(FAMILY_POINTS);
            return true;
        }

        public bool IsMixedSequence(Card[] playerHand) {

            if (!IsFullHand(playerHand))
                return false;

            Array.Sort(playerHand);
            bool IsSameSuit = true;
            string suitToCheck = playerHand[0].Suit;

            for (int i = 0; i < playerHand.Length - 1; i++) {
                if ((playerHand[i].Number + 1) != playerHand[i + 1].Number)
                    return false;
                if (!playerHand[i].Suit.Equals(suitToCheck))
                    IsSameSuit = false;
            }

            if (IsSameSuit)
                return false;

            AddPoints(MIXED_SEQUENCE_POINTS);
            return true;
        }

        //Checks if had has 2 pairs of different pairs, and any other card
        public bool IsDoubleTwins(Card[] playerHand) {

            if (!IsFullHand(playerHand))
                return false;

            Dictionary<int, int> cardCount = new Dictionary<int, int>();

            foreach (Card card in playerHand) {
                if (cardCount.ContainsKey(card.Number))
                    cardCount[card.Number] += 1;
                else
                    cardCount.Add(card.Number, 1);
            }

            if (cardCount.Count != 3)
                return false;
            else if (!(cardCount.ElementAt(0).Value != 2) && !(cardCount.ElementAt(1).Value != 2) && !(cardCount.ElementAt(2).Value != 1) &&
                     !(cardCount.ElementAt(0).Value != 2) && !(cardCount.ElementAt(1).Value != 1) && !(cardCount.ElementAt(2).Value != 2) &&
                     !(cardCount.ElementAt(0).Value != 1) && !(cardCount.ElementAt(1).Value != 2) && !(cardCount.ElementAt(2).Value != 2))
                return false;

            AddPoints(DOUBLE_TWINS_POINTS);
            return true;
        }

        public bool IsFullHand(Card[] playerHand) {
            return playerHand.Length == 5;
        }
        
        public bool IsWinningCombination(Card[] playerHand) {

            if (IsHighFive(playerHand)) {
                AddPoints(HIGH_FIVE_POINTS);
                return true;
            }
            else if (IsSequence(playerHand)) {
                AddPoints(SEQUENCE_POINTS);
                return true;
            }
            else if (IsQuadruplets(playerHand)) {
                AddPoints(QUADRUPLETS_POINTS);
                return true;
            }
            else if (IsFamily(playerHand)) {
                AddPoints(FAMILY_POINTS);
                return true;
            }
            else if (IsMixedSequence(playerHand)) {
                AddPoints(MIXED_SEQUENCE_POINTS);
                return true;
            }
            else if (IsDoubleTwins(playerHand)) {
                AddPoints(DOUBLE_TWINS_POINTS);
                return true;
            }
            else {
                SubtractPoints(1000);
                return false;
            }
        }
        #endregion

        public string PrintHand() {
            StringBuilder sbHand = new StringBuilder();

            foreach (Card card in hand) {
                sbHand.Append(card + " ");
            }

            return sbHand.ToString();

        }

        public override string ToString() {
            return $"You have {points} and currently hold {PrintHand()}";
        }
    }
}
