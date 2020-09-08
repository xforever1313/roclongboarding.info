﻿//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;
using System.Collections.Generic;
using System.Text;

namespace MapPlugin
{
    public static class Conversions
    {
        public static double KmToMiles( double km )
        {
            return km * 0.62137119;
        }
    }
}
