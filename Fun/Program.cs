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
    
    class Warior:Player
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
        protected virtual void Bandage()
        {


            if (BandageCountdown == 0)
            {
                this.CurrentHP += LastTakenDamage;
                Console.WriteLine(string.Format("{0} -- {1} -- healed himself for {2} hitpoints", this.popis, this.Name, this.LastTakenDamage));
                this.CurentHitPoints();
                this.HealChanged(this.CurrentHP);
                this.BandageCountdown = this.BandageCooldown;
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
        public override void PlayerMove()
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
        protected Player player { get; set; }
        protected Enemy Enemy { get; set; }
    
       

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
            this.player = (Player)player ;
            this.Enemy = (Enemy)ennemy;

            FightOn = true;

            this.player.Attacking += Enemy.Defend;
            this.Enemy.Attacking += this.player.Defend;
            this.player.CharacterDeath += OnCharacterDeath;
            this.Enemy.CharacterDeath += OnCharacterDeath;
            this.EndOfROund += this.player.onEndOfRound;
            this.EndOfROund += this.Enemy.onEndOfRound;
            
        }
        public void Fight()
        {
            while(FightOn)
            {
        
                player.MoveHint();
                player.PlayerMove();
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

