using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf.Win32
{
    public static class PInvoke
    {
        public const uint HTTP_INITIALIZE_SERVER = 0x00000001;
        public const uint HTTP_INITIALIZE_CONFIG = 0x00000002;
        public const uint HTTP_DEMAND_CBT = 0x00000004;

        public const uint NO_ERROR = 0;
        public const uint ERROR_FILE_NOT_FOUND = 2;
        public const uint ERROR_ACCESS_DENIED = 5;
        public const uint ERROR_INVALID_HANDLE = 6;
        public const uint ERROR_INVALID_PARAMETER = 87;
        public const uint ERROR_INSUFFICIENT_BUFFER = 122;
        public const uint ERROR_ALREADY_EXISTS = 183;
        public const uint ERROR_MORE_DATA = 234;
        public const uint ERROR_NO_MORE_ITEMS = 259;

        [DllImport("httpapi.dll", SetLastError = true)]
        public static extern uint HttpInitialize(HTTPAPI_VERSION Version, uint Flags, IntPtr pReserved);

        [DllImport("httpapi.dll", SetLastError = true)]
        public static extern uint HttpTerminate(uint Flags,IntPtr pReserved);

        [DllImport("httpapi.dll", SetLastError = true)]
        public static extern uint HttpQueryServiceConfiguration(IntPtr ServiceIntPtr,HTTP_SERVICE_CONFIG_ID ConfigId,IntPtr pInputConfigInfo,int InputConfigInfoLength,IntPtr pOutputConfigInfo,int OutputConfigInfoLength,[Optional()]out int pReturnLength,IntPtr pOverlapped);

        [DllImport("httpapi.dll", SetLastError = true)]
        public static extern uint HttpSetServiceConfiguration(IntPtr ServiceIntPtr,HTTP_SERVICE_CONFIG_ID ConfigId,IntPtr pConfigInformation,int ConfigInformationLength,IntPtr pOverlapped);

        [DllImport("httpapi.dll", SetLastError = true)]
        public static extern uint HttpDeleteServiceConfiguration(IntPtr ServiceIntPtr,HTTP_SERVICE_CONFIG_ID ConfigId,IntPtr pConfigInformation,int ConfigInformationLength,IntPtr pOverlapped);
    }

    public enum HTTP_SERVICE_CONFIG_QUERY_TYPE
    {
        HttpServiceConfigQueryExact = 0,
        HttpServiceConfigQueryNext,
        HttpServiceConfigQueryMax
    }

    public enum HTTP_SERVICE_CONFIG_ID
    {
        HttpServiceConfigIPListenList = 0,
        HttpServiceConfigSSLCertInfo,
        HttpServiceConfigUrlAclInfo,
        HttpServiceConfigMax
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HTTP_SERVICE_CONFIG_URLACL_QUERY
    {
        public HTTP_SERVICE_CONFIG_QUERY_TYPE QueryDesc;
        public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;
        public uint dwToken;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct HTTP_SERVICE_CONFIG_URLACL_KEY
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pUrlPrefix;

        public HTTP_SERVICE_CONFIG_URLACL_KEY(string urlPrefix)
        {
            pUrlPrefix = urlPrefix;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HTTP_SERVICE_CONFIG_URLACL_SET
    {
        public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;
        public HTTP_SERVICE_CONFIG_URLACL_PARAM ParamDesc;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct HTTP_SERVICE_CONFIG_URLACL_PARAM
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pStringSecurityDescriptor;

        public HTTP_SERVICE_CONFIG_URLACL_PARAM(string securityDescriptor)
        {
            pStringSecurityDescriptor = securityDescriptor;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct HTTPAPI_VERSION
    {
        public ushort HttpApiMajorVersion;
        public ushort HttpApiMinorVersion;

        public HTTPAPI_VERSION(ushort majorVersion, ushort minorVersion)
        {
            HttpApiMajorVersion = majorVersion;
            HttpApiMinorVersion = minorVersion;
        }
    }
}
