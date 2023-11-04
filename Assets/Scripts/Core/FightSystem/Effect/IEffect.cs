using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.FightSystem.AttackSystem
{
    public interface IEffect
    {
        public void CreateFromLine(string[] words);

        public void Apply(ITargetable itargetable);

        public bool SelfTarget();

    }

    public interface IWordBuilder
    {
        public IEffect BuildEffect(string[] words);

        public string GetKeyWord();

        public bool NestedKeyword();

    }

    public class EffectFactory
    {

        public static Dictionary<string, Type> _typeMatch  = new Dictionary<string, Type>()
        {
            {"InflictBuilder",typeof( Core.FightSystem.AttackSystem.InflictEffect.InflictBuilder ) },
            {"GainBuilder",typeof(Core.FightSystem.AttackSystem.GainEffect.GainBuilder ) },
            {"MayBuilder",typeof(Core.Exploration.ExplorationKeyword.May.MayBuilder ) },
            {"EncounterBuilder",typeof(Core.Exploration.ExplorationKeyword.Encounter.EncounterBuilder ) }
        };

        public static Dictionary<string, IWordBuilder> _effectsBuilders = new Dictionary<string, IWordBuilder>();

        public static List<IEffect> ParseEffect(string effectText)
        {
            List<IEffect> effectlist = new List<IEffect>();
            string[] lines = effectText.Split("\n");
            string nestedlines = "";
            bool nestedEffect = false;

            foreach (string line in lines)
            {
                string[] words = line.Trim().Split();
                if ( nestedEffect && words[0].CompareTo("}") != 0 && words[0].CompareTo( "{") != 0 )
                {
                    nestedlines += line +" \n";
                }
                else
                {
                    IEffect effect = null;
                    if (words.Length >= 0)
                    {
                        if( _effectsBuilders.ContainsKey(words[0]) )
                        {
                            effect = _effectsBuilders[words[0]].BuildEffect(words);
                            nestedEffect = _effectsBuilders[words[0]].NestedKeyword();
                        }
                        else
                        {
                            switch (words[0])
                            {
                                case "Injury":
                                    effect = new InjuryEffect();
                                    effect.CreateFromLine(words);
                                    break;
                                case "Loss":
                                    effect = new LossEffect();
                                    effect.CreateFromLine(words);
                                    break;
                                case "Select":
                                    effect = new SelectEffect();
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
                        }
                        if (effect != null)
                            effectlist.Add(effect);
                    }
                }
            }
            return effectlist;
    }

        public static void Register(IWordBuilder builder)
        {

            if(_effectsBuilders == null)
            {
                _effectsBuilders = new Dictionary<string, IWordBuilder>();
            }

            _effectsBuilders.Add(builder.GetKeyWord(), builder);

        }

        public static Type GetEffectTypeByName(string typename)
        {
            return _typeMatch[typename];
        }

    }
}
