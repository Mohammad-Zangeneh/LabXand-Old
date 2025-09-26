using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabXand.Core
{
    /// <summary>
    /// Represents a paginated data containing total rows count.
    /// </summary>
    public interface IPaginated<T>
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets the total number of data.
        /// </summary>
        int TotalCount { get; set; }
    }
}
