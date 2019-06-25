using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entity.Business
{
    /// <summary>
    /// TimingTask
    /// </summary>
    public class TaskEntity
    {
        /// <summary>
        /// Gets or sets the task title.
        /// </summary>
        /// <value>
        /// The task title.
        /// </value>
        public string TaskTitle { get; set; }
        /// <summary>
        /// Gets or sets the full name of the task.
        /// </summary>
        /// <value>
        /// The full name of the task.
        /// </value>
        public string TaskFullName { get; set; }
        /// <summary>
        /// Gets or sets the task intervals.
        /// </summary>
        /// <value>
        /// The task intervals.
        /// </value>
        public int TaskIntervals { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [task status].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [task status]; otherwise, <c>false</c>.
        /// </value>
        public bool TaskStatus { get; set; }
    }
}
