using System;

namespace Laba_2
{   
class Program
    {
        static void Main(string[] args)
        {
            MyTime myTime = new MyTime(9, 20, 0);

            Console.WriteLine(myTime.WhatLesson());
        }
    }
}