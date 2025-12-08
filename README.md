# 3112 Final - Business Simulation Game

A C# console-based business simulation game where you manage employees, adjust product prices, and compete against a rival company over 7 turns.

## Requirements

- .NET 9.0 SDK

## Setup and Running

From the project root directory:

```bash
dotnet build
dotnet run
```

Or navigate to the `src` directory:

```bash
cd src
dotnet build
dotnet run
```

## Game Overview

Compete against Rival Co. by managing:
- **Employees**: Assign tasks to Cashiers (Sales), Stockers (reduce spoilage), and Managers (lead the team)
- **Pricing**: Adjust product prices to attract customers and maximize profit
- **Market Events**: Respond to random market conditions (holidays, slowdowns, layoffs)

Your goal is to outperform the rival and achieve the highest profit over 7 days.

## Architecture

The codebase follows SOLID principles with clear separation of concerns:

### Core Classes
- `GameEngine`: Orchestrates the game flow and turn execution
- `GameInitializer`: Handles player and opponent setup, employee creation, and difficulty-based adjustments
- `TurnExecutor`: Executes each turn, manages market events, and processes turn results
- `ResultsProcessor`: Calculates final results and determines the winner
- `TurnSimulator`: Simulates business outcomes based on employee tasks, pricing, and market conditions
- `MarketService`: Creates and applies market events

### Models
- `Player` / `Opponent`: Represent competitors
- `Employee` and subclasses (`Cashier`, `Manager`, `Stocker`): Employee types with distinct task effects
- `FinancialReport`: Contains revenue, expenses, and profit calculations
- `TurnResult`: Tracks per-employee performance metrics
- `PlayerContext`: Aggregates employee results for a turn
- `MarketState`: Represents market conditions affecting demand

### Gameplay Mechanics
- **Sales Multiplier**: Employees assigned to Sales/Lead roles boost revenue based on skill level
- **Spoilage Multiplier**: Stockers reduce product loss through proper storage
- **Market Demand**: Adjusted by global market events and multiplied by pricing strategy
- **Revenue Split**: Market demand is split between player and opponent based on attractiveness (sales multiplier / price ratio)
