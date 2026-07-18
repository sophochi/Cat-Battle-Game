# Cat Battle 🐾

A 2D turn-based strategic RPG developed using the **Unity** engine and **C#**. The game focuses on clean software architecture, extensively leveraging Object-Oriented Programming (OOP) principles and design patterns. Created as a course project at Igor Sikorsky Kyiv Polytechnic Institute.

The player controls a squad of capable combat cats traveling across a strategic map. The objective is to manage team setups, gather resources, upgrade characteristics, and defeat unique tactical enemies in structured card-driven turn-based battles.

## 🎮 Core Gameplay Mechanics
* **Strategic Map Navigation:** Smooth point-to-point character interpolation using custom coroutines without standard Unity Animators.
* **Crew Setup:** Players pick a tactical squad of exactly 3 out of 4 distinct hero cat classes before initializing a match.
* **Card-Driven Turn-Based Combat:** A strict finite state machine governs combat states (Player Turn, Enemy Turn, Win, Loss). Players spend action points utilizing unique character ability cards.
* **Progression System:** Earn *Catnip* tokens through victories or discovery chests to dynamically level up attributes.
* **Cross-Scene State Management:** Progress state retention and scene routing handled seamlessly via data binding over `PlayerPrefs` alongside static structural loadouts.

## 🏗️ Architecture & OOP Implementation
The software codebase is meticulously separated by responsibilities (`GameManager`, `MapManager`, `PanelManager`, etc.):
* **Abstraction & Inheritance:** Spawning instances from a core parent model `Entity`, divided into multi-level hierarchies: `Cat` (subclasses: `Ninja`, `Tank`, `Healer`, `Berserk`) and `Enemy`.
* **Polymorphism:** Unique implementations of `AttackTarget()` method across concrete objects parsed dynamically via common references.
* **Encapsulation:** Safe operational bounds over attributes managed explicitly through standardized mutations (`TakeDamage`, `RestoreHP`).
* **Design Patterns:** Complete integration of the **Factory Method** design pattern (`EnemyFactory`) to isolate unit initialization from game loop state managers.

## 👾 Operational Characters & Adversaries

### Heroes (Cats)
* **Ninja:** Pure striker role dealing immense swift damage.
* **Tank:** Frontline bulk featuring shielding capabilities via `Iron Wall`.
* **Healer:** Essential support unit overriding baseline capabilities to restore ally health.
* **Berserk:** High-risk tactical combatant executing double damage scaling upon dropping below threshold HP.

### Adversaries (Enemies)
* **Garbage:** Entry-level opponent with singular attack cycles.
* **RobotVacuum:** Multi-strike threat with distinct utility to siphon player currency.
* **MuscleMouse:** Formidable attacker incorporating custom `Rage` status enhancements.
* **MutantCucumber:** Advanced adversary enforcing strict `Stunned` status effects.
* **WildGranny:** Final boss orchestrating 5 actions per phase alongside heavy status debuffs.

## 🛠️ System Overview & Development Info
* **Engine:** Unity
* **Language:** C#
* **IDE / Assets:** Visual Studio, custom UI Panels, Toggle Groups, custom font typography, and audio components.
