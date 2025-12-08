# 3112 Final - Business Simulation Game

## 1. Project Overview

A turn-based business simulation game where you compete against a rival company over 7 days. The player manages a team of employees, adjusts product pricing strategies, and responds to dynamic market conditions to maximize profit.

**Core Features:**
- Employee management system with three distinct roles (Cashier, Manager, Stocker)
- Dynamic pricing mechanism affecting market share and revenue
- Random market events that influence customer demand
- Strategic AI opponent with difficulty-based behavior patterns
- Real-time financial tracking and performance metrics

**Target Audience:** Students learning object-oriented design principles and business simulation enthusiasts.

**Key Assumptions:**
- Each turn represents one business day
- Market demand is shared between player and opponent based on pricing and employee performance
- Employee skill levels range from 1-10 and directly impact their effectiveness
- The game ends after exactly 7 turns with final profit comparison

## 2. Build & Run Instructions

**Tools and Versions:**
- .NET SDK: 9.0
- Language: C# 12
- IDE: Any (Visual Studio, VS Code, Rider, or command line)
- Build Tool: dotnet CLI

**Steps to Build and Run:**

1. Clone or download the repository
2. Navigate to the project root directory
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the application:
   ```bash
   dotnet run
   ```

Alternatively, from the `src` directory:
```bash
cd src
dotnet build
dotnet run
```

**Configuration:**
- No additional configuration required
- No command-line arguments needed
- Game difficulty is selected at runtime via console menu

**Test Data:**
- Employee names and initial stats are hardcoded in `GameInitializer.cs`
- Product prices initialize to: Noodles ($5.00), Drink ($2.50), Snack ($3.25)
- Market events are pseudo-randomly generated based on turn number

## 3. Required OOP Features

| OOP Feature | File Name | Line Numbers | Reasoning / Purpose |
|------------|-----------|--------------|---------------------|
| **Inheritance #1** | `src/Models/Employee.cs` | 8 | Abstract base class defining common employee attributes (Name, SkillLevel, Type) and behavior (AssignTask, ExecuteTurn). |
| **Inheritance #1 (cont.)** | `src/Models/Cashier.cs` | 5-28 | Cashier inherits from Employee and implements PerformTask to boost sales multiplier when assigned to Sales. |
| **Inheritance #2** | `src/Models/Manager.cs` | 5-28 | Manager inherits from Employee and implements PerformTask to provide team leadership bonuses. |
| **Inheritance #3** | `src/Models/Stocker.cs` | 5-28 | Stocker inherits from Employee and implements PerformTask to reduce spoilage multiplier. |
| **Inheritance #4** | `src/Models/Opponent.cs` | 5-13 | Opponent inherits from Player to add AI decision-making via Strategy pattern while reusing player data structures. |
| **Inheritance #5** | `src/Models/MarketEvent.cs` | 10-38 | Three concrete market event classes (HolidayEvent, SlowDayEvent, LayoffsEvent) inherit from abstract MarketEvent base. |
| **Interface #1** | `src/Interfaces/IAssignable.cs` | 4-7 | Defines contract for entities that can be assigned tasks. Implemented by Employee class to enable task assignment. |
| **Interface #2** | `src/Interfaces/ITurnAction.cs` | 5-8 | Defines contract for entities that perform actions during a turn. Implemented by Employee to execute turn logic. |
| **Interface #3** | `src/Interfaces/IGameObserver.cs` | 4-7 | Observer pattern interface for receiving game notifications. Implemented by ConsoleUI to display game events. |
| **Interface #4** | `src/Interfaces/IDecisionMaker.cs` | 4-7 | Defines contract for AI decision-making. Implemented by Player (no-op) and overridden by Opponent. |
| **Interface #5** | `src/Services/Strategies.cs` | 7-10 | Strategy pattern interface allowing interchangeable AI behaviors. Implemented by 4 concrete strategy classes. |
| **Polymorphism #1** | `src/Models/Cashier.cs` | 11-26 | Override PerformTask from abstract Employee. At runtime, calling employee.PerformTask() dispatches to correct subclass implementation. |
| **Polymorphism #2** | `src/Models/Opponent.cs` | 12 | Override DecideTurn from Player base class. Enables opponent to use Strategy pattern for AI decisions instead of human input. |
| **Polymorphism #3** | `src/Models/MarketEvent.cs` | 17, 27, 37 | Each market event subclass overrides AffectMarket() to apply different demand multipliers to MarketState. |
| **Access Modifiers** | `src/Models/Employee.cs` | 11-14 | `public` properties (Name, SkillLevel, Type) for external read access. `private set` on AssignedTask prevents external modification, ensuring task changes go through AssignTask method. |
| **Access Modifiers** | `src/Models/GameEngine.cs` | 12-13 | `private readonly` for turn reports ensures encapsulation - only GameEngine modifies internal state, external code uses public methods. |
| **Access Modifiers** | `src/Models/GameInitializer.cs` | 19, 32, 40 | `private static` helper methods encapsulate initialization logic, exposing only the public InitializeGame factory method. |
| **Struct** | `src/Models/FinancialReport.cs` | 3-17 | Value type struct for financial data (Revenue, Expenses, Profit). Chosen for immutability and efficient stack allocation since reports are small, frequently created, and never modified after construction. |
| **Enum #1** | `src/Enums/GameDifficulty.cs` | 3-9 | Defines difficulty levels (Easy, Normal, Hard, Legend) for type-safe difficulty selection and AI strategy assignment. |
| **Enum #2** | `src/Enums/EmployeeType.cs` | 3-8 | Defines employee categories (Cashier, Manager, Stocker) for factory pattern and runtime type checking. |
| **Data Structure #1** | `src/Models/GameEngine.cs` | 12-13 | List<FinancialReport> stores per-turn financial reports for both players, enabling cumulative profit calculation at game end. |
| **Data Structure #2** | `src/Models/Player.cs` | 10 | Dictionary<string, double> maps product names to prices, allowing O(1) price lookups during revenue calculation. |
| **Data Structure #3** | `src/Models/Player.cs` | 9 | List<Employee> stores employee collection with Add/iteration operations for turn execution and management. |
| **I/O - Input** | `src/UI/ConsoleUI.cs` | 35-64 | Console.ReadLine() captures user menu choices (NextTurn, AssignTask, AdjustPrice, Quit). Validates input and re-prompts on invalid entries. |
| **I/O - Output** | `src/UI/ConsoleUI.cs` | 21-23 | Implements IGameObserver.Update() to write game events, financial reports, and employee status to console via Console.WriteLine(). |
| **I/O - Input** | `src/UI/ConsoleUI.cs` | 75-94 | AssignTaskMenu reads employee selection and task assignment from console input with validation. |
| **I/O - Input** | `src/UI/ConsoleUI.cs` | 97-121 | AdjustPriceMenu reads product selection and new price value with parsing and validation. |

