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
            string nestedlines = "";
            bool nestedEffect = false;
            foreach (string line in lines)
            {
                string[] words = line.Split();
                if (nestedEffect && words[0] != "}" )
                {
                    nestedlines += line +" \n";
                }
                else
                {
                    IEffect effect = null;
                    if (words.Length >= 0)
                    {
                        switch (words[0])
                        {
                            case "Inflict":
                                effect = new InflictEffect(0, DamageType.Health);
                                effect.CreateFromLine(words);
                                break;
                            case "Gain":
                                effect = new GainEffect("error", 0);
                                effect.CreateFromLine(words);
                                break;
                            case "Bleed":
                                effect = new BleedEffect();
                                effect.CreateFromLine(words);
                                break;
                            case "Select":
                                effect = new SelectEffect();
                                effect.CreateFromLine(words);
                                nestedEffect = true;
                                break;
                            case "Injury":
                                effect = new InjuryEffect();
                                effect.CreateFromLine(words);
                                break;
                            case "Loss":
                                effect = new LossEffect();
                                effect.CreateFromLine(words);
                                break;
                            case "{":
                                string[] newwords = new string[words.Length - 1];
                                Array.Copy(words, 1, newwords, 0, words.Length - 1);
                                string newLine = "";
                                int k = 0;
                                foreach( string strg in newwords )
                                {
                                    if(k ==0)
                                    {
                                        newLine = strg;
                                    }
                                    else 
                                    {
                                        newLine += string.Format("{0} {1}", newLine, strg);
                                    }
                                    k++;
                                }
                                nestedlines= newLine+"\n";
                                nestedEffect = true;
                                break;
                            case "}":
                                IEffect nested =  effectlist[effectlist.Count -1];
                                if( nested is NestedEffect )
                                {
                                    ((NestedEffect)nested).SetNestedEffect(EffectFactory.ParseEffect(nestedlines));
                                }
                                nestedEffect = false;
                                break;
                        }
                        if (effect != null)
                            effectlist.Add(effect);
                    }
                }
            }
            return effectlist;
        }

    }
}
