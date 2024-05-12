# Blackjack Calculator

## Summary

The aim of this project is to apply graph theory to explore the sample space for blackjack, a popular card game in casinos that has been widely studied and analyzed. My goal is to generate the various possible game states and how they are connected with each other to see which sequence of player actions results in the highest expected value for a given starting hand and the visible dealer card.

## Blackjack

Blackjack is a popular card game in casinos. It is generally played with one to four *players* who all aim to beat the 9. All the *cards* have a specific value that counts towards the score of a *hand* (i.e. the cards that belong to a player or the dealer).

### Terminology

#### Shoe

The shoe is a box that contains one or more standard decks of cards from which cards are drawn during a round of blackjack. Historically, the cards were shuffled first and then placed inside the shoe, but nowadays there are continuous shuffling machines which shuffle the cards inside the shoe.

#### Hand

A hand is a set of cards that is used for scoring. In American blackjack, the dealer starts with a hand of one visible card (face up) and one hole card (face-down, not visible). The dealer can never have more than one hand at a time. Players each start with a hand of two visible cards, and if the hand is *splittable*, they can split the hand into two and receive two more cards from the shoe, resulting in more than one hand for the player. Scoring happens independently for each hand, so it is possible for a player to have one hand that beats the dealer's and another that loses, for example.

#### Dealer

The dealer in blackjack deals the cards and draws cards from the shoe when necessary. The dealer also has a hand and therefore plays against the players, representing the house. They collect lots bets or pay out players based on their hands and their decisions. When all the players' turns have ended, the dealer follows a fixed set of instructions until their turn ends. When all the players and the dealer have played, the round concludes, at which point any lost bets are collected and any winning hands are paid out.

#### Player

The player is someone who bets against the house and plays against the dealer. They are allowed to make certain decisions during their turn, such as asking more cards from the shoe, splitting a hand or surrendering.

### Scoring

In blackjack, each card has a value which is used to calculate the total hand score. The face cards or court cards (Jack, Queen and King) have a value of `10`, all other non-Ace cards have a value equal to their number of pips, and the Ace can count as `1` or `11`, whichever yields the highest score *without* exceeding `21`. A blackjack, which is what the game is named after, is a hand that consists of an Ace and a ten-valued card. In many rule sets, a blackjack beats non-blackjack hands that have a total score of `21`.

#### Hard and soft totals

If a hand contains an Ace and its value counts as `11`, then that total is considered *soft*. For example, `A♣6♥` is a soft `17`. A hard total applies to hands that do not contain an Ace, or where the Ace can only count as a `1`. For example, `8♣9♥` and `A♣6♥Q♠` are both hard `17`s.

#### Bust

If the score of a hand exceeds `21`, it has gone *bust*.

### Flow of a game

The dealer deals the cards one by one, first for the players then for themselves, until everyone has two cards. For simplicity, let's assume there is only one player.

#### Player actions

The player can make decisions that may change the score of their hand, hopefully to beat the dealer's hand.

| Decision | Description | Hand signal |
| -------- | ----------- | ----------- |
| Stand | End your turn. | Wave your hand. |
| Hit | Get the next card from the shoe and add it to your hand. | Tap the table. |
| Double down | Double your bet, hit one card and end your turn. | Increase your bet and point with one finger. |
| Surrender | Give up half of your bet and end your turn with no further losses. | - |
| Split | If your hand has two cards with the same value, split them into two hands and get two new cards from the shoe to bring each hand to two cards. | Increase your bet and make a "peace" sign. |

#### Dealer actions

The dealer's decisions are more constrained. Their initial hand score after revealing their hole card will dictate what the dealer must do next. If the hand total is `17` or less, the dealer must hit. For a hand total of `18` or more, the dealer stands. Most casinos have the dealer hit on a soft `17`. For hand scores of `18` or more, the dealer always stands, no matter if the total is soft or hard.

#### Payout

A player loses their entire bet when they lose, or half when they surrender. Winning means that a player get twice their original bet back, or more for a blackjack (generally 2.5x or 3x).

## Game States

The game state encapsulates the necessary information about the player, the dealer and the shoe to be able to calculate all possible next states, for example because a player or the dealer make a decision. When there are no more possible states, it means that the round has come to an end.

### Player state

The player has one or more hands which can be in play or which can be ended (by standing, doubling down, surrendering or busting). Each hand has a state and allowed player decisions that could lead to future states.

### Dealer state

The dealer state is similar to the player state, with a single hand, state and allowed decisions which may lead to future states.

### Shoe

The shoe state simply contains the remaining cards for a given state. All the cards in play (i.e. the dealer's cards and the player's cards) and the remaining cards in the shoe should make up the starting number of full standard decks.

### State equivalence

#### Player state

Two players are equivalent if their hands are equivalent, and two hands are equivalent if they contain the same number of equivalent cards and if the hand states and decisions are the same.

### Dealer state

Two dealers are equivalent if their hands, their state and their allowed decisions are equivalent.

### Shoe

Two shoes are equivalent if they contain the same number of cards and if each card in a specific position of the first shoe is equivalent to the card in the same position in the other shoe.

### Game state

Two game states are equivalent if their player states, dealer states and shoe states are all equivalent.

## Probability calculation

### Decision trees

### Expected value