## 4. Design Patterns

| Pattern Name | Category | File Name | Line Numbers | Rationale |
|--------------|----------|-----------|--------------|----------|
| **Strategy** | Behavioral | `src/Services/Strategies.cs` | 7-60 | Encapsulates AI decision algorithms (ConservativeStrategy, AggressiveStrategy, MixedStrategy, UltraAggressiveStrategy) allowing runtime selection based on difficulty. Solves the problem of varying AI behaviors without conditional logic in Opponent class. The Opponent holds an IStrategy reference and delegates decision-making via Execute(), enabling behavior swapping and adhering to Open/Closed Principle. |
| **Factory Method** | Creational | `src/Services/EmployeeFactory.cs` | 6-18 | Centralizes employee object creation based on EmployeeType enum. Eliminates scattered `new Cashier/Manager/Stocker` calls throughout codebase and provides single point for instantiation logic. The static Create method uses switch expression to return appropriate Employee subclass, hiding construction details from clients and simplifying future employee type additions. |
| **Observer** | Behavioral | `src/Interfaces/IGameObserver.cs`, `src/Models/GameEngine.cs` | IGameObserver: 4-7, GameEngine: 21-45 | Implements publish-subscribe mechanism where GameEngine notifies ConsoleUI of game events without tight coupling. GameEngine maintains observer list and calls Update() on state changes. This decouples game logic from presentation layer - GameEngine doesn't know about ConsoleUI implementation, only that observers conform to IGameObserver contract. Enables multiple observers (e.g., logger, GUI) without modifying GameEngine. |
| **Template Method** | Behavioral | `src/Models/Employee.cs` | 30-34 | ExecuteTurn() defines skeleton algorithm (call PerformTask, register result) while subclasses override PerformTask() to provide specific task effects. Base class controls workflow and ensures results are properly registered, while derived classes customize only the task execution logic. Prevents code duplication across Cashier/Manager/Stocker and ensures consistent turn processing. |

## 5. Design Decisions

**Component Architecture:**
The system is organized into clear layers following SOLID principles, particularly Single Responsibility:
- **Game Orchestration**: `GameEngine` coordinates turn flow but delegates initialization (GameInitializer), turn execution (TurnExecutor), simulation (TurnSimulator), and result processing (ResultsProcessor)
- **Business Logic**: Simulation calculations isolated in `TurnSimulator`, market mechanics in `MarketService`
- **Presentation**: `ConsoleUI` handles all I/O, completely separated from game logic via Observer pattern

**Key Abstractions:**
- **Employee hierarchy**: Abstract base class with concrete implementations enables polymorphic task execution. Each employee type has distinct effects (sales boost, spoilage reduction, team leadership) while sharing common attributes.
- **Strategy pattern for AI**: Difficulty levels map to behavioral strategies, making AI extensible without modifying Opponent class.
- **Value objects**: `FinancialReport` and `TurnResult` as immutable data carriers prevent unintended state mutations.

**Tradeoffs:**
- **Simplicity vs Realism**: Market demand split uses basic attractiveness ratio (sales multiplier / price). More realistic models (elasticity curves, brand loyalty) were sacrificed for implementation clarity.
- **Strategy coupling**: Opponent.DecideTurn() currently receives nullable GameEngine reference (line 30 in TurnExecutor) but strategies don't use it. Future AI improvements might need game state access, but current design minimizes coupling.
- **Observer granularity**: Single Update(string) method requires formatting in callers. Alternative: typed event objects would enable structured logging but increase complexity.
- **Factory scope**: EmployeeFactory is static utility class rather than instantiable factory. Chosen for simplicity since employee creation has no shared state or configuration needs.

**Revenue Calculation Model:**
Market demand per product is split between player and opponent based on attractiveness weight: `salesMultiplier / price`. This creates strategic tension - higher prices increase revenue per unit but reduce market share, while aggressive pricing captures demand at lower margins. Spoilage multiplier then reduces units sold, rewarding proper inventory management (Stocker assignment).

**SOLID Compliance:**
- **Single Responsibility**: Classes have focused purposes (e.g., TurnSimulator only computes financial outcomes, doesn't manage UI or persistence)
- **Open/Closed**: Strategy pattern and abstract Employee allow extension without modification
- **Liskov Substitution**: All Employee subclasses properly override PerformTask and can substitute for base class
- **Interface Segregation**: Small, focused interfaces (IAssignable, ITurnAction, IGameObserver) rather than large monolithic contracts
- **Dependency Inversion**: GameEngine depends on Observer abstraction (IGameObserver), not concrete ConsoleUI
