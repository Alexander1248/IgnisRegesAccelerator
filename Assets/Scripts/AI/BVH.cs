#nullable enable
using System.Collections.Generic;
using AI;
using Math;
using UnityEngine;


public class BVH<T> where T : TrackableBehaviour<T>
{
    private readonly Stack<Node> _stack = new();
    private readonly Branch _root;

    public BVH()
    {
        _root = new Branch(null, default!);
        _root.Root = _root;
    }

    public uint Depth()
    {
        return _root.Depth();
    }
    public void Add(T value)
    {
        _root.Add(value);
    }
    public void SafeAdd(T value)
    {
        _stack.Push(_root);
        while (_stack.Count > 0)
        {
            var node = _stack.Pop();
            node.Add(value, _stack);
            if (node is Branch branch)
                branch.UpdateAABB();
        }
    }

    public void OptimizedAdd(T value)
    {
        _root.OptimizedAdd(value);
    }
    public void SafeOptimizedAdd(T value)
    {
        _stack.Push(_root);
        while (_stack.Count > 0)
        {
            var node = _stack.Pop();
            node.OptimizedAdd(value, _stack);
            if (node is Branch branch)
                branch.UpdateAABB();
        }
    }
    
    public void Remove(T value)
    {
        _root.Remove(value);
    }
    public void SafeRemove(T value)
    {
        _stack.Push(_root);
        while (_stack.Count > 0)
        {
            var node = _stack.Pop();
            node.Remove(value, _stack);
            if (node is not Branch branch) continue;
            if (branch.Left == null && branch.Right == null)
            {
                if (branch.Parent == null) return;
                if (branch.Parent.Left == branch)
                    branch.Parent.Left = null;
                else
                    branch.Parent.Right = null;
            }
            else branch.UpdateAABB();
        }
    }

    public void FindNearest(Vector3 position, float radius, List<T> result)
    {
        _root.FindNearestFwd(position, radius, result);
    }
    public void SafeFindNearest(Vector3 position, float radius, ref List<T> result)
    {
        _stack.Push(_root);
        while (_stack.Count > 0)
        {
            var node = _stack.Pop();
            node.FindNearestFwd(position, radius, result, _stack);
        }
    }
    
    public List<T> FindNearest(Vector3 position, float radius)
    {
        var result = new List<T>();
        FindNearest(position, radius, result);
        return result;
    }
    public List<T> SafeFindNearest(Vector3 position, float radius)
    {
        var result = new List<T>();
        SafeFindNearest(position, radius, ref result);
        return result;
    }


    public abstract class Node
    {
        protected internal Branch Root;
        protected internal Branch? Parent;
        protected internal Box3 AABB;

        protected Node(Box3 aabb, Branch? parent, Branch root)
        {
            AABB = aabb;
            Parent = parent;
            Root = root;
        }

        public abstract uint Depth();
        
        public abstract void Add(T value);
        public abstract void Add(T value, Stack<Node> stack);
        public abstract void OptimizedAdd(T value);
        public abstract void OptimizedAdd(T value, Stack<Node> stack);
        public abstract void Remove(T value);
        public abstract void Remove(T value, Stack<Node> stack);
        public abstract void FindNearestFwd(Vector3 position, float radius, List<T> result);
        public abstract void FindNearestFwd(Vector3 position, float radius, List<T> result, Stack<Node> stack);

        public void FindNearestBwd(Vector3 position, float radius, List<T> result)
        {
            if (Parent == null) return;
            if (Parent.Left == this)
                Parent.Right?.FindNearestFwd(position, radius, result);
            else
                Parent.Left?.FindNearestFwd(position, radius, result);
            Parent.FindNearestBwd(position, radius, result);
        }
    }
    
    public class Branch : Node
    {
        protected internal Node? Left;
        protected internal Node? Right;
        
        public Branch(Branch? parent, Branch root) : base(new Box3(Vector3.zero, Vector3.one), parent, root) { }
        public override uint Depth()
        {
            var left = Left?.Depth() ?? 0;
            var right = Right?.Depth() ?? 0;
            return System.Math.Max(left, right) + 1;
        }

