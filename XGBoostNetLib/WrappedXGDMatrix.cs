﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace XGBoostNetLib
{
    /// <summary>
    /// Wrapper of DMatrix object of XGBoost
    /// </summary>
    public sealed class DMatrix : IDisposable
    {
#pragma warning disable MSML_PrivateFieldName
        private bool disposed = false;
#pragma warning restore MSML_PrivateFieldName
#pragma warning disable IDE0044
        private IntPtr _handle;
#pragma warning restore IDE0044
        public IntPtr Handle => _handle;
        private const float Missing = 0f;

        /// <summary>
        /// Create a <see cref="DMatrix"/> for storing training and prediction data under XGBoost framework.
        /// </summary>
#nullable enable
        public unsafe DMatrix(float[] data, uint nrows, uint ncols, float[]? labels = null)
        {
            int errp = WrappedXGBoostInterface.XGDMatrixCreateFromMat(data, nrows, ncols, Missing, out _handle);
            if (errp == -1)
            {
                string reason = WrappedXGBoostInterface.XGBGetLastError();
                throw new XGBoostDLLException(reason);
            }

            if (labels != null)
            {
                SetLabel(labels);
            }

        }
#nullable disable

        public int GetNumRows()
        {
            ulong numRows;
            int errp = WrappedXGBoostInterface.XGDMatrixNumRow(_handle, out numRows);
            if (errp == -1)
            {
                string reason = WrappedXGBoostInterface.XGBGetLastError();
                throw new XGBoostDLLException(reason);
            }
            return (int)numRows;
        }

        public ulong GetNumCols()
        {
            ulong numCols;
            int errp = WrappedXGBoostInterface.XGDMatrixNumCol(_handle, out numCols);
            if (errp == -1)
            {
                string reason = WrappedXGBoostInterface.XGBGetLastError();
                throw new XGBoostDLLException(reason);
            }
            return numCols;
        }

        public void SetLabel(float[] labels)
        {
	#if false
            Contracts.AssertValue(labels);
            Contracts.Assert(labels.Length == GetNumRows());
	    #endif

            int errp = WrappedXGBoostInterface.XGDMatrixSetFloatInfo(_handle, "label", labels, (ulong)labels.Length);
            if (errp == -1)
            {
                string reason = WrappedXGBoostInterface.XGBGetLastError();
                throw new XGBoostDLLException(reason);
            }
        }

#if false
        public Span<float> GetLabels()
        {
            // FIXME -- some issue with the span serializer
            int errp = WrappedXGBoostInterface.XGDMatrixGetFloatInfo(_handle, "label", out ulong labellen, out Span<float> result);
            if (errp == -1)
            {
                string reason = WrappedXGBoostInterface.XGBGetLastError();
                throw new XGBoostDLLException(reason);
            }
            return result;
        }
#else
        public float[] GetLabels()
        {
            unsafe
            {
                float* arrayPtr;
                ulong length;
                WrappedXGBoostInterface.XGDMatrixGetFloatInfo(_handle, "label", out length, out arrayPtr);

                // Marshal the native array into a managed array
                float[] managedArray = new float[length];
                Marshal.Copy((IntPtr)arrayPtr, managedArray, 0, (int)length);
                return managedArray;
            }
        }
#endif

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            int errp = WrappedXGBoostInterface.XgdMatrixFree(_handle);
            if (errp == -1)
            {
                string reason = WrappedXGBoostInterface.XGBGetLastError();
                throw new XGBoostDLLException(reason);
            }
            disposed = true;

        }

    }
}


