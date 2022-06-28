Game game = new Game();
game.Start();

class Hero
{
    public string Name { get; set; }
    public int BaseStrength { get; set; }
    public int BaseDefence { get; set; }
    public int OriginalHealth { get; set; }
    public int CurrentHealth { get; set; }
    public Weapon EquippedWeapon { get; set; }
    public Armour EquippedArmour { get; set; }

    public Hero(string name, int baseStrength, int baseDefence, int originalHealth, Weapon equippedWeapon, Armour armour)
    {
        Name = name;
        BaseStrength = baseStrength;
        BaseDefence = baseDefence;
        OriginalHealth = originalHealth;
        EquippedWeapon = equippedWeapon;
        EquippedArmour = armour;
        CurrentHealth = originalHealth;
    }

    //Show all basic property of the hero. When player enter inventory page, it would be invoked. I used this in inventory page to let them player know these information to help them to choose weapons and armours.
    public string ShowStats()
    {
        return $"The hero {Name}:\n" +
            $"Base Strength: {BaseStrength}\n" +
            $"Base Defence: {BaseDefence}\n" +
            $"Original Health: {OriginalHealth}\n" +
            $"Current Health: {CurrentHealth}";
    }

    //Show the weapon and armour the hero is wearing. When player enter inventory page, it would be invoked. Include this in project also for check whether changing equipment work or not.
    public string ShowInventory()
    {
        return $"The current weapon equipped with is {EquippedWeapon.Name}.\n" +
            $"The current armour equipped with is {EquippedArmour.Name}.";
    }

    //Change the equipment of hero. When player enter a relative number in ventory page, this method would be invoked.
    public void EquipWeapon(Weapon newWeapon)
    {
        EquippedWeapon = newWeapon;
    }

    //Change the equipment of hero. When player enter a relative number in ventory page, this method would be invoked.
    public void EquipArmour(Armour newArmour)
    {
        EquippedArmour = newArmour;
    }
}

class Monster
{
    public Monster(string name, int strength, int defense,int originalHealth)
    {
        Name=name;
        Strength = strength;
        Defense=defense;
        OriginalHealth=originalHealth;
        CurrentHealth=originalHealth;
    }
    public string Name { get; set; }
    public int Strength { get; set; }
    public int Defense { get; set; }
    public int OriginalHealth { get; set; }
    public int CurrentHealth { get; set; }
}

class Weapon
{
    public Weapon(string name,int power)
    {
        Name = name;
        Power = power;
    }

    public string Name { get; set; }
    public int Power { get; set; }
}

class Armour
{
    public Armour(string name,int power)
    {
        Name = name;
        Power = power;
    }
    public string Name { get; set; }
    public int Power { get; set; }
}

static class WeaponList
{
    public static List<Weapon> Weapons = new List<Weapon>();
}

static class ArmourList
{
    public static List<Armour> Armours = new List<Armour>();
}

class Fight
{
    public Hero hero { get; set; }
    public Monster monster { get; set; }
    public bool fightOver { get; set; } = false;
    public Game game { get; set; }
    public Fight(Game game1, Hero hero1,Monster monster1)
    {
        game = game1;
        hero = hero1;
        monster = monster1;

        //Keep fighting until the hero win or lose. When fight begins, this method would be invoked.
        while (!fightOver)
        {
            HeroTurn();
            if (Win(monster))
            {
                game.coins++;
                Console.WriteLine("You earned a coin. You can spend a coin to strengthen your hero.");
                Console.WriteLine("Back to the main menu success.");
                game1.ToMainMenu();
                break;
            }
            MonsterTurn();
            if (Lose(hero))
            {
                Console.WriteLine("Back to the main menu success.");
                game1.ToMainMenu();
                break;
            }
        }
        Console.WriteLine(Game.win);
        Console.WriteLine(Game.lose);
    }

    //Hero turn to attack, the current health of monster would decrease. This method and MonsterTurn method would be invoked by turns.
    public void HeroTurn()
    {
        monster.CurrentHealth -= (hero.BaseStrength + hero.EquippedWeapon.Power);       
        Console.WriteLine("Hero attack!");
    }

    //Monster turn to attack, the current health of hero would decrease. This method and HeroTurn method would be invoked by turns.
    public void MonsterTurn()
    {
        hero.CurrentHealth -= monster.Strength;
        Lose(hero);
        Console.WriteLine("Monster attack!");
    }

    //Check monster's current health to decide whether hero wins or not. This would be invoked after hero attack.
    public bool Win(Monster monster)
    {
        if (monster.CurrentHealth <= 0)
        {
            Game.win++;
            fightOver = true;
            game.coins++;
            Console.WriteLine("Hero win!!!");
            return true;
        }
        return false;
    }

    //Check hero's current health to decide whether hero wins or not. This would be invoked after monster attack.
    public bool Lose(Hero hero)
    {
        if (hero.CurrentHealth <= 0)
        {
            Game.lose++;
            fightOver=true;
            Console.WriteLine("Hero lose...");
            return true;
        }
        return false;
    }
}

