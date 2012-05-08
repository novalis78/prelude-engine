// import external dll functions

using System;
using System.Text;
using System.Runtime.InteropServices;

public class QRNG
{
    public enum _qrng_error {
        QRNG_SUCCESS, // = 0 
        QRNG_ERR_FAILED_TO_BASE_INIT, 
        QRNG_ERR_FAILED_TO_INIT_SOCK, 
        QRNG_ERR_FAILED_TO_INIT_SSL, 
        QRNG_ERR_FAILED_TO_CONNECT, 
        QRNG_ERR_SERVER_FAILED_TO_INIT_SSL, 
        QRNG_ERR_FAILED_SSL_HANDSHAKE, 
        QRNG_ERR_DURING_USER_AUTH, 
        QRNG_ERR_USER_CONNECTION_QUOTA_EXCEEDED, 
        QRNG_ERR_BAD_CREDENTIALS, 
        QRNG_ERR_NOT_CONNECTED,
	    QRNG_ERR_BAD_BYTES_COUNT,
	    QRNG_ERR_BAD_BUFFER_ADDRESS,
	    QRNG_ERR_PASSWORD_CHARLIST_TOO_LONG,
	    QRNG_ERR_READING_RANDOM_DATA_FAILED_ZERO,
	    QRNG_ERR_READING_RANDOM_DATA_FAILED_INCOMPLETE,
	    QRNG_ERR_READING_RANDOM_DATA_OVERFLOW,
	    QRNG_ERR_FAILED_TO_READ_WELCOMEMSG,
	    QRNG_ERR_FAILED_TO_READ_AUTH_REPLY,
	    QRNG_ERR_FAILED_TO_READ_USER_REPLY,
	    QRNG_ERR_FAILED_TO_READ_PASS_REPLY,
	    QRNG_ERR_FAILED_TO_SEND_COMMAND
        // you may obtain between 1 to 2147483647 bytes with one get_random_bytes() call*/  });    
    }

    public static readonly string[] qrng_error_strings = {
    	"QRNG_SUCCESS",
	    "QRNG_ERR_FAILED_TO_BASE_INIT",
	    "QRNG_ERR_FAILED_TO_INIT_SOCK",
	    "QRNG_ERR_FAILED_TO_INIT_SSL",
	    "QRNG_ERR_FAILED_TO_CONNECT",
	    "QRNG_ERR_SERVER_FAILED_TO_INIT_SSL",
	    "QRNG_ERR_FAILED_SSL_HANDSHAKE",
	    "QRNG_ERR_DURING_USER_AUTH",
	    "QRNG_ERR_USER_CONNECTION_QUOTA_EXCEEDED",
	    "QRNG_ERR_BAD_CREDENTIALS",
	    "QRNG_ERR_NOT_CONNECTED",
	    "QRNG_ERR_BAD_BYTES_COUNT",
	    "QRNG_ERR_BAD_BUFFER_ADDRESS",
	    "QRNG_ERR_PASSWORD_CHARLIST_TOO_LONG",
	    "QRNG_ERR_READING_RANDOM_DATA_FAILED_ZERO",
	    "QRNG_ERR_READING_RANDOM_DATA_FAILED_INCOMPLETE",
	    "QRNG_ERR_READING_RANDOM_DATA_OVERFLOW",
	    "QRNG_ERR_FAILED_TO_READ_WELCOMEMSG",
	    "QRNG_ERR_FAILED_TO_READ_AUTH_REPLY",
	    "QRNG_ERR_FAILED_TO_READ_USER_REPLY",
	    "QRNG_ERR_FAILED_TO_READ_PASS_REPLY",
	    "QRNG_ERR_FAILED_TO_SEND_COMMAND"
    };

    public bool CheckDLL()
    {
        QRNGDLLLoaded = true;
        return QRNGDLLLoaded;
    }

    //{+// All library functions (except disconnect()) return 0 (= QRNG_SUCCESS) if }
    //{-the command succeeded, otherwise an error taken from enum _qrng_error. }
    //{=Use the qrng_error_strings array to output the error code as a string. }

    //{+// connect to QRNG service first, by default no ssl will be used*/ }
    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect(StringBuilder username,
                                            StringBuilder password);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_SSL(StringBuilder username,
                                                StringBuilder password);

    //{+// read bytes / double(s) / int(s) (requires an established connection) }
    //{-make sure your program allocated the value / array beforehand }
    //{=if connected via SSL, the data will be also encrypted }
    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_get_byte_array(ref Byte byte_array,
                                                   Int32 byte_array_size,
                                                   ref Int32 actual_bytes_rcvd);

    //{+// returns double value(s) within range [0, 1)*/ }
    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_get_double(Double value);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_get_double_array(ref Double double_array,
                                                     Int32 double_array_size,
                                                     ref Int32 actual_doubles_rcvd);

    //{+// returns integer value(s)*/ }
    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_get_int(ref Int32 value);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_get_int_array(ref Int32 int_array,
                                                  Int32 int_array_size,
                                                  ref Int32 actual_ints_rcvd);

    //{+// this function will return a random string containing the characters a-zA-Z0-9 }
    //{=of length password_length terminated by a null character }
    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_generate_password(StringBuilder tobeused_password_chars,
                                                      StringBuilder generated_password,
                                                      Int32 password_length);

    //{+// here are some handy one-liner functions which automatically a) connect to the QRNG service, }
    //{-b) retrieve the requested data and c) disconnect again. }
    //{-You can use them, if you retrieve data only once in a while. }
    //{=(By default no SSL will be used.) }
    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_byte_array(StringBuilder username,
                                                               StringBuilder password,
                                                               ref Byte byte_array,
                                                               Int32 byte_array_size,
                                                               ref Int32 actual_bytes_rcvd);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_byte_array_SSL(StringBuilder username,
                                                                   StringBuilder password,
                                                                   ref Byte byte_array,
                                                                   Int32 byte_array_size,
                                                                   ref Int32 actual_bytes_rcvd);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_double_array(StringBuilder username,
                                                                 StringBuilder password,
                                                                 ref Double double_array,
                                                                 Int32 double_array_size,
                                                                 ref Int32 actual_doubles_rcvd);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_double_array_SSL(StringBuilder username,
                                                                     StringBuilder password,
                                                                     ref Double double_array,
                                                                     Int32 double_array_size,
                                                                     ref Int32 actual_doubles_rcvd);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_int_array(StringBuilder username,
                                                              StringBuilder password,
                                                              ref Int32 int_array,
                                                              Int32 int_array_size,
                                                              ref Int32 actual_ints_rcvd);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_int_array_SSL(StringBuilder username,
                                                                  StringBuilder password,
                                                                  ref Int32 int_array,
                                                                  Int32 int_array_size,
                                                                  ref Int32 actual_ints_rcvd);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_double(StringBuilder username,
                                                           StringBuilder password,
                                                           Double value);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_double_SSL(StringBuilder username,
                                                               StringBuilder password,
                                                               Double value);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_int(StringBuilder username,
                                                        StringBuilder password,
                                                        Int32 value);

    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_connect_and_get_int_SSL(StringBuilder username,
                                                            StringBuilder password,
                                                            Int32 value);

    //{+// disconnect*/ }
    [DllImport("libqrng.dll")]
    public static extern Int32 qrng_disconnect();

    private bool QRNGDLLLoaded = false;
}



