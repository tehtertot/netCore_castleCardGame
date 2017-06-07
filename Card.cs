namespace castle
{
    public class Card {
        public string stringVal, suit;
        public int val;
        public Card(int v, string s) {
            val = v;
            suit = s;
            if (v == 14) {
                //aces high
                stringVal = "Ace";
            }
            else if (v == 11) {
                stringVal = "Jack";
            }
            else if (v == 12) {
                stringVal = "Queen";
            }
            else if (v == 13) {
                stringVal = "King";
            }
            else {
                stringVal = v.ToString();
            }
        }
        public override string ToString() {
            string special = "";
            if (val == 3 || val == 7 || val == 10) { special = "*";};
            return "[" + stringVal + " of " + suit + "]" + special;
        }
    }
}