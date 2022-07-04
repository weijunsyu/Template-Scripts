/*
MIT License

Copyright (c) 2022 WeiJun Syu

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Generic;
using System;

public class LookupTree<T, U>
{
    private UniqueChildrenNode<T> _root;
    private U _rootValue;
    private Dictionary<UniqueChildrenNode<T>, U> _lookupDict;

    public LookupTree(T rootKey, U rootValue)
    {
        _root = new UniqueChildrenNode<T>(rootKey);
        _rootValue = rootValue;
        _lookupDict = new Dictionary<UniqueChildrenNode<T>, U>();
    }
    public UniqueChildrenNode<T> GetRoot()
    {
        return _root;
    }

    public bool AddPath(U lookupValue, List<T> nodeValues)
    {
        if (_root.GetValue().Equals(nodeValues[0]))
        {
            return false;
        }

        UniqueChildrenNode<T> current = _root;
        for (int i = 1; i < nodeValues.Count; i++)
        {
            UniqueChildrenNode<T> child = current.GetChild(nodeValues[i]);
            if (child != null)
            {
                // Move to next node
                current = child;
            }
            else
            {
                // We are at a leaf node; add the new node into tree
                current = current.AddChild(nodeValues[i]);
            }
        }
        // finished checking/adding all nodes into tree, current should be the node of the path value
        _lookupDict.Add(current, lookupValue);
        return true;
    }
    public U Lookup(List<T> nodeValues)
    {
        UniqueChildrenNode<T> current = _root;
        Stack<U> lookupStack = new Stack<U>();
        for (int i = 0; i < nodeValues.Count; i++)
        {
            if (current.GetValue().Equals(nodeValues[i]))
            {
                U lookupValue;
                // Check if current node holds a lookup value
                if (_lookupDict.TryGetValue(current, out lookupValue))
                {
                    // if the lookup value exists on node push it to the stack
                    lookupStack.Push(lookupValue);
                }
                // Make sure there exists another node in the node list
                if (i + 1 < nodeValues.Count)
                {
                    UniqueChildrenNode<T> child = current.GetChild(nodeValues[i + 1]);
                    if (child != null)
                    {
                        // Move to next node
                        current = child;
                    }
                    else
                    {
                        // We are at a leaf node which we already checked for a lookup value thus exit loop
                        break;
                    }
                }
            }
        }
        // finished checking lookup values in list now return the correct lookup value (longest/last valid lookup)
        if (lookupStack.Count != 0)
        {
            return lookupStack.Pop();
        }
        // If stack is empty then no lookup value found
        return default(U);
    }

    // Print to some printer function the contents of the stack. (Ex. Debug.Log in Unity)
    public void Print(Action<string> printer)
    {
        List<T> traversal = new List<T>();
        TreeTraversal(_root, ref traversal);
        string traversalStr = "";

        for (int i = 0; i < traversal.Count; i++)
        {
            traversalStr += " " + traversal[i].ToString() + " ";
        }

        printer("[" + traversalStr + "]");
    }

    private void TreeTraversal(UniqueChildrenNode<T> root, ref List<T> traversal)
    {
        Dictionary<T, UniqueChildrenNode<T>> children = root.GetChildren();

        if (children == null)
        {
            traversal.Add(root.GetValue());
        }
        else
        {
            foreach (var child in children.Values)
            {
                TreeTraversal(child, ref traversal);
            }
        }
    }
}

public class UniqueChildrenNode<T>
{
    private T _value;
    private UniqueChildrenNode<T> _parent;
    private Dictionary<T, UniqueChildrenNode<T>> _children;

    public UniqueChildrenNode(T value, UniqueChildrenNode<T> parent=null)
    {
        _value = value;
        _parent = parent;
        // list of child nodes such that key: value of node, value: node itself
        _children = new Dictionary<T, UniqueChildrenNode<T>>();
    }
    public T GetValue()
    {
        return _value;
    }
    public UniqueChildrenNode<T> GetParent()
    {
        return _parent;
    }
    public Dictionary<T, UniqueChildrenNode<T>> GetChildren()
    {
        return _children;
    }
    public bool HasChild()
    {
        if (_children.Count != 0) {
            return true;
        }
        return false;
    }
    public UniqueChildrenNode<T> AddChild(T value)
    {
        UniqueChildrenNode<T> node = new UniqueChildrenNode<T>(value, this);
        _children.Add(value, node);
        return node;
    }
    public UniqueChildrenNode<T> AddChild(UniqueChildrenNode<T> node)
    {
        node.AddParent(this);
        _children.Add(node.GetValue(), node);
        return node;
    }
    public void AddParent(UniqueChildrenNode<T> parent)
    {
        _parent = parent;
    }
    public UniqueChildrenNode<T> GetChild(T value)
    {
        UniqueChildrenNode<T> child;
        if (_children.TryGetValue(value, out child))
        {
            return child;
        }
        return null;
    }
}
