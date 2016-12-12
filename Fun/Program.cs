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
        protected int Level { get; set; }
        protected int CurrentHP { get; set; }
        protected string Name { get; set; }
        protected int Offense { get; set; }
        protected int Defense { get; set; }
        protected string popis { get; set; }
        protected ConsoleKeyInfo ChoosenOption { get; set; }
        protected int DefenseDiceThrow{ get; set; }
        protected int OffenseDiecThrow { get; set; }
        protected Dictionary<ConsoleKey,int> AbillityPriority { get; set; }
      
       

       public Character(string name, int maxHp )
        {
            this.MaxHP = maxHp;
            this.CurrentHP = maxHp;
            this.Name = name;
            this.Defense = 0;
            this.Offense = 0;
            this.popis = "Character";
            this.Level = 0;
            this.AbillityPriority = new Dictionary<ConsoleKey, int>();
        
        }
        public Character(string name, int maxHp,int offense,int defense)
        {
            
            this.MaxHP = maxHp;
            this.CurrentHP = maxHp;
            this.Name = name;
            this.Defense = Defense;
            this.Offense = offense;
            this.Level = 0;
            this.AbillityPriority = new Dictionary<ConsoleKey, int>();

        }
        protected virtual void FillAbillityPriority()
        {
            this.AbillityPriority.Add(ConsoleKey.A, 1);

        }
        protected ConsoleKey HighestPriorityMove()
        {
            
            int priority = -1;

            foreach (KeyValuePair<ConsoleKey, int> item in this.AbillityPriority)
            {
               if(priority==-1)
                {
                    priority = item.Value;
                }
               else if(item.Value >0)
                {
                    if (priority < item.Value)
                    {
                        priority = item.Value;
                    }
                }
            }
            foreach (KeyValuePair<ConsoleKey, int> item in this.AbillityPriority)
            {

                if (item.Value == priority)
                {
                    return item.Key;

                }

            }
            throw new Exception();
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
        public virtual void onEndOfRound(object source, CharacterEventArgs args)
        {

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

        public virtual void PlayerMove(ConsoleKey chooseOption=ConsoleKey.NoName )
        {
            if (chooseOption != ConsoleKey.NoName)
            {
                ChoosenOption = Console.ReadKey();
            }
            if(ChoosenOption.Key == ConsoleKey.A)
            {
                Attack();
                
            }
        }
    }
    class Warior:Character
    {
        protected int LastTakenDamage { get; set; }
        protected byte  BandageCooldown { get; set; }
        protected byte BandageCountdown { get; set; }

        protected byte StrongAttackCooldown { get; set; }
        protected byte StrongAttackCountdown { get; set; }

        public Warior(string name, int maxHp, int offense, int defense) : base(name, maxHp, offense, defense)
        {
            this.popis = "Warior";

            this.LastTakenDamage = 0;

            this.BandageCooldown = 3;
            this.BandageCountdown = 0;

            this.StrongAttackCooldown = 4;
            this.StrongAttackCountdown = 0;

        }

        protected override void FillAbillityPriority()
        {
            base.FillAbillityPriority();
            base.AbillityPriority.Add(ConsoleKey.B, 2);
            base.AbillityPriority.Add(ConsoleKey.S, 4);
        }
        protected virtual void Bandage()
        {


            if (BandageCountdown == 0)
            {
                this.CurrentHP += LastTakenDamage;
                Console.WriteLine(string.Format("{0} -- {1} -- healed himself for {2} hitpoints", this.popis, this.Name, this.LastTakenDamage));
                this.CurentHitPoints();
                this.HealChanged(this.CurrentHP);
                this.BandageCountdown = this.BandageCooldown;
                this.AbillityPriority[ConsoleKey.B]= 0;
                this.LastTakenDamage = 0;
            }
            else if (BandageCountdown > 0)
            {
                Console.WriteLine(string.Format("{0} -- {1} -- can´t use Bandages for {2} rounds", this.popis, this.Name, this.BandageCountdown));
            }

        }
        
        protected virtual void StrongAttack()
        {
            if( BandageCountdown==0)
            {
                this.OffenseDiecThrow = ThrowDice();
                Console.WriteLine(string.Format("{0} -- {1} -- used Strong attack for {2} damage", this.popis, this.Name, this.Offense + this.Level + OffenseDiecThrow));
                OnAttacking((Offense +this.Level+ OffenseDiecThrow));
            }
            else if(StrongAttackCountdown>0)
            {
                Console.WriteLine(string.Format("{0} -- {1} -- can´t use StrongAttack for {2} rounds", this.popis, this.Name, StrongAttackCountdown));
            }
           
        }
        public override void onEndOfRound(object source,CharacterEventArgs args)
        {
            if(BandageCountdown>0)
            {
                BandageCountdown--;
            }
            if(BandageCountdown==0)
            {
                this.AbillityPriority[ConsoleKey.B] = 3;
            }
            if(StrongAttackCountdown>0)
            {
                StrongAttackCountdown--;
            }
        }
        public override void Defend(object source, CharacterEventArgs args)
        {
            
            
            base.Defend(source ,args);
            if (args.Attack > this.Defense+base.DefenseDiceThrow)
            {
                this.LastTakenDamage = (args.Attack - (this.Defense+base.DefenseDiceThrow));
            }
        }
        public override void MoveHint()
        {
            base.MoveHint();
            Console.WriteLine(string.Format("-b- to use bandages!  cooldown {0} rounds",this.BandageCooldown));
            Console.WriteLine(string.Format("-s- to use Strong attack! cooldown {0} rounds", this.StrongAttackCooldown));
        }
        public override void PlayerMove(ConsoleKey chooseOption = ConsoleKey.NoName)
        {
            base.PlayerMove();
            if(base.ChoosenOption.Key ==ConsoleKey.B)
            {
                Bandage();
                
            }
            if(base.ChoosenOption.Key==ConsoleKey.S)
            {
                StrongAttack();
            }
            
        }



    }
    class CharacterEventArgs : EventArgs
    {
        public string Name { get; set; }
        public int Healt { get; set; }
        public int Attack { get; set; }
        public int Round { get; set; }

    }
    class Arena
    {
        protected int Round { get; set; }
        protected bool FightOn { get; set; }
        protected Character Player { get; set; }
        protected Character Enemy { get; set; }
    
       

        public event EventHandler<CharacterEventArgs> EndOfROund;
        protected virtual void RoundEnded(int round)
        {
            if(EndOfROund!=null)
            {

                EndOfROund(this, new CharacterEventArgs() { Round = round});
                round++;
            }
        }
        public Arena(Character player, Character ennemy)
        {
            this.Player = player;
            this.Enemy = ennemy;

            FightOn = true;

            this.Player.Attacking += Enemy.Defend;
            this.Enemy.Attacking += Player.Defend;
            this.Player.CharacterDeath += OnCharacterDeath;
            this.Enemy.CharacterDeath += OnCharacterDeath;
            this.EndOfROund += this.Player.onEndOfRound;
            this.EndOfROund += this.Enemy.onEndOfRound;
            
            
        }
        public void Fight()
        {
            while(FightOn)
            {
                Player.MoveHint();
                Player.PlayerMove();
                Enemy.Attack();
                Console.WriteLine("-------------------------------------------------------------");
                RoundEnded(Round);
               
                
            }

            Console.WriteLine("The end");


        }
        public void OnCharacterDeath(object source , CharacterEventArgs args)
        {
            FightOn = false;
        }

        
    }
}

