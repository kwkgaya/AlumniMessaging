namespace AlumniMessaging.Services
{
    public interface IPermissionRequest
    {
        bool CheckAndRequestPermissions(string permission);
    }

    public sealed class Permission
    {
        public const string AccessCoarseLocation = "android.permission.ACCESS_COARSE_LOCATION";
        public const string AccessFineLocation = "android.permission.ACCESS_FINE_LOCATION";
        public const string AccessMockLocation = "android.permission.ACCESS_MOCK_LOCATION";
        public const string AccessNetworkState = "android.permission.ACCESS_NETWORK_STATE";
        public const string BroadcastSms = "android.permission.BROADCAST_SMS";
        public const string CallPhone = "android.permission.CALL_PHONE";
        public const string Internet = "android.permission.INTERNET";
        public const string ReadCallLog = "android.permission.READ_CALL_LOG";
        public const string ReadContacts = "android.permission.READ_CONTACTS";
        public const string ReadExternalStorage = "android.permission.READ_EXTERNAL_STORAGE";
        public const string ReadLogs = "android.permission.READ_LOGS";
        public const string ReadPhoneNumbers = "android.permission.READ_PHONE_NUMBERS";
        public const string ReadPhoneState = "android.permission.READ_PHONE_STATE";
        public const string ReadSms = "android.permission.READ_SMS";
        public const string ReceiveMms = "android.permission.RECEIVE_MMS";
        public const string ReceiveSms = "android.permission.RECEIVE_SMS";
        public const string SendRespondViaMessage = "android.permission.SEND_RESPOND_VIA_MESSAGE";
        public const string SendSms = "android.permission.SEND_SMS";
        public const string WriteExternalStorage = "android.permission.WRITE_EXTERNAL_STORAGE";
    }
}