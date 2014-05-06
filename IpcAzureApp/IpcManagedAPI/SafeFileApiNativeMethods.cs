﻿// Copyright © Microsoft Open Technologies, Inc.
//
// All Rights Reserved
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS
// OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION
// ANY IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A
// PARTICULAR PURPOSE, MERCHANTABILITY OR NON-INFRINGEMENT.
//
// See the Apache License, Version 2.0 for the specific language
// governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace Microsoft.InformationProtectionAndControl
{
    
    public static class SafeFileApiNativeMethods
    {

        public static string IpcfEncryptFile(
            string inputFile,
            string templateId,
            EncryptFlags flags,
            bool suppressUI,
            bool offline,
            bool hasUserConsent,
            System.Windows.Forms.Form parentForm,
            SymmetricKeyCredential symmKey = null,
            string outputDirectory = null)
        {
            int hr = 0;
            IntPtr encryptedFileName = IntPtr.Zero;
            string outputFileName = null;

            SafeIpcPromptContext ipcContext =
                SafeNativeMethods.CreateIpcPromptContext(suppressUI,
                    offline,
                    hasUserConsent,
                    parentForm,
                    symmKey);

            IntPtr licenseInfoPtr = Marshal.StringToHGlobalUni(templateId);

            try
            {
                hr = UnsafeFileApiMethods.IpcfEncryptFile(
                    inputFile,
                    licenseInfoPtr,
                    (uint)EncryptLicenseInfoTypes.IPCF_EF_TEMPLATE_ID,
                    (uint)flags,
                    (IpcPromptContext)ipcContext,
                    outputDirectory,
                    out encryptedFileName);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                outputFileName = Marshal.PtrToStringUni(encryptedFileName);
                if (null == outputFileName || 0 == outputFileName.Length)
                {
                    outputFileName = inputFile;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(licenseInfoPtr);
                UnsafeFileApiMethods.IpcFreeMemory(encryptedFileName);
                SafeNativeMethods.ReleaseIpcPromptContext(ipcContext);
            }

            return outputFileName;
        }

        public static string IpcfEncryptFile(
            string inputFile,
            SafeInformationProtectionLicenseHandle licenseHandle,
            EncryptFlags flags,
            bool suppressUI,
            bool offline,
            bool hasUserConsent,
            System.Windows.Forms.Form parentForm,
            SymmetricKeyCredential symmKey,
            string outputDirectory = null)
        {
            int hr = 0;
            IntPtr encryptedFileName = IntPtr.Zero;
            string outputFileName = null;

            SafeIpcPromptContext ipcContext =
                SafeNativeMethods.CreateIpcPromptContext(suppressUI,
                    offline,
                    hasUserConsent,
                    parentForm,
                    symmKey);

            try
            {
                hr = UnsafeFileApiMethods.IpcfEncryptFile(
                    inputFile,
                    licenseHandle.Value,
                    (uint)EncryptLicenseInfoTypes.IPCF_EF_LICENSE_HANDLE,
                    (uint)flags,
                    (IpcPromptContext)ipcContext,
                    outputDirectory,
                    out encryptedFileName);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                outputFileName = Marshal.PtrToStringUni(encryptedFileName);
                if (null == outputFileName || 0 == outputFileName.Length)
                {
                    outputFileName = inputFile;
                }
            }
            finally
            {
                UnsafeFileApiMethods.IpcFreeMemory(encryptedFileName);
                SafeNativeMethods.ReleaseIpcPromptContext(ipcContext);
            }

            return outputFileName;
        }

        public static string IpcfEncryptFileStream(
           Stream inputStream,
           string inputFilePath,
           string templateId,
           EncryptFlags flags,
           bool suppressUI,
           bool offline,
           bool hasUserConsent,
           System.Windows.Forms.Form parentForm,
           SymmetricKeyCredential symmKey,
           ref Stream outputStream)
        {
            int hr = 0;
            IntPtr encryptedFileName = IntPtr.Zero;
            string outputFileName = null;
            ILockBytes ilInputStream = new LockBytesOverStream(inputStream);
            ILockBytes ilOutputStream = new LockBytesOverStream(outputStream);


            SafeIpcPromptContext ipcContext =
                SafeNativeMethods.CreateIpcPromptContext(suppressUI,
                    offline,
                    hasUserConsent,
                    parentForm,
                    symmKey);

            IntPtr licenseInfoPtr = Marshal.StringToHGlobalUni(templateId);

            try
            {
                hr = UnsafeFileApiMethods.IpcfEncryptFileStream(
                    ilInputStream,
                    inputFilePath,
                    licenseInfoPtr,
                    (uint)EncryptLicenseInfoTypes.IPCF_EF_TEMPLATE_ID,
                    (uint)flags,
                    (IpcPromptContext)ipcContext,
                    ilOutputStream,
                    out encryptedFileName);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                outputFileName = Marshal.PtrToStringUni(encryptedFileName);
                if (null == outputFileName || 0 == outputFileName.Length)
                {
                    outputFileName = inputFilePath;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(licenseInfoPtr);
                UnsafeFileApiMethods.IpcFreeMemory(encryptedFileName);
            }

            return outputFileName;
        }


        public static string IpcfEncryptFileStream(
            Stream inputStream,
            string inputFilePath,
            SafeInformationProtectionLicenseHandle licenseHandle,
            EncryptFlags flags,
            bool suppressUI,
            bool offline,
            bool hasUserConsent,
            System.Windows.Forms.Form parentForm,
            SymmetricKeyCredential symmKey,
            ref Stream outputStream)
        {
            int hr = 0;
            IntPtr encryptedFileName = IntPtr.Zero;
            string outputFileName = null;
            ILockBytes ilInputStream = new LockBytesOverStream(inputStream);
            ILockBytes ilOutputStream = new LockBytesOverStream(outputStream);

            SafeIpcPromptContext ipcContext =
                SafeNativeMethods.CreateIpcPromptContext(suppressUI,
                    offline,
                    hasUserConsent,
                    parentForm,
                    symmKey);

            try
            {
                hr = UnsafeFileApiMethods.IpcfEncryptFileStream(
                    ilInputStream,
                    inputFilePath,
                    licenseHandle.Value,
                    (uint)EncryptLicenseInfoTypes.IPCF_EF_LICENSE_HANDLE,
                    (uint)flags,
                    (IpcPromptContext)ipcContext,
                    ilOutputStream,
                    out encryptedFileName);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                outputFileName = Marshal.PtrToStringUni(encryptedFileName);
                if (null == outputFileName || 0 == outputFileName.Length)
                {
                    outputFileName = inputFilePath;
                }
            }
            finally
            {
                UnsafeFileApiMethods.IpcFreeMemory(encryptedFileName);
            }

            return outputFileName;
        }


        public static string IpcfDecryptFile(
            string inputFile,
            DecryptFlags flags,
            bool suppressUI,
            bool offline,
            bool hasUserConsent,
            System.Windows.Forms.Form parentForm,
            SymmetricKeyCredential symmKey,
            string outputDirectory = null)
        {
            int hr = 0;
            IntPtr decryptedFileNamePtr = IntPtr.Zero;
            string decryptedFileName = null;

            SafeIpcPromptContext ipcContext =
                SafeNativeMethods.CreateIpcPromptContext(suppressUI,
                    offline,
                    hasUserConsent,
                    parentForm,
                    symmKey);

            try
            {
                hr = UnsafeFileApiMethods.IpcfDecryptFile(
                    inputFile,
                    (uint)flags,
                    (IpcPromptContext)ipcContext,
                    outputDirectory,
                    out decryptedFileNamePtr);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                decryptedFileName = Marshal.PtrToStringUni(decryptedFileNamePtr);
                if (null == decryptedFileName || 0 == decryptedFileName.Length)
                {
                    decryptedFileName = inputFile;
                }
            }
            finally
            {
                UnsafeFileApiMethods.IpcFreeMemory(decryptedFileNamePtr);
                SafeNativeMethods.ReleaseIpcPromptContext(ipcContext);
            }

            return decryptedFileName;
        }

        public static string IpcfDecryptFileStream(
            Stream inputStream,
            string inputFilePath,
            DecryptFlags flags,
            bool suppressUI,
            bool offline,
            bool hasUserConsent,
            System.Windows.Forms.Form parentForm,
            ref Stream outputStream)
        {
            int hr = 0;
            IntPtr decryptedFileNamePtr = IntPtr.Zero;
            string decryptedFileName = null;
            ILockBytes ilInputStream = new LockBytesOverStream(inputStream);
            ILockBytes ilOutputStream = new LockBytesOverStream(outputStream);

            SafeIpcPromptContext ipcContext =
                SafeNativeMethods.CreateIpcPromptContext(suppressUI,
                    offline,
                    hasUserConsent,
                    parentForm);

            try
            {
                hr = UnsafeFileApiMethods.IpcfDecryptFileStream(
                    ilInputStream,
                    inputFilePath,
                    (uint)flags,
                    (IpcPromptContext)ipcContext,
                    ilOutputStream,
                    out decryptedFileNamePtr);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                decryptedFileName = Marshal.PtrToStringUni(decryptedFileNamePtr);
                if (null == decryptedFileName || 0 == decryptedFileName.Length)
                {
                    decryptedFileName = inputFilePath;
                }
            }
            finally
            {
                UnsafeFileApiMethods.IpcFreeMemory(decryptedFileNamePtr);
            }

            return decryptedFileName;
        }

        public static byte[] IpcfGetSerializedLicenseFromFile(string inputFile)
        {
            byte[] license = null;
            int hr = 0;

            IntPtr licensePtr = IntPtr.Zero;
            try
            {
                hr = UnsafeFileApiMethods.IpcfGetSerializedLicenseFromFile(
                    inputFile,
                    out licensePtr);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                license = SafeNativeMethods.MarshalIpcBufferToManaged(licensePtr);
            }
            finally
            {
                UnsafeFileApiMethods.IpcFreeMemory(licensePtr);
            }
            return license;
        }

        public static byte[] IpcfGetSerializedLicenseFromFileStream(
            Stream inputStream,
            string inputFilePath)
        {
            byte[] license = null;
            int hr = 0;
            ILockBytes ilInputStream = new LockBytesOverStream(inputStream);

            IntPtr licensePtr = IntPtr.Zero;
            try
            {
                hr = UnsafeFileApiMethods.IpcfGetSerializedLicenseFromFileStream(
                    ilInputStream,
                    inputFilePath,
                    out licensePtr);

                SafeNativeMethods.ThrowOnErrorCode(hr);

                license = SafeNativeMethods.MarshalIpcBufferToManaged(licensePtr);
            }
            finally
            {
                UnsafeFileApiMethods.IpcFreeMemory(licensePtr);
            }
            return license;
        }

        public static FileEncryptedStatus IpcfIsFileEncrypted(string inputFile)
        {
            uint fileStatus;
            int hr = UnsafeFileApiMethods.IpcfIsFileEncrypted(inputFile, out fileStatus);
            SafeNativeMethods.ThrowOnErrorCode(hr);

            return (FileEncryptedStatus)fileStatus;

        }

        public static bool IpcfIsFileStreamEncrypted(Stream inputStream, string inputFilePath)
        {
            uint fileStatus;
            ILockBytes ilInputStream = new LockBytesOverStream(inputStream);
            int hr = UnsafeFileApiMethods.IpcfIsFileStreamEncrypted(ilInputStream, inputFilePath, out fileStatus);
            SafeNativeMethods.ThrowOnErrorCode(hr);

            return (FileEncryptedStatus)fileStatus != FileEncryptedStatus.IPCF_FILE_STATUS_DECRYPTED;
        }

        public enum FileEncryptedStatus
        {
            IPCF_FILE_STATUS_DECRYPTED                              = 0,
            IPCF_FILE_STATUS_ENCRYPTED_CUSTOM                       = 1,
            IPCF_FILE_STATUS_ENCRYPTED                              = 2
        }

        public enum EncryptLicenseInfoTypes
        {
            IPCF_EF_TEMPLATE_ID                                     = 0,
            IPCF_EF_LICENSE_HANDLE                                  = 1
        }

        [Flags]
        public enum EncryptFlags
        {
            IPCF_EF_FLAG_DEFAULT                                     = 0x00000000,
            IPCF_EF_FLAG_UPDATELICENSE_BLOCKED                       = 0x00000001,
            IPCF_EF_FLAG_KEY_NO_PERSIST                              = 0x00000002,
            IPCF_EF_FLAG_KEY_NO_PERSIST_DISK                         = 0x00000004,
            IPCF_EF_FLAG_KEY_NO_PERSIST_LICENSE                      = 0x00000008
        }

        [Flags]
        public enum DecryptFlags
        {
            IPCF_DF_FLAG_DEFAULT            = 0x00000000,
            IPCF_DF_FLAG_OPEN_AS_RMS_AWARE =  0x00000001
        }
    }
}
