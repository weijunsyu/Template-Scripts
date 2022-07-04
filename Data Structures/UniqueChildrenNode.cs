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
