using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fun
{
    class Enemy:Character
    {
        public Enemy(string name, int maxHp):base(name,maxHp)
        {

        }
        public Enemy(string name, int maxHp, int offense, int defense) : base(name,maxHp,offense,defense)
        {

        }
        public Enemy(string name, int maxHP,int Level = 1) :base(name,maxHP,Dice.Next(0,Level),Dice.Next(0,1))
        {

        }
    }
}
