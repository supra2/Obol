using Core.FightSystem.AttackSystem;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public async UniTask Execute()
        {
           var target =  await UICombatController.Instance.SelectAdversaire(CancellationToken.None);
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
