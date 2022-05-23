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

using System;

public class FixedStack<T>
{
    private int _capacity; // Max number of elements in fixedstack
    private bool _isLocked;
    private int _length; // Number of elements currently in the stack
    private int _bottom; // The index to the bottom of the stack in the array
    private int _top; // The index to the top of the stack in the array
    private T[] _buffer; // The array holding the stack

    public FixedStack(int size)
    {
        _capacity = size;
        _buffer = new T[_capacity];
        _isLocked = false;
        _length = 0;
        _bottom = -1;
        _top = -1;
    }
    public int Length()
    {
        return _length;
    }
    public FixedStack<T> Copy()
    {
        FixedStack<T> copyStack = new FixedStack<T>(_capacity);
        copyStack._capacity = _capacity;
        copyStack._isLocked = _isLocked;
        copyStack._length = _length;
        copyStack._bottom = _bottom;
        copyStack._top = _top;
        _buffer.CopyTo(copyStack._buffer, 0);

        return copyStack;
    }
    // Print to some printer function the contents of the stack. (Ex. Debug.Log in Unity)
    public void Print(Action<string> printer)
    {
        printer("---------------------------------------------------");
        printer("________Input Buffer Print_________________________");

        printer("Length: " + _length);
        printer("Bottom: " + _bottom);
        printer("Top: " + _top);

        string rawBufferStr = "";
        string adjBufferStr = "";

        for (int i = 0; i < _capacity; i++)
        {
            rawBufferStr += " " + _buffer[i] + " ";
            adjBufferStr += " " + _buffer[Math.Abs(Modulo(_top - i, _capacity))] + " ";
        }
        printer("PHYSICAL buffer: [" + rawBufferStr + "]");
        printer("LOGICAL buffer: [" + adjBufferStr + "]");

        printer("---------------------------------------------------");
    }
    public void Reset()
    {
        for (int i = 0; i < _capacity; i++)
        {
            _buffer[i] = default(T);
        }
    }
    // returns a tuple denoting first the status of the result then the purged item if stack was full
    // where status is given: 1 = stack full, 0 = stack not full, -1 = stack was locked
    public (int, T) Push(T item)
    {
        if (!_isLocked)
        {
            // Add to the top of the stack (stack is empty)
            if (_length == 0)
            {
                // bot and top currently == -1 thus:
                _length++;
                _bottom = 0;
                _top = 0;
                _buffer[_top] = item;
            }
            // Stack is not empty but is not full
            else if (_length < _capacity)
            {
                _length++;
                _top = Modulo(_top + 1, _capacity);
                _buffer[_top] = item;
            }
            // Stack is full (purge last + add new)
            else
            {
                T purgedItem = _buffer[_bottom]; // copy item to be purged
                _buffer[_bottom] = item; // swap out item to be purged
                // move pointers such that new top is at old bottom (and bottom adjusted accordingly)
                _top = Modulo(_top + 1, _capacity);
                _bottom = Modulo(_bottom + 1, _capacity);
                return (1, purgedItem);
            }
            return (0, default(T)); // stack was not full
        }
        else
        {
            return (-1, default(T)); // stack is locked
        }
    }
    public T Peek()
    {
        return _buffer[_top];
    }
    public T PeekNext()
    {
        return _buffer[Math.Abs(Modulo(_top - 1, _capacity))];
    }
    public T PeekAt(int index)
    {
        return _buffer[Math.Abs(Modulo(_top - index, _capacity))];
    }
    public T Pop()
    {
        T retValue = _buffer[_top];

        if (!_isLocked)
        {
            _top = Math.Abs(Modulo(_top - 1, _capacity));

            if (--_length == 0)
            {
                _top = _bottom = -1;
            }
        }

        return retValue;
    }
    private static int Modulo(int a, int b)
    {
        return (a % b + b) % b;
    }
}
