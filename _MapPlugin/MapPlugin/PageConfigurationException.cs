﻿//
//          Copyright Seth Hendrick 2020.
// Distributed under the Boost Software License, Version 1.0.
//    (See accompanying file LICENSE_1_0.txt or copy at
//          http://www.boost.org/LICENSE_1_0.txt)
//

using System;

namespace MapPlugin
{
    public class PageConfigurationException : Exception
    {
        // ---------------- Constructor ----------------

        public PageConfigurationException( string message ) :
            base( message )
        {
        }

        public PageConfigurationException( string message, Exception innerException ) :
            base( message, innerException )
        {
        }
    }
}