        public override void Add(T value)
        {
            var left = Left == null ? 0 : Box3.Distance(Left.AABB, value.transform.position);
            var right = Right == null ? 0 : Box3.Distance(Right.AABB, value.transform.position);

            if (left < right)
            {
                if (Left == null)
                    Left = new Leaf(this, Root, value);
                else Left.Add(value);
            }
            else
            {
                if (Right == null)
                    Right = new Leaf(this, Root, value);
                else Right.Add(value);
            }
            UpdateAABB();
        }

        public override void Add(T value, Stack<Node> stack)
        {
            var left = Left == null ? 0 : Box3.Distance(Left.AABB, value.transform.position);
            var right = Right == null ? 0 : Box3.Distance(Right.AABB, value.transform.position);

            if (left < right)
            {
                if (Left == null)
                    Left = new Leaf(this, Root, value);
                else stack.Push(Left);
            }
            else
            {
                if (Right == null)
                    Right = new Leaf(this, Root, value);
                else stack.Push(Right);
            }
        }

        public override void OptimizedAdd(T value)
        {
            var left = Left == null ? 0 : Box3.Distance(Left.AABB, value.transform.position);
            var right = Right == null ? 0 : Box3.Distance(Right.AABB, value.transform.position);

            if (left < right)
            {
                if (Left == null)
                    Left = new Leaf(this, Root, value);
                else if (left <= 0)
                    Left.OptimizedAdd(value);
                else
                {
                    var node = new Branch(this, Root);
                    node.Left = new Leaf(node, Root, value);
                    node.Right = Left;
                    Left.Parent = node;
                    Left = node;
                    node.Balance();
                    node.UpdateAABB();
                }
            }
            else
            {
                if (Right == null)
                    Right = new Leaf(this, Root, value);
                else if (right <= 0)
                    Right.OptimizedAdd(value);
                else
                {
                    var node = new Branch(this, Root);
                    node.Left = new Leaf(node, Root, value);
                    node.Right = Right;
                    Right.Parent = node;
                    Right = node;
                    node.Balance();
                    node.UpdateAABB();
                }
            }
            UpdateAABB();
        }

        public override void OptimizedAdd(T value, Stack<Node> stack)
        { 
            var left = Left == null ? 0 : Box3.Distance(Left.AABB, value.transform.position);
            var right = Right == null ? 0 : Box3.Distance(Right.AABB, value.transform.position);

            if (left < right)
            {
                if (Left == null)
                    Left = new Leaf(this, Root, value);
                else if (left <= 0)
                    stack.Push(Left);
                else
                {
                    var node = new Branch(Left.Parent, Root);
                    node.Left = new Leaf(node, Root, value);
                    node.Right = Left;
                    Left.Parent = node;
                    Left = node;
                    node.Balance();
                    node.UpdateAABB();
                }
            }
            else
            {
                if (Right == null)
                    Right = new Leaf(this, Root, value);
                else if (right <= 0)
                    stack.Push(Right);
                else
                {
                    var node = new Branch(Right.Parent, Root);
                    node.Left = new Leaf(node, Root, value);
                    node.Right = Right;
                    Right.Parent = node;
                    Right = node;
                    node.Balance();
                    node.UpdateAABB();
                }
            }
        }

        private void Balance()
        {
            if (Right is Branch rb)
            {
                if (Left == null) return;
                var left = rb.Left == null ? 0 : Box3.Distance(Left.AABB, rb.Left.AABB);
                var right = rb.Right == null ? 0 : Box3.Distance(Left.AABB, rb.Right.AABB);
               
                if (left < right)
                    (Left, rb.Right) = (rb.Right, Left);
                else
                    (Left, rb.Left) = (rb.Left, Left);
                
                rb.Balance();
                rb.UpdateAABB();
                return;
            } 
            if (Left is Branch lb)
            {
                if (Right == null) return;
                var left = lb.Left == null ? 0 : Box3.Distance(Right.AABB, lb.Left.AABB);
                var right = lb.Right == null ? 0 : Box3.Distance(Right.AABB, lb.Right.AABB);
                
                if (left < right)
                    (Right, lb.Right) = (lb.Right, Right);
                else
                    (Right, lb.Left) = (lb.Left, Right);
                
                lb.Balance();
                lb.UpdateAABB();
                return;
            }
        }

