using System.Collections.Generic;

public class PermNode<T>
{
    private T _value;
    private PermNode<T> _parent;
    private Dictionary<T, PermNode<T>> _children;

    public PermNode(T value, PermNode<T> parent=null)
    {
        _value = value;
        _parent = parent;
        // list of child nodes such that key: value of node, value: node itself
        _children = new Dictionary<T, PermNode<T>>();
    }
    public T GetValue()
    {
        return _value;
    }
    public PermNode<T> GetParent()
    {
        return _parent;
    }
    public Dictionary<T, PermNode<T>> GetChildren()
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
    public PermNode<T> AddChild(T value)
    {
        PermNode<T> node = new PermNode<T>(value, this);
        _children.Add(value, node);
        return node;
    }
    public PermNode<T> AddChild(PermNode<T> node)
    {
        node.AddParent(this);
        _children.Add(node.GetValue(), node);
        return node;
    }
    public void AddParent(PermNode<T> parent)
    {
        _parent = parent;
    }
    public PermNode<T> GetChild(T value)
    {
        PermNode<T> child;
        if (_children.TryGetValue(value, out child))
        {
            return child;
        }
        return null;
    }
}
