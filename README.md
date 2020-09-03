# Comparing Poker Hands
Shell projects for the August 19 C3 meetup.


## Create a poker hand class that has a method to compare itself to another poker hand:

    Result PokerHand.CompareWith(PokerHand hand);

## A poker hand has a constructor that accepts a string containing 5 cards:

    PokerHand hand = new PokerHand("KS 2H 5C JD TD");

## The characteristics of the string of cards are:

    Each card consists of two characters, where:
        1.	The first character is the value of the card: 2, 3, 4, 5, 6, 7, 8, 9, T(en), J(ack), Q(ueen), K(ing), A(ce)
        2.	The second character represents the suit: S(pades), H(earts), D(iamonds), C(lubs)
    A space is used as card separator between cards

## The result of your poker hand compare can be one of these 3 options:
    public enum Result 
    { 
        Win, 
        Loss, 
        Tie 
    }

## A tie results when the two hands are of the same type. For example, if both hands have a pair, then it’s a tie.

## If you have time, implement tie-breaker rules: 

    Straight Flush: highest rank at the top of the sequence wins
    Four of a Kind: highest 4 of a kind wins
    Full House: Highest 3 cards wins
    Flush: highest ranked card wins
    Straight: highest ranking card at the top of the sequence wins
    Three of a kind: highest ranking 3 of a kind wins
    Two Pair: highest pair wins
    Pair: highest pair wins
    High Card: highest card wins
    Royal Flush: if you have a tie with a royal flush, one of you is going to be shot.

## Constraints:
    1.	There is no ranking for the suits.
    2.	Low aces are not valid for this exercise.

## Ranking of hands in increasing order

    Highcard          Simple value of the highest card. Lowest: 2 - Highest: Ace
    Pair              Two cards with the same value
    Two pair          Two sets of two cards with the same value
    Three of a Kind   Three cards with the same value
    Straight          Sequence of 5 cards in increasing value
    Flush             5 cards of the same suit
    Full House        Combination of three of a kind and a pair
    Four of a Kind    Four cards of the same value
    Straight Flush    Straight of the same suit
    Royal Flush       Straight flush where the straight is Ten through Ace


## Refactoring

Once the initial requirements are satisfied, consider implementing the following refactorings:

    1.	Consider low aces. In this case an Ace can be the high card in a straight (above the King) or it can be the low card (below a 2).
    2.	Refactor to support comparing multiple players.
    3.	Refactor to get the best hand from 7 cards.
    4.	Refactor to handle wild cards.
    5.	Validate that no one is obviously cheating. For example, ensure the same cards don’t appear in more than one hand.
