using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.AttackSystem
{
    public interface IEffect
    {
        public  void CreateFromLine(string[] words);

        public void Apply(ITargetable itargetable);

    }

    public class EffectFactory
    {
        public static List<IEffect> ParseEffect(string effectText)
        {

            List<IEffect> effectlist = new List<IEffect>();
            string[] lines = effectText.Split("\n");
            bool AddCardToChoice = false;
            int CardToAddtoChoice = 0;
            foreach (string line in lines)
            {
                string[] words = line.Split();
                IEffect effect = null;
                ChooseEffect chooseEffect = null;
                if (words.Length >= 0)
                {
                    switch (words[0])
                    {
                        case "Inflict":
                            effect = new InflictEffect(0, DamageType.Health);
                            effect.CreateFromLine(words);
                            effectlist.Add(effect);
                            break;
                        case "Choose":
                            chooseEffect = new ChooseEffect("empty_key");
                            chooseEffect.CreateFromLine(words);
                            effectlist.Add(effect);
                            CardToAddtoChoice = 2;
                            break;
                        case "Gain":
                            effect = new GainEffect("error", 0);
                            effect.CreateFromLine(words);
                            effectlist.Add(effect);
                            break;
                        case "(":
                            AddCardToChoice = true;
                            break;
                        case ")":
                            CardToAddtoChoice--;
                            AddCardToChoice = false;
                            break;
                    }
                }
                if (AddCardToChoice && CardToAddtoChoice == 2)
                {
                    chooseEffect.Choices1.Add(effect);
                }
                else if (AddCardToChoice && CardToAddtoChoice == 1)
                {
                    chooseEffect.Choices2.Add(effect);
                }
            }
            return effectlist;
        }

    }
}
