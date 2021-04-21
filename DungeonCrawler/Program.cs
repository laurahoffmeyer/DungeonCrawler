using System;
using System.Collections;
using System.Collections.Generic;

namespace DungeonCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            // ENEMIES
            var enemiesCollection = new List<Enemy> {
            new Enemy() { Name="A Troll", Damage= new Random().Next(5, 10)},
            new Enemy() { Name="A Gablin", Damage= new Random().Next(4, 14)},
            new Enemy() { Name="The Purple People Eater", Damage= new Random().Next(18, 22)},
            new Enemy() { Name="The Thing", Damage= new Random().Next(0, 5)},
            new Enemy() { Name="Godzilla", Damage= new Random().Next(30, 36)},
            new Enemy() { Name="The Lockness Monster", Damage= new Random().Next(20, 28)},
            new Enemy() { Name="Bigfoot", Damage= new Random().Next(10, 18)},
            };

            Stack<Enemy> enemyDeck = new Stack<Enemy>(); // Enemy deck

            for (int i = 0; i < 40; i++) // Loads 40 random enemies into enemyDeck - Enemies can be repetitive
            {
                int randomEnemy = new Random().Next(0, enemiesCollection.Count);
                enemyDeck.Push(enemiesCollection[randomEnemy]);
            }

            // LOCATIONS

            var locations = new List<Location> {
            new Location() { LocationName="Blockbuster", NumbOfEnemies= new Random().Next(1, 4)},
            new Location() { LocationName="The Mall", NumbOfEnemies= new Random().Next(1, 3)},
            new Location() { LocationName="Jumanji", NumbOfEnemies= new Random().Next(1, 4)},
            new Location() { LocationName="The Bermuda Triangle", NumbOfEnemies= new Random().Next(1, 6)},
            new Location() { LocationName="The Space Station", NumbOfEnemies= new Random().Next(1, 2)},
            new Location() { LocationName="Pluto", NumbOfEnemies= new Random().Next(1, 4)},
            new Location() { LocationName="Neptune", NumbOfEnemies= new Random().Next(1, 6)},
            new Location() { LocationName="Trump's Toupee", NumbOfEnemies= new Random().Next(1, 8)}
            };

            Queue<Location> LocationsQueue = new Queue<Location>(); // Locations queue

            for (int i = 0; i < 10; i++) // Loads 10 random locations into LocationsQueue - Locations can be repetitive
            {
                int randomLocation = new Random().Next(0, locations.Count);
                LocationsQueue.Enqueue(locations[randomLocation]);
            }

            int numberoflocations = LocationsQueue.Count;


            // CHARACTERS
            var characters = new List<Character> { };
            var battleLog = new List<BattleLog> { }; // battle log to keep track of past battles


            // GAME SET UP

            for (int i = 0; i < 6; i++) // Creates the 6 characters
            {
                Console.WriteLine("Please enter a character name.");
                string characterName = Console.ReadLine(); // Character name
                int xRandomLocationIndex = new Random().Next(0, locations.Count); // Index of location character cannot fight in
                string xRandomLocationName = locations[xRandomLocationIndex].LocationName; // Name of location character cannot fight in
                int xRandomEnemy = new Random().Next(0, enemiesCollection.Count); // Index of enemy character cannot fight
                string xRandomEnemyName = enemiesCollection[xRandomEnemy].Name; // Name of enemy character cannot fight

                characters.Add(new Character() { CharName = characterName, HitPoints = new Random().Next(15, 30), xLocation = xRandomLocationName, xEnemy = xRandomEnemyName }); // Pushes all character properties in the characters list
            }


            // GAMEPLAY

            for (int i = 0; i < numberoflocations; i++) // Loops through LocationsQueue for gameplay
            {

                Location nextLocation = LocationsQueue.Peek(); // Peeks at next location in queue
                string nextLocationName = nextLocation.LocationName; // Gets name of location
                int nextLocationEnemies = nextLocation.NumbOfEnemies; // Gets number of enemies in location

                foreach (Character character in characters)
                {
                    Console.WriteLine("{0}: {1} HP", character.CharName, character.HitPoints);
                }

                Console.WriteLine("Characters are taken to {0} which has {1} enemies.", nextLocationName, nextLocationEnemies); // Tells player info of location

                for (int e = 0; e < nextLocationEnemies; e++) // Pulls enemies from enemyDeck based on number of enemies at location
                {
                    Enemy nextEnemy = enemyDeck.Peek(); // Peeks at next enemy in deck

                    int randomCharacterIndex = new Random().Next(0, characters.Count); // Index of random character 
                    Character randomCharacter = characters[randomCharacterIndex]; // Selected random character and access all properties

                    Console.WriteLine("Battle {0}: {1} (Enemy) and {2} (Character) enter {3}.", e + 1, nextEnemy.Name, randomCharacter.CharName, nextLocationName);

                    // Checks battleLog to see if the selected character has taken damage from the selected enemny or within the selected location.
                    bool charhasFought = battleLog.Exists(b => b.CharName == randomCharacter.CharName && (b.EnName == nextEnemy.Name || b.LocName == nextLocationName));

                    // Implemented a counter for the while loop as a failsafe in case there are no characters avilable that do not meet the charhasFought conditions.
                    int counter = 0;

                    while (charhasFought == true && characters.Count > 2)
                    {
                        Console.WriteLine("{0} has already taken damage from this enemy OR in this location. A new character is chosen.", randomCharacter.CharName);

                        // Select a New Random Character
                        randomCharacterIndex = new Random().Next(0, characters.Count); // Index of random character 
                        randomCharacter = characters[randomCharacterIndex]; // Selected random character and access all properties

                        // Checks battleLog to see if the newly selected character has taken damage from the selected enemny or within the selected location.
                        charhasFought = battleLog.Exists(b => b.CharName == randomCharacter.CharName && (b.EnName == nextEnemy.Name || b.LocName == nextLocationName));

                        // Break out of loop when you find a character that has not taken damage from the selected enemny or within the selected location.
                        if (charhasFought == false || counter >= 9)
                        {
                            Console.WriteLine("{0} enters the battle.", randomCharacter.CharName);
                            break;
                        }
                        counter++;
                    };

                    if (randomCharacter.xEnemy == nextEnemy.Name || randomCharacter.xLocation == nextLocationName) // If character cannot fight this enemy or location
                    {
                        Console.WriteLine("{0} cannot fight {1} OR cannot fight in {2} and takes full enemy damage({3} HP).", randomCharacter.CharName, nextEnemy.Name, nextLocationName, nextEnemy.Damage);

                        randomCharacter.HitPoints -= nextEnemy.Damage; // Character HitPoints are reduced by the enemy damage value

                        if (randomCharacter.HitPoints <= 0) // If this character has died...
                        {
                            Console.WriteLine("{0} has died and can no longer fight.", randomCharacter.CharName);
                            characters.RemoveAt(randomCharacterIndex);
                            enemyDeck.Pop(); // Remove enemy from the enemyDeck
                        }
                        else // Else, deduct the enemy damage from the character
                        {
                            Console.WriteLine("{0} hit points are deducted from {1}. {1} has {2} hit points remaining.", nextEnemy.Damage, randomCharacter.CharName, randomCharacter.HitPoints);
                            battleLog.Add(new BattleLog() { CharName = randomCharacter.CharName, EnName = nextEnemy.Name, LocName = nextLocationName }); // Adding new battle log as this character has taken damage from this enemy
                            enemyDeck.Pop(); // Remove enemy from the enemyDeck
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0} has defeated {1}.", randomCharacter.CharName, nextEnemy.Name);
                        enemyDeck.Pop(); // Remove enemy from the enemyDeck
                    }

                    if (characters.Count <= 0) // If all characters have been defeated, break from loop.
                    {
                        break;
                    }

                }

                if (characters.Count <= 0) // If all characters have been defeated, break from loop.
                {
                    break;
                }
                else // If characters still exist, move to next location.
                {
                    Console.WriteLine("All enemies in this location have been battled. Onto the next battle arena.");
                    LocationsQueue.Dequeue(); // Removes location from deck.
                }
            }

            if (characters.Count > 0) // If there are still characters left, display winning message.
            {
                Console.WriteLine("You win! Enemies in all locations have been defeated.");
                Console.WriteLine("Remaining Character(s):");
                foreach (Character character in characters)
                {
                    Console.WriteLine("{0}: {1} HP", character.CharName, character.HitPoints);
                }
            }
            else // If there are still characters left, display losing message.
            {
                Console.WriteLine("You lose. All of your characters have been defeated.");
            }

        }
    }
    public class Enemy
    {
        public string Name { get; set; }
        public int Damage { get; set; }
    }

    public class Location
    {
        public string LocationName { get; set; }
        public int NumbOfEnemies { get; set; }
    }

    public class Character
    {
        public string CharName { get; set; }
        public int HitPoints { get; set; }
        public string xLocation { get; set; }
        public string xEnemy { get; set; }
    }

    public class BattleLog
    {
        public string CharName { get; set; }
        public string EnName { get; set; }
        public string LocName { get; set; }
    }
}