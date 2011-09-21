﻿using System;
using LibGit2Sharp.Core;

namespace LibGit2Sharp
{
    public class StatusEntry : IEquatable<StatusEntry>
    {
        static readonly LambdaEqualityHelper<StatusEntry> equalityHelper =
            new LambdaEqualityHelper<StatusEntry>(new Func<StatusEntry, object>[] { x => x.FilePath, x => x.State });

        readonly string filePath;
        readonly FileStatus state;

        internal StatusEntry(string filePath, FileStatus state)
        {
            this.filePath = filePath;
            this.state = state;
        }

        #region Public Properties

        public string FilePath
        {
            get { return filePath; }
        }

        public FileStatus State
        {
            get { return state; }
        }

        #endregion

        #region IEquatable<StatusEntry> Members

        /// <summary>
        ///   Determines whether the specified <see cref = "StatusEntry" /> is equal to the current <see cref = "StatusEntry" />.
        /// </summary>
        /// <param name = "other">The <see cref = "StatusEntry" /> to compare with the current <see cref = "StatusEntry" />.</param>
        /// <returns>True if the specified <see cref = "StatusEntry" /> is equal to the current <see cref = "StatusEntry" />; otherwise, false.</returns>
        public bool Equals(StatusEntry other)
        {
            return equalityHelper.Equals(this, other);
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Determines whether the specified <see cref = "Object" /> is equal to the current <see cref = "StatusEntry" />.
        /// </summary>
        /// <param name = "obj">The <see cref = "Object" /> to compare with the current <see cref = "StatusEntry" />.</param>
        /// <returns>True if the specified <see cref = "Object" /> is equal to the current <see cref = "StatusEntry" />; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as StatusEntry);
        }

        /// <summary>
        ///   Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return equalityHelper.GetHashCode(this);
        }

        #endregion

        /// <summary>
        ///   Tests if two <see cref = "StatusEntry" /> are equal.
        /// </summary>
        /// <param name = "left">First <see cref = "StatusEntry" /> to compare.</param>
        /// <param name = "right">Second <see cref = "StatusEntry" /> to compare.</param>
        /// <returns>True if the two objects are equal; false otherwise.</returns>
        public static bool operator ==(StatusEntry left, StatusEntry right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///   Tests if two <see cref = "StatusEntry" /> are different.
        /// </summary>
        /// <param name = "left">First <see cref = "StatusEntry" /> to compare.</param>
        /// <param name = "right">Second <see cref = "StatusEntry" /> to compare.</param>
        /// <returns>True if the two objects are different; false otherwise.</returns>
        public static bool operator !=(StatusEntry left, StatusEntry right)
        {
            return !Equals(left, right);
        }
    }
}
