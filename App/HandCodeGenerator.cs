using System.Text;

namespace App;

public static class HandCodeGenerator
{
    // Create a dictionary of numbers up to twenty-one written in CapitalCamelCase
    public static readonly Dictionary<int, string> Numbers = new()
    {
        { 1, "One" },
        { 2, "Two" },
        { 3, "Three" },
        { 4, "Four" },
        { 5, "Five" },
        { 6, "Six" },
        { 7, "Seven" },
        { 8, "Eight" },
        { 9, "Nine" },
        { 10, "Ten" },
        { 11, "Eleven" },
        { 12, "Twelve" },
        { 13, "Thirteen" },
        { 14, "Fourteen" },
        { 15, "Fifteen" },
        { 16, "Sixteen" },
        { 17, "Seventeen" },
        { 18, "Eighteen" },
        { 19, "Nineteen" },
        { 20, "Twenty" },
        { 21, "TwentyOne" },
        { 22, "TwentyTwo" }
    };

    public static string GetCardHandsCode()
    {
        StringBuilder records = new("""
        using Blackjack.Base;

        namespace Blackjack;


        """);
        foreach ((int number, string name) in Numbers)
        {
            Numbers.TryGetValue(number + 1, out string? next);
            if (next is null) continue;
            
            string nextName = next;
            string constructor = string.Join(",\n    ", Enumerable.Range(1, number).Select(i => $"Card Card{i}"));
            string addedConstructor = string.Join(",\n            ", Enumerable.Range(1, number).Select(i => $"Card{i}"));
            string record = $$"""
            public readonly record struct {{name}}CardHand(
                {{constructor}}) : IHand
            {
                public {{nextName}}CardHand AddCard(Card card)
                {
                    List<Card> cards =
                    [
                        {{addedConstructor}},
                        card
                    ];
                    List<Card> ordered = [.. cards.OrderBy(c => c.Rank).ThenBy(c => c.Suit) ];
                    return new {{nextName}}CardHand(
                        {{string.Join(",\n            ", Enumerable.Range(0, number + 1).Select(i => $"ordered[{i}]"))}});
                }

                public bool Equals(IHand? other)
                    => other is {{name}}CardHand o
                    && {{string.Join("\n        && ", Enumerable.Range(1, number).Select(i => $"Card{i}.Rank == o.Card{i}.Rank && Card{i}.Suit == o.Card{i}.Suit"))}};

                IHand IHand.AddCard(Card card)
                    => AddCard(card);
            }
            """;
            records.AppendLine(record);
        }
        return records.ToString();
    }
}