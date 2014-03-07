using ngrok.editor.desktop.wpf.Model;
using ngrok.editor.desktop.wpf.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ngrok.editor.desktop.wpf
{
    public class UrlAclDataService : IUrlAclDataService
    {
        public UrlAclDataService()
        {
            this.UrlAcls = new List<UrlAcl>();

            Load();
        }

        public List<UrlAcl> UrlAcls { get; set; }

        private void Load() 
        {
            var retVal = PInvoke.HttpInitialize(new HTTPAPI_VERSION(2, 0), PInvoke.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);

            if ((uint)PInvoke.NO_ERROR != retVal)
            {
                throw new Exception(string.Format("Could not set the initialize the HTTP API.  Code {0}", retVal));
            }

            HTTP_SERVICE_CONFIG_URLACL_QUERY inputConfigInfoQuery = new HTTP_SERVICE_CONFIG_URLACL_QUERY();
            inputConfigInfoQuery.QueryDesc = HTTP_SERVICE_CONFIG_QUERY_TYPE.HttpServiceConfigQueryNext;
            
            int i = 0;
	        while (retVal == 0)
	        {
                inputConfigInfoQuery.dwToken = (uint)i;

                IntPtr pInputConfigInfo = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_URLACL_QUERY)));
                Marshal.StructureToPtr(inputConfigInfoQuery, pInputConfigInfo, false);

                HTTP_SERVICE_CONFIG_URLACL_SET outputConfigInfo = new HTTP_SERVICE_CONFIG_URLACL_SET();
                IntPtr pOutputConfigInfo = Marshal.AllocCoTaskMem(0);
    
                int returnLength = 0;
                retVal = PInvoke.HttpQueryServiceConfiguration(IntPtr.Zero,
                    HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                    pInputConfigInfo,
                    Marshal.SizeOf(inputConfigInfoQuery),
                    pOutputConfigInfo,
                    returnLength,
                    out returnLength,
                    IntPtr.Zero);

                if (PInvoke.ERROR_INSUFFICIENT_BUFFER == retVal)
                {
                    //Marshal the proper buffer size back from the response
                    Marshal.FreeCoTaskMem(pOutputConfigInfo);
                    pOutputConfigInfo = Marshal.AllocCoTaskMem(Convert.ToInt32(returnLength));

                    retVal = PInvoke.HttpQueryServiceConfiguration(IntPtr.Zero,
                         HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                         pInputConfigInfo,
                         Marshal.SizeOf(inputConfigInfoQuery),
                         pOutputConfigInfo,
                         returnLength,
                         out returnLength,
                         IntPtr.Zero);
                }

                if (PInvoke.NO_ERROR == retVal)
                {
                    outputConfigInfo = (HTTP_SERVICE_CONFIG_URLACL_SET)Marshal.PtrToStructure(pOutputConfigInfo, typeof(HTTP_SERVICE_CONFIG_URLACL_SET));
                    Debug.WriteLine(outputConfigInfo.KeyDesc.pUrlPrefix);
                    this.UrlAcls.Add(new UrlAcl(outputConfigInfo));

                    Marshal.FreeCoTaskMem(pOutputConfigInfo);
	                Marshal.FreeCoTaskMem(pInputConfigInfo);
                }
                else if (PInvoke.ERROR_NO_MORE_ITEMS==retVal)
                {
                    Marshal.FreeCoTaskMem(pOutputConfigInfo);
                    Marshal.FreeCoTaskMem(pInputConfigInfo);
                }
                else {

                    Marshal.FreeCoTaskMem(pOutputConfigInfo);
                    Marshal.FreeCoTaskMem(pInputConfigInfo);

                    throw new Exception(string.Format("Could not list Url Acls.  Code {0}", retVal));
                }

                //Marshal.FreeCoTaskMem(pOutputConfigInfo);
                //Marshal.FreeCoTaskMem(pInputConfigInfo);

                i++;
            }

            PInvoke.HttpTerminate(PInvoke.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
        }
    }
}
