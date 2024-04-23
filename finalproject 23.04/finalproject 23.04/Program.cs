using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace finalproject_23._04
{
    internal class Program
    {
        public class Item
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public int Value { get; set; }
            public double Weight { get; set; }

            public Item(string name, string type, int value, double weight)
            {
                Name = name;
                Type = type;
                Value = value;
                Weight = weight;
            }

            public override string ToString()
            {
                return $"Предмет: {Name}, Тип: {Type}, Вартість: {Value}, Вага: {Weight}";
            }
        }

        public class Character
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public int Health { get; set; }
            public int Strength { get; set; }
            public int Agility { get; set; }
            public int Intelligence { get; set; }
            public List<Item> Inventory { get; set; }

            public Character(string name, int level, int health, int strength, int agility, int intelligence)
            {
                Name = name;
                Level = level;
                Health = health;
                Strength = strength;
                Agility = agility;
                Intelligence = intelligence;
                Inventory = new List<Item>();
            }

            public override string ToString()
            {
                return $"Персонаж: {Name}, Рівень: {Level}, Здоров'я: {Health}, " +
                       $"Сила: {Strength}, Спритність: {Agility}, Інтелект: {Intelligence}";
            }
        }
        public class Game
        {
            public List<Character> Characters { get; set; }
            public List<Item> Items { get; set; }

            public Game()
            {
                Characters = new List<Character>();
                Items = new List<Item>();
            }

            public void AddCharacter(Character character)
            {
                Characters.Add(character);
            }

            public void RemoveCharacter(string name)
            {
                Character characterToRemove = Characters.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
                if (characterToRemove != null)
                {
                    Characters.Remove(characterToRemove);
                    Console.WriteLine($"Персонаж {name} успішно видалено.");
                }
                else
                {
                    Console.WriteLine($"Персонаж {name} не знайдено.");
                }
            }

            public void AddItem(string characterName, Item item)
            {
                Character character = Characters.FirstOrDefault(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
                if (character != null)
                {
                    character.Inventory.Add(item);
                    Console.WriteLine($"Предмет {item.Name} додано до інвентаря персонажа {characterName}.");
                }
                else
                {
                    Console.WriteLine($"Персонаж {characterName} не знайдено.");
                }
            }

            public void RemoveItem(string characterName, string itemName)
            {
                Character character = Characters.FirstOrDefault(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
                if (character != null)
                {
                    Item itemToRemove = character.Inventory.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
                    if (itemToRemove != null)
                    {
                        character.Inventory.Remove(itemToRemove);
                        Console.WriteLine($"Предмет {itemName} видалено з інвентаря персонажа {characterName}.");
                    }
                    else
                    {
                        Console.WriteLine($"Предмет {itemName} не знайдено в інвентарі персонажа {characterName}.");
                    }
                }
                else
                {
                    Console.WriteLine($"Персонаж {characterName} не знайдено.");
                }
            }

            public void ShowInventory(string characterName)
            {
                Character character = Characters.FirstOrDefault(c => c.Name.Equals(characterName, StringComparison.OrdinalIgnoreCase));
                if (character != null)
                {
                    Console.WriteLine($"Інвентар персонажа {characterName}:");
                    foreach (var item in character.Inventory)
                    {
                        Console.WriteLine(item);
                    }
                }
                else
                {
                    Console.WriteLine($"Персонаж {characterName} не знайдено.");
                }
            }

            public void SaveGame(string filePath)
            {
                XDocument doc = new XDocument(
                    new XElement("Game",
                        new XElement("Characters",
                            from character in Characters
                            select new XElement("Character",
                                new XElement("Name", character.Name),
                                new XElement("Level", character.Level),
                                new XElement("Health", character.Health),
                                new XElement("Strength", character.Strength),
                                new XElement("Agility", character.Agility),
                                new XElement("Intelligence", character.Intelligence),
                                new XElement("Inventory",
                                    from item in character.Inventory
                                    select new XElement("Item",
                                        new XElement("Name", item.Name),
                                        new XElement("Type", item.Type),
                                        new XElement("Value", item.Value),
                                        new XElement("Weight", item.Weight)
                                    )
                                )
                            )
                        ),
                        new XElement("Items",
                            from item in Items
                            select new XElement("Item",
                                new XElement("Name", item.Name),
                                new XElement("Type", item.Type),
                                new XElement("Value", item.Value),
                                new XElement("Weight", item.Weight)
                            )
                        )
                    )
                );
                doc.Save(filePath);
                Console.WriteLine($"Дані гри збережено в файлі: {filePath}");
            }
        }
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Game game = new Game();

            while (true)
            {
                Console.WriteLine("Оберіть опцію:");
                Console.WriteLine("1. Додати персонажа");
                Console.WriteLine("2. Видалити персонажа");
                Console.WriteLine("3. Додати предмет персонажу");
                Console.WriteLine("4. Видалити предмет з інвентаря персонажа");
                Console.WriteLine("5. Показати інвентар персонажа");
                Console.WriteLine("6. Вийти");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\nДодавання нового персонажа...");
                        Character character = CreateCharacter();
                        game.AddCharacter(character);
                        break;
                    case "2":
                        Console.WriteLine("\nВидалення персонажа...");
                        Console.Write("Введіть ім'я персонажа для видалення: ");
                        string characterNameToRemove = Console.ReadLine();
                        game.RemoveCharacter(characterNameToRemove);
                        break;
                    case "3":
                        Console.WriteLine("\nДодавання предмету до інвентаря персонажа...");
                        Console.Write("Введіть ім'я персонажа: ");
                        string characterNameToAddItem = Console.ReadLine();
                        Console.Write("Введіть дані предмета:");
                        Item itemToAdd = CreateItem();
                        game.AddItem(characterNameToAddItem, itemToAdd);
                        break;
                    case "4":
                        Console.WriteLine("\nВидалення предмету з інвентаря персонажа...");
                        Console.Write("Введіть ім'я персонажа: ");
                        string characterNameToRemoveItem = Console.ReadLine();
                        Console.Write("Введіть ім'я предмету для видалення: ");
                        string itemNameToRemove = Console.ReadLine();
                        game.RemoveItem(characterNameToRemoveItem, itemNameToRemove);
                        break;
                    case "5":
                        Console.WriteLine("\nПоказ інвентаря персонажа...");
                        Console.Write("Введіть ім'я персонажа: ");
                        string characterNameToDisplayInventory = Console.ReadLine();
                        game.ShowInventory(characterNameToDisplayInventory);
                        break;
                    case "6":
                        Console.WriteLine("\nЗбереження даних гри...");
                        Console.Write("Введіть назву файлу: ");
                        string saveFilePath = Console.ReadLine();
                        game.SaveGame(saveFilePath);
                        Console.WriteLine("\nЗавершення програми.");
                        return;
                    default:
                        Console.WriteLine("\nНевірний вибір. Будь ласка, виберіть опцію знову.");
                        break;
                }
            }
        }

        static Character CreateCharacter()
        {
            Console.WriteLine("Введіть дані персонажа:");
            Console.Write("Ім'я: ");
            string name = Console.ReadLine();
            Console.Write("Рівень: ");
            int level = int.Parse(Console.ReadLine());
            Console.Write("Здоров'я: ");
            int health = int.Parse(Console.ReadLine());
            Console.Write("Сила: ");
            int strength = int.Parse(Console.ReadLine());
            Console.Write("Спритність: ");
            int agility = int.Parse(Console.ReadLine());
            Console.Write("Інтелект: ");
            int intelligence = int.Parse(Console.ReadLine());
            return new Character(name, level, health, strength, agility, intelligence);
        }

        static Item CreateItem()
        {
            Console.Write("Назва: ");
            string name = Console.ReadLine();
            Console.Write("Тип: ");
            string type = Console.ReadLine();
            Console.Write("Вартість: ");
            int value = int.Parse(Console.ReadLine());
            Console.Write("Вага: ");
            double weight = double.Parse(Console.ReadLine());
            return new Item(name, type, value, weight);
        } 
    }
}
