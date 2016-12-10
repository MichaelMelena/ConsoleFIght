using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fun
{
    class Program
    {
        static void Main(string[] args)
        {
            Warior p = new Warior("Michael", 50, 3, 0);
            Warior r = new Warior("Robot", 20,1 , 0);
            Arena a = new Arena(p, r);
            a.Fight();


            Console.ReadLine();

        }
    }
    class Character
    {

        protected static Random Dice = new Random();
        protected int MaxHP { get; set; }
        protected int CurrentHP { get; set; }
        protected string Name { get; set; }
        protected int Offense { get; set; }
        protected int Defense { get; set; }
        protected string popis { get; set; }
        protected ConsoleKeyInfo ChoosenOption { get; set; }
        protected int DefenseDiceThrow{ get; set; }
        protected int OffenseDiecThrow { get; set; }
      
       

       public Character(string name, int maxHp )
        {
            this.MaxHP = maxHp;
            this.CurrentHP = maxHp;
            this.Name = name;
            this.Defense = 0;
            this.Offense = 0;
            this.popis = "Character";
            
        
        }
        public Character(string name, int maxHp,int offense,int defense)
        {
            
            this.MaxHP = maxHp;
            this.CurrentHP = maxHp;
            this.Name = name;
            this.Defense = Defense;
            this.Offense = offense;
           
        }


        public delegate void characterDeath(object source, CharacterEventArgs args);
        public event characterDeath CharacterDeath;
        protected virtual void CharacterDied(string name)
        {
            if(CharacterDeath!=null)
            {
                CharacterDeath(this, new CharacterEventArgs() { Name = name });
            }
        }

        public delegate void changeHealt(object source, CharacterEventArgs args);
        public event changeHealt HealhChange;
        protected virtual void HealChanged(int healt)
        {
            if (healt <= 0)
            {
                CharacterDied(this.Name);
            }
            if (HealhChange!=null)
            {
                
                HealhChange(this, new CharacterEventArgs() { Healt = healt });
            }
        }

        public event EventHandler<CharacterEventArgs>Attacking;
        protected virtual void OnAttacking(int damage)
        {
            if(Attacking!=null)
            {
                Attacking(this, new CharacterEventArgs() { Attack = damage });
            }
        }

        public virtual void Attack()
        {
            this.OffenseDiecThrow = ThrowDice();
            Console.WriteLine(string.Format("{0} -- {1} -- Attacks for {2} damage", this.popis, this.Name, this.Offense+OffenseDiecThrow));
            OnAttacking((Offense+OffenseDiecThrow));
        }
        public virtual void Defend(object source,CharacterEventArgs args)
        {
            this.DefenseDiceThrow = this.ThrowDice();

            if (args.Attack > this.Defense+DefenseDiceThrow)
            {
                Console.WriteLine(string.Format("{0} -- {1} -- was damaged for {2}", this.popis, this.Name, (args.Attack - (this.Defense+DefenseDiceThrow))));
                this.CurrentHP = this.CurrentHP - (args.Attack - (this.Defense+DefenseDiceThrow));
                
                CurentHitPoints();
                HealChanged(CurrentHP);
            }
            else if (args.Attack == this.Defense+DefenseDiceThrow)
            {
                Console.WriteLine(string.Format("{0} -- {1} --  wasn't damaged Atack was same as his defense", this.popis, this.Name));
                CurentHitPoints();
            }
            else
            {
                Console.WriteLine(string.Format("{0} -- {1} -- was wasn't damaged Atack was lower than his defense", this.popis, this.Name));
                CurentHitPoints();
            }
        }
        protected int ThrowDice()
        {
          return Dice.Next(1, 10);
        }
        public virtual void CurentHitPoints()
        {
            Console.WriteLine(string.Format("{0} -- {1} -- has {2} hitpoints", this.popis, this.Name, this.CurrentHP));
        }
        public virtual void MoveHint()
        {
            Console.WriteLine(string.Format(" {0} -- {1} -- press:",this.popis,this.Name));
            Console.WriteLine("-a- for attack!");
        }

        public virtual void PlayerMove()
        {
            
            ChoosenOption = Console.ReadKey();
            if(ChoosenOption.Key == ConsoleKey.A)
            {
                Attack();
                
            }
        }
    }
    class Warior:Character
    {
        int LastTakenDamage { get; set; }
        public Warior(string name, int maxHp, int offense, int defense) : base(name, maxHp, offense, defense)
        {
            this.popis = "Warior";
            this.LastTakenDamage = 0;
        }
        public virtual void Bandage()
        {
            if (this.CurrentHP + LastTakenDamage < this.MaxHP)
            {
                this.CurrentHP += LastTakenDamage;
                Console.WriteLine(string.Format("{0} -- {1} -- healed himself for {2} hitpoints",this.popis,this.Name,this.LastTakenDamage));
                this.CurentHitPoints();
                this.HealChanged(this.CurrentHP);
            }
        }
        public override void Defend(object source, CharacterEventArgs args)
        {
            
            
            base.Defend(source ,args);
            if (args.Attack > this.Defense+base.DefenseDiceThrow)
            {
                this.LastTakenDamage = (args.Attack - this.Defense+base.DefenseDiceThrow);
            }
        }
        public override void MoveHint()
        {
            base.MoveHint();
            Console.WriteLine("-b- to use bandages!");
        }
        public override void PlayerMove()
        {
            base.PlayerMove();
            if(base.ChoosenOption.Key ==ConsoleKey.B)
            {
                Bandage();
                
            }
            
        }



    }
    class CharacterEventArgs : EventArgs
    {
        public string Name { get; set; }
        public int Healt { get; set; }
        public int Attack { get; set; }

    }
    class Arena
    {
        protected int Round { get; set; }
        protected bool FightOn { get; set; }
        protected Character Player { get; set; }
        protected Character Enemy { get; set; }
        
        public Arena(Character player, Character ennemy)
        {
            this.Player = player;
            this.Enemy = ennemy;
            FightOn = true;
            player.Attacking += Enemy.Defend;
            Enemy.Attacking += Player.Defend;
            this.Player.CharacterDeath += OnCharacterDeath;
            this.Enemy.CharacterDeath += OnCharacterDeath;

        }
        public void Fight()
        {
            while(FightOn)
            {
                Player.MoveHint();
                Player.PlayerMove();
                Enemy.Attack();
                Console.WriteLine();
                
            }

            Console.WriteLine("The end");


        }
        public void OnCharacterDeath(object source , CharacterEventArgs args)
        {
            FightOn = false;
        }

        
    }
}

