public class HxCrypt
{
    /******************************************************************************************
    // 출처 : userpark.net / userpark@userpark.net
    // 배포 라이센스 : GNU
    // 원 소스 출처 : http://adodb.org/ , crypt.inc.php, MD5Crypt
    // .NET Standard 2.0 기준 제작 / System.Security.Cryptography
    *******************************************************************************************/
    
    //랜덤(난수)값
    // - PHP에서는 랜덤값 중복을 방지하기 위하여 sland를 이용하였으나
    // - C#에서는 단위 변수로 이용시 중복되지 않음
    private static Random sland = new Random();

    #region base64 Encode/Decode
    /// <summary>
    /// Base64 Encode
    /// </summary>
    /// <param name="input">입력값</param>
    /// <param name="encodingType">Encoding Type</param>
    /// <returns>Base64 Encode 문자열</returns>
    public static string base64_encode(string input, HxEncodingType encodingType = HxEncodingType.ASCII)
    {
        //byte[] inputStringAsBytes = Encoding.ASCII.GetBytes(input);
        byte[] inputStringAsBytes = GetString2Bytes(input, encodingType);
        string Result = Convert.ToBase64String(inputStringAsBytes);
        return Result;
    }

    /// <summary>
    /// Base64 Decode
    /// </summary>
    /// <param name="input">입력값</param>
    /// <param name="encodingType">Encoding Type</param>
    /// <returns>Base64 Decode 문자열</returns>
    public static string base64_decode(string input, HxEncodingType encodingType = HxEncodingType.ASCII)
    {
        byte[] inputStringAsBytes = Convert.FromBase64String(input);

        //string Result = Encoding.ASCII.GetString(inputStringAsBytes);
        string Result = GetBytes2String(inputStringAsBytes, encodingType);
        return Result;
    }
    #endregion

