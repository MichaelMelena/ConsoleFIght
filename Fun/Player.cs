using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fun
{
    class Player:Character
    {
        public Player(string name,int maxHp):base(name,maxHp)
        {

        }
        public Player(string name,int maxHp, int offense ,int defense) : base(name,maxHp,offense,defense)
        {

        }
        public virtual void PlayerMove()
        {

            ChoosenOption = Console.ReadKey();
            if (ChoosenOption.Key == ConsoleKey.A)
            {
                Attack();

            }
        }
        public virtual void MoveHint()
        {
            Console.WriteLine(string.Format(" {0} -- {1} -- press:", this.popis, this.Name));
            Console.WriteLine("-a- for attack!");
        }
    }
}
