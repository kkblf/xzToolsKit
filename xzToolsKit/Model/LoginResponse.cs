using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xzToolsKit.model
{
    public class LoginResponse
    {
        /**
         * {"access_token":"aa56e4d7-bb84-4f6b-a517-2135f409937b",
         * "token_type":"bearer",
         * "expires_in":425509,
         * "scope":"colibri_sms document_project_files_write document_my_files_write account_token_login document_enterprise_files_write account_info_read document_appdata_read document_project_files_read document_enterprise_files_read clouddoc_xiezhu document_appdata_write document_my_files_read document_incoming_files_read colibri_email document_incoming_files_write"}
         */

        public String access_token { get; set; }
        public String token_type { get; set; }
        public Int64 expires_in { get; set; }
        public String scope { get; set; }
    }
}