class Game
{
    public static int win = 0;
    public static int lose = 0;
    public int coins = 0;
    public Hero hero1;
    public Monster monster1;
    List<Monster> monsters;
    public Game()
    {
        for(int i = 0; i < 3; i++)
        {
            WeaponList.Weapons.Add(new Weapon($"weapon{i+1}", i+2));
            ArmourList.Armours.Add(new Armour($"Armour{i+1}", i+2));
        }
        hero1 = new Hero("hero1", 3, 3, 15, WeaponList.Weapons[0], ArmourList.Armours[0]);
        monster1 = new Monster("Monster1", 1, 1, 9);
        monsters = new List<Monster>() { monster1};
        for (int i = 0; i < 5; i++)
        {
            monsters.Add(new Monster($"monster{i + 2}",  i + 1 , i, i + 10));
        }    
    }

    //Show all instructor to player. This would be invoked everytime player back to the main menu.
    public void Display()
    {
        Console.WriteLine();
        Console.WriteLine("This is the main menu:");
        Console.WriteLine("Enter \"a\" to show statics.");
        Console.WriteLine("Enter \"b\" to show your inventory.");
        Console.WriteLine("Enter \"c\" to fight with monster.");
        Console.WriteLine("Enter \"d\" to strengthen your hero.");
        Console.WriteLine();
        SelectOption();
    }

    //Receive order from player. If user enter a right letter, pass it to SwitchMenus method, or remind user enter again if they enter a wrong letter.
    public void SelectOption()
    {
        bool isValid = false;
        string checkedInput = "";
        do
        {
            string userInput = Console.ReadLine();
            if (userInput == "a" || userInput == "b" || userInput == "c" || userInput == "d")
            {
                isValid = true;
                checkedInput = userInput;
            }
            else
            {
                Console.WriteLine("Please enter a valid letter.");
            }
        } while (!isValid);
        SwitchMenus(checkedInput);
    }

    //According to user's input, show different message.
    public void SwitchMenus(string checkedInput)
    {
        switch (checkedInput)
        {
            //Show main menu page information.
            case "a":
                Console.WriteLine();
                Console.WriteLine("This is \"Statics\" page:");
                Console.WriteLine($"You have played {Game.win + Game.lose} games.");
                Console.WriteLine($"Number of fights won: {Game.win}");
                Console.WriteLine($"Number of fights lose: {Game.lose}");
                Console.WriteLine();
                ToMainMenu();
                break;

            //Show inventory page information. User could change their weapon and armour here.    
            case "b":              
                Console.WriteLine("This is \"Inventory\" page:");
                Console.WriteLine("You can change weapons and armours here.");
                Console.WriteLine();
                Console.WriteLine(hero1.ShowStats());
                Console.WriteLine(hero1.ShowInventory());
                Console.WriteLine();
                Console.WriteLine("There are all weapons you have: ");
                for (int i = 0; i < WeaponList.Weapons.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {WeaponList.Weapons[i].Name}");
                }
                Console.WriteLine("Enter the relative number to wear the weapon. Or you can enter other numbers to back to the main menu:");
                int weaponNum = int.Parse(Console.ReadLine());
                if (weaponNum <= WeaponList.Weapons.Count && weaponNum > 0)
                {
                    //int.Parse(Console.ReadKey().Key.ToString()) <= WeaponList.Weapons.Count
                    hero1.EquipWeapon(WeaponList.Weapons[weaponNum - 1]);
                    Console.WriteLine("Weapon equip success!");
                }
                else
                {
                    Console.WriteLine("Back to the main menu success.");
                    ToMainMenu();
                }
                Console.WriteLine();
                Console.WriteLine("There are all armours you have: ");
                for (int i = 0; i < ArmourList.Armours.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ArmourList.Armours[i].Name}");
                }
                Console.WriteLine("Enter the relative number to wear the aromour or back. Or you can enter other numbers to back to the main menu:");
                int aromourNum = int.Parse(Console.ReadLine());
                if (aromourNum <= ArmourList.Armours.Count && aromourNum > 0)
                {
                    hero1.EquipArmour(ArmourList.Armours[aromourNum - 1]);
                    Console.WriteLine("Amour equip success!");
                }
                else
                {
                    Console.WriteLine("Back to the main menu success.");
                    ToMainMenu();
                }
                ToMainMenu();
                Console.WriteLine();
                break;

            //Fight with a random monster.
            case "c":
                Random num = new Random();
                int j = num.Next(6);
                Monster monsterToFight = monsters[j];
                Console.WriteLine($"Fight with {monsterToFight.Name}!");
                Fight fight = new Fight(new Game() ,hero1, monster1);
                break;

            //User could spend coins to strengthen the hero.
            case "d":
                StrengthenHero();
                ToMainMenu();
                break;
        }
    }

    //Player could spend money to strengthen some basic property. This method would be invoked in main menu and enter "d" key.
    public void StrengthenHero()
    {
        if (coins > 0)
        {
            coins--;
            hero1.BaseStrength++;
            hero1.BaseDefence++;
            Console.WriteLine("Hero strengthen success!");
        }
        else
        {
            Console.WriteLine("You do not have enough coin to strength hero.");
        }
    }

    //Ask player to enter name and welcome the player. This method would be invoked at begin.
    public void Start()
    {
        Console.WriteLine("Please enter your name. Click Enter key to save your name:");
        string userName = Console.ReadLine();
        Console.WriteLine();
        Console.WriteLine($"Welcome! {userName}");
        ToMainMenu();
    }

    //This method helps player to back to the main page. This method would be invoked everytime player want to back to the main menu.
    public void ToMainMenu()
    {
        Display();
        SelectOption();
    }
}