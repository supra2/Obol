using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary>
    /// Slot 
    /// </summary>
    public abstract class Slot : MonoBehaviour, IDropHandler
    {

        #region Event
        protected GameObject _slot;
        #endregion

        #region public Method

        public virtual void OnDrop(PointerEventData eventData)
        {
            _slot= eventData.pointerDrag;
            
        }

        public abstract void UnSlot(Draggable draggable);

        #endregion

    }

}
