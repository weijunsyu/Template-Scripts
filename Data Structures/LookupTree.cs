using System.Collections.Generic;
using System;

public class LookupTree<T, U>
{
    private PermNode<T> _root;
    private U _rootValue;
    private Dictionary<PermNode<T>, U> _lookupDict;

    public LookupTree(T rootKey, U rootValue)
    {
        _root = new PermNode<T>(rootKey);
        _rootValue = rootValue;
        _lookupDict = new Dictionary<PermNode<T>, U>();
    }
    public PermNode<T> GetRoot()
    {
        return _root;
    }
    
    public bool AddPath(U lookupValue, List<T> nodeValues)
    {
        if (_root.GetValue().Equals(nodeValues[0]))
        {
            return false;
        }

        PermNode<T> current = _root;
        for (int i = 1; i < nodeValues.Count; i++)
        {
            PermNode<T> child = current.GetChild(nodeValues[i]);
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
        PermNode<T> current = _root;
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
                    PermNode<T> child = current.GetChild(nodeValues[i + 1]);
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

    private void TreeTraversal(PermNode<T> root, ref List<T> traversal)
    {
        Dictionary<T, PermNode<T>> children = root.GetChildren();

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
