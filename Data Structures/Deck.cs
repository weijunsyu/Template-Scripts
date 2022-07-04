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
using System.Collections.Generic;

public class Deck<T>
{
    private LinkedList<T> _cards;


    public Deck()
    {
        _cards = new LinkedList<T>();
    }
    public Deck(LinkedList<T> cards)
    {
        _cards = cards;
    }
    public Deck(params T[] cards)
    {
        _cards = new LinkedList<T>(cards);
    }

    public LinkedList<T> Cards { get { return _cards; } }
    public int Size { get { return _cards.Count; } }

    public List<T> ToList()
    {
        return new List<T>(_cards);
    }
    // return the number of instances of removals,
    // if strict is true then only remove if the number of instances are at least the amount specified
    public int Remove(T cardToRemove, int amount=1, bool strict=false)
    {
        int counter = 0;
        if (strict)
        {
            // only remove from deck if deck contains at least the amount specified otherwise do nothing
            List<T> deckList = this.ToList();
            foreach(T card in deckList)
            {
                if (cardToRemove.Equals(card))
                {
                    counter++;
                }
            }
            if (counter >= amount)
            {
                // Remove the specified amount from the list
                return this.Remove(cardToRemove, amount, strict: false);
            }
            else
            {
                return 0;
            }
        }
        else
        {
            // remove specified amount of instances of card and return the number of instances removed be it 0 or otherwise
            for (int i = 0; i < amount; i++)
            {
                if (_cards.Contains(cardToRemove))
                {
                    counter++;
                    _cards.Remove(cardToRemove);
                }
            }
            return counter;
        }
    }
    public void Add(params T[] cards)
    {
        foreach(T card in cards)
        {
            _cards.AddLast(card);
        }
    }
    public T[] Draw(int numCards=1)
    {
        if (_cards.Count > 0)
        {
            T[] cardArray = new T[numCards];
            for(int i = 0; i < numCards; i++)
            {
                cardArray[i] = _cards.First.Value;
                _cards.RemoveFirst();
            }
            return cardArray;
        }
        return null;
    }
    public void Shuffle(int? seed=null)
    {
        Random random;
        if (seed == null)
        {
            random = new Random();
        }
        else
        {
            random = new Random(seed.Value);
        }
        LinkedList<T> shuffled = new LinkedList<T>();
        List<T> deckList = this.ToList();
        for (int i = 0; i < _cards.Count; i++)
        {
            int index = random.Next(0, _cards.Count);
            shuffled.AddLast(deckList[index]);
        }
        _cards = shuffled;
    }
}
