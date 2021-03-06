﻿using System;
using System.Runtime.InteropServices;
using LibGit2Sharp.Core;

namespace LibGit2Sharp
{
    /// <summary>
    ///   A GitObject
    /// </summary>
    public class GitObject : IEquatable<GitObject>
    {
        internal static GitObjectTypeMap TypeToTypeMap =
            new GitObjectTypeMap
                {
                    {typeof (Commit), GitObjectType.Commit},
                    {typeof (Tree), GitObjectType.Tree},
                    {typeof (Blob), GitObjectType.Blob},
                    {typeof (TagAnnotation), GitObjectType.Tag},
                    {typeof (GitObject), GitObjectType.Any},
                };

        private static readonly LambdaEqualityHelper<GitObject> equalityHelper =
            new LambdaEqualityHelper<GitObject>(new Func<GitObject, object>[] {x => x.Id});

        /// <summary>
        /// Initializes a new instance of the <see cref="GitObject"/> class.
        /// </summary>
        /// <param name="id">The <see cref="ObjectId"/> it should be identified by.</param>
        protected GitObject(ObjectId id)
        {
            Id = id;
        }

        /// <summary>
        ///   Gets the id of this object
        /// </summary>
        public ObjectId Id { get; private set; }

        /// <summary>
        ///   Gets the 40 character sha1 of this object.
        /// </summary>
        public string Sha
        {
            get { return Id.Sha; }
        }

        internal static GitObject CreateFromPtr(IntPtr obj, ObjectId id, Repository repo)
        {
            try
            {
                var type = NativeMethods.git_object_type(obj);
                switch (type)
                {
                    case GitObjectType.Commit:
                        return Commit.BuildFromPtr(obj, id, repo);
                    case GitObjectType.Tree:
                        return Tree.BuildFromPtr(obj, id, repo);
                    case GitObjectType.Tag:
                        return TagAnnotation.BuildFromPtr(obj, id);
                    case GitObjectType.Blob:
                        return Blob.BuildFromPtr(obj, id, repo);
                    default:
                        throw new InvalidOperationException(string.Format("Unexpected type '{0}' for object '{1}'.", type, id));
                }
            }
            finally
            {
                NativeMethods.git_object_close(obj);
            }
        }

        internal static ObjectId ObjectIdOf(IntPtr obj)
        {
            var ptr = NativeMethods.git_object_id(obj);
            return new ObjectId((GitOid)Marshal.PtrToStructure(ptr, typeof(GitOid)));
        }

        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current <see cref="GitObject"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current <see cref="GitObject"/>.</param>
        /// <returns>True if the specified <see cref="Object"/> is equal to the current <see cref="GitObject"/>; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as GitObject);
        }

        /// <summary>
        /// Determines whether the specified <see cref="GitObject"/> is equal to the current <see cref="GitObject"/>.
        /// </summary>
        /// <param name="other">The <see cref="GitObject"/> to compare with the current <see cref="GitObject"/>.</param>
        /// <returns>True if the specified <see cref="GitObject"/> is equal to the current <see cref="GitObject"/>; otherwise, false.</returns>
        public bool Equals(GitObject other)
        {
            return equalityHelper.Equals(this, other);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return equalityHelper.GetHashCode(this);
        }

        /// <summary>
        /// Tests if two <see cref="GitObject"/> are equal.
        /// </summary>
        /// <param name="left">First <see cref="GitObject"/> to compare.</param>
        /// <param name="right">Second <see cref="GitObject"/> to compare.</param>
        /// <returns>True if the two objects are equal; false otherwise.</returns>
        public static bool operator ==(GitObject left, GitObject right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Tests if two <see cref="GitObject"/> are different.
        /// </summary>
        /// <param name="left">First <see cref="GitObject"/> to compare.</param>
        /// <param name="right">Second <see cref="GitObject"/> to compare.</param>
        /// <returns>True if the two objects are different; false otherwise.</returns>
        public static bool operator !=(GitObject left, GitObject right)
        {
            return !Equals(left, right);
        }
    }
}