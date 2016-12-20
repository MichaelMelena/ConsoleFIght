using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fun
{
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
        protected int DefenseDiceThrow { get; set; }
        protected int OffenseDiecThrow { get; set; }



        public Character(string name, int maxHp)
        {
            this.MaxHP = maxHp;
            this.CurrentHP = maxHp;
            this.Name = name;
            this.Defense = 0;
            this.Offense = 0;
            this.popis = "Character";
            this.Level = 0;


        }
        public Character(string name, int maxHp, int offense, int defense)
        {

            this.MaxHP = maxHp;
            this.CurrentHP = maxHp;
            this.Name = name;
            this.Defense = Defense;
            this.Offense = offense;
            this.Level = 0;

        }


        public delegate void characterDeath(object source, CharacterEventArgs args);
        public event characterDeath CharacterDeath;
        protected virtual void CharacterDied(string name)
        {
            if (CharacterDeath != null)
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
            if (HealhChange != null)
            {

                HealhChange(this, new CharacterEventArgs() { Healt = healt });
            }
        }

        public event EventHandler<CharacterEventArgs> Attacking;
        protected virtual void OnAttacking(int damage)
        {
            if (Attacking != null)
            {
                Attacking(this, new CharacterEventArgs() { Attack = damage });
            }
        }

        public virtual void Attack()
        {
            this.OffenseDiecThrow = ThrowDice();
            Console.WriteLine(string.Format("{0} -- {1} -- Attacks for {2} damage", this.popis, this.Name, this.Offense + OffenseDiecThrow));
            OnAttacking((Offense + OffenseDiecThrow));
        }
        public virtual void Defend(object source, CharacterEventArgs args)
        {
            this.DefenseDiceThrow = this.ThrowDice();

            if (args.Attack > this.Defense + DefenseDiceThrow)
            {
                Console.WriteLine(string.Format("{0} -- {1} -- was damaged for {2}", this.popis, this.Name, (args.Attack - (this.Defense + DefenseDiceThrow))));
                this.CurrentHP = this.CurrentHP - (args.Attack - (this.Defense + DefenseDiceThrow));

                CurentHitPoints();
                HealChanged(CurrentHP);
            }
            else if (args.Attack == this.Defense + DefenseDiceThrow)
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
        

        
    }
}
