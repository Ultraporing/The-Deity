/*
    Written by Tobias Lenz
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    /// <summary>
    /// Interface used by all Managers, provides Update function
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// The Update function
        /// </summary>
        void Update();
    }
}
