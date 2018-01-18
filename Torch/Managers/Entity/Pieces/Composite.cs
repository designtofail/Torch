﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Entities.Cube;
using Torch.API;
using VRage.Game.Entity;

namespace Torch.Managers.Entity.Pieces
{
    public class EntityQueryComposite : Piece
    {
        public enum LogicOp
        {
            And, Or
        }

        public LogicOp Operation { get; }

        public IReadOnlyList<Piece> Children { get; }

        public EntityQueryComposite(ITorchBase torch, LogicOp op, IEnumerable<Piece> children) : base(torch)
        {
            Operation = op;
            Children = children.ToList();
        }

        /// <inheritdoc/>
        public override bool Test(MySlimBlock block)
        {
            foreach (Piece k in Children)
            {
                bool r = k.Test(block);
                switch (Operation)
                {
                    case LogicOp.And when !r:
                        return false;
                    case LogicOp.Or when r:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Operation == LogicOp.And;
        }

        /// <inheritdoc/>
        public override bool Test(MyEntity entity)
        {
            foreach (Piece k in Children)
            {
                bool r = k.Test(entity);
                switch (Operation)
                {
                    case LogicOp.And when !r:
                        return false;
                    case LogicOp.Or when r:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Operation == LogicOp.And;
        }

        /// <inheritdoc/>
        public override bool ChildrenRelevant(MyEntity entity)
        {
            foreach (Piece k in Children)
            {
                bool r = k.ChildrenRelevant(entity);
                switch (Operation)
                {
                    case LogicOp.And when !r:
                        return false;
                    case LogicOp.Or when r:
                        return true;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Operation == LogicOp.And;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Operation + "(" + string.Join(", ", Children) + ")";
        }

        /// <inheritdoc/>
        public override bool CanTest(MySlimBlock objectType)
        {
            foreach (Piece k in Children)
                if (!k.CanTest(objectType))
                    return false;
            return true;
        }

        /// <inheritdoc/>
        public override bool CanTest(MyEntity objectType)
        {
            foreach (Piece k in Children)
                if (!k.CanTest(objectType))
                    return false;
            return true;
        }
    }
}