    /// <summary>
    /// CryptAPI를 이용한 암호화, 복호화 키 생성
    /// </summary>
    /// <param name="inputValue">입력 문자</param>
    /// <param name="keyValue">키 문자</param>
    /// <returns>생성 Key 문자열</returns>
    public static string keyED(string inputValue, string keyValue)
    {
        string Result = string.Empty;
        try
        {
            keyValue = Md5(keyValue);
            int ctr = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < inputValue.Length; i++)
            {
                if (ctr == keyValue.Length)
                    ctr = 0;
                //char cTxt = Convert.ToChar(txt.Substring(i, 1));
                //char cKey = Convert.ToChar(encrypt_key.Substring(ctr, 1));
                //int iVal = Convert.ToInt32(cTxt) ^ Convert.ToInt32(cKey);
                int iTxt = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                int iKey = Convert.ToInt32(Convert.ToChar(keyValue.Substring(ctr, 1)));
                int iVal = iTxt ^ iKey;
                sb.Append(Convert.ToChar(iVal));
                ctr++;
            }
            Result = sb.ToString();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Result = string.Empty;
        }
        return Result;
    }

    /// <summary>
    /// CryptAPI를 이용한 암호화
    /// </summary>
    /// <param name="inputValue">암호화할(일반) 문자열</param>
    /// <param name="keyValue">키 문자</param>
    /// <returns>암호화된 문자열</returns>
    public static string Encrypt(string inputValue, string keyValue)
    {
        string Result = string.Empty;
        try
        {
            if (!String.IsNullOrWhiteSpace(inputValue))
            {
                string encrypt_key = Md5(sland.Next(0, 32000).ToString());
                int ctr = 0;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < inputValue.Length; i++)
                {
                    if (ctr == encrypt_key.Length)
                        ctr = 0;
                    char cKey = Convert.ToChar(encrypt_key.Substring(ctr, 1));
                    int iTxt = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                    int iKey = Convert.ToInt32(cKey);
                    int iVal = iTxt ^ iKey;
                    sb.Append(cKey);
                    sb.Append(Convert.ToChar(iVal));
                }
                Result = base64_encode(keyED(sb.ToString(), keyValue));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Result = string.Empty;
        }
        return Result;
    }

    /// <summary>
    /// CryptAPI를 이용한 복호화
    /// </summary>
    /// <param name="inputValue">암호화된 문자열</param>
    /// <param name="keyValue">키 문자</param>
    /// <returns>복호화된 문자열</returns>
    public static string Decrypt(string inputValue, string keyValue = null)
    {
        string Result = string.Empty;
        try
        {
            if (!String.IsNullOrWhiteSpace(inputValue))
            {
                inputValue = keyED(base64_decode(inputValue), keyValue);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < inputValue.Length; i++)
                {
                    //char cKey = Convert.ToChar(txt.Substring(i, 1));
                    int iKey = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                    i++;
                    //char cTxt = Convert.ToChar(txt.Substring(i, 1));
                    int iTxt = Convert.ToInt32(Convert.ToChar(inputValue.Substring(i, 1)));
                    //int iVal = Convert.ToInt32(cTxt) ^ Convert.ToInt32(cKey);
                    int iVal = iTxt ^ iKey;
                    sb.Append(Convert.ToChar(iVal));
                }
                Result = sb.ToString();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Result = string.Empty;
        }
        return Result;
    }

    /// <summary>
    /// 랜덤으로 요청 자리수 만큼의 문자열 생성
    /// </summary>
    /// <param name="maxLength">요청 자리수(1 이상, 0일 경우 기본값(8))</param>
    /// <returns>랜덤 문자열</returns>
    public static string RandPass(uint maxLength = 8)
    {
        string Result = string.Empty;
        if (maxLength <= 0)
        {
            maxLength = 8;
        }
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < maxLength; i++)
        {
            int randnumber = sland.Next(48, 120);
            while ((randnumber >= 58 && randnumber <= 64) || (randnumber >= 91 && randnumber <= 96))
            {
                randnumber = sland.Next(48, 120);
            }
            sb.Append(Convert.ToChar(randnumber));
        }
        Result = sb.ToString();
        return Result;
    }

    /// <summary>
    /// Byte형을 Encoding Type 문자열로 변환
    /// </summary>
    /// <param name="input">입력값</param>
    /// <param name="encodingType">Encoding Type</param>
    /// <returns>변환 값</returns>
    public static string GetBytes2String(byte[] input, HxEncodingType encodingType)
    {
        string Result;
        switch (encodingType)
        {
            case HxEncodingType.UTF7:
                Result = Encoding.UTF7.GetString(input);
                break;
            case HxEncodingType.UTF32:
                Result = Encoding.UTF32.GetString(input);
                break;
            case HxEncodingType.Unicode:
                Result = Encoding.Unicode.GetString(input);
                break;
            case HxEncodingType.BigEndianUnicode:
                Result = Encoding.BigEndianUnicode.GetString(input);
                break;
            case HxEncodingType.ASCII:
                Result = Encoding.ASCII.GetString(input);
                break;
            case HxEncodingType.Default:
                Result = Encoding.Default.GetString(input);
                break;
            case HxEncodingType.UTF8:
            case HxEncodingType.None:
            default:
                Result = Encoding.UTF8.GetString(input);
                break;
        }
        return Result;
    }

    public static byte[] GetString2Bytes(string input, HxEncodingType encodingType = HxEncodingType.None)
    {
        byte[] Result;
        switch (encodingType)
        {
            case HxEncodingType.UTF7:
                Result = Encoding.UTF7.GetBytes(input);
                break;
            case HxEncodingType.UTF32:
                Result = Encoding.UTF32.GetBytes(input);
                break;
            case HxEncodingType.Unicode:
                Result = Encoding.Unicode.GetBytes(input);
                break;
            case HxEncodingType.BigEndianUnicode:
                Result = Encoding.BigEndianUnicode.GetBytes(input);
                break;
            case HxEncodingType.ASCII:
                Result = Encoding.ASCII.GetBytes(input);
                break;
            case HxEncodingType.Default:
                Result = Encoding.Default.GetBytes(input);
                break;
            case HxEncodingType.UTF8:
            case HxEncodingType.None:
            default:
                Result = Encoding.UTF8.GetBytes(input);
                break;
        }
        return Result;
    }

    /// <summary>
    /// Byte형을 문자열로 변환
    /// </summary>
    /// <param name="input">입력 값</param>
    /// <param name="format">문자 포멧</param>
    /// <returns>변환 값</returns>
    public static string GetBytes2String(byte[] input, string format = null)
    {
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < input.Length; i++)
        {
            sBuilder.Append(input[i].ToString(format));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    #region System.Security.Cryptography
    /// <summary>
    /// MD5 
    /// </summary>
    /// <param name="inputValue"></param>
    /// <param name="encodingType"></param>
    /// <returns></returns>
    public static string Md5(string inputValue, HxEncodingType encodingType = HxEncodingType.None)
    {
        string Result = null;
        using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
        {
            Result = GetMd5Hash(md5Hash, inputValue, encodingType);

        }
        //String.IsNullOrWhiteSpace
        return Result;
    }

    private static string GetMd5Hash(System.Security.Cryptography.MD5 md5Hash, string input, HxEncodingType encodingType = HxEncodingType.None)
    {

        // Convert the input string to a byte array and compute the hash.
        //byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        //byte[] data = md5Hash.ComputeHash(Encoder.Default.GetBytes(input));
        byte[] data = md5Hash.ComputeHash(GetString2Bytes(input, encodingType));
        return GetBytes2String(data, "x2");

        //byte[] dataEA = md5Hash.ComputeHash(Encoding.ASCII.GetBytes(input));
        //byte[] dataE7 = md5Hash.ComputeHash(Encoding.UTF7.GetBytes(input)); 
        //byte[] dataE8 = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        //byte[] dataEU = md5Hash.ComputeHash(Encoding.Unicode.GetBytes(input));
        //byte[] dataE32 = md5Hash.ComputeHash(Encoding.UTF32.GetBytes(input));
        //byte[] dataED = md5Hash.ComputeHash(Encoding.Default.GetBytes(input));

        //byte[] dataAA = md5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(input));
        //byte[] dataA7 = md5Hash.ComputeHash(ASCIIEncoding.UTF7.GetBytes(input));
        //byte[] dataA8 = md5Hash.ComputeHash(ASCIIEncoding.UTF8.GetBytes(input));
        //byte[] dataAU = md5Hash.ComputeHash(ASCIIEncoding.Unicode.GetBytes(input));
        //byte[] dataA32 = md5Hash.ComputeHash(ASCIIEncoding.UTF32.GetBytes(input));
        //byte[] dataAD = md5Hash.ComputeHash(ASCIIEncoding.Default.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        /*
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
        */

    }

    // Verify a hash against a string.
    private static bool VerifyMd5Hash(System.Security.Cryptography.MD5 md5Hash, string input, string hash, HxEncodingType encodingType = HxEncodingType.None)
    {
        // Hash the input.
        string hashOfInput = GetMd5Hash(md5Hash, input, encodingType);

        // Create a StringComparer an compare the hashes.
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        if (0 == comparer.Compare(hashOfInput, hash))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static string Sha1(string inputValue, HxEncodingType encodingType = HxEncodingType.None)
    {
        string Result = null;
        using (System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider())
        {
            byte[] data = sha.ComputeHash(GetString2Bytes(inputValue, encodingType));
            Result = GetBytes2String(data);
        }
        return Result;
    }

    #endregion
}

public enum HxEncodingType
{
    None = 0,
    Default,
    ASCII,
    UTF7,
    UTF8,
    UTF32,
    Unicode,
    BigEndianUnicode,
    //CryptApiMD5AsciiEncoding = Ascii,
    //PhpMD5DefaultEncoding = UTF8, //EUC-KR, CP949, UTF-8, ISO-8859-1
    //OracleRawToHexMD5DefaultEncoding = Default, //KO16KSC5601, KO16MSWIN949, UTF8, AL32UTF8, US7ASCII, WE8ISO8859P1
}