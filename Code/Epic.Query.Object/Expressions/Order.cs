//  
//  Order.cs
//  
//  Author:
//       Giacomo Tesio <giacomo@tesio.it>
// 
//  Copyright (c) 2010-2012 Giacomo Tesio
// 
//  This file is part of Epic.NET.
// 
//  Epic.NET is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Epic.NET is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
// 
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Epic.Query.Object.Expressions
{
    [Serializable]
    public sealed class Order<TEntity> : Expression<IEnumerable<TEntity>>
        where TEntity : class
    {
        private readonly Expression<IEnumerable<TEntity>> _source;
        private readonly OrderCriterion<TEntity> _criterion; 

        public Order (Expression<IEnumerable<TEntity>> source, OrderCriterion<TEntity> criterion)
        {
            if (null == source)
                throw new ArgumentNullException ("source");
            if (null == criterion)
                throw new ArgumentNullException ("criterion");
            _source = source;
            _criterion = criterion;
        }

        public Order<TEntity> ThanBy(OrderCriterion<TEntity> criterion)
        {
            if(null == criterion)
                throw new ArgumentNullException("criterion");
            return new Order<TEntity>(_source, _criterion.Chain(criterion));
        }
        
        public Expression<IEnumerable<TEntity>> Source {
            get { return _source; }
        }

        public OrderCriterion<TEntity> Criterion {
            get { return _criterion; }
        }

        public override TResult Accept<TResult> (IVisitor<TResult> visitor, IVisitContext context)
        {
            return AcceptMe (this, visitor, context);
        }

        protected override void GetObjectData (SerializationInfo info, StreamingContext context)
        {
            info.AddValue ("R", _source, typeof(Expression<IEnumerable<TEntity>>));
            info.AddValue ("S", _criterion, typeof(OrderCriterion<TEntity>));
        }

        private Order (SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            _source = (Expression<IEnumerable<TEntity>>)info.GetValue ("R", typeof(Expression<IEnumerable<TEntity>>));
            _criterion = (OrderCriterion<TEntity>)info.GetValue ("S", typeof(OrderCriterion<TEntity>));
        }
    }
}
