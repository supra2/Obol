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

        public bool SelfTarget();

    }

    public class EffectFactory
    {

        public static List<IEffect> ParseEffect(string effectText)
        {

            List<IEffect> effectlist = new List<IEffect>();
            string[] lines = effectText.Split("\n");
            foreach (string line in lines)
            {
                string[] words = line.Split();
                IEffect effect = null;
                if (words.Length >= 0)
                {
                    switch (words[0])
                    {
                        case "Inflict":
                            effect = new InflictEffect(0, DamageType.Health);
                            effect.CreateFromLine(words);
                            effectlist.Add(effect);
                            break;
                        case "Gain":
                            effect = new GainEffect("error", 0);
                            effect.CreateFromLine(words);
                            effectlist.Add(effect);
                            break;
                        case "Bleed":
                            effect = new BleedEffect();
                            effect.CreateFromLine(words);
                            break;
                        case "Select":
                            effect = new SelectEffect();
                            effect.CreateFromLine(words);
                            break;
                    }
                }
            }
            return effectlist;
        }

    }
}