        public override void Remove(T value)
        {
            Left?.Remove(value);
            Right?.Remove(value);
            if (Left == null && Right == null)
            {
                if (Parent == null) return;
                if (Parent.Left == this)
                    Parent.Left = null;
                else
                    Parent.Right = null;
            }
            else UpdateAABB();
        }

        public override void Remove(T value, Stack<Node> stack)
        {
            if (Left != null) stack.Push(Left);
            if (Right != null) stack.Push(Right);
        }

        public override void FindNearestFwd(Vector3 position, float radius, List<T> result)
        {
            if (!Box3.SphereIntersection(AABB, position, radius)) return;
            Left?.FindNearestFwd(position, radius, result);
            Right?.FindNearestFwd(position, radius, result);
        }

        public override void FindNearestFwd(Vector3 position, float radius, List<T> result, Stack<Node> stack)
        {
            if (!Box3.SphereIntersection(AABB, position, radius)) return;
            if (Left != null) stack.Push(Left);
            if (Right != null) stack.Push(Right);
        }

        protected internal void UpdateAABB()
        {
            if (Left == null && Right == null) return;
            var left = Left?.AABB ?? Right!.AABB;
            var right = Right?.AABB ?? Left!.AABB;
            AABB = Box3.Union(left, right);
        }

        public void Relocate(T value)
        {
            if (Parent == null)
            {
                OptimizedAdd(value);
                return;
            }

            if (Left == null && Right == null)
            {
                if (Parent == null) return;
                if (Parent.Left == this)
                    Parent.Left = null;
                else
                    Parent.Right = null;
                Parent.UpdateAABB();
            }
            
            if (Parent.AABB.ContainsInclusive(value.transform.position))
            {
                Parent.OptimizedAdd(value);
                return;
            }

            Parent.Relocate(value);
        }
    }

    public class Leaf : Node
    {
        private readonly T _value;
        public Leaf(Branch? parent, Branch root, T value) : base(new Box3(
            value.transform.position, 
            value.transform.position), parent, root)
        {
            _value = value;
            _value.Location = this;
        }

        public override uint Depth()
        {
            return 1;
        }

        public override void Add(T value)
        {
            OptimizedAdd(value);
        }

        public override void Add(T value, Stack<Node> stack)
        {
            OptimizedAdd(value);
        }

        public void Relocate()
        {
            if (Parent == null) return;
            Remove(_value);
            Parent.UpdateAABB();
            
            if (Parent.AABB.ContainsInclusive(_value.transform.position))
            {
                Parent.OptimizedAdd(_value);
                return;
            }
            Parent.Relocate(_value);
        }

        public override void OptimizedAdd(T value)
        {
            if (Parent == null) return;
            if (Parent.Left == this)
            {
                var node = new Branch(Parent, Root);
                node.Left = new Leaf(node, Root, value);
                node.Right = this;
                Parent.Left = node;
                Parent = node;
                node.UpdateAABB();
            }
            else
            {
                var node = new Branch(Parent, Root);
                node.Left = new Leaf(node, Root, value);
                node.Right = this;
                Parent.Right = node;
                Parent = node;
                node.UpdateAABB();
            }
        }

        public override void OptimizedAdd(T value, Stack<Node> stack)
        {
            OptimizedAdd(value);
        }

        public override void Remove(T value)
        { 
            if (Parent == null) return;
            if (Parent.Left == this)
                Parent.Left = null;
            else
                Parent.Right = null;
        }

        public override void Remove(T value, Stack<Node> stack)
        {
            Remove(value);
        }

        public override void FindNearestFwd(Vector3 position, float radius, List<T> result)
        {
            if (Vector3.Distance(position, _value.transform.position) > radius) return;
            result.Add(_value);
        }

        public override void FindNearestFwd(Vector3 position, float radius, List<T> result, Stack<Node> stack)
        {
            FindNearestFwd(position, radius, result);
        }
    }
}