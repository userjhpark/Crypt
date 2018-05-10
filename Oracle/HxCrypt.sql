CREATE OR REPLACE PACKAGE HxCrypt AS
/******************************************************************************
   // 출처 : userpark.net / userpark@userpark.net
   // 배포 라이센스 : GNU
   // 원 소스 출처 : http://adodb.org/ , crypt.inc.php, MD5Crypt
   NAME:       HxCrypt
   PURPOSE:

   REVISIONS:
   Ver        Date        Author           Description
   ---------  ----------  ---------------  ------------------------------------
   1.0        2018/05/10      userpark       1. Created this package.
******************************************************************************/

  FUNCTION base64_encode(inputString VARCHAR2) RETURN VARCHAR2;
  FUNCTION base64_decode(inputString VARCHAR2) RETURN VARCHAR2;
  FUNCTION Md5(inputString VARCHAR2) RETURN VARCHAR2;
  FUNCTION keyED(inputValue VARCHAR2, inputKey VARCHAR2) RETURN VARCHAR2;
  FUNCTION Encrypt (inputValue VARCHAR2, inputKey VARCHAR2) RETURN VARCHAR2;
  FUNCTION Decrypt (inputValue VARCHAR2, inputKey VARCHAR2) RETURN VARCHAR2;
  
END HxCrypt;
/

CREATE OR REPLACE PACKAGE BODY HxCrypt AS
  outputString VARCHAR2(2000);
   

FUNCTION base64_encode(inputString VARCHAR2) RETURN VARCHAR2 AS
  BEGIN
    outputString := utl_raw.cast_to_varchar2(utl_encode.base64_encode(utl_raw.cast_to_raw(inputString)));
    RETURN outputString;
    EXCEPTION
     WHEN NO_DATA_FOUND THEN
       NULL;
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
  END base64_encode;
  
  FUNCTION base64_decode(inputString VARCHAR2) RETURN VARCHAR2 AS
  BEGIN
    outputString := utl_raw.cast_to_varchar2(utl_encode.base64_decode(utl_raw.cast_to_raw(inputString)));
    RETURN outputString;
    EXCEPTION
     WHEN NO_DATA_FOUND THEN
       NULL;
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
  END base64_decode;
  
  FUNCTION Md5(inputString VARCHAR2) RETURN VARCHAR2 AS
  BEGIN
    --outputString := Md5(inputString);
    outputString := LOWER(RAWTOHEX(UTL_RAW.CAST_TO_RAW(sys.dbms_obfuscation_toolkit.md5(input_string => inputString))));
    RETURN outputString;
    EXCEPTION
     WHEN NO_DATA_FOUND THEN
       NULL;
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
  END Md5;
  
  FUNCTION KeyED (inputValue VARCHAR2, inputKey VARCHAR2) RETURN VARCHAR2 IS
ctr NUMBER := 0;
i NUMBER := 0;
nInput NUMBER := 0;
nKey NUMBER := 0;
keyValue VARCHAR2(2000);
iTxt NUMBER;
iKey NUMBER;
iVal NUMBER;
BEGIN
    i := 1;
    keyValue := Md5(inputKey);
    nInput := LENGTH(inputValue);
    nKey := LENGTH(keyValue);
    ctr := 1;
    outputString := null;
    WHILE i <= nInput LOOP
        IF ctr = nKey THEN 
           ctr := 1;
        END IF;
        --utl_raw.cast_to_raw(inputString)
        iTxt := ASCII(SUBSTR(TO_CHAR(inputValue), i, 1));
        --iTxt := utl_raw.cast_to_raw(SUBSTR(TO_CHAR(inputString), i + 1, 1));
        iKey := ASCII(SUBSTR(TO_CHAR(keyValue), ctr, 1));
        --iKey := utl_raw.cast_to_raw(SUBSTR(TO_CHAR(keyValue), ctr + 1, 1));
        --iVal := iTxt ^ iKey;
        iVal := PKG_UTILS.BITXOR(iTxt,iKey);
        outputString := outputString || CHR(iVal);
        --outputString := outputString || utl_raw.cast_to_varchar2(TO_CHAR(iVal));
        ctr := ctr + 1;
        i := i + 1;
        EXIT WHEN i > nInput; 
    END LOOP;
    RETURN outputString;
   EXCEPTION
     WHEN NO_DATA_FOUND THEN
       NULL;
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
END KeyED;

FUNCTION Encrypt (inputValue VARCHAR2, inputKey VARCHAR2) RETURN VARCHAR2 IS
Result VARCHAR2(2000);
ctr NUMBER := 0;
i NUMBER := 0;
n NUMBER := 0;
keyValue VARCHAR2(2000);
nKey NUMBER := 0;
iTxt NUMBER;
iKey NUMBER;
iVal NUMBER;
cKey VARCHAR2(8);
BEGIN
    i := 1;
    ctr := 1;
    keyValue := Md5(TO_CHAR(CEIL(DBMS_RANDOM.VALUE(0,32000))));
    n := LENGTH(inputValue);
    nKey := LENGTH(keyValue);
    Result := null;
    WHILE i <= n LOOP
        IF ctr = nKey THEN 
           ctr := 1;
        END IF;
        cKey := SUBSTR(TO_CHAR(keyValue), ctr, 1);
        iTxt := ASCII(SUBSTR(TO_CHAR(inputValue), i, 1));
        iKey := ASCII(cKey);
        --iVal := iTxt ^ iKey;
        iVal := PKG_UTILS.BITXOR(iTxt,iKey);
        Result := Result || cKey || CHR(iVal);
        ctr := ctr + 1;
        i := i + 1;
        EXIT WHEN i > n; 
    END LOOP;
    outputString := base64_encode(KeyED(Result, inputKey));
    RETURN outputString;
   EXCEPTION
     WHEN NO_DATA_FOUND THEN
       NULL;
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
END Encrypt;

FUNCTION Decrypt (inputValue VARCHAR2, inputKey VARCHAR2) RETURN VARCHAR2 IS
i NUMBER := 0;
n NUMBER := 0;
keyValue VARCHAR2(2000);
iTxt NUMBER;
iKey NUMBER;
iVal NUMBER;
BEGIN
    i := 1;
    keyValue := KeyED(base64_decode(inputValue), inputKey);
    n := LENGTH(keyValue);
    outputString := null;
    WHILE i <= n LOOP
        iKey := ASCII(SUBSTR(TO_CHAR(keyValue), i, 1));
        i := i + 1;
        iTxt := ASCII(SUBSTR(TO_CHAR(keyValue), i, 1));
        --iVal := iTxt ^ iKey;
        iVal := PKG_UTILS.BITXOR(iTxt,iKey);
        outputString := outputString || CHR(iVal);
        i := i + 1;
        EXIT WHEN i > n; 
    END LOOP;
    
    RETURN outputString;
   
   EXCEPTION
     WHEN NO_DATA_FOUND THEN
       NULL;
     WHEN OTHERS THEN
       -- Consider logging the error and then re-raise
       RAISE;
           
END Decrypt;


END HxCrypt;
/