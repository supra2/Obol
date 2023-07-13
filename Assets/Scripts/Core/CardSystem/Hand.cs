using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CardSystem
{
    public class Hand<T> where T : ICard
    {
        #region Members
        List<T> _cards;
        bool _isDirty;
        #endregion
        #region Getter
        public bool IsDirty {
            get => _isDirty;
            set => _isDirty = value;

        }
        #endregion

        #region Getter
        public T this[int i]
        {
            get => _cards[i];
            set => _cards[i]=value;
        }
        #endregion

        #region Method
        public Hand()
        {
            _cards = new List<T>();
        }

        public void Add(T c)
        {
            _cards.Add( c );
            _isDirty = true;
        }
        public void InsertBefore(T card , T cardtoplaceBefore )
        {
            int index =  _cards.FindIndex((x) => x.Equals( cardtoplaceBefore));
            _cards.Insert(index, card);
            _isDirty = true;
        }

        public void Remove( T card)
        {
            _cards.Remove(card);
            _isDirty = true;
        }

        public void Clear()
        {
            _cards.Clear();
            _isDirty = true;
        }
        #endregion
    }
}
