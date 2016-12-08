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

        }
    }
    class Character
    {
       protected int MaxHP { get; set; }
       protected int CurrentHP { get; set; }
       protected string Name { get; set;}
       protected int Offense { get; set; }
       protected int Defense { get; set; }
       protected string popis { get; set; } 
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
        public virtual int Attack()
        {
            Console.WriteLine(string.Format("{0} -- {1} -- Attacks for {2} damage"), this.popis, this.Name, this.Offense);
            return Offense;
        }
        public virtual void Defend(int attack)
        {
            if (attack > this.Defense)
            {
                Console.WriteLine(string.Format("{0} -- {1} -- was damaged for{2}"), this.popis, this.Name, (attack - this.Defense));
                this.CurrentHP = this.CurrentHP - (attack - this.Defense);
                CurentHitPoints();
            }
            else if (attack == this.Defense)
            {
                Console.WriteLine(string.Format("{0} -- {1} -- was wasn't damaged Atack was same as his defense"), this.popis, this.Name);
                CurentHitPoints();
            }
            else
            {
                Console.WriteLine(string.Format("{0} -- {1} -- was wasn't damaged Atack was lower than his defense"), this.popis, this.Name);
                CurentHitPoints();
            }
        }
        public virtual void CurentHitPoints()
        {
            Console.WriteLine(string.Format("{0} -- {1} -- has {2} hitpoints"), this.popis, this.Name, this.CurrentHP);
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
            }
        }
        public override void Defend(int attack)
        {
            if(attack> this.Defense)
            {
                this.LastTakenDamage = (attack - this.Defense);
            }
            base.Defend(attack);
        }

    }
}
