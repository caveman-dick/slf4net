﻿//The MIT License (MIT)
//Copyright © 2012 Englishtown <opensource@englishtown.com>

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the “Software”), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.


using System.Collections.Generic;
using System.Globalization;

namespace slf4net.Factories
{
    /// <summary>
    /// A base class for factories which created named logger
    /// instance.
    /// </summary>
    public abstract class NamedLoggerFactoryBase : ILoggerFactory
    {
        /// <summary>
        /// A cache of named loggers
        /// </summary>
        private readonly Dictionary<string, ILogger> _loggers = new Dictionary<string, ILogger>();
        /// <summary>
        /// An object which is used for locking
        /// </summary>
        private readonly object _lockObj = new object();

        /// <summary>
        /// Obtains an <see cref="ILogger"/> instance that is identified by
        /// the given name.
        /// </summary>
        /// <param name="name">The logger name.</param>
        /// <returns>A factory that can be identified by the 
        /// given name in the target output for this logger</returns>
        public ILogger GetLogger(string name)
        {
            if (name == null) name = string.Empty;
            string lowerName = name.ToLower(CultureInfo.InvariantCulture);

            ILogger logger;

            if (!_loggers.TryGetValue(lowerName, out logger))
            {
                lock (_lockObj)
                {
                    if (!_loggers.TryGetValue(lowerName, out logger))
                    {
                        logger = CreateLogger(name);
                        _loggers.Add(lowerName, logger);
                    }
                }
            }

            return logger;
        }

        /// <summary>
        /// Constructs a logger with the given name
        /// </summary>
        /// <param name="name">The logger name.</param>
        /// <returns>A logger with the given name.</returns>
        protected abstract ILogger CreateLogger(string name);

    }
}
