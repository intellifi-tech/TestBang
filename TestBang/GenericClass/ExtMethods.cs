﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TestBang.GenericClass
{
    public static class ExtMethods
    {
        public static bool Contains (this string source, string toCheck, StringComparison comparisonType)
        {
            try
            {
                return (source.IndexOf(toCheck, comparisonType) >= 0);
            }
            catch 
            {
                return false;
             
            }
           
        }
    }
}
