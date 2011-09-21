﻿using System;

namespace LibGit2Sharp.Core
{
    internal class ObjectSafeWrapper : IDisposable
    {
        private IntPtr objectPtr = IntPtr.Zero;

        public ObjectSafeWrapper(ObjectId id, Repository repo)
        {
            if (id == null)
            {
                return;
            }

            GitOid oid = id.Oid;
            int res = NativeMethods.git_object_lookup(out objectPtr, repo.Handle, ref oid, GitObjectType.Any);
            Ensure.Success(res);
        }

        public IntPtr ObjectPtr
        {
            get { return objectPtr; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (objectPtr == IntPtr.Zero)
            {
                return;
            }

            NativeMethods.git_object_close(objectPtr);
            objectPtr = IntPtr.Zero;
        }

        ~ObjectSafeWrapper()
        {
            Dispose(false);
        }
    }
}
