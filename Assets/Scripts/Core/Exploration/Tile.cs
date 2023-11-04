using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Exploration
{
    [CreateAssetMenu(fileName = "Tile",
      menuName = "Exploration/Tile",
      order = 2)]
    [Serializable]
    public class Tile : ScriptableObject, ICloneable
    {

        #region Enum

        [Flags]
        public enum Direction
        {
            None =0,
            Up = 1,
            Bottom = 2,
            Left = 4,
            Right = 8
        }

        #endregion

        #region Members
        #region Visible
        [SerializeReference]
        public Sprite _sprite;
        [SerializeReference]
        public float _rotationY;
        [SerializeReference]
        public bool NorthDirection;
        [SerializeReference]
        public bool SouthDirection;
        [SerializeReference]
        public bool WestDirection;
        [SerializeReference]
        public bool EastDirection;
        #endregion
        #region Hidden
        protected Direction _availableDirections =Direction.None;
        protected ExplorationEvent _associatedEvent;
        #endregion
        #endregion

        #region Getter

        public Sprite Sprite
        {
            get => _sprite;

            set => _sprite = value;
        }

        public float RotationY => _rotationY;

        public Direction DirectionFlags => _availableDirections;
        #endregion

        #region Initialisation
        //-----------------------------------------------------------

        public void Init(  )
        {
            _availableDirections = Direction.None;
            if (NorthDirection)
                _availableDirections = (Direction)(((int)_availableDirections) | 1 << 0);
             if (SouthDirection)
                _availableDirections = (Direction)(((int)_availableDirections) | 1 << 1);
            if (WestDirection)
                _availableDirections = (Direction)(((int)_availableDirections) | 1 << 2);
            if (EastDirection)
                _availableDirections = (Direction)(((int)_availableDirections) | 1 << 3);
            
        }

        //-----------------------------------------------------------

        public void Init(ExplorationEvent associatedEvent)
        {
            Init();
            _associatedEvent = associatedEvent;

        }

        //-----------------------------------------------------------
        #endregion

        #region Public Method
        //-----------------------------------------------------------

        public bool DirectionAvailable( Direction direction )
        {
            return (direction & _availableDirections) != 0;
        }

        //-----------------------------------------------------------

        public object Clone()
        {
            Tile clone = ScriptableObject.Instantiate(this);
            clone.Init(_associatedEvent);
            return clone;
        }

        //-----------------------------------------------------------

        /// <summary>
        /// Reveal
        /// </summary>
        public void Reveal()
        {
            _associatedEvent?.Reveal();
        }

        //-----------------------------------------------------------
        #endregion

    }
}
