using Core.FightSystem.AttackSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.FightSystem.CombatFlow
{
    public class AttackCommand : ICommand
    {

        #region Members
        #region Hidden
        /// <summary>
        /// callback lanched for attack once target is determined
        /// </summary>
        protected Action<ITargetable> _callback;
        /// <summary>
        /// Attack resolve
        /// </summary>
        protected bool _ended;
        #endregion
        #endregion

        #region Initialisation
        //-------------------------------------------------------
        public AttackCommand(Action<ITargetable> callback)
        {
            _callback = callback;
           
        }
        //-------------------------------------------------------
        #endregion

        #region Public Methods
        public void Execute()
        {
            UICombatController.Instance.SelectAdversaire(
                OnAdversaireSelected);
        }
        //-------------------------------------------------------
        public bool IsCommandEnded()
        {
            return _ended;
        }
        //-------------------------------------------------------
        protected void OnAdversaireSelected(ITargetable targetable)
        {
            _callback(targetable);
            _ended = true;
        }
        //-------------------------------------------------------
        #endregion

    }
}
