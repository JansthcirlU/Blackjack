// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Blackjack;
using Calculators;
using Calculators.Graphs;

Console.WriteLine("Generating graph.");

BlackjackRules rules = new(1, false);
BlackjackGraph graph = BlackjackGraphCalculator.GenerateGraph(rules);

Debugger.Break();