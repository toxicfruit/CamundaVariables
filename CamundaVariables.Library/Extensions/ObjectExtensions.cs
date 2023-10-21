﻿using System;

namespace CamundaVariables.Library.Extensions
{
    internal static class ObjectExtensions
    {
        internal static T Also<T>(this T self, Action<T> action)
        {
            action(self);
            return self;
        }
    }
}
