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

// Perform a mathematical modulo operation: a modulo b.
// NOT to be confused with the built in % operator in C# which is a REMAINDER function.
public static int Modulo(int a, int b)
{
    return (a % b + b) % b;
}

// Converts a percentage value of audio to decibels given a min and max decibel value.
// Uses log to calculate decibels to beter simulate human hearing of audio levels. (Rather than a linear path)
public static double ConvertPercentageRawToDecibels(double percentValue, double max, double min)
{
    if (percentValue == 0)
    {
        return min;
    }
    return Math.Clamp(Math.Log(percentValue, 10) * 20, max, min);
}
